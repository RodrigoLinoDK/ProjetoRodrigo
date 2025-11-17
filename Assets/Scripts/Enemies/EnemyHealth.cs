using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public float stunDuration = 0.3f;

    private int currentHealth;
    public bool isStunned = false;
    public bool isDead = false;

    private Animator anim;
    public Collider attackHitbox;

    void Start()
    {
        currentHealth = maxHealth;
        //anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {

        Debug.Log("Inimigo recebeu dano = " + damage);

        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Vida atual = " + currentHealth);

        StartCoroutine(Stun());

         if (currentHealth <= 0)

            Die();

        //if (anim)
           // anim.SetTrigger("Hit");

       
    }

    IEnumerator Stun()
    {
        isStunned = true;
        if (attackHitbox) attackHitbox.enabled = false;

        yield return new WaitForSeconds(stunDuration);

        if (!isDead && attackHitbox)
            attackHitbox.enabled = true;

        isStunned = false;
    }

    void Die()
    {
        isDead = true;
        
        Destroy(gameObject, 2f);
        //if (anim)
            //anim.SetTrigger("Die");

        // delay before destruction
        
    }
}