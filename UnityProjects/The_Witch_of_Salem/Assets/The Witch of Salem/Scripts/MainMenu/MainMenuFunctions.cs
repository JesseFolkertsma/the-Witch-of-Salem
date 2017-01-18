using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuFunctions : MonoBehaviour {

    public void NewGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGameButton()
    {
        _GameManager.instance.LoadLevelWithSave();
    }
}
