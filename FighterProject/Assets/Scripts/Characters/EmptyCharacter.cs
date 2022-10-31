using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EmptyCharacter : CharacterActions
{
    void Update()
    { }

    public override void Jump_Input_Action(InputAction.CallbackContext context)
    { }

    public override IEnumerator DoCharacterActions_IEnumerator(Attack attack, bool onGround)
    {
        switch (attack.attackType)
        {
            case Attack.AttackType.basic_side:
                break;
            case Attack.AttackType.basic_up:
                break;
            case Attack.AttackType.basic_down:
                break;
            case Attack.AttackType.special_side:
                break;
            case Attack.AttackType.special_up:
                break;
            case Attack.AttackType.special_down:
                break;
        }

        yield return new WaitForSeconds(0);
    }
}
