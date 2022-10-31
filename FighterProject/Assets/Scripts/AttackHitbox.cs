using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackHitbox : MonoBehaviour
{

    PlayerController_2D player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController_2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //PlayerController_2D playerDamaged = collision.GetComponentInParent<PlayerController_2D>();

        //if (playerDamaged != null)
        //    player.DealDamage(playerDamaged, Attack.AttackType.basic_side);
    }

    public void AttackHit(OnTriggerDelegation delegation)
    {
        if (!delegation.Other.CompareTag("HitTrigger") || delegation.Other.GetComponentInParent<PlayerController_2D>() == player) return;

        //string AttackName = delegation.Caller.transform.gameObject.name;
        string AttackName = delegation.Caller.tag;

        Attack attackType = Array.Find(player.attacks, attack => attack.attackName == AttackName);

        PlayerController_2D playerDamaged = delegation.Other.GetComponentInParent<PlayerController_2D>();

        player.DealDamage(playerDamaged, attackType);
    }
}
