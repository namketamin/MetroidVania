using UnityEngine;

[CreateAssetMenu(fileName = "CureItem", menuName = "New CureItem")]
public class CureItemDataSO : ItemDataSO
{
    public string poisonType;
    public float time;
    private void OnValidate()
    {
        itemType = ItemType.Cure;
    }
    protected override bool Use(GameObject player)
    {
        var ph = player.GetComponent<PlayerHealth>();
        ph.CurePoison(time);
        return true;
    }
}
