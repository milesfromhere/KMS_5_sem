using UnityEngine;

public class BlueLightTrigger : MonoBehaviour
{
    public Light blueLight;

    void Start()
    {
        // Автопоиск синего света
        if (blueLight == null)
        {
            blueLight = GameObject.Find("BluePointLight")?.GetComponent<Light>();
        }

        // Выключаем свет в начале
        if (blueLight != null)
        {
            blueLight.intensity = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TankControllerFixed>() != null)
        {
            if (blueLight != null)
            {
                blueLight.intensity = 10f;
                Debug.Log("Синий свет ВКЛЮЧЕН");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TankControllerFixed>() != null)
        {
            if (blueLight != null)
            {
                blueLight.intensity = 0f;
                Debug.Log("Синий свет ВЫКЛЮЧЕН");
            }
        }
    }
}