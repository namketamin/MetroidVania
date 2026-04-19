using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmSource;
    [SerializeField][Range(0, 1)] float themeVolume;
    public AudioClip mainTheme;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        PlayBGM(mainTheme,themeVolume);
    }
    void Update()
    {
        
    }
    public void PlayBGM(AudioClip clip, float volume)
    {
        bgmSource.clip = clip;
        bgmSource.volume = volume;
        bgmSource.Play();
    }
    public void PlaySound(AudioClip clip)
    {

    }
}
