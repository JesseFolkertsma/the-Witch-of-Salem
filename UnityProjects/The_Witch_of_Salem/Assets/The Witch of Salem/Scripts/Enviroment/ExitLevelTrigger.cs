using UnityEngine;
using System.Collections;

public class ExitLevelTrigger : MonoBehaviour {

    public int nextLevelBuildIndex;

    void ChangeLevel()
    {
        _GameManager.instance.SaveGame();
        Invoke("ChangeLevelFollowup", 4f);
    }

    void ChangeLevelFollowup()
    {
        _GameManager.instance.LoadOtherLevelWithSave(nextLevelBuildIndex);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            ChangeLevel();
        }
    }
}
