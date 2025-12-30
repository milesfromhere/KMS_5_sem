/// Задание 24: Короутина для запуска снаряда танка-бота с задержкой по времени
/// Использует корутину для выстрела с задержкой
public class BotShootCoroutine : MonoBehaviour
{   public GameObject shellPrefab;
    public Transform firePoint;
    public float fireForce = 1000f;
    public float shootDelay = 3f; // Задержка перед выстрелом
    void Start()
    {   StartCoroutine(ShootWithDelay());}// Запускаем корутину выстрела   
    IEnumerator ShootWithDelay()/// Корутина для выстрела с задержкой
    {while (true)
        { yield return new WaitForSeconds(shootDelay); // Ждем заданное время
          Shoot();}} // Выполняем выстрел
    void Shoot()
    { if (shellPrefab != null && firePoint != null)
        { GameObject shell = Instantiate(shellPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = shell.GetComponent<Rigidbody>();
            if (rb != null)
            { rb.AddForce(firePoint.forward * fireForce);}}}}

