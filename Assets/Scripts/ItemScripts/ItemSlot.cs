using UnityEngine;
[System.Serializable]
public class ItemSlot 
{
    public ItemDataSO itemData;
    public int quantity;

    public ItemSlot(ItemDataSO item, int quantity)
    {
        this.itemData = item;
        this.quantity = quantity;
    }
}
