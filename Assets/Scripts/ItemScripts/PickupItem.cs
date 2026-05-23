using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    private PlayerAudio playerAudio;
    void Start()
    {
        playerAudio = GetComponentInChildren<PlayerAudio>();
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item")) 
        {
            Item item = collision.GetComponent<Item>();
            inventory.AddItem(item.itemData);
            playerAudio.PlaySFXClip(PlayerSFX.PickupItem);
            Destroy(item.gameObject);
        }
    }
}
