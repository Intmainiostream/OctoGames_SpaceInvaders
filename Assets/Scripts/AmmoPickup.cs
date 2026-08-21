using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 20;
    [SerializeField] private AudioClip pickupSfx;

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player")) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null) return;

        player.RefillAmmo(ammoAmount);
        player.TriggerGlow(Color.yellow * 3f, 0.8f);

        if (pickupSfx != null)
        {
            AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
        }

        Debug.Log("Barrel picked up, destroying now");
        Destroy(gameObject);
    }
}