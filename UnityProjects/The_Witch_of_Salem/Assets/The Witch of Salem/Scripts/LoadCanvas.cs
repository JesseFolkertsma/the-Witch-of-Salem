using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadCanvas : MonoBehaviour {

    public Text text;
    public Image image;

    void Awake()
    {
        image.CrossFadeAlpha(0, 0f, false);
        text.CrossFadeAlpha(0, 0f, false);
    }
    void Start()
    {
        image.CrossFadeAlpha(1, 2f, false);
        text.CrossFadeAlpha(1, 3f, false);
    }
}
