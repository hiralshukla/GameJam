using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decides where doors lead. 
/// Door mappings are consistent until Reshuffle() is called.
/// </summary>
public class MansionRouter : MonoBehaviour
{
    [Header("Special Rooms")]
    public string startRoomId = "Start";
    public string ballroomRoomId = "Ballroom";

    // Core mapping:
    // (roomId, exitDirection) -> destinationRoomId
    private Dictionary<(string, Direction), string> doorMap =
        new Dictionary<(string, Direction), string>();

    /// <summary>
    /// Called by doors to find the destination room.
    /// </summary>
    public string GetDestinationRoomId(string currentRoomId, Direction exitDirection)
    {
        var key = (currentRoomId, exitDirection);

        if (!doorMap.ContainsKey(key))
        {
            Debug.LogWarning($"[MansionRouter] No mapping for {currentRoomId} {exitDirection}, using fallback.");
            return startRoomId; // safe fallback
        }

        return doorMap[key];
    }

    /// <summary>
    /// Called by GameManager when a key is collected.
    /// Rebuilds door mappings (except anchored rooms).
    /// </summary>
    public void Reshuffle()
    {
        Debug.Log("[MansionRouter] Reshuffling mansion layout...");
        // Teammate implements logic here
    }

    /// <summary>
    /// Optional helper so RoomLoader / GameManager can notify about anchored rooms later.
    /// </summary>
    public void SetRoomAnchored(string roomId, bool anchored)
    {
        // Optional extension
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
