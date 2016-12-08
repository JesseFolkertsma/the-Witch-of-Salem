using UnityEngine;
using System.Collections;

public class _Player : _PlayerBaseCombat {

    public enum VelocityState
    {
        Standard,
        Clamped,
        EXTREME
    };

    public VelocityState velocityState;

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
        CheckVelocityState();
    }

    void CheckVelocityState()
    {
        switch (velocityState)
        {
            case VelocityState.Clamped:
                if (rb.velocity.x > 5)
                {
                    rb.velocity = new Vector3(5, 0, 0);
                }
                else if (rb.velocity.x < -5)
                {
                    rb.velocity = new Vector3(-5, 0, 0);
                }
                if (rb.velocity.y > 10)
                {
                    rb.velocity = new Vector3(10, 0, 0);
                }
                else if (rb.velocity.x < -10)
                {
                    rb.velocity = new Vector3(-10, 0, 0);
                }
                break;
            case VelocityState.EXTREME:
                rb.velocity = rb.velocity * 2;
                break;
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
