using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    public GameObject barrelPrefab;
    [SerializeField] private UIManager uiManager;
    public float minInterval = 5f;
    public float maxInterval = 10f;
    public int bossPhaseScore = 1000;
    public float bossMinInterval = 2f;
    public float bossMaxInterval = 4f;
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
        Invoke(nameof(SpawnBarrel), delay);
    }

    void SpawnBarrel()
    {
        float x = Random.Range(xMin, xMax);
        Vector3 spawnPos = new Vector3(x, transform.position.y, spawnZ);
        GameObject barrel = Instantiate(barrelPrefab, spawnPos, barrelPrefab.transform.rotation);

        int ammoAmount = 10;
        if (uiManager != null)
        {
            int score = uiManager.CurrentScore;
            if (score >= 1000) ammoAmount = 30;
            else if (score >= 500) ammoAmount = 20;
        }

        AmmoPickup pickup = barrel.GetComponent<AmmoPickup>();
        if (pickup != null) pickup.ammoAmount = ammoAmount;

        ScheduleNextSpawn();
    }
}