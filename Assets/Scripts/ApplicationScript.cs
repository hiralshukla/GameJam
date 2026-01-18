using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationScript : MonoBehaviour
{
    public bool MapOpen;
    [SerializeField] protected GameObject TheMap;
    public int numKeys;
    [SerializeField] private TextMeshProUGUI keysText;
    [SerializeField] private GameObject inventoryMenu;
    public bool inventoryOpen;
    public itemSlot[] itemSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MapOpen = false;
        inventoryOpen = false;
        TheMap.SetActive(MapOpen);
        inventoryMenu.SetActive(inventoryOpen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateKeyUI()
    {
        keysText.text = $"Keys: {numKeys}";
    }

    public void loadMap()
    {
        MapOpen = !MapOpen;
        inventoryOpen = !inventoryOpen;
        TheMap.SetActive(MapOpen);
        inventoryMenu.SetActive(inventoryOpen);
        if (MapOpen) UpdateKeyUI();

    }

    public void AddItem(string itemName, int quantity, Sprite sprite)
    {
       
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, sprite);
                numKeys++;
                return;
            }
        }
        UpdateKeyUI();
    }
}
