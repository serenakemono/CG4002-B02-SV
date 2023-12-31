using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public int playerId;
    public int state;
    public GameObject UIPanel;
    public GameObject ARPanel;
    public GameObject EndgamePanel;
    public GameObject InfoControlPanel;
    public GameObject PlayerControlButtons;

    public GameObject InfoPanel;
    private float InfoPanelDelay;
    public TMPro.TMP_Text MissedActionInfoText;

    public EvalServerData evalServerData;
    public Text messageToPublish;
    public mqttReceiver mqttReceiver;

    public bool hasLoggedOut;

    void Start()
    {
        playerId = 1;
        state = 0;
        InfoPanelDelay = 0f;
        hasLoggedOut = false;
        mqttReceiver.isVisible = false;
    }

    void Update()
    {
        mqttReceiver.isVisible = m_isTracking;
        if (m_isTracking)
        {
            trackingInfo.text = "Opponent detected!";
        }
        else
        {
            trackingInfo.text = "No opponent detected...";
        }

        if (state == 1)
        {
            UIPanel.SetActive(true);
            InfoControlPanel.SetActive(true);
        } else
        {
            UIPanel.SetActive(false);
            InfoControlPanel.SetActive(false);
        }

        if (state == 2)
        {
            ARPanel.SetActive(true);
        } else
        {
            ARPanel.SetActive(false);
        }

        if (hasLoggedOut)
        {
            EndgamePanel.SetActive(true);
        }
        else
        {
            EndgamePanel.SetActive(false);
        }

        if (playerId == 1)
        {
            playerIdInfo.text = "PLAYER 1";
        } else if (playerId == 2)
        {
            playerIdInfo.text = "PLAYER 2";
        }

        if (InfoPanelDelay > 0)
        {
            InfoPanel.SetActive(true);
            InfoPanelDelay--;
        } else
        {
            InfoPanel.SetActive(false);
        }
    }

    public void UpdateState()
    {
        state++;
        if (state > 2)
        {
            state = 0;
        }
        Debug.Log("State: " + state);
    }

    public void TogglePlayerControlState()
    {
        PlayerControlButtons.SetActive(!PlayerControlButtons.active);
    }

    /* Player ID setup */
    public Text playerIdInfo;
    public void SetPlayerId(int id)
    {
        playerId = id;
        Debug.Log("Set player id to " + playerId);
    }

    /* Opponent tracker */
    public Text trackingInfo;
    private bool m_isTracking;
    public bool isTracking
    {
        get
        {
            return m_isTracking;
        }
        set
        {
            m_isTracking = value;
            Debug.Log("[State Manager] tracking? " + m_isTracking);
        }
    }

    public Player player;
    public Player opponent;

    /* UI elements */
    public void ReducePlayerHealth(int hp)
    {
        player.UpdateHealth(player.currentHealth - 10);
        player.TakeDamage();
    }

    public void ReduceOpponentHealth(int hp)
    {
        opponent.UpdateHealth(opponent.currentHealth - 10);
    }

    public void ReduceAmmoByOne()
    {
        player.UpdateAmmo(player.currentAmmo - 1);
    }

    public void Reload()
    {
        if (player.currentAmmo <= 0)
        {
            player.UpdateAmmo(player.maxAmmo);
        }
    }

    public void ReduceGrenadeByOne()
    {
        Debug.Log("Current grenade: " + player.currentGrenade);
        player.UpdateGrenade(player.currentGrenade - 1);
    }

    public void ActivatePlayerShield()
    {
        if (player.currentShieldHealth <= 0 && player.currentShieldCount > 0)
        {
            player.UpdateShieldHealth(player.maxShieldHealth);
            player.UpdateShield(player.currentShieldCount - 1);
        }
    }

    public void ReducePlayerShieldHealth(int shieldHp)
    {
        if (player.currentShieldHealth > 0)
        {
            player.UpdateShieldHealth(player.currentShieldHealth - shieldHp);
        }
    }

    public void SetPlayerShieldHpTo0()
    {
        player.UpdateShieldHealth(0);
    }

    public void ActivateOpponentShield()
    {
        if (opponent.currentShieldHealth <= 0 && player.currentShieldCount > 0)
        {
            opponent.UpdateShieldHealth(opponent.maxShieldHealth);
            opponent.UpdateShield(opponent.currentShieldCount - 1);
        }
    }

    public void SetOpponentShieldHpTo0()
    {
        opponent.UpdateShieldHealth(0);
    }

    public void IncreasePlayerScoreByOne()
    {
        player.score++;
    }

    public void IncreaseOpponentScoreByOne()
    {
        opponent.score++;
    }

    public void SimulateEvalServerUpdate()
    {
        PopulateEvalServerData();

        player.UpdateHealth(evalServerData.player.hp);
        player.UpdateShield(evalServerData.player.num_shield);
        player.UpdateShieldHealth(evalServerData.player.hp_shield);
        player.UpdateAmmo(evalServerData.player.num_bullets);
        player.UpdateGrenade(evalServerData.player.num_grenades);
        player.score = evalServerData.opponent.num_deaths;

        opponent.score = evalServerData.player.num_deaths;
        opponent.UpdateHealth(evalServerData.opponent.hp);
        opponent.UpdateShield(evalServerData.opponent.num_shield);
        opponent.UpdateShieldHealth(evalServerData.opponent.hp_shield);
        opponent.UpdateAmmo(evalServerData.opponent.num_bullets);
        opponent.UpdateGrenade(evalServerData.opponent.num_grenades);
    }

    private void PopulateEvalServerData()
    {
        evalServerData.player.hp = 100;
        evalServerData.player.num_shield = 3;
        evalServerData.player.hp_shield = 0;
        evalServerData.player.num_bullets = 6;
        evalServerData.player.num_grenades = 2;
        evalServerData.player.num_deaths = 0;

        evalServerData.opponent.hp = 100;
        evalServerData.opponent.num_shield = 3;
        evalServerData.opponent.hp_shield = 0;
        evalServerData.opponent.num_bullets = 6;
        evalServerData.opponent.num_grenades = 2;
        evalServerData.opponent.num_deaths = 0;
    }

    public void DisplayActionMissedInfo(int action)
    {
        switch (action)
        {
            case 1:
                MissedActionInfoText.SetText("No more bullets.");
                break;
            case 3:
                MissedActionInfoText.SetText("No more shields.");
                break;
            case 4:
                MissedActionInfoText.SetText("Shield has been activated.");
                break;
            case 6:
                MissedActionInfoText.SetText("No more grenades.");
                break;
            case 7:
                MissedActionInfoText.SetText("You threw a grenade\nbut missed ://");
                break;
            case 9:
                MissedActionInfoText.SetText("Cannot reload.\nThere are still bullets.");
                break;
            case 10:
                MissedActionInfoText.SetText("You threw a web\nbut missed ://");
                break;
            case 11:
                MissedActionInfoText.SetText("You activated a portal\nbut missed ://");
                break;
            case 12:
                MissedActionInfoText.SetText("You punched\nbut missed ://");
                break;
            case 13:
                MissedActionInfoText.SetText("You threw a hammer\nbut missed ://");
                break;
            case 14:
                MissedActionInfoText.SetText("You threw a spear\nbut missed ://");
                break;
            case 15:
                MissedActionInfoText.SetText("End of game!! Woohoooo uwu");
                break;
            default:
                MissedActionInfoText.SetText("Oops...");
                break;
        }
        //MissedActionInfoText.SetText("Oops");
        InfoPanelDelay = 60f;
    }
}
