using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance;

[SerializeField] private GameObject youDiePanel;
[SerializeField] private GameObject successPanel;
[SerializeField] private AudioClip youDieSfx;
[SerializeField] private AudioClip victorySfx;

[SerializeField] private AudioSource musicSource;
[SerializeField] private AudioClip bgMusic;

private AudioSource audioSource;

private void Awake()
{
    Instance = this;
    audioSource = GetComponent<AudioSource>();
}

private void Start()
{
    if (musicSource != null && bgMusic != null)
    {
        musicSource.clip = bgMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
}

public void ShowYouDie()
{
    if (youDiePanel != null) youDiePanel.SetActive(true);
    if (youDieSfx != null) audioSource.PlayOneShot(youDieSfx);
    if (musicSource != null) musicSource.Stop();
    Time.timeScale = 0f;
}

public void ShowMissionComplete()
{
    if (successPanel != null) successPanel.SetActive(true);
    if (victorySfx != null) audioSource.PlayOneShot(victorySfx);
    if (musicSource != null) musicSource.Stop();
    Time.timeScale = 0f;
}

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}