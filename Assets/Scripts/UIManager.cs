using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    private int score = 0;
    private int highScore = 0;
    private const string HighScoreKey = "HighScore";

    public int CurrentScore => score;

    private void Awake()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        UpdateScoreText();
        UpdateHighScoreText();
    }

    private void OnEnable()
    {
        if (player == null) return;
        player.OnHealthChanged += UpdateHealthText;
        player.OnAmmoChanged += UpdateAmmoText;
    }

    private void OnDisable()
    {
        if (player == null) return;
        player.OnHealthChanged -= UpdateHealthText;
        player.OnAmmoChanged -= UpdateAmmoText;
    }

    private void UpdateHealthText(float current, float max)
    {
        if (healthText != null) healthText.text = "Life: " + Mathf.CeilToInt(current);
    }

    private void UpdateAmmoText(int current, int max)
    {
        if (ammoText != null) ammoText.text = "Ammo: " + current;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            UpdateHighScoreText();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null) highScoreText.text = "High Score: " + highScore;
    }
}