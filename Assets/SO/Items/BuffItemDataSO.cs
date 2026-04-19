using UnityEngine;

public abstract class BuffItemDataSO : ItemDataSO
{
    public float duration;
    private void OnValidate()
    {
        itemType=ItemType.Buff;
    }
}
