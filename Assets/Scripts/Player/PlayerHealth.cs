using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public ScreenFade screenFade;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        if (screenFade == null)
            screenFade = FindAnyObjectByType<ScreenFade>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        CameraShake shake = FindAnyObjectByType<CameraShake>();
        if (shake != null)
            shake.Shake(0.2f, 0.2f);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    void Die()
    {
        Debug.Log("Player morreu");
        //Opcional: impedir movimento do jogador
        //GetComponent<PlayerMovement>().enabled = false;

        // Reiniciar a cena após X segundos
        screenFade.FadeOutAndRestart();
    }
}