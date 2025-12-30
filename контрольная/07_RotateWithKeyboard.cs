/// Задание 7: Вращение 3D-объекта клавишами клавиатуры
/// Q/E - вращение по Y, W/S - по X, A/D - по Z
public class RotateWithKeyboard : MonoBehaviour
{
    public float rotationSpeed = 90f;
    void Update()
    {
        float rotX = 0, rotY = 0, rotZ = 0;
        // Вращение по осям
        if (Input.GetKey(KeyCode.W)) rotX = rotationSpeed;
        if (Input.GetKey(KeyCode.S)) rotX = -rotationSpeed;
        if (Input.GetKey(KeyCode.Q)) rotY = rotationSpeed;
        if (Input.GetKey(KeyCode.E)) rotY = -rotationSpeed;
        if (Input.GetKey(KeyCode.A)) rotZ = rotationSpeed;
        if (Input.GetKey(KeyCode.D)) rotZ = -rotationSpeed;
        // Применяем вращение
        transform.Rotate(rotX * Time.deltaTime, rotY * Time.deltaTime, rotZ * Time.deltaTime);
    }
}

