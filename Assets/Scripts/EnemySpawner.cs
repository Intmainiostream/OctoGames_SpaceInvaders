using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int scoreCap = 1000;
    [SerializeField] private int minScoreToStart = 0;
    public float minInterval = 5f;
    public float maxInterval = 8f;

    public float topSpawnZ = 12f;
    public float formationTargetZ = 2f;

    public float xMin = -6f;
    public float xMax = 6f;
    public float formationSpacing = 1.5f;
    public float minFormationDistance = 4f;

    private class FormationInfo
    {
        public float centerX;
        public int aliveCount;
    }

    private List<FormationInfo> activeFormations = new List<FormationInfo>();

    [SerializeField] private int maxAliveEnemies = 12;
    private int totalAliveEnemies = 0;

    public static int TotalAliveAcrossAllSpawners = 0;

    void Start()
    {
        TotalAliveAcrossAllSpawners = 0;
        ScheduleNextSpawn();
    }

    void ScheduleNextSpawn()
    {
        if (uiManager != null && uiManager.CurrentScore >= scoreCap) return;

        if (uiManager != null && uiManager.CurrentScore < minScoreToStart)
        {
            Invoke(nameof(ScheduleNextSpawn), 1f);
            return;
        }

        if (totalAliveEnemies + 3 > maxAliveEnemies)
        {
            Invoke(nameof(ScheduleNextSpawn), 1f);
            return;
        }

        float delay = Random.Range(minInterval, maxInterval);
        Invoke(nameof(SpawnFormation), delay);
    }

    float PickCenterX()
    {
        for (int attempt = 0; attempt < 20; attempt++)
        {
            float candidate = Random.Range(xMin, xMax);
            bool tooClose = false;

            foreach (FormationInfo formation in activeFormations)
            {
                if (Mathf.Abs(candidate - formation.centerX) < minFormationDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose) return candidate;
        }

        return Random.Range(xMin, xMax);
    }

    void SpawnFormation()
    {
        float centerX = PickCenterX();

        FormationInfo formation = new FormationInfo { centerX = centerX, aliveCount = 3 };
        activeFormations.Add(formation);
        totalAliveEnemies += 3;
        TotalAliveAcrossAllSpawners += 3;

        Vector3[] offsets = new Vector3[]
        {
            new Vector3(0f, 0f, formationSpacing),
            new Vector3(-formationSpacing, 0f, 0f),
            new Vector3(formationSpacing, 0f, 0f)
        };

        foreach (Vector3 offset in offsets)
        {
            Vector3 spawnPos = new Vector3(centerX + offset.x, transform.position.y, topSpawnZ + offset.z);
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, enemyPrefab.transform.rotation);

            EnemyMover mover = enemy.GetComponent<EnemyMover>();
            if (mover != null) mover.targetZ = formationTargetZ + offset.z;

            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.OnDeath += () =>
                {
                    formation.aliveCount--;
                    totalAliveEnemies--;
                    TotalAliveAcrossAllSpawners--;
                    if (formation.aliveCount <= 0)
                    {
                        activeFormations.Remove(formation);
                    }
                    if (uiManager != null) uiManager.AddScore(health.ScoreValue);
                };
            }
        }

        ScheduleNextSpawn();
    }
}