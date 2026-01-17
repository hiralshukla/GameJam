using System.Collections.Generic;
using UnityEngine;

public enum Direction { N, E, S, W }

public class roomController : MonoBehaviour
{
    [Header("Room Identity")]
    public string roomId; // e.g. "Start", "LibraryA"

    [Header("Entry Spawn Points")]
    public Transform enterFromN;
    public Transform enterFromE;
    public Transform enterFromS;
    public Transform enterFromW;

    [Header("Optional groups (for organization)")]
    public GameObject doorsRoot;
    public GameObject torchesRoot;
    public GameObject chestsRoot;

    // You can expand later to store torch/chest components, etc.
    public Transform GetEntryPoint(Direction enteredFrom)
    {
        return enteredFrom switch
        {
            Direction.N => enterFromN,
            Direction.E => enterFromE,
            Direction.S => enterFromS,
            Direction.W => enterFromW,
            _ => enterFromS
        };
    }

        // Called by RoomLoader after instantiation
    public void ApplyState(RoomState state)
    {
        // Later: light torches, disable collected key chest, etc.
        // For now: just a placeholder
        // Debug.Log($"Applying state for {roomId}: keyCollected={state.keyCollected}");
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

[System.Serializable]
public class RoomState
{
    public bool keyCollected;
    public bool anchored;
    public HashSet<int> litTorches = new HashSet<int>();
}