using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public Transform player;
    public float spawnInterval = 2f;
    public float spawnDistance = 100f;
    public float moveSpeed = 15f;
    public float damage = 20f;
    public float despawnDistance = 50f;

    private float nextSpawnTime;

    void Update()
    {
        // Spawning new asteroid in front of the player
        if (Time.time >= nextSpawnTime)
        {
            SpawnAsteroidInFront();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnAsteroidInFront()
    {
        if (asteroidPrefab == null || player == null) return;

        Vector3 spawnPos = player.position + player.forward * spawnDistance;
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

        AsteroidMovement mover = asteroid.AddComponent<AsteroidMovement>();
        mover.player = player;
        mover.moveSpeed = moveSpeed;
        mover.damage = damage;
        mover.despawnDistance = despawnDistance;
    }
}

public class AsteroidMovement : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 15f;
    public float damage = 20f;
    public float despawnDistance = 50f;

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Despawn if player has passed the asteroid
        float relativeZ = Vector3.Dot(player.forward, transform.position - player.position);
        if (relativeZ < -despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rocket rocket = collision.gameObject.GetComponent<Rocket>();
            if (rocket != null)
                rocket.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
