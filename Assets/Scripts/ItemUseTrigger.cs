using UnityEngine;

public class ItemUseTrigger : MonoBehaviour
{
    [SerializeField] private string requiredItemName = "Key"; // or "BlueGem" etc.
    [SerializeField] private ApplicationScript appScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (appScript.TryUseItem(requiredItemName))
            {
                Debug.Log($"Used {requiredItemName}!");
                // e.g. open a door, play animation, etc.
                Destroy(gameObject); // optional if one-time
            }
            else
            {
                Debug.Log($"You need {requiredItemName}!");
            }
        }
    }
}
