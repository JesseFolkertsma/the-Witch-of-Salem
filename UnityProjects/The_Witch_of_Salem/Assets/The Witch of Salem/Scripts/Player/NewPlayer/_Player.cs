using UnityEngine;
using System.Collections;

public class _Player : _PlayerBaseCombat {



    void Start()
    {
        BaseStart();
        _UIManager.instance.UpdateUI();
    }

    void Update()
    {
        if (baseState != BaseState.CantMove)
        {
            InputHandler();
            CheckState();
            Checks();
        }
    }

    void FixedUpdate()
    {
        //if (baseState != BaseState.CantMove)
        //{
        //    CheckState();
        //    Checks();
        //}
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
            if (combatState == CombatState.Blocking)
            {
                Blocking();
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
                ChargeBow();
                break;
            case CombatState.JumpAttack:
                JumpAttacking();
                break;
        }
    }

    void Grounded()
    {
        Walking();
    }
}
