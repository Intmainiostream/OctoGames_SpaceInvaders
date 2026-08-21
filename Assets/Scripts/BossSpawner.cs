using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject corePrefab;
    public GameObject leftWingPrefab;
    public GameObject rightWingPrefab;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private int minScoreToStart = 1000;

    public float spawnZ = 10f;
    public float spawnX = 0f;

    [SerializeField] private float leftWingOffsetX = -3f;
    [SerializeField] private float rightWingOffsetX = 3f;

    private bool coreAlive = false;
    private bool bossSpawned = false;
    public static bool BossHasSpawned = false;

    void Start()
    {
        BossHasSpawned = false;
        ScheduleCheck();
    }

    void ScheduleCheck()
    {
        if (bossSpawned) return;

        if (uiManager != null && uiManager.CurrentScore < minScoreToStart)
        {
            Invoke(nameof(ScheduleCheck), 1f);
            return;
        }

        if (EnemySpawner.TotalAliveAcrossAllSpawners > 0)
        {
            Invoke(nameof(ScheduleCheck), 1f);
            return;
        }

        if (MeteorSpawner.AliveCount > 0)
        {
            Invoke(nameof(ScheduleCheck), 1f);
            return;
        }

        if (coreAlive)
        {
            Invoke(nameof(ScheduleCheck), 1f);
            return;
        }

        SpawnBoss();
    }

    void SpawnBoss()
    {
        Vector3 corePos = new Vector3(spawnX, transform.position.y, spawnZ);
        GameObject core = Instantiate(corePrefab, corePos, corePrefab.transform.rotation);

        coreAlive = true;
        bossSpawned = true;
        BossHasSpawned = true;

        int bossPartsAlive = 3;

        EnemyHealth coreHealth = core.GetComponent<EnemyHealth>();
        if (coreHealth != null)
        {
            coreHealth.OnDeath += () =>
            {
                coreAlive = false;
                bossPartsAlive--;
                Debug.Log($"Boss CORE died. Remaining: {bossPartsAlive}");
                if (bossPartsAlive <= 0 && EndGameManager.Instance != null)
                {
                    EndGameManager.Instance.ShowMissionComplete();
                }
            };
        }

        if (leftWingPrefab != null)
        {
            Vector3 leftPos = corePos + new Vector3(leftWingOffsetX, 0f, 0f);
            GameObject leftWing = Instantiate(leftWingPrefab, leftPos, leftWingPrefab.transform.rotation);

            EnemyHealth leftHealth = leftWing.GetComponent<EnemyHealth>();
            if (leftHealth != null)
            {
                leftHealth.OnDeath += () =>
                {
                    bossPartsAlive--;
                    Debug.Log($"Boss LEFT WING died. Remaining: {bossPartsAlive}");
                    if (bossPartsAlive <= 0 && EndGameManager.Instance != null)
                    {
                        EndGameManager.Instance.ShowMissionComplete();
                    }
                };
            }
        }

        if (rightWingPrefab != null)
        {
            Vector3 rightPos = corePos + new Vector3(rightWingOffsetX, 0f, 0f);
            GameObject rightWing = Instantiate(rightWingPrefab, rightPos, rightWingPrefab.transform.rotation);

            EnemyHealth rightHealth = rightWing.GetComponent<EnemyHealth>();
            if (rightHealth != null)
            {
                rightHealth.OnDeath += () =>
                {
                    bossPartsAlive--;
                    Debug.Log($"Boss RIGHT WING died. Remaining: {bossPartsAlive}");
                    if (bossPartsAlive <= 0 && EndGameManager.Instance != null)
                    {
                        EndGameManager.Instance.ShowMissionComplete();
                    }
                };
            }
        }
    }
}