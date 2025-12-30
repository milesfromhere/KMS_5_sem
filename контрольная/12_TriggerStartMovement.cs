/// Задание 12: Запуск движения 3D-объекта при попадании другого объекта в триггер
/// Требования: Collider с isTrigger = true на триггере
public class TriggerStartMovement : MonoBehaviour
{   public GameObject objectToMove; // Объект, который нужно двигать
    public float moveSpeed = 5f;
    public Vector3 moveDirection = Vector3.forward;
    private bool shouldMove = false;
    void Update()
    {   // Двигаем объект, если триггер активирован
        if (shouldMove && objectToMove != null)
        {
            objectToMove.transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    { shouldMove = true;// При входе объекта в триггер запускаем движение
    }
    void OnTriggerExit(Collider other)
    {   shouldMove = false;// Останавливаем движение при выходе
    }
}

