using UnityEngine;
using System.Collections.Generic;

public class RoomState : MonoBehaviour
{
    public bool keyCollected;
    public bool anchored;

    // Unity does NOT serialize HashSet in the inspector, but it works fine at runtime.
    public HashSet<int> litTorches = new HashSet<int>();
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
