using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMvm : MonoBehaviour
{
    public Rigidbody2D rigBody;
    public float speed = 5f;
    public Animator animator;

    private Vector2 dir;

    public Transform aim;
    bool isWalking = false;
    private Vector2 lastMoveDirection;

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

        if (isWalking)
        {
            Vector3 vector3 = Vector3.left * dir.x + Vector3.down * dir.y;
            aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (((moveX == 0 && moveY == 0) && (dir.x != 0 || dir.y != 0))){
            isWalking = false;
            lastMoveDirection = dir;
            Vector3 vector3 = Vector3.left * lastMoveDirection.x + Vector3.down * lastMoveDirection.y;
            aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
        else if (moveX != 0 || moveY != 0){
            isWalking = true;   
        }
    }

    private void Update()
    {
        ProcessInputs();
    }
}
