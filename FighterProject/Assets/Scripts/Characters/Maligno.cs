using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Maligno : CharacterActions
{

    float normalGravityScale;


    // True cuando esta bajando para reventar el suelo
    bool goingToSmashGround;

    [SerializeField] BoxCollider2D basicDown_Collider;

    [SerializeField] float specialDown_Speed;

    private void Awake()
    {
        normalGravityScale = playerController.rb.gravityScale;
    }

    void Update()
    {
        if (goingToSmashGround)
        {
            playerController.rb.velocity = new Vector2(0, -specialDown_Speed);

            if (playerController.onGround)
            {
                StartCoroutine(playerController.TriggerAttack(special_down));
                goingToSmashGround = false;
                playerController.ignoreVelocityLimit = false;

                basicDown_Collider.GetComponent<SpriteRenderer>().enabled = false;
                basicDown_Collider.enabled = false;
            }
        }
    }

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
                ZeroGravity();
                playerController.rb.velocity = Vector2.zero;
                float animationSeconds = special_up.animationClip.length;
                Invoke("ReturnToNormalGravity", animationSeconds);
                break;
            case Attack.AttackType.special_down:
                goingToSmashGround = true;
                playerController.ignoreVelocityLimit = true;
                basicDown_Collider.enabled = true;
                basicDown_Collider.GetComponent<SpriteRenderer>().enabled = true;
                break;
        }

        yield return new WaitForSeconds(0);
    }


    #region SpecialUp

    [SerializeField] float teleportUpDistance;

    public void TeleportUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + teleportUpDistance);
    }

    void ZeroGravity()
    { playerController.rb.gravityScale = 0; }

    void ReturnToNormalGravity()
    { playerController.rb.gravityScale = normalGravityScale; }

    #endregion

}
