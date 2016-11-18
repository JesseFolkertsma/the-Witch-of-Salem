using UnityEngine;
using System.Collections;

public class _PlayerBaseCombat : _PlayerBase {

    public enum CombatState
    {
        Staggered,
        JumpAttack,
        Rolling,
        Blocking,
        Aiming,
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



    public float swordDamage;
    public float arrowDamage;

}
