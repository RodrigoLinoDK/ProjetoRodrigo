using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack }
    public State currentState = State.Patrol;

    [Header("References")]
    public EnemyHealth health;
    public Transform player;
    public Animator anim;

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float attackRange = 2f;
    public float detectionRange = 10f;

    [Header("Waypoints")]
    public Transform[] waypoints;
    private int waypointIndex = 0;

    [Header("Attack")]
    public float attackCooldown = 1.5f;
    private bool canAttack = true;
    private bool attackWindupDone = false;

    void Start()
    {
        if (!health) health = GetComponent<EnemyHealth>();

        // Encontrar player automaticamente
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
            Debug.LogError("EnemyAI: Player não encontrado! Certifique-se de que o Player tem a tag 'Player'.");

        // Encontrar waypoints automaticamente
        if (waypoints == null || waypoints.Length == 0)
        {
            GameObject group = GameObject.Find("WaypointGroup");
            if (group != null)
            {
                waypoints = group.GetComponentsInChildren<Transform>();
            }
        }
    }

    void Update()
    {
        if (player == null) return; // evita erros se o player morrer
        if (health.isDead) return;

        if (health.isStunned)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
            currentState = State.Attack;
        else if (distance <= detectionRange)
            currentState = State.Chase;
        else
            currentState = State.Patrol;

        switch (currentState)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: Chase(); break;
            case State.Attack: Attack(); break;
        }
    }

    void Patrol()
    {
        if (waypoints == null || waypoints.Length <= 1) return;

        Transform target = waypoints[waypointIndex];
        MoveTowards(target.position, patrolSpeed);

        if (Vector3.Distance(transform.position, target.position) < 1f)
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
    }

    void Chase()
    {
        MoveTowards(player.position, chaseSpeed);
    }

    void Attack()
    {
        transform.LookAt(player.position);

        if (!attackWindupDone)
        {
            StartCoroutine(AttackWindup());
            return;
        }

        if (canAttack)
            StartCoroutine(DoAttack());

    }

    IEnumerator AttackWindup()
    {
        attackWindupDone = true;
        yield return new WaitForSeconds(attackCooldown);
    }


    IEnumerator DoAttack()
    {

        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(10);
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0;

        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
