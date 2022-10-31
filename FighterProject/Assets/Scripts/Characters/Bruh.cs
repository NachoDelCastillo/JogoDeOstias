using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bruh : PlayerController_2D
{

    public override void Jump_Input(InputAction.CallbackContext context)
    {
        base.Jump_Input(context);
        Debug.Log("Jump_Input_Defaultina");
    }


    public override void BasicAttack_Input(InputAction.CallbackContext context)
    {
        base.BasicAttack_Input(context);
        Debug.Log("BasicAttack_Input_Defaultina");
    }
}
