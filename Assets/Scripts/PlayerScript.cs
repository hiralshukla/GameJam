using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int maxGlow = 100;
    public int currentGlow;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage; 
    }
}
