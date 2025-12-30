/// Задание 16: Реализация выстрела танка с использованием префаба снаряда
/// Нажмите Space или левую кнопку мыши для выстрела
public class TankShoot : MonoBehaviour
{   public GameObject shellPrefab; // Префаб снаряда
    public Transform firePoint; // Точка выстрела (позиция ствола)
    public float fireForce = 1000f;
    void Update()
    {   // Выстрел по нажатию клавиши или кнопки мыши
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {   Shoot();    }
    }
    void Shoot()
    {
        if (shellPrefab != null && firePoint != null)
        {   // Создаем снаряд в точке выстрела
            GameObject shell = Instantiate(shellPrefab, firePoint.position, firePoint.rotation);
            // Добавляем силу для полета снаряда
            Rigidbody rb = shell.GetComponent<Rigidbody>();
            if (rb != null)
            {   rb.AddForce(firePoint.forward * fireForce);}}}}

