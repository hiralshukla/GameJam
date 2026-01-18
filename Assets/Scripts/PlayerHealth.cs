using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public RedHealthBarScript healthBar;
    Vector2 startPos;
    public Animator animator;

    void Start()
    {
        startPos = transform.position;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BatScript enemy = collision.GetComponent<BatScript>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            animator.SetBool("isDead", true);
            Die();
        }
    }

    void Die()
    {
        Respawn();
    }

    void Respawn()
    {
        transform.position = startPos;
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
        animator.SetBool("isDead", false);
    }
}