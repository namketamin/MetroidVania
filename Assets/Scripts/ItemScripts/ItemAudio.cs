using UnityEngine;

public class ItemAudio : MonoBehaviour
{
    public AudioClip healSFX;
    public AudioClip buffSFX;
    public AudioClip cureSFX;

    private Inventory inventory;

    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }
    public AudioClip GetClip(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Heal:
                return healSFX;

            case ItemType.Buff:
                return buffSFX;

            case ItemType.Cure:
                return cureSFX;
        }

        return null;
    }
    public void PlayItemSFX(ItemType itemType)
    {
        AudioClip clip =GetClip(itemType);

        PlaySFX(clip);
    }

    private void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
