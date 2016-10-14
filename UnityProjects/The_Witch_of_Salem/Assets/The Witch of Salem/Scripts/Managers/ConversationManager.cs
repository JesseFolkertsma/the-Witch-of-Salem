using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ConversationManager : MonoBehaviour {

    public Text npcText;
    public float writeSpeed = 0.035f;
    public bool writing = false;

    string displayText;
    string writingText;

    void Start()
    {
        npcText = GameObject.Find("NpcText").GetComponent<Text>();
        //DisplayText("Ik hou zoooooooooooo veel van pindakaas");
    }

    void Update()
    {
        if(writing == true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                npcText.text = writingText;
                StopAllCoroutines();
                writing = false;
            }
        }
    }

    public void DisplayText (string text)
    {
        npcText.text = "";
        displayText = "";
        writingText = "";
        StartCoroutine(AddLetters(text));
        writingText = text;
    }

    IEnumerator AddLetters(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds(writeSpeed);
            writing = true;
            displayText += text[i];
            npcText.text = displayText;
        }
        writing = false;
    }

}
