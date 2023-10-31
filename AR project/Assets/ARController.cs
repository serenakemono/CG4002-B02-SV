using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ARController : MonoBehaviour
{
    
    [Header("Game Info")]
    public StateManager stateManager;
    public Player player;
    public Player Opponent;
    public GameObject OpponentObject;
    public GameObject OpponentShield;

    [Header("Shoot and Reload")]
    public GameObject handGun;
    static float shootDelayReset = 1.0f;
    static float reloadDelayReset = 2.0f;
    float delay;

    [Header("Opponent Hit")]
    public GameObject bloodSplatter;
    public GameObject shieldRipples;
    private VisualEffect shieldRipplesVFX;

    [Header("Captain Marvel")]
    public GameObject fist;
    static float punchDelayReset = 2.0f;

    [Header("Throw Grenade")]
    public GrenadeThrower grenadeThrower;

    [Header("Spiderman Web")]
    public WebThrower webThrower;

    [Header("Dr Strange Portal")]
    public PortalSpawner portalSpawner;
    public MagicPortalSpawner magicPortalSpawner;

    [Header("Thor Hammer")]
    public HammerThrower hammerThrower;

    [Header("Wakanda Spear")]
    public SpearThrower spearThrower;

    // Start is called before the first frame update
    void Start()
    {
        handGun.SetActive(false);
        fist.SetActive(false);
        delay = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        grenadeThrower.isTracking = stateManager.isTracking;

        delay -= Time.deltaTime;
        if (delay <= 0.0f)
        {
            handGun.SetActive(false);
            fist.SetActive(false);
        }
    }

    public void Shoot()
    {
        handGun.SetActive(true);
        delay = shootDelayReset;

        if (stateManager.isTracking)
        {
            OpponentHit();
        }
    }

    public void Reload()
    {
        handGun.SetActive(true);
        delay = reloadDelayReset;
    }

    public void Punch()
    {
        fist.SetActive(true);
        delay = punchDelayReset;
    }

    private void OpponentHit()
    {
        if (Opponent.currentShieldHealth <= 0)
        {
            Vector3 pos = OpponentObject.transform.position;
            pos.y += 0.5f;
            Instantiate(bloodSplatter, OpponentObject.transform.position, Quaternion.identity);
        }
        else
        {
            
        }
    }

    public void ThrowGrenade()
    {
        grenadeThrower.ThrowGrenade();
    }

    public void ThrowWeb()
    {
        webThrower.ThrowWeb();
    }

    public void StartPortal()
    {
        portalSpawner.Play();
    }

    public void StartMagicCircle()
    {
        magicPortalSpawner.SpawnMagicCircle();
    }

    public void ThrowHammer()
    {
        hammerThrower.ThrowHammer();
    }

    public void ThrowSpear()
    {
        spearThrower.ThrowSpear();
    }
}
