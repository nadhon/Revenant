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
    [SerializeField] private float jumpForce = 5f;


    public static UIHandler instance { get; private set; }

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isCrouching;
    private bool isJumping;

    private bool isAttacking;
    public bool talkAction;

    [SerializeField] private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
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

        PlayerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
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
        if (isJumping)
        {
            playerAnimator.SetTrigger("JUMP");
            isJumping = false;
            Debug.Log("Pulei otario");
        }
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
        isJumping = false;
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        Debug.Log("Grounded: " + isGrounded);


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
            rb.linearVelocity = new Vector2(rb.linearVelocity.y, jumpForce);
            playerAnimator.SetBool("JUMP", true);
            Debug.Log("Pulei otario");

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
        }
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
        if (collision.CompareTag("enemy") && !isDead)
        {
            playerAnimator.SetBool("HIT", true);
            MoveEnemy enemy = collision.GetComponent<MoveEnemy>();
            if (enemy != null)
            {
                ChangeHealth(-enemy.Damage);
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
                playerAnimator.SetTrigger("DEATH");
                GetComponent<PlayerPlatformer>().enabled = false;
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
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null)
            {
                UIHandler.instance.DisplayDialogue();
            }

        }
    }
    public void PlaySound(AudioClip walk)
    {
        if (moveInput.x != 0 && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(walk);
        }
    }
    
    public void PlayerSound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
