using UnityEngine;
using UnityEngine.InputSystem;

public class DebugRoomSwap : MonoBehaviour
{
     public RoomLoader loader;

    [Header("Test Room Ids")]
    public string room1 = "Start";
    public string room2 = "TestA";

    void Update()
    {
        if (Keyboard.current == null || loader == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Debug.Log($"[Debug] Loading {room1}");
            loader.LoadRoom(room1, Direction.S);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Debug.Log($"[Debug] Loading {room2}");
            loader.LoadRoom(room2, Direction.N);
        }
    }
}
