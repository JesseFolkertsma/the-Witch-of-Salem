using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour{
    public int lives;
    public float swordDamage;
    public float arrowDamage;
    public int apples;
    public int arrows;
    public int explosiveArrows;
    public int comboLength;
    public bool hasSword;
    public bool hasBow;
    public bool hasJumpAttack;
    public bool hasExplosives;
}
[System.Serializable]
public class JonasWeapons
{
    public GameObject startSword;
    public GameObject endSword;
    public GameObject startShield;
    public GameObject endShield;
    public GameObject bow;
    public Transform bowpos1, bowpos2;
}
