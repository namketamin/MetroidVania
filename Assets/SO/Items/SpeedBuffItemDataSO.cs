using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBuffItem", menuName = "New SpeedBuffItem")]
public class SpeedBuffItemDataSO : BuffItemDataSO
{
    public float speedMutiplier;
    protected override bool Use(GameObject player)
    {
        return true;
    }
}
