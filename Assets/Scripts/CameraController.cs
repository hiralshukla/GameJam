using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    public float followSpeed = 2f;
    public float zPerspective = -10f;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, zPerspective); 

        if(newPos.x < minBounds.x) newPos.x = minBounds.x;
        if(newPos.x > maxBounds.x) newPos.x = maxBounds.x;
        if(newPos.y < minBounds.y) newPos.y = minBounds.y;
        if(newPos.y > maxBounds.y) newPos.y = maxBounds.y;

        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
