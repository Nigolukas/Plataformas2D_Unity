using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player_script : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 input;
    private PlayerInput Control;
    private Color originalColor;

    [Header("Sistema de movimiento")]
    [SerializeField] private Transform PlayerFoot;
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float GroundDistance;
    [SerializeField] private LayerMask CanJumpOn;

    [Header("Sistema de combate")]
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float AttackRatio;
    [SerializeField] private float AttackDamage;
    [SerializeField] private LayerMask CanDamage;

    [Header("Sistema de Dash")]
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private TrailRenderer trail; // opcional, para efectos

    private bool canDash = true;
    private bool isDashing = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float jumpTime; 
    private bool isJumping; 
    private float MaxJumpTime = 0.2f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetFloat("PosX", -6.0f);
        PlayerPrefs.SetFloat("PosY", -1.0f);
        rb = GetComponent<Rigidbody2D>();
        Control = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing != true)
        {
            Movimiento();
        }
        
    }

    private void Movimiento()
    {
        if(transform.position.y < -7)
        {
            float x = PlayerPrefs.GetFloat("PosX");
            float y = PlayerPrefs.GetFloat("PosY");
            Debug.Log(" x " + x);
            Debug.Log(" y " + y);
            transform.position = new Vector2(x, y);
            LifeSystem_player life = GetComponent<LifeSystem_player>();
            life.Damage(20);
        }

        input = Control.actions["move"].ReadValue<Vector2>();
        if (input.x != 0)//movimiento
        {
            animator.SetBool("Running", true);
            if(input.x > 0)//derecha
            {
                transform.eulerAngles = Vector3.zero;
            }
            else//izquierda
            {
                transform.eulerAngles = new Vector3(0,180,0);
            }
        }
        else //input 0
        {
            animator.SetBool("Running", false);
        }
        rb.linearVelocity = new Vector2(input.x * Speed, rb.linearVelocityY);
    }

    public void Saltar(InputAction.CallbackContext context)// sistema de acumulacion de fuerza de salto segun cuanto se mantenga presionado
    {
        if (context.started && OnGround())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce); // salto inmediato
            animator.SetTrigger("Jump");
            isJumping = true;
            jumpTime = MaxJumpTime;
        }

        if (context.performed && isJumping)
        {
            if (jumpTime > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);//acumula fuerza de salto
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (context.canceled)
        {
            isJumping = false;

            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }

    public bool OnGround()
    {
        bool IsOnGround = Physics2D.Raycast(PlayerFoot.position,Vector3.down, GroundDistance, CanJumpOn);
        return IsOnGround;
    }

    public void LanzarAtaque(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger("Attack");
        }
    }
    //se ejecuta en evento de animacion
    public void Ataque()
    {
        //trigger instantaneo
        Collider2D[] enemiesDamaged = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRatio, CanDamage);
        foreach (Collider2D enemy in enemiesDamaged)
        {
            LifeSystem EnemmySystem = enemy.gameObject.GetComponent<LifeSystem>();
            EnemmySystem.Damage(AttackDamage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(AttackPoint.position, AttackRatio);
    }


    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started && canDash)
        {
            StartCoroutine(DoDash());
        }
    }

    private IEnumerator DoDash()
    {
        spriteRenderer.color = new Color(0, 0, 1f);
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        float direction = transform.eulerAngles.y == 0 ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * dashForce, 0);
        trail.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        spriteRenderer.color = originalColor;
        trail.emitting = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
