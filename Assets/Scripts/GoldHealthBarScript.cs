using UnityEngine;
using UnityEngine.UI;

public class GoldHealthBarScript : MonoBehaviour
{
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