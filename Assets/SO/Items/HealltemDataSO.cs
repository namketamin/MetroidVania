using UnityEngine;

[CreateAssetMenu(fileName = "HealItem", menuName = "New HealItem")]
public class HealItemDataSO : ItemDataSO
{
    public float healAmount;

    private void OnValidate()
    {
        itemType = ItemType.Heal;
    }
    protected override bool Use(GameObject player)
    {
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph.currentHP == ph.maxHP) return true;
        if (ph.currentHP+healAmount > ph.maxHP)
        {
            ph.currentHP = ph.maxHP;
        }
        else
        {
            ph.currentHP += healAmount;
        }
        return true;
    }
}
