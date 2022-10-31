using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterActions : MonoBehaviour
{
    // Perfil con los datos de este personaje
    [SerializeField] public CharacterData characterData;

    [SerializeField] public Attack basic_side;
    [SerializeField] public Attack basic_up;
    [SerializeField] public Attack basic_down;

    [SerializeField] public Attack special_side;
    [SerializeField] public Attack special_up;
    [SerializeField] public Attack special_down;

    [HideInInspector] public PlayerController_2D playerController;

    public virtual void Jump_Input_Action(InputAction.CallbackContext context)
    { }

    public virtual IEnumerator DoCharacterActions_IEnumerator(Attack attack, bool onGround)
    { yield return new WaitForSeconds(0); }
}
