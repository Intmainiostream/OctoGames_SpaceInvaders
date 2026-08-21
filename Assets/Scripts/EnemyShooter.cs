using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private AudioClip shootSfx;

    private float nextFireTime;

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireCooldown;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoints == null) return;

        foreach (Transform firePoint in firePoints)
        {
            if (firePoint == null) continue;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.isEnemyBullet = true;
                bulletScript.damage = damage;
            }

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = firePoint.forward * bulletSpeed;
        }

        if (shootSfx != null)
        {
            AudioSource.PlayClipAtPoint(shootSfx, transform.position);
        }
    }
}