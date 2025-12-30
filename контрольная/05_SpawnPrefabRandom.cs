/// Задание 5: Генерация объекта из Prefab в случайной позиции на плоскости
/// Нажмите Space для создания объекта в случайном месте
public class SpawnPrefabRandom : MonoBehaviour
{
    public GameObject prefab; // Префаб для создания
    public KeyCode spawnKey = KeyCode.Space;
    public float spawnArea = 10f; // Размер области спавна

    void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            // Генерируем случайную позицию на плоскости X-Z
            float x = Random.Range(-spawnArea, spawnArea);
            float z = Random.Range(-spawnArea, spawnArea);
            Vector3 randomPos = new Vector3(x, 0, z);
            // Создаем объект в случайной позиции
            Instantiate(prefab, randomPos, Quaternion.identity);
        }
    }
}

