using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;          // O jogador
    public Transform cameraTransform; // A câmera em si

    [Header("Configuração")]
    public float sensitivity = 1f;    // Velocidade da rotação da câmera
    public float distance = 6f;       // Distância atrás da personagem
    public float height = 3f;         // Altura da câmera

    private Vector2 lookInput;        // Entrada do mouse/right stick
    private float rotationX;          // Rotação vertical

    // Chamado automaticamente pelo PlayerInput (Look Action)
    public void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        if (player == null) return;

        RotateCamera();
        FollowPlayer();
    }

    void RotateCamera()
    {
        // Rotação vertical
        rotationX += lookInput.y * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -20f, 0f); // trava olhar para cima

        // Rotação horizontal (gira o rig)
        transform.Rotate(Vector3.up, lookInput.x * sensitivity);

        // Aplica rotação vertical no pivô da câmera
        cameraTransform.localRotation = Quaternion.Euler(-rotationX, 0, 0);
    }

    void FollowPlayer()
    {
        // Mantém câmera presa ao jogador
        transform.position = player.position;

        // Posiciona a câmera atrás e acima
        cameraTransform.localPosition = new Vector3(0, height, -distance);
    }
}
