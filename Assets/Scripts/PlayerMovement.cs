using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    //Configuração de movimentos
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    //Componentes
    public CharacterController controller;
    public Animator animator;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => Jump();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);
        UpdateAnimation();
    }

    void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        //Ajusta a direção com base na camera
       move = Camera.main.transform.TransformDirection(move);
        move.y = 0;

        controller.Move(move * moveSpeed * Time.deltaTime);

        //Rotaciona o personagem na direção do movimento
        if (move.magnitude > 0.1f)
        {
            Vector3 lookDir = new Vector3(move.x, 0, move.z);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(lookDir),
                Time.deltaTime * 2f);
        }
    }

    void ApplyGravity()
    {
        isGrounded = controller.isGrounded;

        if (controller.isGrounded && velocity.y <0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
    }

    void Jump()
    {
        if (isGrounded)
        {
            velocity.y = jumpForce;
            animator.SetTrigger("Jump");
        }
    }

    void UpdateAnimation()
    {
        float speed = new Vector2(moveInput.x, moveInput.y).magnitude;
        animator.SetFloat("Speed", speed);
    }
}
