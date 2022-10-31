using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Defaultina : CharacterActions
{
    [SerializeField] float specialUp_Speed;
    [SerializeField] float specialDown_Speed;

    void Update()
    {
        if (specialAttackingUp)
            playerController.rb.velocity = new Vector2(0, specialUp_Speed);

        if (goingToSmashGround)
        {
            playerController.rb.velocity = new Vector2(0, -specialDown_Speed);

            if (playerController.onGround)
            {
                StartCoroutine(playerController.TriggerAttack(special_down));
                goingToSmashGround = false;
                playerController.ignoreVelocityLimit = false;
            }
        }
    }

    public override void Jump_Input_Action(InputAction.CallbackContext context)
    { }


    // True cuando esta subiendo haciendo el ataque especial hacia arriba
    bool specialAttackingUp;

    // True cuando esta bajando para reventar el suelo
    bool goingToSmashGround;

    public override IEnumerator DoCharacterActions_IEnumerator(Attack attack, bool onGround)
    {
        switch (attack.attackType)
        {
            case Attack.AttackType.basic_side:
                break;
            case Attack.AttackType.basic_up:
                break;
            case Attack.AttackType.basic_down:
                // Impulso hacia abajo
                playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, 0);
                playerController.rb.AddForce(new Vector2(0f, 1000));
                break;
            case Attack.AttackType.special_side:
                break;
            case Attack.AttackType.special_up:
                specialAttackingUp = true;
                yield return new WaitForSeconds(attack.animationClip.length);
                specialAttackingUp = false;
                break;
            case Attack.AttackType.special_down:
                goingToSmashGround = true;
                playerController.ignoreVelocityLimit = true;
                break;
        }

        yield return new WaitForSeconds(0);
    }

}
