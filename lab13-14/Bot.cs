using UnityEngine;
using System.Collections;

public class Bot : MonoBehaviour
{
    [Header("Настройки движения")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float turretRotationSpeed = 90f;

    [Header("Компоненты бота")]
    public Transform turret;
    public Transform barrel;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab; // Префаб взрыва

    [Header("Настройки стрельбы")]
    public float shootCooldown = 3f;
    public float detectionRange = 30f;
    public float attackRange = 20f;
    public float minComfortDistance = 8f;

    [Header("Здоровье")]
    public int maxLife = 3;
    private int _currentLife = 3;
    public int currentLife
    {
        get { return _currentLife; }
        set
        {
            _currentLife = value;
            Debug.Log($"Бот получил урон! Осталось жизней: {_currentLife}");
            UpdateTankAppearance();
            if (_currentLife <= 0)
            {
                Die();
            }
        }
    }

    [Header("Материалы при уроне")]
    public Material damagedMaterial1;
    public Material damagedMaterial2;
    public Material destroyedMaterial;

    private bool canShoot = true;
    private Transform player;
    private Rigidbody rb;
    private Renderer tankRenderer;
    private Material originalMaterial;
    private bool isDead = false;
    private SphereCollider detectionCollider;
    private bool isMoving = false;
    private Vector3 targetDirection;
    private int hitCount = 0;
    private BoxCollider mainCollider; // Основной коллайдер танка

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Получаем основной BoxCollider танка
        mainCollider = GetComponent<BoxCollider>();
        if (mainCollider == null)
        {
            mainCollider = GetComponentInChildren<BoxCollider>();
            if (mainCollider == null)
            {
                Debug.LogError("У бота нет BoxCollider! Добавьте BoxCollider к модели танка.");
            }
        }

        // Добавляем или получаем сферический триггерный коллайдер для обнаружения
        detectionCollider = GetComponent<SphereCollider>();
        if (detectionCollider == null)
        {
            detectionCollider = gameObject.AddComponent<SphereCollider>();
        }

        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRange;

        // Настраиваем смещение, чтобы триггер не мешал основному коллайдеру
        detectionCollider.center = new Vector3(0, 1.5f, 0);

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 1000f;
            rb.linearDamping = 0.5f;
            rb.angularDamping = 2f;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        tankRenderer = GetComponent<Renderer>();
        if (tankRenderer == null)
        {
            tankRenderer = GetComponentInChildren<Renderer>();
        }

        if (tankRenderer != null)
        {
            originalMaterial = tankRenderer.material;
        }

        if (turret == null)
        {
            turret = FindChildWithName(transform, "turret") ?? transform;
        }

        if (barrel == null && turret != null)
        {
            barrel = FindChildWithName(turret, "barrel") ?? turret;
        }

        // Устанавливаем тег для бота
        gameObject.tag = "goal";

        // Инициализируем жизни
        _currentLife = maxLife;

        Debug.Log($"Бот инициализирован. Здоровье: {currentLife}");
    }

    Transform FindChildWithName(Transform parent, string name)
    {
        if (parent == null) return null;

        foreach (Transform child in parent)
        {
            if (child.name.ToLower().Contains(name.ToLower()))
                return child;
        }
        return null;
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= detectionRange)
        {
            // Поворачиваем башню в направлении игрока
            RotateTurretTowardsPlayer();

            // Проверяем видимость игрока и возможность стрельбы
            if (distance <= attackRange && canShoot && IsPlayerVisible())
            {
                StartCoroutine(ShootCoroutine());
            }

            // Логика движения в зависимости от дистанции
            UpdateMovement(distance);
        }
        else
        {
            // Если игрок вне зоны обнаружения, останавливаемся
            isMoving = false;
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;
        ApplyMovement();
    }

