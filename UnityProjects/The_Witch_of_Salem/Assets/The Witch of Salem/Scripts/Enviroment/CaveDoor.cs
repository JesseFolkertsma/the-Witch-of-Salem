using UnityEngine;
using System.Collections;

public class CaveDoor : MonoBehaviour {

    bool puzzle1 = false;
    bool puzzle2 = false;
    bool puzzle3 = false;

    public void FinishedPuzzle1()
    {
        puzzle1 = true;
        Check();
    }
    public void FinishedPuzzle2()
    {
        puzzle2 = true;
        Check();
    }
    public void FinishedPuzzle3()
    {
        puzzle3 = true;
        Check();
    }

    void Check()
    {
        if(puzzle1 && puzzle2 && puzzle3)
        {
            GetComponent<Animator>().SetTrigger("OpenDoor");
        }
    }
}
