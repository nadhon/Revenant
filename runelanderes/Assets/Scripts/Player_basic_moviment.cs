using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlatformer : MonoBehaviour
{
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

    Animator playerAnimator;

    private Vector3 originalScale;

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

    }

    private void FixedUpdate()
    {
        // Movimentação horizontal
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

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
    
}