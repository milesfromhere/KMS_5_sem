/// Задание 1: Триггерные функции OnTriggerEnter(), OnTriggerExit(), OnTriggerStay()
/// Требования: Collider с isTrigger = true, Rigidbody на одном из объектов
public class TriggerFunctions : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Вызывается один раз при входе объекта в триггер
        other.GetComponent<Renderer>().material.color = Color.green;
    }
    void OnTriggerStay(Collider other)
    {
        // Вызывается каждый кадр, пока объект в триггере
        other.GetComponent<Renderer>().material.color = Color.yellow;
    }
    void OnTriggerExit(Collider other)
    {
        // Вызывается один раз при выходе объекта из триггера
        other.GetComponent<Renderer>().material.color = Color.red;
    }
}

