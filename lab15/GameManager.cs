using UnityEngine;

public class SimpleTankGUI : MonoBehaviour
{
    // Ссылка на скрипт танка
    public TankControllerFixed tankController;

    private bool showPanel = true;
    private float sliderValue = 10f; // Начальное значение, совпадающее с moveSpeed

    void Start()
    {
        if (tankController == null)
        {
            tankController = FindObjectOfType<TankControllerFixed>();
        }

        if (tankController != null)
        {
            sliderValue = tankController.moveSpeed;
        }
    }

    void OnGUI()
    {
        if (tankController == null) return;

        if (!showPanel)
        {
            // Простая кнопка "Показать"
            if (GUI.Button(new Rect(10, 10, 120, 40), "Показать панель"))
            {
                showPanel = true;
            }
            return;
        }

        // Панель управления - простой бокс
        GUI.Box(new Rect(10, 10, 260, 160), "Управление танком");

        // Текст
        GUI.Label(new Rect(30, 40, 200, 30), $"Скорость танка: {tankController.moveSpeed:F1}");

        // Слайдер
        GUI.Label(new Rect(30, 75, 60, 30), "Медленно");
        sliderValue = GUI.HorizontalSlider(new Rect(90, 80, 120, 30), sliderValue, 5f, 20f);
        GUI.Label(new Rect(220, 75, 60, 30), "Быстро");

        // Применяем значение слайдера к танку
        tankController.moveSpeed = sliderValue;

        // Кнопки для быстрой настройки
        if (GUI.Button(new Rect(30, 110, 60, 30), "-1"))
        {
            sliderValue = Mathf.Max(5f, sliderValue - 1f);
            tankController.moveSpeed = sliderValue;
        }

        if (GUI.Button(new Rect(100, 110, 80, 30), "Сброс (10)"))
        {
            sliderValue = 10f;
            tankController.moveSpeed = sliderValue;
        }

        if (GUI.Button(new Rect(190, 110, 60, 30), "+1"))
        {
            sliderValue = Mathf.Min(20f, sliderValue + 1f);
            tankController.moveSpeed = sliderValue;
        }

        // Кнопка "Скрыть"
        if (GUI.Button(new Rect(80, 150, 120, 30), "Скрыть панель"))
        {
            showPanel = false;
        }
    }

    void Update()
    {
        // Горячие клавиши
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showPanel = !showPanel;
        }
    }
}