using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public float healAmount = 1f;
    [SerializeField] private AudioClip pickupSfx;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null) return;

        player.Heal(healAmount);
        player.TriggerGlow(Color.yellow * 3f, 0.8f);

        if (pickupSfx != null)
        {
            AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
        }

        Debug.Log("Heart picked up, destroying now");
        Destroy(gameObject);
    }
}