/// Задание 4: Генерация 3D-объекта со свойствами твердого тела и падением на плоскость
/// Нажмите Space для создания падающего объекта
public class FallingObject : MonoBehaviour
{
    public GameObject prefab; // Префаб объекта для создания
    public KeyCode spawnKey = KeyCode.Space; // Клавиша для создания
    void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            // Создаем объект
            GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
            // Добавляем Rigidbody для физики (твердое тело)
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = obj.AddComponent<Rigidbody>();
            }
        }
    }
}