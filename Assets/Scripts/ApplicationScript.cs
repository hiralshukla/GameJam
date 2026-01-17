using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationScript : MonoBehaviour
{
    public bool MapOpen;
    [SerializeField] protected GameObject TheMap;
    public int numKeys;
    [SerializeField] private TextMeshProUGUI keysText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MapOpen = false;
        TheMap.SetActive(MapOpen);
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
        TheMap.SetActive(MapOpen);
        if (MapOpen) UpdateKeyUI();

    }
}
