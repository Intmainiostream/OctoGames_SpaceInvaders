using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public enum EnemyPhase { Phase1, Phase2, Boss }

    [SerializeField] private EnemyPhase phase = EnemyPhase.Phase1;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private GameObject explosionVfx;
    [SerializeField] private AudioClip explosionSfx;
    [SerializeField] private Renderer[] enemyRenderers;
    private MaterialPropertyBlock glowBlock;
    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");
    private int currentHealth;

    public System.Action OnDeath;
    public EnemyPhase Phase => phase;
    public int ScoreValue => scoreValue;

    private void Awake()
    {
        currentHealth = maxHealth;
        glowBlock = new MaterialPropertyBlock();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (explosionVfx != null)
            {
                Instantiate(explosionVfx, transform.position, Quaternion.identity);
            }
            if (explosionSfx != null)
            {
                AudioSource.PlayClipAtPoint(explosionSfx, transform.position);
            }
            Destroy(gameObject);
        }
        else
        {
            TriggerGlow(Color.red * 3f, 0.3f);
        }
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

            foreach (Renderer r in enemyRenderers)
            {
                if (r == null) continue;
                r.GetPropertyBlock(glowBlock);
                glowBlock.SetColor(EmissionColorId, current);
                r.SetPropertyBlock(glowBlock);
            }

            yield return null;
        }

        foreach (Renderer r in enemyRenderers)
        {
            if (r == null) continue;
            r.GetPropertyBlock(glowBlock);
            glowBlock.SetColor(EmissionColorId, Color.black);
            r.SetPropertyBlock(glowBlock);
        }
    }

    private void OnDestroy()
    {
        OnDeath?.Invoke();
    }
}