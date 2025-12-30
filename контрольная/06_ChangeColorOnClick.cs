/// Задание 6: Смена цвета 3D-объекта при щелчке по нему мышью
/// Требования: Collider на объекте для обнаружения клика
public class ChangeColorOnClick : MonoBehaviour
{
    private Renderer rend;
    private Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow };
    private int colorIndex = 0;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void OnMouseDown()
    {
        // Вызывается при клике мышью по объекту
        colorIndex = (colorIndex + 1) % colors.Length;
        rend.material.color = colors[colorIndex];
        Debug.Log("Цвет изменен");
    }
}