/// Задание 10: Обработка столкновения двух 3D-объектов с изменением цвета и текстуры
/// Требования: Collider (без isTrigger) и Rigidbody на объектах
public class CollisionColorTexture : MonoBehaviour
{public Color collisionColor = Color.cyan;
    public Texture collisionTexture; // Текстура для применения при столкновении
    private Renderer rend;
    private Color originalColor;
    private Texture originalTexture;
    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        originalTexture = rend.material.mainTexture;
    }
    void OnCollisionEnter(Collision collision)
    {   // Меняем цвет
        rend.material.color = collisionColor; // Меняем текстуру, если она задана
        if (collisionTexture != null)
        {
            rend.material.mainTexture = collisionTexture;
        }
    }
    void OnCollisionExit(Collision collision)
    {// Возвращаем оригинальные цвет и текстуру
        rend.material.color = originalColor;
        rend.material.mainTexture = originalTexture;
    }
}

