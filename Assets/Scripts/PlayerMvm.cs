using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMvm : MonoBehaviour
{
    public Rigidbody2D rigBody;
    public float speed = 5f;
    public Animator animator;

    private Vector2 dir;

    void FixedUpdate()
    {
        dir = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) dir.y += 1;
        if (Keyboard.current.sKey.isPressed) dir.y -= 1;
        if (Keyboard.current.aKey.isPressed) dir.x -= 1;
        if (Keyboard.current.dKey.isPressed) dir.x += 1;

        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        rigBody.linearVelocity = dir * speed;

        // Animation logic
        if (dir != Vector2.zero) 
        { 
            animator.SetBool("isRunning", true);
            animator.SetFloat("InputX", dir.x);
            animator.SetFloat("InputY", dir.y);
        } 
        else 
        { 
            animator.SetBool("isRunning", false); 
        }
    }
}
