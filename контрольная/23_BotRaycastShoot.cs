/// Задание 23: Обработка попадания "луча" от башни танка-бота в танк-игрока с запуском выстрела
/// Использует Raycast для обнаружения цели и выстрела
public class BotRaycastShoot : MonoBehaviour
{   public Transform firePoint; // Точка выстрела (на башне)
    public float raycastDistance = 50f; // Дальность луча
    public float shootCooldown = 2f; // Задержка между выстрелами
    public GameObject shellPrefab;
    public float fireForce = 1000f;
    private float lastShootTime = 0f;
    void Update()
    {   if (Time.time - lastShootTime >= shootCooldown)// Проверяем, можем ли стрелять (прошла ли задержка)
            { RaycastHit hit;// Пускаем луч от башни вперед
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, raycastDistance))
            {if (hit.collider.CompareTag("Player")) // Если луч попал в игрока
                {Shoot(); lastShootTime = Time.time;}}}}
    void Shoot()    {   if (shellPrefab != null && firePoint != null)
        {   GameObject shell = Instantiate(shellPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = shell.GetComponent<Rigidbody>();
            if (rb != null) {  rb.AddForce(firePoint.forward * fireForce);}}}}

