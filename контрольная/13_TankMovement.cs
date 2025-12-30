/// Задание 13: Управление движением танка по сцене в горизонтальной плоскости
/// Управление: W/S - вперед/назад, A/D - поворот
public class TankMovement : MonoBehaviour
{   public float moveSpeed = 5f;
    public float rotationSpeed = 90f;
    void Update()
    {
        // Движение вперед/назад
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(0, 0, move);

        // Поворот влево/вправо
        float rotate = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotate, 0);
    }
}

