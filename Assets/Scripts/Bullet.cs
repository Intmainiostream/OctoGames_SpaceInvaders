using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    public bool isEnemyBullet = false;
    public float damage = 1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEnemyBullet)
        {
            if (!other.CompareTag("Player")) return;

            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        else
        {
            if (other.CompareTag("Player")) return;

            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }

            MeteorHealth meteor = other.GetComponent<MeteorHealth>();
            if (meteor != null)
            {
                meteor.TakeDamage(1);
            }
        }

        Destroy(gameObject);
    }
}