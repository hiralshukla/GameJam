using UnityEngine;

public class TorchLight : MonoBehaviour
{
    public bool lightOn = false;
    public Transform player;
    public float interactDistance = 3f;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        // Force off for testing (optional)
        lightOn = false;

        if (mainCam == null)
            Debug.LogError("[TorchLight] Camera.main is NULL. Tag your camera as MainCamera.");
    }

    void Update()
    {
        if (lightOn) return;
        if (mainCam == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);

            // OverlapPoint is often simpler than raycast for clicking 2D objects
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);

            if (hit == null)
            {
                Debug.Log("[TorchLight] Clicked, but hit NOTHING.");
                return;
            }

            Debug.Log($"[TorchLight] Clicked hit: {hit.gameObject.name}");

            // Accept clicks on this object OR its children
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                if (player != null && Vector2.Distance(player.position, transform.position) > interactDistance)
                {
                    Debug.Log("[TorchLight] Too far to interact.");
                    return;
                }

                TurnOn();
            }
        }
    }

    private void TurnOn()
    {
        lightOn = true;
        Debug.Log($"[TorchLight] Torch turned ON (bool set true): {gameObject.name}");
    }
}