    void UpdateMovement(float distance)
    {
        if (isDead) return;
        if (player == null) return;

        // Если игрок слишком близко - отходим назад без разворота
        if (distance < minComfortDistance)
        {
            targetDirection = -transform.forward;
            isMoving = true;
            return;
        }

        Vector3 toPlayer = (player.position - transform.position).normalized;
        toPlayer.y = 0;

        if (distance > 15f)
        {
            targetDirection = toPlayer;
            isMoving = true;
        }
        else if (distance >= minComfortDistance && distance <= 15f)
        {
            Vector3 perpendicular = Vector3.Cross(toPlayer, Vector3.up).normalized;
            float sideDirection = Mathf.Sin(Time.time * 0.5f) > 0 ? 1f : -1f;
            targetDirection = (toPlayer * 0.3f + perpendicular * sideDirection * 0.7f).normalized;
            isMoving = true;
        }

        // Плавный поворот к целевому направлению
        if (targetDirection != Vector3.zero && distance >= minComfortDistance)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                rotationSpeed * Time.deltaTime);
        }
    }

    void ApplyMovement()
    {
        if (isDead) return;
        if (rb == null) return;

        if (isMoving && !isDead)
        {
            Vector3 moveDir = targetDirection;
            moveDir.y = 0;

            if (moveDir.magnitude > 0.1f)
            {
                float angle = Vector3.Angle(transform.forward, moveDir);
                if (angle > 10f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(moveDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,
                        rotationSpeed * 0.5f * Time.fixedDeltaTime);
                }

                rb.AddForce(transform.forward * moveSpeed * 30f, ForceMode.Force);

                if (rb.linearVelocity.magnitude > moveSpeed)
                {
                    rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
                }
            }
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 3f);
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 3f);
        }
    }

    void RotateTurretTowardsPlayer()
    {
        if (isDead) return;
        if (turret == null || player == null) return;

        Vector3 directionToPlayer = player.position - turret.position;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            turret.rotation = Quaternion.RotateTowards(
                turret.rotation,
                targetRotation,
                turretRotationSpeed * Time.deltaTime
            );
        }
    }

    IEnumerator ShootCoroutine()
    {
        if (isDead) yield break;

        canShoot = false;

        if (IsPlayerVisible())
        {
            Shoot();
        }

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    void Shoot()
    {
        if (isDead) return;
        if (bulletPrefab == null || barrel == null) return;

        Vector3 spawnPosition = barrel.position + barrel.forward * 2f;
        Quaternion bulletRotation = barrel.rotation * Quaternion.Euler(0, 180f, 0);

        GameObject newBullet = Instantiate(bulletPrefab, spawnPosition, bulletRotation);
        newBullet.tag = "BotBullet";

        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = -barrel.forward * 30f;
        }

        Debug.Log("Бот выстрелил!");
    }

    bool IsPlayerVisible()
    {
        if (isDead) return false;
        if (barrel == null || player == null) return false;

        RaycastHit hit;
        Vector3 direction = (player.position - barrel.position).normalized;

        if (Physics.Raycast(barrel.position, direction, out hit, attackRange))
        {
            return hit.transform.CompareTag("Player");
        }

        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        // Обрабатываем только столкновения с основным коллайдером
        if (collision.gameObject.CompareTag("core"))
        {
            HandleHit(collision.gameObject, collision.contacts[0].point);
        }
    }

    // УБИРАЕМ OnTriggerEnter - чтобы бот не умирал от попадания в триггер
    // void OnTriggerEnter(Collider other) 
    // {
    //     // Этот метод удален, чтобы бот не реагировал на попадания в SphereCollider
    // }

    void HandleHit(GameObject bullet, Vector3 hitPoint)
    {
        Debug.Log("ПОПАДАНИЕ ПО БОТУ! В основной коллайдер.");

        // Воспроизводим анимацию взрыва
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, hitPoint, Quaternion.identity);
        }

        // Увеличиваем счетчик попаданий
        hitCount++;
        Debug.Log($"Попадание #{hitCount} по боту");

        // Уничтожаем снаряд
        Destroy(bullet);

        // Уменьшаем жизни
        if (currentLife > 0)
        {
            currentLife--;
        }
    }

    void UpdateTankAppearance()
    {
        if (tankRenderer == null) return;

        if (currentLife == 3)
        {
            if (originalMaterial != null)
                tankRenderer.material = originalMaterial;
        }
        else if (currentLife == 2 && damagedMaterial1 != null)
        {
            tankRenderer.material = damagedMaterial1;
        }
        else if (currentLife == 1 && damagedMaterial2 != null)
        {
            tankRenderer.material = damagedMaterial2;
        }
        else if (currentLife <= 0)
        {
            if (destroyedMaterial != null)
            {
                tankRenderer.material = destroyedMaterial;
            }
            else
            {
                Material blackMat = new Material(Shader.Find("Standard"));
                blackMat.color = Color.black;
                tankRenderer.material = blackMat;
            }
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("БОТ УМИРАЕТ ПОСЛЕ " + hitCount + " ПОПАДАНИЙ!");

        // Создаем взрыв при смерти
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        StopAllCoroutines();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (detectionCollider != null)
            detectionCollider.enabled = false;

        UpdateTankAppearance();

        MonoBehaviour[] allComponents = GetComponents<MonoBehaviour>();
        foreach (var component in allComponents)
        {
            if (component != this && component.enabled)
            {
                component.enabled = false;
            }
        }

        MonoBehaviour[] childComponents = GetComponentsInChildren<MonoBehaviour>();
        foreach (var component in childComponents)
        {
            if (component != this && component.enabled)
            {
                component.enabled = false;
            }
        }

        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Debug.Log("Бот уничтожен!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (detectionCollider != null ? detectionCollider.center : Vector3.zero),
            detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minComfortDistance);

        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}