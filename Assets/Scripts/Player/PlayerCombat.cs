using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public int lightDamage = 10;
    public int heavyDamage = 20;
    public float attackRange = 1.5f;

    public void LightAttackHit()
    {
        DoDamage(lightDamage);
    }

    public void HeavyAttackHit()
    {
        DoDamage(heavyDamage);
    }

    void DoDamage(int damage)
    {
        int mask = ~(1 << LayerMask.NameToLayer("Enemy Attack"));
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * attackRange, 1.2f);

        foreach (Collider col in hits)
            {
                EnemyHealth enemy = col.GetComponentInParent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Debug.Log("Acertou Inimigo");
                    Debug.Log("Colidiu com: " + col.name);

                }
            }
    }
}
