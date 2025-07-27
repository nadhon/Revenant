using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlatformer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public PlayerInputActions PlayerInputActions { get; private set; }

    private Rigidbody2D rb;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isJumping;

    private void Awake()
    {
        PlayerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        PlayerInputActions.Player.Jump.performed += ctx => isJumping = true;
        PlayerInputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        PlayerInputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        PlayerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        PlayerInputActions.Player.Jump.performed -= ctx => isJumping = true;
        PlayerInputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        // Movimentação horizontal
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        // Checar se está no chão
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Pular
        if (isJumping && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        isJumping = false;
    }

    private void Jump()
    {
        // Checar se está no chão
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Pular
        if (isJumping && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        isJumping = false;
    }
}

