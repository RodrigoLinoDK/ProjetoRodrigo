using UnityEngine;
using UnityEngine.UI;


public class HealthBarUI : MonoBehaviour
{

    public PlayerHealth playerHealth;
    public Image healthFill;

    // Update is called once per frame
    void Update()
    {
        if (playerHealth == null) return;

        float fill = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        healthFill.fillAmount = fill;
    }
}
