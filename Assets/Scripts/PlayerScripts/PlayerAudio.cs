using System.Collections.Generic;
using UnityEngine;
public enum PlayerSFX
{
    Jump,
    Land,
    Hurt,
    Slash,
    PushStone,
    PickupItem
}
public class PlayerAudio : MonoBehaviour
{
    [System.Serializable]
    public struct FootstepData
    {
        public GroundType groundType;
        public AudioClip clip;
    }

    [SerializeField] private AudioSource oneShotSFXSource;
    [SerializeField] private AudioSource loopSFXSource;

    [field: SerializeField, Range(0, 1)]
    public float sfxVolume { get; private set; }

    [Header("Footstep")]
    [SerializeField] private FootstepData[] footstepDatas;
    Dictionary<GroundType, AudioClip> footstepDict;

    [SerializeField] private AudioClip landingClip;
    [SerializeField] private AudioClip jumpClip;

    [SerializeField] private AudioClip pushStoneClip;
    [SerializeField] private AudioClip slashClip;
    [SerializeField] private AudioClip hurtClip;

    [SerializeField] private AudioClip pickupItemClip;

    private void Awake()
    {
        footstepDict = new Dictionary<GroundType, AudioClip>();

        foreach (var data in footstepDatas)
        {
            footstepDict[data.groundType] = data.clip;
        }
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void PlaySFXClip(PlayerSFX type)
    {
        AudioClip clip = GetClip(type);
        if (clip == null) return;

        oneShotSFXSource.PlayOneShot(clip, sfxVolume);
    }

    private AudioClip GetClip(PlayerSFX type)
    {
        switch (type)
        {
            case PlayerSFX.Jump: return jumpClip;
            case PlayerSFX.Land: return landingClip;
            case PlayerSFX.Hurt: return hurtClip;
            case PlayerSFX.Slash: return slashClip;
            case PlayerSFX.PushStone: return pushStoneClip;
            case PlayerSFX.PickupItem: return pickupItemClip;
        }
        return null;
    }

    public void PlayLoop(PlayerSFX type)
    {
        AudioClip clip = GetClip(type);
        if (clip == null) return;

        if (loopSFXSource.clip == clip && loopSFXSource.isPlaying) return;

        loopSFXSource.clip = clip;
        loopSFXSource.volume = sfxVolume;
        loopSFXSource.Play();
    }

    public void StopLoop()
    {
        loopSFXSource.Stop();
    }
    public void PlayFootstep(GroundType type)
    {
        if (!footstepDict.TryGetValue(type, out AudioClip clip)) return;
        if (clip == null) return;

        oneShotSFXSource.PlayOneShot(clip, sfxVolume);
    }
}
