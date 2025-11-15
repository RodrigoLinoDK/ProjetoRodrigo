using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    public Transform player;
    public Transform cameraTransform;
    public float sensitivity = 1f;
    public float distance = 6f;
    public float height = 3f;

    private PlayerInputActions input;
    private Vector2 lookInput;
    private float rotationX;

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        input.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

    private void LateUpdate()
    {
        if (player == null) return;

        RotateCamera();
        FollowPlayer();
    }

    void RotateCamera()
    {
        rotationX += lookInput.y * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -20f, 0f);
        transform.Rotate(Vector3.up, lookInput.x * sensitivity);
        cameraTransform.localRotation = Quaternion.Euler(-rotationX, 0, 0);
    }

    void FollowPlayer()
    {
        transform.position = player.position;
        cameraTransform.localPosition = new Vector3(0, height, -distance);
    }
 
}
