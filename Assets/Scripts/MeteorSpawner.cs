using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int minScoreToStart = 1500;
    [SerializeField] private int maxAliveMeteors = 5;
    public float minInterval = 8f;
    public float maxInterval = 15f;
    public float spawnZ = 12f;
    public float xMin = -8f;
    public float xMax = 8f;

    private static int aliveCount = 0;
    public static int AliveCount => aliveCount;

    public static void NotifyMeteorDestroyed()
    {
        aliveCount--;
        if (aliveCount < 0) aliveCount = 0;
    }

    void Start()
    {
        aliveCount = 0;
        ScheduleNextSpawn();
    }

    void ScheduleNextSpawn()
    {
        if (BossSpawner.BossHasSpawned) return;

        if (uiManager != null && uiManager.CurrentScore < minScoreToStart)
        {
            Invoke(nameof(ScheduleNextSpawn), 1f);
            return;
        }

        if (aliveCount >= maxAliveMeteors)
        {
            Invoke(nameof(ScheduleNextSpawn), 1f);
            return;
        }

        float delay = Random.Range(minInterval, maxInterval);
        Invoke(nameof(SpawnMeteor), delay);
    }

    void SpawnMeteor()
    {
        float x = Random.Range(xMin, xMax);
        Vector3 spawnPos = new Vector3(x, transform.position.y, spawnZ);
        Instantiate(meteorPrefab, spawnPos, meteorPrefab.transform.rotation);
        aliveCount++;
        ScheduleNextSpawn();
    }
}