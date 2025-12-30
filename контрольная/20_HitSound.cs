/// Задание 20: Озвучивание события попадания снаряда в цель
/// Этот скрипт должен быть на объекте, который может быть поражен
public class HitSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hitSound; // Звук попадания
    void OnCollisionEnter(Collision collision)
    {
        // Проверяем, попал ли в нас снаряд
        if (collision.gameObject.CompareTag("Shell") || collision.gameObject.name.Contains("Shell"))
        {
            PlayHitSound();
        }
    }
    void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}

