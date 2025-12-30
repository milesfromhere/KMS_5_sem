/// Задание 2: Движение 3D-объекта в горизонтальной плоскости X-Z
/// Управление: WASD или стрелки клавиатуры
public class MovementXZ : MonoBehaviour
{   public float speed = 5f;
    private float fixedY;
    void Start()
    {
        fixedY = transform.position.y; // Сохраняем высоту
    }
    void Update()
    {
        // Получаем ввод с клавиатуры
        float h = Input.GetAxis("Horizontal"); // A/D
        float v = Input.GetAxis("Vertical");   // W/S
        // Движение только по X и Z, Y остается фиксированным
        Vector3 move = new Vector3(h, 0, v) * speed * Time.deltaTime;
        transform.position += move;
        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }
}

