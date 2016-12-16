using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RunePuzzle : MonoBehaviour {

    public UnityEvent eventWhenCompleted;
    RuneCircle[] circles;

    bool completed = false;

    void Start()
    {
        circles = GetComponentsInChildren<RuneCircle>();
    }

    void Update()
    {
        if (!completed)
        {
            if (circles[0].StanceIsRight && circles[1].StanceIsRight && circles[2].StanceIsRight)
            {
                FinishPuzzle();
            }
        }
    }

    public void FinishPuzzle()
    {
        completed = true;
        eventWhenCompleted.Invoke();
        foreach(RuneCircle r in circles)
        {
            r.disable = true;
        }
    }
}
