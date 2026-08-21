using UnityEngine;

public class MeteorHealth : MonoBehaviour
{
    [SerializeField] private bool isHalf = false;
    [SerializeField] private GameObject crackVfx;
    [SerializeField] private GameObject meteorHalfPrefab;
    [SerializeField] private GameObject deathVfx;
    [SerializeField] private AudioClip meteorSfx;

    public void TakeDamage(int amount)
    {
        if (crackVfx != null)
        {
            Instantiate(crackVfx, transform.position, Quaternion.identity);
        }

        if (meteorSfx != null)
        {
            AudioSource.PlayClipAtPoint(meteorSfx, transform.position);
        }

        if (!isHalf && meteorHalfPrefab != null)
        {
            Instantiate(meteorHalfPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.TakeDamage(9999f);
        }

        if (deathVfx != null)
        {
            Instantiate(deathVfx, transform.position, Quaternion.identity);
        }

        if (meteorSfx != null)
        {
            AudioSource.PlayClipAtPoint(meteorSfx, transform.position);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        MeteorSpawner.NotifyMeteorDestroyed();
    }
}