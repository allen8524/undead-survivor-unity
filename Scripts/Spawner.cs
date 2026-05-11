using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    int level;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();

        if (GameManager.instance == null || spawnData == null || spawnData.Length == 0)
        {
            enabled = false;
            return;
        }

        levelTime = Mathf.Max(0.01f, GameManager.instance.maxGameTime / spawnData.Length);
    }

    void Update()
    {
        if (GameManager.instance == null || !GameManager.instance.isLive || spawnData == null || spawnData.Length == 0)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

        if (spawnData[level] == null)
            return;

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        if (spawnPoint == null || spawnPoint.Length <= 1 || GameManager.instance == null || GameManager.instance.pool == null)
            return;

        GameObject enemy = GameManager.instance.pool.Get(0);
        if (enemy == null)
            return;

        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent == null)
        {
            enemy.SetActive(false);
            return;
        }

        enemyComponent.Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public float health;
    public float speed;
}
