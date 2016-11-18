using UnityEngine;
using System.Collections;

public class _Player : _PlayerBaseCombat {

    void Start()
    {
        BaseStart();
    }

    void Update()
    {
        if (baseState != BaseState.CantMove)
        {
            InputHandler();
        }
    }

    void FixedUpdate()
    {
        if (baseState != BaseState.CantMove)
        {
            CheckState();
            Checks();
        }
    }

    void CheckState()
    {
        switch (baseState)
        {
            case BaseState.CantMove:
                break;
            case BaseState.Grounded:
                Grounded();
                break;
            case BaseState.Climbing:
                break;
            case BaseState.Falling:
                break;
        }
    }

    void Grounded()
    {
        Walking();
    }
}
