/// Задание 22: Поворот и движение танка-бота в сторону танка-игрока при достижении дистанции
/// Бот движется к игроку, когда тот находится в определенной дистанции
public class BotMovement : MonoBehaviour
{   public Transform playerTank;
    public float detectionDistance = 10f; // Дистанция обнаружения
    public float moveSpeed = 3f;
    public float rotationSpeed = 60f;
    void Update()
    {
        if (playerTank == null) return;// Вычисляем расстояние до игрока
        float distance = Vector3.Distance(transform.position, playerTank.position);
        // Если игрок в зоне обнаружения
        if (distance <= detectionDistance)
        {   Vector3 direction = playerTank.position - transform.position;// Вычисляем направление к игроку
            direction.y = 0; // Только горизонтальное движение
            // Поворачиваем танк к игроку
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            // Двигаемся к игроку
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);}}}

