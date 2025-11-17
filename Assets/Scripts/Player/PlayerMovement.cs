using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;          // Velocidade andando/correndo
    public float rotationSpeed = 10f;     // Velocidade para virar na direção do movimento

    [Header("Pulo & Gravidade")]
    public float jumpForce = 8f;          // Força do pulo
    public float gravity = -20f;          // Gravidade realista
    private Vector3 velocity;             // Armazena velocidade vertical

    [Header("Ground Check")]
    public Transform groundCheck;         // Empty abaixo dos pés
    public float groundRadius = 0.3f;     // Raio da esfera de detecção
    public LayerMask groundMask;          // Layers que contam como chão
    private bool isGrounded;              // Está tocando o chão?

    [Header("Referências")]
    public CharacterController controller;
    public Animator animator;

    // Input
    private Vector2 moveInput;            // Entrada WASD
    private bool jumpPressed;             // Input do pulo

    // ------------------------------------------
    // INPUT CALLBACKS (vêm do PlayerInput)
    // ------------------------------------------

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            jumpPressed = true;
    }

    // ------------------------------------------
    // LOOP PRINCIPAL
    // ------------------------------------------

    private void Update()
    {
        //Debug.Log("Grounded: " + isGrounded + " | velocityY: " + velocity.y);
        CheckGround();
        HandleMovement();
        ApplyGravity();
        HandleJump();

        controller.Move(velocity * Time.deltaTime);
        UpdateAnimation();
    }

    // ------------------------------------------
    // GROUND CHECK PRECISO
    // ------------------------------------------
    void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            0.3f,
            groundMask
        );

        if (isGrounded && velocity.y < 0)
            velocity.y = -5f;  // mais realista
    }

    // ------------------------------------------
    // MOVIMENTO ALINHADO À CÂMERA
    // ------------------------------------------
    void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Se não há movimento, apenas atualiza animação e sai
        if (moveDirection.magnitude < 0.1f)
        {
            return;
        }

        // Direção relativa à câmera
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        moveDirection.y = 0; // impede inclinação para cima ou baixo

        // Move o personagem
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Rotaciona suavemente na direção do movimento
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        Vector3 e = transform.eulerAngles;
        e.x = 0;
        e.z = 0;
        transform.eulerAngles = e;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    // ------------------------------------------
    // GRAVIDADE REALISTA
    // ------------------------------------------
    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime * 1.5f;
    }

    // ------------------------------------------
    // PULO
    // ------------------------------------------
    void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            velocity.y = jumpForce;
            animator.SetTrigger("Jump");
        }

        // Reset input
        jumpPressed = false;
    }

    // ------------------------------------------
    // ANIMAÇÕES
    // ------------------------------------------
    void UpdateAnimation()
    {
        float speedPercent = new Vector2(moveInput.x, moveInput.y).magnitude;
        animator.SetFloat("Speed", speedPercent);
        animator.SetBool("IsGrounded", isGrounded);
    }

    // ------------------------------------------
    // DEBUG VISUAL DO GROUND CHECK
    // ------------------------------------------
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
