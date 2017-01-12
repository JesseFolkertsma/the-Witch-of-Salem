using UnityEngine;
using System.Collections;

public class CaveDoor : MonoBehaviour {

    public int puzzlesToComplete = 1;

    bool[] completedPuzzles;

    void Start()
    {
        completedPuzzles = new bool[puzzlesToComplete];
    }

    public void FinishPuzzle(int puzzleNbr)
    {
        completedPuzzles[puzzleNbr - 1] = true;
        Check();
    }

    void Check()
    {
        bool allDone = true;

        for(int i = 0; i < completedPuzzles.Length; i++)
        {
            if (!completedPuzzles[i])
            {
                allDone = false;
                break;
            }
        }

        if(allDone)
        {
            GetComponent<Animator>().SetTrigger("OpenDoor");
        }
    }
}
