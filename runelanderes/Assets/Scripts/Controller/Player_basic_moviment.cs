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
        PlayerInputActions.Player.Jump.started += ctx => isJumping = true;
        PlayerInputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        PlayerInputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        PlayerInputActions.Player.Crouch.performed += ctx => isCrouching = true;
        PlayerInputActions.Player.Crouch.canceled += ctx => isCrouching = false;
        PlayerInputActions.Player.Ataque.performed += ctx => isAttacking = true;
        PlayerInputActions.Player.Interaction.performed += ctx => talkAction= true;

        PlayerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        PlayerInputActions.Player.Jump.started -= ctx => isJumping = false;
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
        }
        playerAnimator.SetTrigger("CROUCH");
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

    }

    private void FixedUpdate()
    {
        //Movimentação horizontal
        Vector2 position = rb.position + moveInput * moveSpeed * Time.deltaTime;
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
        bool IsGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Pular
        if (isJumping && IsGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            playerAnimator.SetTrigger("JUMP");
            isJumping = false;
        }
        //Agachamento
        playerAnimator.SetBool("CROUCH", isCrouching);
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy") && !isDead)
        {
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

    internal void PlayerSound(AudioClip collectedClip)
    {
        audioSource.PlayOneShot(collectedClip);
    }

}
