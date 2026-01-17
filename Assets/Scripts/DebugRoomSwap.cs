using UnityEngine;
using UnityEngine.InputSystem;

public class DebugRoomSwap : MonoBehaviour
{
    
    public RoomLoader loader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            loader.LoadRoom("Start", Direction.S);


    }
}
