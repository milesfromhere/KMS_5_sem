using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Настройки снаряда")]
    public float speed = 10.0f;              
    public float lifeTime = 5.0f;            

    [Header("Эффекты")]
    public GameObject explosionEffect;       
    public float explosionLifeTime = 2.0f;   

    [Header("Звуки")]
    public AudioClip explosionSound;         // Звук взрыва
    public AudioClip hitSound;               // Звук попадания в цель
    [Range(0f, 1f)]
    public float hitSoundVolume = 1.0f;      // Громкость звука попадания

    private Renderer bulletRenderer;
    private Collider bulletCollider;
    private bool isActive = true;
    private AudioSource playerAudioSource;   // Аудиоисточник игрока

    void Start()
    {
        bulletRenderer = GetComponent<Renderer>();
        bulletCollider = GetComponent<Collider>();

        // Находим аудиоисточник игрока (на камере или танке)
        FindPlayerAudioSource();

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (isActive)
        {
            // Движение вперед по локальной оси Z
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return;

        // Обработка столкновения
        HandleCollision(collision.gameObject, collision.contacts[0].point);
    }

    void HandleCollision(GameObject target, Vector3 hitPoint)
    {
        isActive = false;

        // Скрытие снаряда
        if (bulletRenderer != null)
            bulletRenderer.enabled = false;
        if (bulletCollider != null)
            bulletCollider.enabled = false;

        // Проверка на попадание в цель
        if (target.CompareTag("Goal"))
        {
            PlayHitSound();
            Debug.Log("🎯 Попадание в цель: " + target.name);
        }
        else
        {
            PlayExplosionSound(hitPoint);
            Debug.Log("Попадание в объект: " + target.name);
        }

        // Создание эффекта взрыва
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, hitPoint, Quaternion.identity);
            Destroy(explosion, explosionLifeTime);
        }

        // Уничтожение снаряда
        Destroy(gameObject, 2f);
    }

    void FindPlayerAudioSource()
    {
        // Ищем аудиоисточник на главной камере
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            playerAudioSource = mainCamera.GetComponent<AudioSource>();
            if (playerAudioSource == null)
            {
                // Если на камере нет аудиоисточника, создаем его
                playerAudioSource = mainCamera.gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            // Если камеры нет, ищем на игроке
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerAudioSource = player.GetComponent<AudioSource>();
                if (playerAudioSource == null)
                {
                    playerAudioSource = player.AddComponent<AudioSource>();
                }
            }
        }

        if (playerAudioSource == null)
        {
            Debug.LogWarning("Не найден аудиоисточник игрока! Звуки попадания будут воспроизводиться в точке столкновения.");
        }
    }

    void PlayHitSound()
    {
        if (hitSound != null)
        {
            if (playerAudioSource != null)
            {
                // Воспроизводим звук у игрока
                playerAudioSource.PlayOneShot(hitSound, hitSoundVolume);
                Debug.Log("♫ Воспроизведен звук попадания в цель у игрока!");
            }
            else
            {
                // Fallback: воспроизводим в точке столкновения
                AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
                Debug.Log("♫ Воспроизведен звук попадания в цель (fallback)!");
            }
        }
        else if (explosionSound != null)
        {
            if (playerAudioSource != null)
            {
                playerAudioSource.PlayOneShot(explosionSound, hitSoundVolume);
            }
            else
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, hitSoundVolume);
            }
        }
        else
        {
            Debug.LogWarning("Hit Sound не назначен в инспекторе!");
        }
    }

    void PlayExplosionSound(Vector3 position)
    {
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, position);
        }
    }
}