using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPewfab;
    public float spawnInterval = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPewfab, transform.position, transform.rotation);
    }

}
