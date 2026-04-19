using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    void Start()
    {
        
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
            Destroy(item.gameObject);
        }
    }
}
