using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class Attack : ScriptableObject
{
    public string attackName;

    public enum AttackType { basic_side, basic_up, basic_down, special_side, special_up, special_down }

    public AttackType attackType;

    public int damage;
    public float force;
    public Vector2 dir;

    public float stunTime;

    public bool canExhaust;

    // False: El ataque se realiza cuando se pulsa el boton de ataque correspondiente
    // True: El ataque lo realiza el script de CharacterActions personal del personaje
    public bool triggerManually;

    public AnimationClip animationClip;
}