/// Задание 9: Смена цвета 3D-объекта при щелчке по нему мышью (вариант 2)
/// Требования: Collider на объекте
public class ChangeColorOnClick2 : MonoBehaviour
{
    private Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void OnMouseDown()
    {
        // Генерируем случайный цвет при клике
        rend.material.color = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
    }
}

