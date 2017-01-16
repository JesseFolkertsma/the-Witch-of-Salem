using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Message : MonoBehaviour {

    public List<string> message;
    bool isDone = false;    

    void DisplayMessage()
    {
        if (!isDone)
        {
            isDone = true;
            _UIManager.instance.StartConversation(message);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            DisplayMessage();
        }
    }
}
