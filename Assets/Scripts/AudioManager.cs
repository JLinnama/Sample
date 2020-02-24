using UnityEngine;

public class AudioManager : MonoBehaviour
{

    // Controls the audio, other scripts can easily access PlayRequestedClip to play wanted AudioClips

    public static AudioManager instance;

    public AudioSource mainSource;
    public AudioSource turningSource;

    public AudioClip explosion;
    public AudioClip click;
    public AudioClip shoot;
    public AudioClip score;

    private void Awake()
    {
        instance = this;
    }

    public void PlayRequestedClip(AudioClip clip, float volume)
    {
        mainSource.PlayOneShot(clip, volume);
    }

    public void OnClick()
    {
        PlayRequestedClip(click, 1.0f);
    }
}
