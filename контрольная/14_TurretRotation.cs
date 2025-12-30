/// Задание 14: Управление вращением башни танка
/// Управление: Q/E или стрелки влево/вправо для поворота башни
public class TurretRotation : MonoBehaviour
{
    public float rotationSpeed = 60f;

    void Update()
    {
        // Поворот башни влево/вправо
        float rotate = 0;
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            rotate = -rotationSpeed;
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightArrow))
            rotate = rotationSpeed;

        // Вращаем только по оси Y (горизонтально)
        transform.Rotate(0, rotate * Time.deltaTime, 0);
    }
}

