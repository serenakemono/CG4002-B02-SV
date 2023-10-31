using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mqttController : MonoBehaviour
{
    public string nameController = "Controller 1";
    //public string tagOfTheMQTTReceiver = "";
    public mqttReceiver _eventSender;
    public Text msg;

    void Start()
    {
        //_eventSender = GameObject.FindGameObjectsWithTag(tagOfTheMQTTReceiver)[0].gameObject.GetComponent<mqttReceiver>();
        //Debug.Log("Event sender: " + _eventSender.name);
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnMessageArrivedHandler(string newMsg)
    {
        msg.text = newMsg;
        Debug.Log("Event Fired. The message, from Object " + nameController + " is = " + newMsg);
    }
}
