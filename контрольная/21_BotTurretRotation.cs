/// Задание 21: Поворот башни танка-бота в сторону танка-игрока при попадании в триггер
/// Требования: Collider с isTrigger = true на танке-игроке
public class BotTurretRotation : MonoBehaviour
{ public Transform playerTank; // Танк-игрок
    public float rotationSpeed = 30f;
    private bool playerInRange = false;
    void Update()
    {   // Если игрок в зоне, поворачиваем башню в его сторону
        if (playerInRange && playerTank != null)
        {   // Вычисляем направление к игроку
            Vector3 direction = playerTank.position - transform.position;
            direction.y = 0; // Только горизонтальный поворот
            // Плавный поворот башни к игроку
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {   // Проверяем, вошел ли игрок в триггер
        if (other.CompareTag("Player"))
        { playerInRange = true; }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { playerInRange = false; }
    }
}

