/// Задание 19: Озвучивание события выстрела из ствола танка
/// Воспроизводит звук выстрела
public class ShootSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shootSound; // Звук выстрела

    void Update()
    {
        // Проверяем нажатие кнопки выстрела
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            PlayShootSound();
        }
    }
    void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);}}}