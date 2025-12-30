/// Задание 11: Обработка входа и выхода 3D-объекта в триггер с изменением цвета
/// Требования: Collider с isTrigger = true
public class TriggerColorChange : MonoBehaviour
{public Color enterColor = Color.green;
    public Color exitColor = Color.red;
    private Renderer rend;
    private Color originalColor;
    void Start()
    {   rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }
    void OnTriggerEnter(Collider other)
    {   // При входе в триггер меняем цвет этого объекта
        rend.material.color = enterColor;
    }
    void OnTriggerExit(Collider other)
    {   // При выходе из триггера возвращаем оригинальный цвет
        rend.material.color = exitColor;
    }
}

