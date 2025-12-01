using UnityEngine;

public class RedLightTrigger : MonoBehaviour
{
    public Light redLight;

    void Start()
    {
        // Автопоиск красного света
        if (redLight == null)
        {
            redLight = GameObject.Find("RedPointLight")?.GetComponent<Light>();
        }

        // Выключаем свет в начале
        if (redLight != null)
        {
            redLight.intensity = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TankControllerFixed>() != null)
        {
            if (redLight != null)
            {
                redLight.intensity = 10f;
                Debug.Log("Красный свет ВКЛЮЧЕН");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TankControllerFixed>() != null)
        {
            if (redLight != null)
            {
                redLight.intensity = 0f;
                Debug.Log("Красный свет ВЫКЛЮЧЕН");
            }
        }
    }
}