using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerPlatformer : MonoBehaviour
{
    private const float V = 2.0f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask groundLayer;

    [Header("Jumping ")]
    [Tooltip("Speed height.")]
    [SerializeField] private float jumpForce = 5f;

    public PlayerInputActions PlayerInputActions { get; private set; }
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isCrouching;
    private bool isJumping;

    private bool isAttacking;
    public InputAction talkAction;

    Animator playerAnimator;

    private Vector3 originalScale;
    [Header("Status de vida")]


    public int MaxVida = 5;
    public int VidaAtual;
    public int health { get { return VidaAtual; } }
    public float TimeInvincible = 1f; // Tempo de invencibilidade após receber dano
    private bool isInvincible = false;
    private float damageCooldown = 0f; // Tempo restante de invencibilidade

    private bool isDead = false;

    private void Awake()
    {
        PlayerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        if (groundCheck == null)
        {
            Debug.LogError("Ground Check Transform is not assigned in the PlayerPlatformer script.");
        }
    }

    void Start()
    {
        PlayerInputActions.Enable();
        originalScale = transform.localScale;

        VidaAtual = MaxVida;
        talkAction.Enable();
    }

    private void OnEnable()
    {
        PlayerInputActions.Player.Jump.performed += ctx => isJumping = true;
        PlayerInputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        PlayerInputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        PlayerInputActions.Player.Crouch.performed += ctx => isCrouching = true;
        PlayerInputActions.Player.Crouch.canceled += ctx => isCrouching = false;
        PlayerInputActions.Player.Ataque.performed += ctx => isAttacking = true;

        PlayerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        PlayerInputActions.Player.Jump.performed -= ctx => isJumping = false;
        PlayerInputActions.Player.Ataque.performed -= ctx => isAttacking = false;
        PlayerInputActions.Player.Disable();
    }
    private void Update()
    {
        // Atualizar animação de movimento
        playerAnimator.SetBool("IDLE", moveInput.x == 0);
        playerAnimator.SetBool("RUNNING", Mathf.Abs(moveInput.x) > 0);
        if (isJumping)
        {
            playerAnimator.SetBool("JUMP", true);
        }
        playerAnimator.SetBool("CROUCH", isCrouching);
        if (isInvincible)
        {
            isInvincible = false;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            FindFriend();
        }

    }

    private void FixedUpdate()
    {
        //Movimentação horizontal
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        Vector2 position = (Vector2)rb.position + moveInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(position);

        if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x) * -1, originalScale.y, originalScale.z); // Vira para a esquerda
        }
        else if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Vira para a direita
        }
        playerAnimator.SetBool("RUNNING", moveInput.x != 0);

        // Checar se está no chão
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Pular
        if (isJumping && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            playerAnimator.SetTrigger("JUMP");
        }
        //Ataque
        if (isAttacking)
        {
            playerAnimator.SetTrigger("ATAQUE");
            playerAnimator.SetBool("CROUCH", isCrouching); // Reseta o agachamento após o ataque
            isAttacking = false; // Reseta o ataque após ser executado

        }

        playerAnimator.SetBool("CROUCH", isCrouching);
        // Animação de agachamento

        // Animação de idle

        playerAnimator.SetBool("IDLE", moveInput.x == 0);

        isJumping = false;
    }
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy") && !isDead)
        {
            collision.GetComponent<LifebarPlayer>().TakeDamage(10);
            playerAnimator.SetBool("HIT", true);
            VidaAtual -= 10; // Subtrai vida do jogador
            if (VidaAtual <= 0)
            {
                isDead = true; // Marca o jogador como morto
                playerAnimator.SetBool("DEATH", true);
                Destroy(gameObject); // Destrói o jogador se a vida chegar a zero
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        VidaAtual = Mathf.Clamp(VidaAtual + amount, 0, MaxVida);
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = TimeInvincible;
            if (VidaAtual <= 0)
            {
                isDead = true;
                playerAnimator.SetBool("DEATH", true);
                Destroy(gameObject);
            }
        }
        VidaAtual = Mathf.Clamp(VidaAtual + amount, 0, MaxVida);
        UIHandler.instance.SetHealthValue(VidaAtual / (float)MaxVida);
    }
    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, moveInput, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            //NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            Debug.Log("Rycast has hit the object" + hit.collider.gameObject);

        }
    }
    
    
}
