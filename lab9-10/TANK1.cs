using UnityEngine;

public class TankControllerFixed : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 100f;
    public float turretRotationSpeed = 90f;
    public float barrelTiltSpeed = 45f;
    public float barrelMinAngle = -10f;
    public float barrelMaxAngle = 30f;

    [Header("Tank Parts")]
    public Transform turret;
    public Transform hull;
    public Transform barrel;

    [Header("Barrel Pivot Settings")]
    public Vector3 barrelPivotOffset = Vector3.zero; // Смещение точки вращения дула относительно башни
    public bool useBarrelCustomPivot = false;

    [Header("Turret Rotation Settings")]
    public bool useCustomPivot = false;
    public Vector3 turretPivotOffset = Vector3.zero;
    public Transform manualPivotPoint;

    [Header("Pivot Save/Load")]
    public bool savePivotPosition = true;
    public Vector3 savedPivotLocalPosition = Vector3.zero;

    [Header("Audio")]
    public AudioSource engineAudio;
    public AudioClip engineSound;

    private Rigidbody rb;
    private float currentSpeed;
    private GameObject customPivotObject;
    private GameObject barrelPivotObject;
    private float currentBarrelAngle = 0f;
    private bool isBarrelSetup = false;

    void Start()
    {
        SetupTank();
    }

    void SetupTank()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        rb.mass = 1000f;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 2f;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Автоматический поиск компонентов
        if (turret == null) turret = FindDeepChild(transform, "голова") ?? FindDeepChild(transform, "башня");
        if (hull == null) hull = FindDeepChild(transform, "тело");
        if (barrel == null) barrel = FindDeepChild(transform, "дуло");

        if (hull == null) hull = transform;

        // Настраиваем дуло
        SetupBarrel();

        if (GetComponent<Collider>() == null)
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(2f, 1f, 3f);
            collider.center = new Vector3(0, 0.5f, 0);
        }

        if (engineAudio != null && engineSound != null)
        {
            engineAudio.clip = engineSound;
            engineAudio.loop = true;
            engineAudio.Play();
        }

        if (useCustomPivot && savePivotPosition && savedPivotLocalPosition != Vector3.zero)
        {
            RestoreSavedPivot();
        }
        else if (useCustomPivot && customPivotObject == null)
        {
            CreateCustomPivot();
        }
    }

    void SetupBarrel()
    {
        if (barrel == null) return;

        if (turret != null && barrel.parent != turret)
        {
            Debug.Log($"Barrel parent is {barrel.parent?.name}, should be {turret.name}. Fixing hierarchy...");

            // Сохраняем текущую мировую позицию и вращение
            Vector3 worldPosition = barrel.position;
            Quaternion worldRotation = barrel.rotation;

            // Устанавливаем правильного родителя
            barrel.SetParent(turret);

            // Восстанавливаем мировую позицию и вращение
            barrel.position = worldPosition;
            barrel.rotation = worldRotation;
        }

        // Создаем точку вращения для дула
        CreateBarrelPivot();

        currentBarrelAngle = 0f;
        isBarrelSetup = true;

        Debug.Log($"Barrel setup complete. Parent: {barrel.parent?.name}");
    }

    void CreateBarrelPivot()
    {
        if (barrel == null || turret == null) return;

        // Удаляем старый pivot если есть
        if (barrelPivotObject != null)
        {
            DestroyImmediate(barrelPivotObject);
        }

        barrelPivotObject = new GameObject("BarrelPivot");
        barrelPivotObject.transform.SetParent(turret);

        // Позиционируем pivot в основании дула (предполагаем, что основание ближе к башне)
        // Можно настроить смещение в инспекторе
        Vector3 pivotPosition = turret.position;
        if (barrelPivotOffset != Vector3.zero)
        {
            pivotPosition = turret.TransformPoint(barrelPivotOffset);
        }
        else
        {
            // Автоматически определяем основание дула
            // Предполагаем, что основание ближе к башне по оси Z
            Vector3 barrelLocalPos = barrel.localPosition;
            Vector3 pivotLocalPos = new Vector3(barrelLocalPos.x, barrelLocalPos.y, 0f);
            pivotPosition = turret.TransformPoint(pivotLocalPos);
        }

        barrelPivotObject.transform.position = pivotPosition;
        barrelPivotObject.transform.rotation = turret.rotation;

        Debug.Log($"Barrel pivot created at: {pivotPosition}");
    }

    void Update()
    {
        HandleInput();
        RotateTurret();
        TiltBarrel();
        UpdateEngineSound();
    }

    void FixedUpdate()
    {
        MoveTank();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) Debug.Log("W - Forward");
        if (Input.GetKeyDown(KeyCode.S)) Debug.Log("S - Backward");
        if (Input.GetKeyDown(KeyCode.A)) Debug.Log("A - Left");
        if (Input.GetKeyDown(KeyCode.D)) Debug.Log("D - Right");
        if (Input.GetKeyDown(KeyCode.DownArrow)) Debug.Log("⬇️ - Barrel Down");
        if (Input.GetKeyDown(KeyCode.UpArrow)) Debug.Log("⬆️ - Barrel Up");
    }

    void MoveTank()
    {
        float moveInput = 0f;
        float rotateInput = 0f;

        if (Input.GetKey(KeyCode.W)) moveInput = 1f;
        if (Input.GetKey(KeyCode.S)) moveInput = -1f;
        if (Input.GetKey(KeyCode.A)) rotateInput = -1f;
        if (Input.GetKey(KeyCode.D)) rotateInput = 1f;

        if (moveInput != 0)
        {
            Vector3 movement = transform.right * moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }

        if (rotateInput != 0)
        {
            float rotation = rotateInput * rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(0f, rotation, 0f);
        }

        currentSpeed = Mathf.Abs(moveInput) * moveSpeed;
    }

    void RotateTurret()
    {
        if (turret == null) return;

        float turretInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) turretInput = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) turretInput = 1f;

        if (turretInput != 0)
        {
            if (useCustomPivot)
            {
                RotateTurretAroundCustomPivot(turretInput * turretRotationSpeed * Time.deltaTime);
            }
            else
            {
                turret.Rotate(0f, turretInput * turretRotationSpeed * Time.deltaTime, 0f, Space.Self);
            }
        }
    }

    void TiltBarrel()
    {
        if (barrel == null || !isBarrelSetup || barrelPivotObject == null) return;

        float tiltInput = 0f;
        if (Input.GetKey(KeyCode.DownArrow)) tiltInput = -1f; // Наклон вниз
        if (Input.GetKey(KeyCode.UpArrow)) tiltInput = 1f;  // Наклон вверх

        if (tiltInput != 0)
        {
            // Вращаем дуло вокруг кастомной точки
            float angle = tiltInput * barrelTiltSpeed * Time.deltaTime;
            RotateBarrelAroundPivot(angle);
        }
    }

    void RotateBarrelAroundPivot(float angle)
    {
        if (barrel == null || barrelPivotObject == null) return;

        // Обновляем угол с ограничениями
        currentBarrelAngle += angle;
        currentBarrelAngle = Mathf.Clamp(currentBarrelAngle, barrelMinAngle, barrelMaxAngle);

        // Вращаем дуло вокруг точки pivot
        barrel.RotateAround(barrelPivotObject.transform.position, barrelPivotObject.transform.right, angle);
    }

    void RotateTurretAroundCustomPivot(float angle)
    {
        if (manualPivotPoint != null)
        {
            turret.RotateAround(manualPivotPoint.position, Vector3.up, angle);
        }
        else if (customPivotObject != null)
        {
            turret.RotateAround(customPivotObject.transform.position, Vector3.up, angle);
        }
        else
        {
            Vector3 pivotPoint = turret.TransformPoint(turretPivotOffset);
            turret.RotateAround(pivotPoint, Vector3.up, angle);
        }
    }

    void UpdateEngineSound()
    {
        if (engineAudio != null)
        {
            float targetPitch = 0.8f + (currentSpeed / moveSpeed) * 0.4f;
            engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, targetPitch, Time.deltaTime * 2f);
            engineAudio.volume = 0.5f + (currentSpeed / moveSpeed) * 0.5f;
        }
    }

    // Рекурсивный поиск дочерних объектов по имени
    Transform FindDeepChild(Transform parent, string childName)
    {
        if (parent == null) return null;

        // Сначала проверяем прямых детей
        foreach (Transform child in parent)
        {
            if (child.name.ToLower().Contains(childName.ToLower()))
                return child;
        }

        // Затем рекурсивно проверяем всех детей
        foreach (Transform child in parent)
        {
            Transform result = FindDeepChild(child, childName);
            if (result != null) return result;
        }

        return null;
    }

    void CreateCustomPivot()
    {
        if (turret == null) return;

        if (customPivotObject != null)
        {
            DestroyImmediate(customPivotObject);
        }

        customPivotObject = new GameObject("CustomTurretPivot");
        customPivotObject.transform.SetParent(transform);

        if (manualPivotPoint != null)
        {
            customPivotObject.transform.position = manualPivotPoint.position;
            savedPivotLocalPosition = transform.InverseTransformPoint(manualPivotPoint.position);
        }
        else if (savedPivotLocalPosition != Vector3.zero && savePivotPosition)
        {
            customPivotObject.transform.localPosition = savedPivotLocalPosition;
        }
        else
        {
            Vector3 worldPivot = turret.TransformPoint(turretPivotOffset);
            customPivotObject.transform.position = worldPivot;
            savedPivotLocalPosition = transform.InverseTransformPoint(worldPivot);
        }

        customPivotObject.transform.rotation = turret.rotation;
        Debug.Log("Custom turret pivot created at: " + customPivotObject.transform.position);
    }

    void RestoreSavedPivot()
    {
        if (turret == null) return;

        customPivotObject = new GameObject("CustomTurretPivot");
        customPivotObject.transform.SetParent(transform);
        customPivotObject.transform.localPosition = savedPivotLocalPosition;
        customPivotObject.transform.rotation = turret.rotation;

        Debug.Log("Saved pivot restored at local position: " + savedPivotLocalPosition);
    }

    void SaveCurrentPivotPosition()
    {
        if (customPivotObject != null)
        {
            savedPivotLocalPosition = customPivotObject.transform.localPosition;
            Debug.Log("Pivot position saved: " + savedPivotLocalPosition);
        }
        else if (manualPivotPoint != null)
        {
            savedPivotLocalPosition = transform.InverseTransformPoint(manualPivotPoint.position);
            Debug.Log("Manual pivot position saved: " + savedPivotLocalPosition);
        }
        else
        {
            Debug.LogWarning("No pivot to save!");
        }
    }

    void OnValidate()
    {
        if (customPivotObject != null && savePivotPosition && Application.isEditor && !Application.isPlaying)
        {
            SaveCurrentPivotPosition();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (useCustomPivot && turret != null)
        {
            Gizmos.color = Color.red;
            Vector3 pivotPosition;

            if (manualPivotPoint != null)
            {
                pivotPosition = manualPivotPoint.position;
            }
            else if (customPivotObject != null)
            {
                pivotPosition = customPivotObject.transform.position;
            }
            else if (savedPivotLocalPosition != Vector3.zero)
            {
                pivotPosition = transform.TransformPoint(savedPivotLocalPosition);
            }
            else
            {
                pivotPosition = turret.TransformPoint(turretPivotOffset);
            }

            Gizmos.DrawWireSphere(pivotPosition, 0.2f);
            Gizmos.DrawLine(pivotPosition, pivotPosition + Vector3.up * 0.5f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pivotPosition, turret.position);

            if (savePivotPosition && savedPivotLocalPosition != Vector3.zero)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.TransformPoint(savedPivotLocalPosition), 0.15f);
            }
        }

        // Визуализация дула и его точки вращения
        if (barrel != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(barrel.position, 0.1f);
            Gizmos.DrawLine(barrel.position, barrel.position + barrel.forward * 2f);

            // Точка вращения дула
            if (barrelPivotObject != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(barrelPivotObject.transform.position, 0.15f);
                Gizmos.DrawLine(barrelPivotObject.transform.position, barrel.position);
            }
        }
    }
}