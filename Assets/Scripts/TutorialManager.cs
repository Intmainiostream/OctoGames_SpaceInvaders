using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;

    private void Start()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}