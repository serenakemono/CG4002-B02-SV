using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Player opponent;

    /* score */
    public int score;
    public Text scoreText;

    /* HP */
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    /* shield */
    public int currentShieldHealth;
    public int maxShieldHealth = 30;
    public int currentShieldCount;
    public int maxShieldCount = 3;

    public ShieldHealthBar shieldHealthBar;

    public GameObject shield;

    /* ammo */
    public int maxAmmo = 6;
    public int currentAmmo;

    /* grenade */
    public int maxGrenade = 2;
    public int currentGrenade;

    /* take damage */
    public BloodSplatter bloodSplatter;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = score.ToString();

        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        if (currentShieldHealth > 0)
        {
            shield.SetActive(true);
        }
        else
        {
            shield.SetActive(false);
        }
    }

    void Spawn()
    {
        UpdateHealth(maxHealth);
        UpdateAmmo(maxAmmo);
        UpdateGrenade(maxGrenade);
        UpdateShield(maxShieldCount);
        UpdateShieldHealth(0);
    }

    public void UpdateHealth(int health)
    {
        currentHealth = health;
        healthBar.SetHealth(health);
    }

    public void UpdateAmmo(int ammo)
    {
        currentAmmo = ammo;
    }

    public void UpdateGrenade(int grenade)
    {
        currentGrenade = grenade;
    }

    public void UpdateShield(int shield)
    {
        currentShieldCount = shield;
    }

    public void UpdateShieldHealth(int shieldHp)
    {
        currentShieldHealth = shieldHp;
        if (shieldHealthBar != null)
        {
            shieldHealthBar.SetShieldHealth(shieldHp);
        }
    }

    public void TakeDamage()
    {
        // has not shielded
        if (currentShieldHealth <= 0)
        {
            bloodSplatter.SetIsShowing(true);
        }

        else
        {
            // TODO: show shield is hit effect
        }
    }
}
