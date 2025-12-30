/// Задание 17: Префаб снаряда для обработки полета и попадания снаряда в цель
/// Этот скрипт должен быть на префабе снаряда
public class ShellBehavior : MonoBehaviour
{
    public float lifetime = 5f; // Время жизни снаряда
    public GameObject explosionEffect; // Эффект взрыва (опционально)
    void Start()
    {
        // Уничтожаем снаряд через заданное время
        Destroy(gameObject, lifetime);
    }
    void OnCollisionEnter(Collision collision)
    {   // Создаем эффект взрыва, если он задан
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        
        // Уничтожаем снаряд при попадании
        Destroy(gameObject);
    }
}

