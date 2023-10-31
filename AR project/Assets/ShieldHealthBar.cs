using UnityEngine;
using UnityEngine.UI;

public class ShieldHealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Image border;
    public Image shield;
    public Player player;

    void Update()
    {
        if (player.currentShieldHealth > 0)
        {
            fill.enabled = true;
            border.enabled = true;
            shield.enabled = true;
        }
        else
        {
            fill.enabled = false;
            border.enabled = false;
            shield.enabled = false;
        }
    }

    public void SetShieldHealth(int health)
    {
        slider.value = health;
    }
}
