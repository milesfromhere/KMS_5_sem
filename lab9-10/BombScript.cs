using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Настройки бомбы")]
    public float destroyDelay = 2f;      // Задержка перед уничтожением после падения
    public GameObject explosionEffect;   // Эффект взрыва (опционально)
    public AudioClip explosionSound;     // Звук взрыва

    private bool hasExploded = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Автоматическое уничтожение через 10 секунд на всякий случай
        Destroy(gameObject, 10f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Взрываемся при столкновении с любым объектом, кроме других бомб
        if (!hasExploded && !collision.gameObject.CompareTag("Bomb"))
        {
            Explode();
        }
    }

    void Explode()
    {
        hasExploded = true;

        // Воспроизводим звук
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        // Создаем эффект взрыва
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Debug.Log("Бомба взорвалась! " + transform.position);

        // Уничтожаем бомбу
        Destroy(gameObject, destroyDelay);

        // Делаем бомбу невидимой сразу
        GetComponent<Renderer>().enabled = false;
        if (GetComponent<Collider>() != null)
            GetComponent<Collider>().enabled = false;
    }
}