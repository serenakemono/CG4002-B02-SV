using UnityEngine;

public class mqttGameStateController : MonoBehaviour
{
    public string nameController = "Controller for Game State";
    public mqttReceiver _eventSender;
    public Player player1;
    public Player player2;

    public StateManager stateManager;
    public ARController aRController;

    public Animator reloadAnimator;

    public Animator punchAnimator;

    //public mqttReceiver publisher;

    void Start()
    {
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnMessageArrivedHandler(string newMsg)
    {

        Debug.Log("mqttGameStateController: The message from Object " + nameController + " is = " + newMsg);

        MqttServerMsg jsonMsg = JsonUtility.FromJson<MqttServerMsg>(newMsg);
        int player_id = jsonMsg.player_id;
        Debug.Log("Player ID is: " + player_id);

        int p1MsgStart = newMsg.IndexOf("\"p1\": {") + "\"p1\":{".Length;
        string p1Msg = newMsg.Substring(p1MsgStart).Substring(0, newMsg.Substring(p1MsgStart).IndexOf('}') + 1);
        Debug.Log("p1 message: " + p1Msg);
        MqttServerPlayerMsg p1JsonMsg = MqttServerPlayerMsg.CreateFromJson(p1Msg);

        int p2MsgStart = newMsg.IndexOf("\"p2\": {") + "\"p2\":{".Length;
        string p2Msg = newMsg.Substring(p2MsgStart).Substring(0, newMsg.Substring(p2MsgStart).IndexOf('}') + 1);
        Debug.Log("p2 message: " + p2Msg);
        MqttServerPlayerMsg p2JsonMsg = MqttServerPlayerMsg.CreateFromJson(p2Msg);

        jsonMsg.game_state = new MqttServerGameStateMsg(p1JsonMsg, p2JsonMsg);

        if (player_id == stateManager.playerId)
        {
            ShowAREffect(jsonMsg.recognised_action);
        }

        string string_action = ConvertEnumActionToNumAction(jsonMsg.recognised_action);

        if (string_action != "none" && string_action != "gun")
        {
            _eventSender.Publish(string_action, player_id);
        }

        if (stateManager.playerId == 1)
        {
            Debug.Log("Update for Player 1");
            UpdateGameState(player1, p1JsonMsg, p2JsonMsg.deaths);
            UpdateGameState(player2, p2JsonMsg, p1JsonMsg.deaths);
        } else if (stateManager.playerId == 2)
        {
            Debug.Log("Update for Player 2");
            UpdateGameState(player1, p2JsonMsg, p1JsonMsg.deaths);
            UpdateGameState(player2, p1JsonMsg, p2JsonMsg.deaths);
        } else
        {
            Debug.Log("Player ID not set!");
        }
        

    }

    private string ConvertEnumActionToNumAction(string recognised_action)
    {
        switch (recognised_action)
        {
            case "0":
                return "none";
            case "1":
                return "none";
            case "2":
                return "gun";
            case "3":
                return "none";
            case "4":
                return "none";
            case "5":
                return "none";
            case "6":
                return "none";
            case "7":
                return "grenade";
            case "8":
                return "none";
            case "9":
                return "none";
            case "10":
                return "web";
            case "11":
                return "portal";
            case "12":
                return "punch";
            case "13":
                return "hammer";
            case "14":
                return "spear";
            case "15":
                return "none";
            default:
                return "invalid action";
        }
    }

    private void ShowAREffect(string action)
    {
        Debug.Log("Action is " + action);
        switch (action)
        {
            case "1":
                stateManager.DisplayActionMissedInfo(1);
                break;

            case "2":
                aRController.Shoot();
                break;

            case "3":
                stateManager.DisplayActionMissedInfo(3);
                break;

            case "4":
                stateManager.DisplayActionMissedInfo(4);
                break;

            case "5":
                stateManager.ActivatePlayerShield();
                break;

            case "6":
                stateManager.DisplayActionMissedInfo(6);
                break;

            case "7":
                if (stateManager.isTracking)
                {
                    aRController.ThrowGrenade();
                }
                else
                {
                    stateManager.DisplayActionMissedInfo(7);
                }
                break;

            case "8":
                aRController.Reload();
                reloadAnimator.SetTrigger("Reload");
                break;

            case "9":
                stateManager.DisplayActionMissedInfo(9);
                break;

            case "10":
                if (stateManager.isTracking)
                {
                    aRController.ThrowWeb();
                }
                else
                {
                    stateManager.DisplayActionMissedInfo(10);
                }
                break;

            case "11":
                if (stateManager.isTracking)
                {
                    aRController.StartMagicCircle();
                }
                else
                {
                    stateManager.DisplayActionMissedInfo(11);
                }
                break;

            case "12":
                if (stateManager.isTracking)
                {
                    aRController.Punch();
                    punchAnimator.SetTrigger("Punch");
                }
                else
                {
                    stateManager.DisplayActionMissedInfo(12);
                }
                break;

            case "13":
                if (stateManager.isTracking)
                {
                    aRController.ThrowHammer();
                }
                else
                {
                    stateManager.DisplayActionMissedInfo(13);
                }
                break;

            case "14":
                if (stateManager.isTracking)
                {
                    aRController.ThrowSpear();
                }
                else
                {
                    stateManager.DisplayActionMissedInfo(14);
                }
                break;

            case "15":
                //stateManager.hasLoggedOut = true;
                stateManager.DisplayActionMissedInfo(15);
                Debug.Log("A logout action is received!");
                break;

            default:
                Debug.Log("Action received is not recognised: " + action);
                break;
        }
    }

    private void UpdateGameState(Player player, MqttServerPlayerMsg msg, int score)
    {
        player.score = score;
        player.UpdateHealth(msg.hp);
        player.UpdateAmmo(msg.bullets);
        player.UpdateGrenade(msg.grenades);
        player.UpdateShield(msg.shields);
        player.UpdateShieldHealth(msg.shield_hp);
    }

    /* Mqtt message class */
    [SerializeField]
    private class MqttServerPlayerMsg
    {
        public int hp;
        public int bullets;
        public int grenades;
        public int shield_hp;
        public int deaths;
        public int shields;

        public static MqttServerPlayerMsg CreateFromJson(string jsonString)
        {
            return JsonUtility.FromJson<MqttServerPlayerMsg>(jsonString);
        }

    }

    [SerializeField]
    private class MqttServerGameStateMsg
    {
        public MqttServerGameStateMsg(MqttServerPlayerMsg p1, MqttServerPlayerMsg p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public MqttServerPlayerMsg p1;
        public MqttServerPlayerMsg p2;

        public static MqttServerGameStateMsg CreateFromJson(string jsonString)
        {
            return JsonUtility.FromJson<MqttServerGameStateMsg>(jsonString);
        }
    }

    [SerializeField]
    private class MqttServerMsg
    {
        public int player_id;
        public string recognised_action;
        public MqttServerGameStateMsg game_state;
    }
}
