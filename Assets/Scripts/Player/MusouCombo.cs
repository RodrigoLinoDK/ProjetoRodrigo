using UnityEngine;
using UnityEngine.InputSystem;

public class MusouCombo : MonoBehaviour
{
    public Animator animator;   // Controla animações da personagem

    private int lightComboStep = 0;  // Quantos ataques fracos já foram feitos
    private float comboTimer = 0f;   // Tempo desde último golpe
    private float comboResetTime = 1f; // Tempo para resetar combo
    private bool canAttack = true;   // Controle da "janela de input"

    private void Update()
    {
        // Reset automático após tempo sem atacar
        if (lightComboStep > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboResetTime)
                ResetCombo();
        }
    }

    // Chamado automaticamente pelo PlayerInput (Light Attack)
    public void OnLightAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            LightAttack();
    }

    // Chamado automaticamente pelo PlayerInput (Heavy Attack)
    public void OnHeavyAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            HeavyAttack();
    }

    void LightAttack()
    {
        if (!canAttack) return;

        lightComboStep++;
        comboTimer = 0f;

        switch (lightComboStep)
        {
            case 1: animator.SetTrigger("Light1"); break;
            case 2: animator.SetTrigger("Light2"); break;
            case 3: animator.SetTrigger("Light3"); break;
            case 4: animator.SetTrigger("Light4"); break;

            default:
                ResetCombo();
                animator.SetTrigger("Light1");
                break;
        }
    }

    void HeavyAttack()
    {
        if (!canAttack) return;

        comboTimer = 0f;

        switch (lightComboStep)
        {
            case 0: animator.SetTrigger("Heavy1"); break;
            case 1: animator.SetTrigger("Heavy2"); break;
            case 2: animator.SetTrigger("Heavy3"); break;
            case 3: animator.SetTrigger("Heavy4"); break;
            // 4 também vira Heavy4
            default: animator.SetTrigger("Heavy4"); break;
        }

        ResetCombo();
    }

    public void ResetCombo()
    {
        lightComboStep = 0;
        comboTimer = 0f;
        canAttack = true;
    }

    // Chamados por Animation Events
    public void EnableAttackWindow() => canAttack = true;
    public void DisableAttackWindow() => canAttack = false;
}
