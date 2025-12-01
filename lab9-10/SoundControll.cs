using UnityEngine;

public class KeySoundPlayerHold : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip soundClip;
    public float volume = 1.0f;

    private AudioSource audioSource;
    private bool isPlaying = false;

    void Start()
    {
        // Создаем и настраиваем AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true; 

        if (soundClip != null)
        {
            audioSource.clip = soundClip;
        }

        audioSource.volume = volume;
    }

    void Update()
    {
        CheckKey(KeyCode.A);
        CheckKey(KeyCode.W);
        CheckKey(KeyCode.S);
        CheckKey(KeyCode.D);
    }

    void CheckKey(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            StartSound();
        }

        if (Input.GetKeyUp(key) && isPlaying)
        {
            StopSound();
        }
    }

    void StartSound()
    {
        if (audioSource != null && soundClip != null && !isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }

    void StopSound()
    {
        if (audioSource != null && isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
}