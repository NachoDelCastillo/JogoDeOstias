using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerController_2D : MonoBehaviour
{
    [HideInInspector] public int playerIndex;

    // Script con la informacion del personaje
    [HideInInspector] public CharacterActions thisCharacterActions;

    float speed;


    // Components
    public Rigidbody2D rb;
    [SerializeField] SpriteRenderer gfx;
    [SerializeField] Animator anim;

    PlayerInputActions playerControl;

    // Movement
    protected float input_hor;
    protected float input_ver;
    int dir = 1;

    // Jump
    float jumpForce;
    float smoothTime = .1f;
    private Vector3 m_Velocity = Vector3.zero;
    bool canDoubleJump;
    float jumpRemember = .2f;
    float jumpRememberTimer = -1;

    float groundRemember = .2f;
    float groundRememberTimer = -1;

    // Ground checker
    [SerializeField] Transform groundCheck_tr;
    [SerializeField] LayerMask groundLayer;
    [HideInInspector] public bool onGround;
    float groundCheck_radius = .2f;


    // Attack system
    bool attackingAir;
    bool attackingGround;
    // Esta variable devuelve true el tiempo en el que se esta atacando pero pasa a ser false un instante antes de acabarse el ataque
    // Esto es para poder moverte un poco antes de que termine el ataque y no tener que esperar a que termine del todo para poder moverte
    bool attackingMoveDelay;

    [HideInInspector] public Attack[] attacks;

    bool[] attacksExhausted;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        thisCharacterActions = GetComponent<CharacterActions>();
        thisCharacterActions.playerController = this;

        playerControl = new PlayerInputActions();

        attacks = new Attack[6];

        attacks[0] = thisCharacterActions.basic_side;
        attacks[1] = thisCharacterActions.basic_up;
        attacks[2] = thisCharacterActions.basic_down;

        attacks[3] = thisCharacterActions.special_side;
        attacks[4] = thisCharacterActions.special_up;
        attacks[5] = thisCharacterActions.special_down;

        attacksExhausted = new bool[6];


        // Set up character variables

        //characterData = GameManager.GetInstance().

        speed = thisCharacterActions.characterData.speed;
        jumpForce = thisCharacterActions.characterData.jumpForce;
    }

    bool Attacking()
    {
        return attackingGround || attackingAir;
    }

    void Update()
    {
        UpdateFlipGfx();

        Jump_check();

        // Limitar velocidad descendente en caso de estar stuneado
        if (!stunned && !ignoreVelocityLimit)
        {
            if (rb.velocity.y < -15)
                rb.velocity = new Vector2(rb.velocity.x, -15);
        }

        // Update animator
        if (input_hor > .1f || input_hor < -.1f)
            anim.SetFloat("HorVel", Mathf.Abs(rb.velocity.x));
        else
            anim.SetFloat("HorVel", 0);


        anim.SetFloat("VerVel", rb.velocity.y);

        anim.SetBool("OnGround", onGround);
    }


    #region Input

    public virtual void Jump_Input(InputAction.CallbackContext context)
    {
        if (context.started)
            jumpRememberTimer = jumpRemember;

        if (context.canceled && rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .55f);

        thisCharacterActions.Jump_Input_Action(context);
    }

    public virtual void BasicAttack_Input(InputAction.CallbackContext context)
    {
        if (Attacking()) return;

        if (context.started)
        {
            if (onGround)
            {
                if (input_ver > .5f)
                    StartCoroutine(PerformAttack(thisCharacterActions.basic_up, true));
                else if (input_ver < -.5f)
                    StartCoroutine(PerformAttack(thisCharacterActions.basic_down, true));
                else
                    StartCoroutine(PerformAttack(thisCharacterActions.basic_side, true));
            }
            else
            {
                if (input_ver > .5f)
                    StartCoroutine(PerformAttack(thisCharacterActions.basic_up, false));
                else if (input_ver < -.5f)
                    StartCoroutine(PerformAttack(thisCharacterActions.basic_down, false));
                else
                    StartCoroutine(PerformAttack(thisCharacterActions.basic_side, false));
            }
        }
    }

    public virtual void SpecialAttack_Input(InputAction.CallbackContext context)
    {
        if (Attacking()) return;

        if (context.started)
        {
            if (onGround)
            {
                if (input_ver > .5f)
                    StartCoroutine(PerformAttack(thisCharacterActions.special_up, true));
                else if (input_ver < -.5f)
                    StartCoroutine(PerformAttack(thisCharacterActions.special_down, true));
                else
                    StartCoroutine(PerformAttack(thisCharacterActions.special_side, true));
            }
            else
            {
                if (input_ver > .5f)
                    StartCoroutine(PerformAttack(thisCharacterActions.special_up, false));
                else if (input_ver < -.5f)
                    StartCoroutine(PerformAttack(thisCharacterActions.special_down, false));
                else
                    StartCoroutine(PerformAttack(thisCharacterActions.special_side, false));
            }
        }
    }

    protected virtual IEnumerator PerformAttack(Attack attack, bool onGround)
    {
        int attackIndex = (int)attack.attackType;
        if (attacksExhausted[attackIndex]) yield break;

        // Posibles acciones del personaje
        StartCoroutine(thisCharacterActions.DoCharacterActions_IEnumerator(attack, onGround));

        if (!attack.triggerManually)
            StartCoroutine(TriggerAttack(attack));
    }

    public IEnumerator TriggerAttack(Attack attack)
    {
        // Activa el trigger para que el personaje cambie de animacion
        anim.SetTrigger(attack.attackName);

        //float movilityDelay = .15f;
        float movilityDelay = 0.15f;

        // Actualiza la variable en el animator
        anim.SetBool("Attacking", true);
        attackingMoveDelay = true;
        if (onGround)
        {
            attackingGround = true;
            yield return new WaitForSeconds(attack.animationClip.length - movilityDelay);
            attackingMoveDelay = false;
        }
        else
        {
            attackingAir = true;
            yield return new WaitForSeconds(attack.animationClip.length - movilityDelay);
            attackingMoveDelay = false;
        }
        yield return new WaitForSeconds(movilityDelay);
        attackingGround = false;
        attackingAir = false;
        anim.SetBool("Attacking", false);

        if (attack.canExhaust)
            attacksExhausted[(int)attack.attackType] = true;
    }



    protected virtual void CharacterActions(Attack attack, bool onGround) { }

    #endregion

    #region Movility

    public void GetMoveInput(InputAction.CallbackContext context)
    {
        input_hor = context.ReadValue<Vector2>().x;
        input_ver = context.ReadValue<Vector2>().y;
    }

    void UpdateFlipGfx()
    {
        if (Attacking()) return;

        // Cambiar direccion actual
        if (input_hor != 0)
        {
            if (input_hor < 0)
            { dir = -1; gfx.transform.localScale = new Vector3(-1, 1, 0); }
            else
            { dir = 1; gfx.transform.localScale = new Vector3(1, 1, 0); }
        }
    }

    void FixedUpdate()
    {
        Move();

        CheckGround();
    }

    void Move()
    {
        float realInputHor = input_hor;
        float realInputVer = input_ver;

        if ((attackingMoveDelay && onGround) || stunned)
        { realInputHor = 0; realInputVer = 0; }

        if (!stunned)
        {
            Vector3 targetVelocity = new Vector2(realInputHor * speed, rb.velocity.y);
            // And then smoothing it out and applying it to the character
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, smoothTime);
        }
        else
        {
            Vector3 targetVelocity = new Vector2(0, rb.velocity.y);
            // And then smoothing it out and applying it to the character
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, .5f);
        }
    }

    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck_tr.position, groundCheck_radius, groundLayer);

        if (colliders.Length == 0)
            onGround = false;
        else
        {
            onGround = true;
            canDoubleJump = true;

            groundRememberTimer = groundRemember;

            // Resetear todos los ataques exhaustos
            for (int i = 0; i < attacksExhausted.Length; i++)
                attacksExhausted[i] = false;
        }

        //if (!onGround_Remember && onGround && !isRespawning)
        //    AudioManager_PK.instance.Play("Fall", Random.Range(.3f, .6f));

        onGround_Remember = onGround;
    }
    bool onGround_Remember;

    void Jump_check()
    {
        jumpRememberTimer -= Time.deltaTime;
        groundRememberTimer -= Time.deltaTime;

        if (rb.velocity.y > 4)
            groundRememberTimer = 0;

        if ((Attacking() && onGround) || stunned) return;

        if (jumpRememberTimer > 0 && groundRememberTimer > 0)
        {
            jumpRememberTimer = 0;
            groundRememberTimer = 0;

            onGround = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, jumpForce));

            // AudioManager_PK.instance.Play("Jump", 1);
        }

        if (jumpRememberTimer > 0 && canDoubleJump)
        {
            jumpRememberTimer = 0;

            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    #endregion

    #region Damage

    // El daño recibido actualmente
    [HideInInspector] public int damage;

    // Si devuelve true significa que este jugador esta stuneado
    [HideInInspector] public bool stunned;
    [HideInInspector] public bool ignoreVelocityLimit;


    public void DealDamage(PlayerController_2D playerDamaged, Attack attackType)
    {
        playerDamaged.TakeDamage(attackType, dir);
    }

    public void TakeDamage(Attack attackReceived, int dir)
    {
        // Sumar el daño realizado
        damage += attackReceived.damage;

        // Calcular la velocidad a la que va ha salir disparado
        Vector2 finalVelocity;
        // Direccion en la que sale disparado el jugador
        finalVelocity = attackReceived.dir;
        finalVelocity.Normalize();
        // Direccion en la que sale disparado
        finalVelocity *= new Vector3(dir, 1, 1);
        // Potencia a la que sale disparado el jugador
        finalVelocity *= attackReceived.force * ((damage + 20) * .05f);

        rb.velocity = Vector3.zero;
        rb.velocity = finalVelocity;
        Debug.Log("rb.velocity = " + rb.velocity);

        // Stunnear a este personaje
        stunned = true;
        anim.SetBool("Stunned", true);
        CancelInvoke();
        Invoke("UnstunThis", attackReceived.stunTime);

        // Actualizar su gfx para que mire hacia el ataque que ha recibido
        gfx.transform.localScale = new Vector3(-dir, 1, 1);


        // Informar al UI
        GameplayUI_Manager.GetInstance().UpdatePlayerDamage(playerIndex, damage);

        Debug.Log("DAMAGE DEALED TO THIS WITH " + attackReceived + " with " + damage + " Damage");
    }

    void UnstunThis()
    {
        stunned = false;
        anim.SetBool("Stunned", false);
    }


    #endregion
}