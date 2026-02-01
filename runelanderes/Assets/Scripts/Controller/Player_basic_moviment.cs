using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerPlatformer : MonoBehaviour
{
    [Header("Movimentos")]
    public PlayerInputActions PlayerInputActions { get; private set; }
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jumping ")]
    [Tooltip("Speed height.")]
    [SerializeField] private float jumpForce = 10f;


    public static UIHandler instance { get; private set; }

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isCrouching;
    private bool isJumping;

    private bool isAttacking;
    public bool talkAction;

      Animator playerAnimator;

    private Vector3 originalScale;

    [Header("Status de vida")]


    public int MaxVida = 5;
    public int VidaAtual;
    public int health { get { return VidaAtual; } }
    public float TimeInvincible = 1f; // Tempo de invencibilidade após receber dano
    private bool isInvincible = false;
    private float damageCooldown = 0f; // Tempo restante de invencibilidade

    public GameObject PauseDisplay;
    private InputAction m_pauseActionUI;
    private InputAction m_pauseActionPlayer;

    private void Awake()
    {
        PlayerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        if (groundCheck == null)
        {
            Debug.LogError("Ground Check Transform is not assigned in the PlayerPlatformer script.");
        }
        m_pauseActionUI = PlayerInputActions.FindAction("Player/Pause");
        m_pauseActionPlayer = PlayerInputActions.FindAction("Player/Pause");
        
    }

    void Start()
    {
        PlayerInputActions.Enable();
        originalScale = transform.localScale;

        VidaAtual = MaxVida;
    }

    private void OnEnable()
    {
        PlayerInputActions.Player.Jump.performed += ctx => isJumping = true;
        PlayerInputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        PlayerInputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        PlayerInputActions.Player.Crouch.performed += ctx => isCrouching = true;
        PlayerInputActions.Player.Crouch.canceled += ctx => isCrouching = false;
        PlayerInputActions.Player.Ataque.performed += ctx => isAttacking = true;
        PlayerInputActions.Player.Interaction.performed += ctx => talkAction = true;
        PlayerInputActions.Player.Pause.performed += ctx => DisplayPauseMenu("Player");
        PlayerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        PlayerInputActions.Player.Pause.performed -= ctx => DisplayPauseMenu("Player");
        PlayerInputActions.Player.Jump.performed -= ctx => isJumping = false;
        PlayerInputActions.Player.Ataque.performed -= ctx => isAttacking = true;
        PlayerInputActions.Player.Interaction.performed -= ctx => talkAction = false;
        PlayerInputActions.Player.Disable();
    }
    private void Update()
    {
        // Atualizar animação de movimento
        playerAnimator.SetBool("IDLE", moveInput.x == 0);
        playerAnimator.SetBool("RUNNING", Mathf.Abs(moveInput.x) > 0);
        
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown <= 0)
            {
                isInvincible = false;
            }
        }
        if (talkAction == true)
        {
            FindFriend();
            talkAction = false;
        }
       DisplayPauseMenu("Player");


    }


    private void DisplayPauseMenu(string actionMap)
    {
        if (m_pauseActionPlayer.WasPerformedThisFrame())
        {
            if (PauseDisplay.activeInHierarchy)
            {
                PauseDisplay.SetActive(false);
                PlayerInputActions.FindAction("Player").Disable();
                PlayerInputActions.FindAction("UI").Enable();
            }
            else if (m_pauseActionUI.WasPerformedThisFrame())
            {
                PauseDisplay.SetActive(true);
                PlayerInputActions.FindAction("UI").Disable();
                PlayerInputActions.FindAction("Player").Enable();
            }
        }
    }

    private void FixedUpdate()
    {
        //Movimentação horizontal
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x) * -1, originalScale.y, originalScale.z); // Vira para a esquerda
        }
        else if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Vira para a direita
        }
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (isGrounded && isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            playerAnimator.SetTrigger("JUMP");
            isJumping = false;
        }
        if (isGrounded && rb.linearVelocity.y <= 0.01f)
        {
            playerAnimator.SetBool("JUMP", false);
        }
        // Pular

        isJumping = false;

        //Agachamento
        if (isCrouching)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            playerAnimator.SetBool("CROUCH", true);


        }
        isCrouching = false;
        //Ataque
        if (isAttacking)
        {
            playerAnimator.SetTrigger("ATAQUE");
            isAttacking = false; // Reseta o ataque após ser executado

        }

    }
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.greenYellow;
            Gizmos.DrawWireSphere(groundCheck.position, 0.8f);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, groundCheck.position);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        if (collision.TryGetComponent<PlayerHealth>(out var health))
        {
            health.TakeDamage(1);
        }
    }

    
    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, moveInput, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null)
            {
                UIHandler.instance.DisplayDialogue();
            }

        }
    }

}
