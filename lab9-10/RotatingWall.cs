using UnityEngine;

public class RotatingWall : MonoBehaviour
{
    public float rotationSpeed = 50f; // Скорость вращения

    private bool shouldRotate = false;

    void Update()
    {
        if (shouldRotate)
        {
            // Вращаем стенку
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Простая проверка на танк
        if (other.GetComponent<TankControllerFixed>() != null)
        {
            shouldRotate = true;
            Debug.Log("Танк въехал в триггер - стенка вращается!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Останавливаем вращение когда танк уезжает
        if (other.GetComponent<TankControllerFixed>() != null)
        {
            shouldRotate = false;
            Debug.Log("Танк уехал - стенка остановилась");
        }
    }
}