using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Message : MonoBehaviour {

    public List<string> message;
    public bool isDone; 

    void DisplayMessage()
    {
        if (!isDone)
        {
            isDone = true;
            _UIManager.instance.StartConversation(message);
        }
    }

    public void SetIsDone(bool _isDone)
    {
        isDone = _isDone;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            DisplayMessage();
        }
    }
}
