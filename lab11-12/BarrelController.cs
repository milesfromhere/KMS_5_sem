using UnityEngine;

public class BarrelController : MonoBehaviour
{
    [Header("Настройки выстрела")]
    public GameObject bulletPrefab;         
    public float barrelLength = 2.0f;        
    public float reloadTime = 1.0f;          

    [Header("Звуки")]
    public AudioClip shootSound;             // Звук выстрела

    private AudioSource audioSource;
    private float lastShotTime;              // Время последнего выстрела
    private bool isReloading = false;        // Флаг перезарядки

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        lastShotTime = -reloadTime; // Чтобы можно было стрелять сразу
    }

    void Update()
    {
        // Проверка нажатия пробела и готовности к выстрелу
        if (Input.GetKeyDown(KeyCode.Space) && !isReloading)
        {
            Shoot();
        }

        // Обновление статуса перезарядки
        isReloading = Time.time - lastShotTime < reloadTime;
    }

    void Shoot()
    {
        if (bulletPrefab != null)
        {
            // Расчет точки появления снаряда
            Vector3 spawnPosition = transform.position + transform.forward * barrelLength;

            // Используем поворот ствола без дополнительных поворотов
            Quaternion bulletRotation = transform.rotation;

            // Создание экземпляра снаряда
            Instantiate(bulletPrefab, spawnPosition, bulletRotation);

            // Воспроизведение звука выстрела
            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound);
            }

            // Обновление времени последнего выстрела
            lastShotTime = Time.time;

            Debug.Log("Выстрел! Следующий через: " + reloadTime + " сек.");
        }
        else
        {
            Debug.LogWarning("Префаб снаряда не назначен!");
        }
    }


    // Метод для проверки статуса перезарядки
    public bool IsReloading()
    {
        return isReloading;
    }
}