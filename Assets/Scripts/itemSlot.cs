using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class itemSlot : MonoBehaviour
{
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public Sprite emptySprite;

    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private Image itemImage;

    public void AddItem(string itemName, int quantity, Sprite sprite)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = sprite;
        isFull = true;

        itemNameText.text = itemName;
        itemNameText.enabled = true;
        itemImage.sprite = sprite;
    }

    public void UpdateSlotUI()
    {
        itemNameText.text = itemName;
        itemImage.sprite = itemSprite;
    }

    public void ClearSlot()
    {
        itemNameText.text = "";
        itemImage.sprite = emptySprite;
        itemNameText.enabled = false;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
