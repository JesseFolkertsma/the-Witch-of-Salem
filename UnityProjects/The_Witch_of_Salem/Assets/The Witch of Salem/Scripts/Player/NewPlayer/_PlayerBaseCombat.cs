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

    public CombatState combatState = CombatState.Idle;
    public Weapon currentWeapon;

    public float swordDamage;
    public float arrowDamage;
    public int arrows;
    public float jumpCooldown;
    public GameObject sword;
    public GameObject shield;
    public GameObject bow;
    public GameObject arrow;
    Animator bowAnim;
    int comboInt;
    float str = 0;

    bool canJumpAttack;
    bool attacking;
    bool waitForNextAttack;
    bool canDoDamage;
    bool canRoll = true;
    bool rolling;
    bool aimBow;

    Coroutine waitAttackRoutine;

    public override void BaseStart()
    {
        base.BaseStart();
        bowAnim = bow.GetComponent<Animator>();
        if(currentWeapon == Weapon.Sword)
        {
            bow.SetActive(false);
        }
        else if(currentWeapon == Weapon.Bow)
        {
            sword.SetActive(false);
            shield.SetActive(false);
        }
        else
        {
            sword.SetActive(false);
            shield.SetActive(false);
            bow.SetActive(false);
        }
        canJumpAttack = true;
    }

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
        anim.SetFloat("DrawStrenght", str);
        anim.SetBool("AimBow", aimBow);
        if (bow.activeSelf)
        {
            bowAnim.SetFloat("Blend", str);
        }
    }

    public override void ResetStates()
    {
        base.ResetStates();
        ExitComboLoop();
        combatState = CombatState.Idle;
        canDoDamage = false;
        attacking = false;
        waitForNextAttack = false;
        useRootMovement = true;
        canRoll = true;
        rolling = false;
        aimBow = false;
        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(2, 0);
        anim.SetLayerWeight(3, 0);
        anim.SetLayerWeight(4, 0);
        str = 0;
    }

    public override void Jump()
    {
        base.Jump();
        if (combatState != CombatState.Rolling && baseState == BaseState.Grounded)
        {
            ResetStates();
        }
    }

    public virtual void TakeDamage(int damage)
    {
        lives -= damage;
        _UIManager.instance.UpdateUI();
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
                    anim.SetLayerWeight(3, 1);
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
        else if (currentWeapon == Weapon.Bow && baseState == BaseState.Grounded)
        {
            if (combatState == CombatState.Aiming && arrows > 0)
            {
                combatState = CombatState.ChargeBow;
            }
        }
    }

    public void LeftMouseUp()
    {
        if(combatState == CombatState.ChargeBow)
        {
            ShootArrow(str);
            combatState = CombatState.Aiming;
        }
    }

    public void RightMouseDown()
    {
        if (baseState == BaseState.Grounded)
        {
            if (currentWeapon == Weapon.Bow)
            {
                combatState = CombatState.Aiming;
            }
            if (currentWeapon == Weapon.Sword)
            {
                combatState = CombatState.Blocking;
                useRootMovement = false;
            }
        }
    }

    public void RightMouseUp()
    {
        if(combatState == CombatState.Aiming || combatState == CombatState.ChargeBow)
        {
            combatState = CombatState.Idle;
            aimBow = false;
            anim.SetLayerWeight(2, 0);
            useRootMovement = true;
        }
        if(combatState == CombatState.Blocking)
        {
            anim.SetLayerWeight(1, 0);
            combatState = CombatState.Idle;
            useRootMovement = true;
        }
    }

    public void AimBow()
    {
        useRootMovement = false;
        Move(2, true);
        anim.SetLayerWeight(2, 1);
        aimBow = true;
    }

    public void ChargeBow()
    {
        AimBow();
        str = Mathf.Lerp(str, 20, Time.deltaTime * 1f);
    }

    public void ShootArrow(float _str)
    {
        Vector3 dir = (mouse.GetPosition - transform.position).normalized;
        GameObject a = Instantiate(arrow, new Vector3(bow.transform.position.x, bow.transform.position.y, 0) + dir, Quaternion.identity) as GameObject;
        a.GetComponent<Arrow>().Shoot(dir, _str);
        str = 0;
        arrows--;
        _UIManager.instance.UpdateUI();
    }

    public void JumpAttack()
    {
        if (canJumpAttack)
        {
            combatState = CombatState.JumpAttack;
            anim.SetTrigger("JumpAttack");
        }
    }

    public void JumpAttacking()
    {
        rb.velocity = Vector3.down * 20;
        if(baseState == BaseState.Grounded)
        {
            JumpAttackEffect();
        }
    }

    public void JumpAttackEffect()
    {
        StartCoroutine(JumpAttackCoolDown());
        combatState = CombatState.Idle;

        Collider[] hits = Physics.OverlapSphere(transform.position, 5f);

        List<Rigidbody> enemies = new List<Rigidbody>();

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].GetComponent<BreakableLootObject>())
                {
                    hits[i].GetComponent<BreakableLootObject>().Break(transform.position);
                }

                if (hits[i].attachedRigidbody)
                {
                    if (!enemies.Contains(hits[i].attachedRigidbody))
                    {
                        enemies.Add(hits[i].attachedRigidbody);
                        hits[i].attachedRigidbody.AddExplosionForce(1000, transform.position, 5f, 10);
                        if (hits[i].attachedRigidbody.GetComponent<Enemy>())
                        {
                            hits[i].attachedRigidbody.GetComponent<Enemy>().TakeDamage(2);
                        }
                    }
                }
            }
        }
    }

    IEnumerator JumpAttackCoolDown()
    {
        canJumpAttack = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJumpAttack = true;
    }

    public void ComboAttack(int combo)
    {
        if (waitAttackRoutine != null)
        {
            StopCoroutine(waitAttackRoutine);
        }
        waitForNextAttack = false;
        //useRootMovement = false;

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
        if(currentWeapon == Weapon.Sword && bow != null)
        {
            currentWeapon = Weapon.Bow;
            bow.SetActive(true);
            sword.SetActive(false);
            shield.SetActive(false);
        }
        else if (currentWeapon == Weapon.Bow && sword != null)
        {
            currentWeapon = Weapon.Sword;
            bow.SetActive(false);
            sword.SetActive(true);
            shield.SetActive(true);
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
        anim.SetLayerWeight(3, 0);
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

    public void Blocking()
    {
        anim.SetLayerWeight(1, 1);
        ikHandler.UseIKLookat(mouse.GetPosition, .5f);
        Move(2f, true);

    }

    public void CombatRoll()
    {
        if(baseState == BaseState.Grounded && canRoll)
        {
            ResetStates();
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
        yield return new WaitForSeconds(.1f);
        canRoll = true;
        ResetStates();
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
                if (col.attachedRigidbody.GetComponent<BreakableLootObject>())
                {
                    col.attachedRigidbody.GetComponent<BreakableLootObject>().Break(transform.position);
                }
            }
        }
    }
}
