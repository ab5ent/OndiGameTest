using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    [System.Serializable]
    public class EnemyPool
    {
        public string enemyId;

        public Enemy enemyPrefab;

        public int initialPoolSize = 10;

        public Queue<Enemy> pool = new Queue<Enemy>();
    }

    [SerializeField] private List<EnemyPool> enemyPools = new List<EnemyPool>();

    [SerializeField] private Transform[] path;

    private readonly Dictionary<string, EnemyPool> _enemyPoolLookup = new Dictionary<string, EnemyPool>();

    private Transform _poolContainer;

    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        _poolContainer = transform.Find("PoolContainer");

        foreach (var pool in enemyPools)
        {
            if (pool.enemyPrefab == null) continue;

            _enemyPoolLookup[pool.enemyId] = pool;

            for (int i = 0; i < pool.initialPoolSize; i++)
            {
                var enemy = CreateNewEnemy(pool.enemyPrefab);
                ReturnToPool(enemy);
            }
        }
    }

    public Enemy GetEnemy(string enemyId)
    {
        if (!_enemyPoolLookup.TryGetValue(enemyId, out var pool))
        {
            Debug.LogError($"No enemy pool found for ID: {enemyId}");
            return null;
        }

        Enemy enemy;

        enemy = pool.pool.Count > 0 ? pool.pool.Dequeue() : CreateNewEnemy(pool.enemyPrefab);

        enemy.gameObject.SetActive(true);
        enemy.transform.SetParent(null);

        return enemy;
    }

    public void ReturnToPool(Enemy enemy)
    {
        if (enemy == null) return;

        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(_poolContainer);
        enemy.transform.position = Vector3.zero;

        foreach (var pool in enemyPools)
        {
            if (pool.enemyPrefab.GetType() == enemy.GetType())
            {
                pool.pool.Enqueue(enemy);
                return;
            }
        }

        Debug.LogWarning($"Returning enemy {enemy.name} to pool, but no matching pool found");
        Destroy(enemy.gameObject);
    }

    private Enemy CreateNewEnemy(Enemy prefab)
    {
        var enemy = Instantiate(prefab, _poolContainer);
        enemy.gameObject.name = prefab.name + " (Pooled)";
        enemy.SetEnemiesController(this);
        return enemy;
    }

    private Enemy SpawnEnemy(string enemyId, Vector3 position, Quaternion rotation)
    {
        var enemy = GetEnemy(enemyId);

        if (enemy != null)
        {
            enemy.transform.SetPositionAndRotation(position, rotation);
        }

        return enemy;
    }

    public void SpawnEnemy(string enemyId)
    {
        Enemy enemy = SpawnEnemy(enemyId, path[0].position - new Vector3(-1, 0, 0), Quaternion.identity);
        enemy.Initialize();

        enemy.GetEntityComponent<MovementByPathComponent>().SetPath(path);
        enemy.gameObject.SetActive(true);

        enemy.GetEntityComponent<MovementByPathComponent>().StartMove();
    }
}