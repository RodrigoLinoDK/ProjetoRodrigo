using UnityEngine;

public class HealPickup : MonoBehaviour
{

    public int healAmount = 30;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null )
        {
            ph.Heal(healAmount);
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
