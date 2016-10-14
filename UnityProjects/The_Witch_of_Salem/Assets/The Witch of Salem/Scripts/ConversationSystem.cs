using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationSystem : MonoBehaviour {

    Conversation conv = new Conversation();
    int i = 0;

    void Start()
    {
        conv = new ConversationLoader().LoadText("TestNPC.xml");
        //new ConversationLoader().SaveGame();
        GameManager.instance.cm.DisplayText(conv.text[i]);
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && GameManager.instance.cm.writing == false)
        {
            i++;
            if (i <= conv.text.Count - 1)
            {
                GameManager.instance.cm.DisplayText(conv.text[i]);
            }
            else
            {
                print("No more text");
            }
        }
    }
    
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {

        }
    }
}
