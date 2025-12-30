/// Задание 18: Озвучивание событий движения танка
/// Воспроизводит звук при движении танка
public class TankMovementSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip movementSound; // Звук движения (гул двигателя)
    void Update()
    {   // Проверяем, движется ли танк
        bool isMoving = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
        if (isMoving)
        {   // Воспроизводим звук движения, если он не играет
            if (audioSource != null && movementSound != null && !audioSource.isPlaying)
            {
                audioSource.clip = movementSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {   // Останавливаем звук, если танк не движется
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();}}}}

