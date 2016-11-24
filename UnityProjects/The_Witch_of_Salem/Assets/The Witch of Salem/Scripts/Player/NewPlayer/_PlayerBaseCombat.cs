using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _PlayerBaseCombat : _PlayerBase {

    public enum CombatState
    {
        Staggered,
        JumpAttack,
        Rolling,
        Blocking,
        Aiming,
        ChargeBow,
        Attacking,
        PierceAttack,
        Idle
    };
    public enum Weapon
    {
        Unarmed,
        Sword,
        Bow
    };

    public CombatState combatState;
    public Weapon currentWeapon;

    public float swordDamage;
    public float arrowDamage;
    public int comboInt;

    public bool attacking;
    public bool waitForNextAttack;
    bool canDoDamage;
    bool canRoll = true;
    bool rolling;

    Coroutine waitAttackRoutine;

    public override void InputHandler()
    {
        base.InputHandler();
        if (Input.GetButtonDown("Fire1"))
        {
            LeftMouseDown();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            LeftMouseUp();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            RightMouseDown();
        }
        if (Input.GetButtonUp("Fire2"))
        {
            RightMouseUp();
        }
        if (Input.GetButtonDown("Fire3"))
        {
            SwitchWeapons();
        }
        if (Input.GetButtonDown("Control"))
        {
            CombatRoll();
        }

        //Animator parameters
        anim.SetInteger("Combo", comboInt);
        anim.SetBool("Attacking", attacking);
        anim.SetBool("Rolling", rolling);
    }

    public override void ResetStates()
    {
        base.ResetStates();
        combatState = CombatState.Idle;
        canDoDamage = false;
        attacking = false;
        waitForNextAttack = false;
        useRootMovement = true;
        canRoll = true;
        rolling = false;
    }

    public override void Jump()
    {
        base.Jump();
        ExitComboLoop();
        combatState = CombatState.Idle;
    }

    public virtual void TakeDamage(int damage)
    {
        lives -= damage;
    }

    public void LeftMouseDown()
    {
        if (currentWeapon == Weapon.Sword)
        {
            if (baseState == BaseState.Grounded)
            {
                if (combatState == CombatState.Idle)
                {
                    ComboAttack(Random.Range(1, 4));
                }
                if (combatState == CombatState.Attacking && waitForNextAttack)
                {
                    ComboAttack(comboInt + 1);
                }
            }
            else if (baseState == BaseState.Falling)
            {
                if (combatState == CombatState.Idle)
                {
                    JumpAttack();
                }
            }
        }
        else if (currentWeapon == Weapon.Bow)
        {
            if (combatState == CombatState.Aiming)
            {
                combatState = CombatState.ChargeBow;
            }
        }
    }

    public void LeftMouseUp()
    {
        if(combatState == CombatState.ChargeBow)
        {
            ShootArrow();
            combatState = CombatState.Aiming;
        }
    }

    public void RightMouseDown()
    {
        if(currentWeapon == Weapon.Bow)
        {
            combatState = CombatState.Aiming;
        }
    }

    public void RightMouseUp()
    {
        if(combatState == CombatState.Aiming || combatState == CombatState.ChargeBow)
        {
            combatState = CombatState.Idle;
        }
    }

    public void AimBow()
    {
        Move(2f, true);
    }

    public void ChargeBow()
    {
        AimBow();
    }

    public void ShootArrow()
    {

    }

    public void JumpAttack()
    {

    }

    public void ComboAttack(int combo)
    {
        if (waitAttackRoutine != null)
        {
            StopCoroutine(waitAttackRoutine);
        }
        waitForNextAttack = false;
        useRootMovement = false;

        if(xInput > .1f)
        {
            TurnPlayer(false, 1);
        }
        else if(xInput < -.1f)
        {
            TurnPlayer(false, -1);
        }

        if(combo > 3)
        {
            combo = 1;
        }
        comboInt = combo;
        combatState = CombatState.Attacking;
        attacking = true;
        anim.SetTrigger("Attack");
    }

    public void SwitchWeapons()
    {
        if(currentWeapon == Weapon.Sword)
        {
            currentWeapon = Weapon.Bow;
        }
        else if (currentWeapon == Weapon.Bow)
        {
            currentWeapon = Weapon.Sword;
        }
    }

    public void StartAttackEvent()
    {
        canDoDamage = true;
        hits.Clear();
    }

    public void StopAttackEvent()
    {
        canDoDamage = false;
    }

    public void WaitForNextAttackEvent()
    {
        if (waitAttackRoutine != null)
        {
            StopCoroutine(waitAttackRoutine);
        }
        waitAttackRoutine  = StartCoroutine(WaitForAttack());
    }

    public void ExitComboLoop()
    {
        canDoDamage = false;
        attacking = false;
        waitForNextAttack = false;
        useRootMovement = true;
    }

    IEnumerator WaitForAttack()
    {
        waitForNextAttack = true;
        yield return new WaitForSeconds(1f);
        ExitComboLoop();
        combatState = CombatState.Idle;
    }

    public void CombatRoll()
    {
        if(baseState == BaseState.Grounded && canRoll)
        {
            StartCoroutine(Rolling());
            ExitComboLoop();
            combatState = CombatState.Rolling;
        }
    }

    IEnumerator Rolling()
    {
        rb.velocity += Vector3.right * xInput * 3;
        rolling = true;
        canRoll = false;
        anim.SetTrigger("Roll");
        yield return new WaitForSeconds(1);
        rolling = false;
        yield return new WaitForSeconds(1);
        canRoll = true;
        combatState = CombatState.Idle;
    }

    List<Enemy> hits = new List<Enemy>();
    void OnTriggerEnter(Collider col)
    {
        if (canDoDamage)
        {
            if(col.attachedRigidbody)
            {
                col.attachedRigidbody.AddExplosionForce(500, col.ClosestPointOnBounds(transform.position + Vector3.up * 2), 2f);
                if (col.attachedRigidbody.GetComponent<Enemy>())
                {
                    Enemy e = col.attachedRigidbody.GetComponent<Enemy>();
                    if (!hits.Contains(e))
                    {
                        print("enemy hit");
                        e.TakeDamage(1);
                        hits.Add(e);
                    }
                }
            }
        }
    }
}
