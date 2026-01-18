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

    public GameObject keyItemPrefab;

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

    void Awake()
{
        // Wire chests to this room
    foreach (var chest in GetComponentsInChildren<Chest>(true))
    {
        chest.room = this;
    }

    var loader = FindFirstObjectByType<RoomLoader>();
    var router = FindFirstObjectByType<MansionRouter>(); // or DebugDestinationProvider for now

    foreach (var door in GetComponentsInChildren<Door>(true))
    {
        door.room = this;
        door.loader = loader;
        door.destinationProviderComponent = router;
    }

    if (enterFromN == null || enterFromE == null || enterFromS == null || enterFromW == null)
        Debug.LogWarning($"[roomController] Missing entry spawn points in room '{roomId}' on '{gameObject.name}'.");

}
}
