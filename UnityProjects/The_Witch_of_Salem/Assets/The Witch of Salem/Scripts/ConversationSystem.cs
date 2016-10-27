using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationSystem : MonoBehaviour {

    Conversation conv = new Conversation();

    public string file = "TestNPC";

    public int encounter;

    int i = 0;

    void Start()
    {
        conv = new ConversationLoader().LoadText(file + ".xml");
        //new ConversationLoader().SaveGame();
    }

    void Update()
    {
        if (GameManager.instance.cm.isActive)
        {
            if (Input.GetButtonDown("Fire1") && GameManager.instance.cm.writing == false)
            {
                i++;
                if (i <= conv.text.Count - 1)
                {
                    GameManager.instance.cm.DisplayText(conv.text[i]);
                }
                else
                {
                    GameManager.instance.cm.Play(false);
                    print("No more text");
                }
            }
        }
    }
    
    void OnTriggerEnter(Collider col)
    {
        if(col.attachedRigidbody.tag == "Player" && GameManager.instance.cm.isActive == false && encounter == 0)
        {
            print("IK STA NAAST JE");
            //if (Input.GetButtonDown("E")) {
                print("PRAAT");
                GameManager.instance.cm.Play(true);
                encounter++;
                GameManager.instance.cm.DisplayText(conv.text[i]);
            //}
        }
    }
}
