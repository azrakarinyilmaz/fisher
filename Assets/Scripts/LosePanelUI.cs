using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanelUI : MonoBehaviour
{
    [Header("Scene Names (exact)")]
    [SerializeField] private string dungeonSceneName = "dungeon";
    [SerializeField] private string menuSceneName = "MenuScene";

    void Awake()
    {
        // Oyun başlarken panel kapalı olsun
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f; // oyunu durdur
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(dungeonSceneName);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}