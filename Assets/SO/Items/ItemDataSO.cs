using UnityEngine;

public enum ItemType { Heal,Buff,Cure}

public abstract class ItemDataSO : ScriptableObject
{
    public string itemName;
    [TextArea(2, 5)]
    public string itemInfo;
    public ItemType itemType;
    public Sprite itemIcon;

    [Header("Audio")]
    public AudioClip pickUpSFX;
    public AudioClip useSFX;

    public bool UseItem(GameObject player)
    {
        return Use(player);
    }
    protected abstract bool Use(GameObject player);
}
