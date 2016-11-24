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

    void LateUpdate()
    {
        if(baseState != BaseState.CantMove)
        {
            if(baseState == BaseState.Hanging)
            {
                ikHandler.UseIKHands(holdPos);
            }
            if(combatState == CombatState.Aiming || combatState == CombatState.ChargeBow)
            {
                ikHandler.UseIKLookat(mouse.GetPosition, .5f);
            }
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
                Climbing();
                break;
            case BaseState.Falling:
                Falling();
                break;
        }
        switch (combatState)
        {
            case CombatState.Aiming:
                AimBow();
                break;
            case CombatState.ChargeBow:
                AimBow();
                break;
        }
    }

    void Grounded()
    {
        Walking();
    }
}
