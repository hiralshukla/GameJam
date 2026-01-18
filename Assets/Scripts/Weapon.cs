using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BatScript enemy = collision.GetComponent<BatScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
