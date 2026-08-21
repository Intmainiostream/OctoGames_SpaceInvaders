using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{   
    [SerializeField] private AudioClip deathSfx;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private Vector2 xBounds = new Vector2(-8f, 8f);
    [SerializeField] private Vector2 zBounds = new Vector2(-4.5f, 4.5f);

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float fireCooldown = 0.25f;

    [SerializeField] private int maxAmmo = 50;


    [SerializeField] private float maxHealth = 5f;

    [SerializeField] private AudioClip shootSfx;

    [SerializeField] private AudioClip emptySfx;

    [SerializeField] private Renderer[] shipRenderers;
    [SerializeField] private GameObject deathVfx;
    private MaterialPropertyBlock glowBlock;
    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    private int currentAmmo;
    private float currentHealth;

    private float nextFireTime;
    private AudioSource audioSource;

    public System.Action<int, int> OnAmmoChanged;
    public System.Action<float, float> OnHealthChanged;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;
        currentHealth = maxHealth;
        glowBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        HandleMovement();
        HandleShootInput();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(h, 0f, v).normalized * moveSpeed * Time.deltaTime;
        Vector3 newPos = transform.position + move;

        newPos.x = Mathf.Clamp(newPos.x, xBounds.x, xBounds.y);
        newPos.z = Mathf.Clamp(newPos.z, zBounds.x, zBounds.y);

        transform.position = newPos;

        if (move != Vector3.zero)
        {
            Debug.Log("Ship moving, position: " + transform.position);
        }
    }

    private void HandleShootInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryFire();
        }
    }


    private void TryFire()
    {
        if (Time.time < nextFireTime) return;

        if (currentAmmo <= 0)
        {
            PlaySfx(emptySfx);
            return;
        }

        nextFireTime = Time.time + fireCooldown;
        currentAmmo--;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);

        SpawnBullet();
        PlaySfx(shootSfx);
    }

    private void SpawnBullet()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }
    }


    public void RefillAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, 99);
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }
        public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

public void TakeDamage(float amount)
{
    currentHealth -= amount;
    if (currentHealth < 0) currentHealth = 0;

    OnHealthChanged?.Invoke(currentHealth, maxHealth);

    if (currentHealth <= 0)
    {
        if (deathVfx != null) Instantiate(deathVfx, transform.position, Quaternion.identity);
        if (deathSfx != null) AudioSource.PlayClipAtPoint(deathSfx, transform.position);
        if (EndGameManager.Instance != null) EndGameManager.Instance.ShowYouDie();
        Destroy(gameObject);
    }
    else
    {
        TriggerGlow(Color.red * 3f, 0.8f);
    }
}
    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(9999);
            TakeDamage(currentHealth);
        }
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip != null) audioSource.PlayOneShot(clip);
    }

    public void TriggerGlow(Color color, float duration)
    {
        StartCoroutine(GlowRoutine(color, duration));
    }

    private System.Collections.IEnumerator GlowRoutine(Color color, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = 1f - (elapsed / duration);
            Color current = color * t;

            foreach (Renderer r in shipRenderers)
            {
                if (r == null) continue;
                r.GetPropertyBlock(glowBlock);
                glowBlock.SetColor(EmissionColorId, current);
                r.SetPropertyBlock(glowBlock);
            }

            yield return null;
        }

        foreach (Renderer r in shipRenderers)
        {
            if (r == null) continue;
            r.GetPropertyBlock(glowBlock);
            glowBlock.SetColor(EmissionColorId, Color.black);
            r.SetPropertyBlock(glowBlock);
        }
    }
}