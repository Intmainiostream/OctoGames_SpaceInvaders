using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 3f;
    public float targetZ = 2f;

    [SerializeField] private float idleRadius = 0.5f;
    [SerializeField] private float idleSpeed = 1.5f;

    private EnemyShooter shooter;
    private bool inFormation;
    private Vector3 formationCenter;
    private float idleTimeOffset;

    private void Awake()
    {
        shooter = GetComponent<EnemyShooter>();
        if (shooter != null) shooter.enabled = false;
        idleTimeOffset = Random.Range(0f, 10f);
    }

    private void Update()
    {
        if (!inFormation)
        {
            if (transform.position.z > targetZ)
            {
                transform.position += Vector3.back * fallSpeed * Time.deltaTime;
            }
            else
            {
                inFormation = true;
                formationCenter = transform.position;
                if (shooter != null) shooter.enabled = true;
            }
        }
        else
        {
            float t = (Time.time + idleTimeOffset) * idleSpeed;
            float x = Mathf.Sin(t) * idleRadius;
            float z = Mathf.Cos(t) * idleRadius * 0.5f;

            transform.position = formationCenter + new Vector3(x, 0f, z);
        }
    }
}