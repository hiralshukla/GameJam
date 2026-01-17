using System.Collections.Generic;
using UnityEngine;

public class RoomLoader : MonoBehaviour
{

    [Header("Player")]
    public Transform player;

    [Header("Room Prefabs")]
    public List<roomController> roomPrefabs; // drag prefabs here

    private readonly Dictionary<string, roomController> _prefabById = new();
    private readonly Dictionary<string, RoomState> _stateByRoomId = new();

    private roomController _currentRoom;

    void Awake()
    {
        foreach (var rp in roomPrefabs)
        {
            if (rp == null) continue;
            _prefabById[rp.roomId] = rp;
        }
    }

    public void LoadRoom(string roomId, Direction enteredFrom)
    {
        // Destroy old room
        if (_currentRoom != null)
            Destroy(_currentRoom.gameObject);

        // Spawn new room
        if (!_prefabById.TryGetValue(roomId, out var prefab))
        {
            Debug.LogError($"No room prefab registered for roomId '{roomId}'");
            return;
        }

        _currentRoom = Instantiate(prefab);

        // Ensure state exists
        if (!_stateByRoomId.TryGetValue(roomId, out var state))
        {
            state = new RoomState();
            _stateByRoomId[roomId] = state;
        }

        _currentRoom.ApplyState(state);

        // Place player
        var spawn = _currentRoom.GetEntryPoint(enteredFrom);
        if (spawn != null && player != null)
        {
            player.position = spawn.position;
        }
    }

    // Useful later for key pickup / torch events
    public RoomState GetRoomState(string roomId)
    {
        if (!_stateByRoomId.TryGetValue(roomId, out var state))
        {
            state = new RoomState();
            _stateByRoomId[roomId] = state;
        }
        return state;
    }

    public string GetCurrentRoomId() => _currentRoom != null ? _currentRoom.roomId : "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
