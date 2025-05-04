using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public Transform player;
    public float spawnInterval = 2f;
    public float spawnRadius = 50f;
    public float moveSpeed = 10f;
    public float damage = 20f;
    public float selfDestructTime = 15f;

    private float nextSpawnTime = 0f;

    void Update()
    {
        // Handle asteroid spawning based on interval
        if (Time.time >= nextSpawnTime)
        {
            SpawnAsteroid();
            nextSpawnTime = Time.time + spawnInterval;
        }

        // If the asteroid is already spawned, move towards the player
        if (player != null)
        {
            MoveTowardsPlayer();
        }
    }

    void SpawnAsteroid()
    {
        if (asteroidPrefab == null || player == null) return;

        // Spawn asteroid at a random position around the player
        Vector3 spawnDirection = Random.onUnitSphere;
        Vector3 spawnPoint = player.position + spawnDirection * spawnRadius;

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPoint, Quaternion.identity);

        // Give the asteroid movement behavior
        AsteroidController asteroidController = asteroid.AddComponent<AsteroidController>();
        asteroidController.player = player;
        asteroidController.moveSpeed = moveSpeed;
        asteroidController.damage = damage;
        asteroidController.selfDestructTime = selfDestructTime;
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        // Move asteroid toward the player
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rocket rocket = collision.gameObject.GetComponent<Rocket>();
            if (rocket != null)
            {
                rocket.TakeDamage(damage);
            }

            Destroy(gameObject); // Destroy asteroid on collision
        }
    }
}
