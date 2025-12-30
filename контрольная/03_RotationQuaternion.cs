/// Задание 3: Вращение 3D-объекта с помощью кватернионов Quaternion вокруг произвольной оси
/// Кватернионы используются для плавного вращения без проблем с углами Эйлера
public class RotationQuaternion : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Ось вращения (по умолчанию вверх)
    public float rotationSpeed = 90f; // Скорость вращения в градусах в секунду
    void Update()
    {   // Создаем кватернион для поворота вокруг заданной оси
        Quaternion rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, rotationAxis);
        // Применяем поворот к текущей ориентации объекта
        transform.rotation = rotation * transform.rotation;
    }
}

