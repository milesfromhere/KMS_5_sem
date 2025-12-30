/// Задание 8: Вращение 3D-объекта с помощью мыши
/// Зажмите левую кнопку мыши и двигайте для вращения
public class RotateWithMouse : MonoBehaviour
{   public float rotationSpeed = 5f;
    private bool isDragging = false;
    void Update()
    {   // Начало перетаскивания
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }// Конец перетаскивания
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }// Вращение при перетаскивании
        if (isDragging)
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotY = Input.GetAxis("Mouse Y") * rotationSpeed;
            transform.Rotate(-rotY, rotX, 0);
        }
    }
}

