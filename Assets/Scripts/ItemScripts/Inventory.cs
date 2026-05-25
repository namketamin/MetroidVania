using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> items = new List<ItemSlot>();
    [SerializeField] private Image currentItemImg;
    [SerializeField] private TextMeshProUGUI currentItemQuantityText;
    [SerializeField] private TextMeshProUGUI itemInfoText;
    public ItemSlot currentItem { get; private set; }   

    [SerializeField] private GameObject player;

    private ItemAudio itemAudio;
    private void Awake()
    {
        itemAudio = GetComponent<ItemAudio>();
    }

    void Start()
    {
        SetCurrentItemUIVisible(false);
    }
    void Update()
    {
        ChangeItem();
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (items.Count == 0) return;
            UseItem();
        }
    }
    public void AddItem(ItemDataSO itemData,int amount = 1)
    {
        if(items.Count == 0)
        {
            SetCurrentItemUIVisible(true);
        }
        ItemSlot slot=items.Find(s=>s.itemData==itemData);
        if (slot != null)
        {
            slot.quantity += amount;
        }
        else
        {
            ItemSlot newSlot=new ItemSlot(itemData,1);
            if (items.Count == 0)
            {
                currentItem = newSlot;
            }
            items.Add(newSlot);
        }
        DisplayItemUI();
    }
    public void RemoveItem(ItemSlot slot)
    { 
        ItemSlot slotRemove = items.Find(s => s.itemData == slot.itemData);
        if (slotRemove == null) return;   
        items.Remove(slotRemove);
    }
    void ChangeItem()
    {
        if(items==null || items.Count == 0) return;
        int i = items.IndexOf(currentItem);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            i=(i-1+items.Count)%items.Count;
            currentItem = items[i];
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            i = (i + 1) % items.Count;
            currentItem = items[i];
        }
        DisplayItemUI();
    }
    public void DisplayItemUI()
    {
        if(currentItem!=null)
        {
            currentItemImg.GetComponent<Image>().sprite = currentItem.itemData.itemIcon;
            itemInfoText.text = currentItem.itemData.itemInfo;
            currentItemQuantityText.text = currentItem.quantity.ToString();
        }
    }
    public void UseItem()
    {
        int removedIndex=items.IndexOf(currentItem);

        if (currentItem.itemData.UseItem(player)) currentItem.quantity --;

        itemAudio.PlayItemSFX(currentItem.itemData.itemType);

        if (currentItem.quantity == 0)
        {
            RemoveItem(currentItem);

            if (items.Count == 0)
            {
                currentItem = null;
                SetCurrentItemUIVisible(false);
            }
            else
            {
                if (removedIndex >= items.Count) removedIndex=items.Count-1;

                currentItem = items[removedIndex];
            }
            DisplayItemUI();
        }
    }

    public void SetCurrentItemUIVisible(bool isVisible)
    {
        currentItemImg.gameObject.SetActive(isVisible);
        currentItemQuantityText.gameObject.SetActive(isVisible);
        itemInfoText.gameObject.SetActive(isVisible);
    }
}
