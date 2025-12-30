/// Задание 15: Управление вращением ствола танка с ограничениями по углу поворота
/// Управление: W/S или стрелки вверх/вниз для наклона ствола
public class CannonRotation : MonoBehaviour
{    public float rotationSpeed = 30f;
    public float minAngle = -20f; // Минимальный угол наклона
    public float maxAngle = 20f;  // Максимальный угол наклона
    private float currentAngle = 0f;

    void Update()
    {   // Получаем ввод для наклона ствола
        float input = 0;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            input = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            input = -1;
        // Изменяем угол с ограничениями
        currentAngle += input * rotationSpeed * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
        // Применяем поворот по оси X (наклон вверх/вниз)
        transform.localRotation = Quaternion.Euler(currentAngle, 0, 0);
    }
}

