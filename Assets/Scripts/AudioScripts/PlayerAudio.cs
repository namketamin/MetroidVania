using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioSource pushSFXSource;
    [field:SerializeField,Range(0, 1)] public float sfxVolume;
    [Header("Footstep")]
    public AudioClip grassFootstepClip;
    public AudioClip dirtFootstepClip;

    public AudioClip landingClip;
    public AudioClip jumpClip;

    public AudioClip pushStoneClip;

    public AudioClip slashClip;
    public AudioClip poisonedClip;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void PlaySFXClip(AudioClip clip)
    {
        if (clip.length == 0||clip==null) return;
        sfxSource.PlayOneShot(clip,sfxVolume);
    }
    public void PlayLoopClip(AudioClip clip)
    {
        if (clip.length == 0 || clip == null) return;
        pushSFXSource.clip= clip;
        pushSFXSource.volume= sfxVolume;
        pushSFXSource.Play();
    }
    public void StopLoopClip()
    {
        pushSFXSource.Stop();
    }
    public void PlayFootstep(GroundType type)
    {
        AudioClip clip = type == GroundType.Grass ? grassFootstepClip : dirtFootstepClip;
        PlaySFXClip(clip);
    }
}
