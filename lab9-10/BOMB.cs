using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [Header("Настройки бомб")]
    public GameObject bombPrefab;
    public KeyCode spawnKey = KeyCode.Space;
    public float spawnAreaSize = 10f;    // Размер зоны падения
    public int bombsCount = 5;           // Количество бомб за один вызов
    public float bombHeight = 20f;       // Высота падения

    [Header("Настройки танка")]
    public float bombOffsetForward = 8f; // Смещение вперед от танка

    void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnBombs();
        }
    }

    void SpawnBombs()
    {
        if (bombPrefab == null)
        {
            Debug.LogError("Bomb prefab not assigned!");
            return;
        }

        // Центр зоны бомбардировки - перед танком
        Vector3 tankPosition = transform.position;
        Vector3 tankForward = transform.forward;
        Vector3 bombZoneCenter = tankPosition + tankForward * bombOffsetForward;

        for (int i = 0; i < bombsCount; i++)
        {
            // Случайная позиция в квадратной зоне
            float randomX = Random.Range(-spawnAreaSize / 2f, spawnAreaSize / 2f);
            float randomZ = Random.Range(-spawnAreaSize / 2f, spawnAreaSize / 2f);

            Vector3 spawnPosition = new Vector3(
                bombZoneCenter.x + randomX,
                bombHeight,
                bombZoneCenter.z + randomZ
            );

            // Создаем бомбу
            Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
        }

        Debug.Log($"Создано {bombsCount} бомб в зоне перед танком!");
    }

    // Визуализация зоны бомбардировки в редакторе
    void OnDrawGizmosSelected()
    {
        Vector3 bombZoneCenter = transform.position + transform.forward * bombOffsetForward;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            new Vector3(bombZoneCenter.x, bombHeight / 2f, bombZoneCenter.z),
            new Vector3(spawnAreaSize, bombHeight, spawnAreaSize)
        );

        // Линия от танка к центру зоны
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, bombZoneCenter);
    }
}