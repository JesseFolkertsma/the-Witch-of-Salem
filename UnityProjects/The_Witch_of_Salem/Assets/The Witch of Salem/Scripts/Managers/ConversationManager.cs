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

    public GameObject convCanvas;
    public bool isActive = false;

    void Start()
    {
        convCanvas = Instantiate(convCanvas) as GameObject;
        npcText = GameObject.Find("NpcText").GetComponent<Text>();
        convCanvas.SetActive(false);
    }

    public void Play(bool b)
    {
        if (b == true)
        {
            convCanvas.SetActive(true);
            isActive = true;
        }
        else
        {
            convCanvas.SetActive(false);
            isActive = false;
        }
    }

    void LateUpdate()
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
