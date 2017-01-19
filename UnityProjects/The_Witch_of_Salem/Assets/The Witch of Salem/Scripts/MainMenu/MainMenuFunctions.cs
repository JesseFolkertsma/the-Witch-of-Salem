using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuFunctions : MonoBehaviour {

    [SerializeField]
    Text noSaveText;

    void Start()
    {
        noSaveText.CrossFadeAlpha(0f, 0f, false);
    }

    public void NewGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGameButton()
    {
        if (!_GameManager.instance.LoadLevelWithSave())
        {
            noSaveText.CrossFadeAlpha(1f, 2f, false);
        }
    }
}
