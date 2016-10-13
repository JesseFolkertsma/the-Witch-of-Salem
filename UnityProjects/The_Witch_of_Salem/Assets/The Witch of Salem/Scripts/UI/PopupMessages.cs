using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupMessages : MonoBehaviour {

    public Text popupText;

    void Start()
    {
        popupText.CrossFadeAlpha(0, 0, false);
    }

    public void DisplayPopup(string text, float time)
    {
        popupText.text = text;
        StartCoroutine(PopupCoroutine(text, time));
    }

    IEnumerator PopupCoroutine (string text, float time)
    {
        popupText.CrossFadeAlpha(1, 1, false);
        yield return new WaitForSeconds(time);
        popupText.CrossFadeAlpha(0, 1, false);
    }

}
