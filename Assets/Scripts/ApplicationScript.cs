using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationScript : MonoBehaviour
{
    public bool MapOpen;
    [SerializeField] protected GameObject TheMap;

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


    public void loadMap()
    {
        MapOpen = !MapOpen;
        TheMap.SetActive(MapOpen);
    }
}
