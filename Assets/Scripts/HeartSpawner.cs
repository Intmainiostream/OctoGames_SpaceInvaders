using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    public GameObject heartPrefab;
    [SerializeField] private UIManager uiManager;
    public float minInterval = 20f;
    public float maxInterval = 35f;
    public int bossPhaseScore = 1000;
    public float bossMinInterval = 8f;
    public float bossMaxInterval = 15f;
    public float spawnZ = 6f;
    public float xMin = -8f;
    public float xMax = 8f;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void ScheduleNextSpawn()
    {
        float delay;
        if (uiManager != null && uiManager.CurrentScore >= bossPhaseScore)
        {
            delay = Random.Range(bossMinInterval, bossMaxInterval);
        }
        else
        {
            delay = Random.Range(minInterval, maxInterval);
        }
        Invoke(nameof(SpawnHeart), delay);
    }

    void SpawnHeart()
    {
        float x = Random.Range(xMin, xMax);
        Vector3 spawnPos = new Vector3(x, transform.position.y, spawnZ);
        Instantiate(heartPrefab, spawnPos, heartPrefab.transform.rotation);
        ScheduleNextSpawn();
    }
}