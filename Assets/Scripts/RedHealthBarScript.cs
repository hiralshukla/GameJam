using UnityEngine;
using UnityEngine.UI; 

public class RedHealthBarScript : MonoBehaviour
{
    public int maxHealth;
    public Slider slider;

    public void SetHealth(int health)
    {
        slider.value = health; 
    }

    public void SetMaxHealth(int health)
    {
        slider.value = health;
        slider.maxValue = health; 
    }

}
