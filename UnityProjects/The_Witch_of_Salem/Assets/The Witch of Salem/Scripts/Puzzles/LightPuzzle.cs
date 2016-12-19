using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LightPuzzle : MonoBehaviour {

    LightStone[] stones;
    bool isDone = false;
    public UnityEvent eventWhenDone;

    void Start()
    {
        stones = GetComponentsInChildren<LightStone>();
    }

    public void CheckIfDone()
    {
        if (!isDone)
        {
            int actives = 0;
            foreach (LightStone l in stones)
            {
                if (l.active)
                {
                    actives++;
                }
            }
            if (actives == stones.Length)
            {
                FinishPuzzle();
            }
        }
    }

    public void FinishPuzzle()
    {
        isDone = true;
        eventWhenDone.Invoke();
        foreach(LightStone l in stones)
        {
            l.disable = true;
        }
    }
}
