using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CharControl char_controller;

    private void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");
        char_controller.GetComponent<CharControl>().enabled = false;

        //Time.timeScale = 0f; // Oyunu durdurur

        // Ýstersen burada panel açabilirsin
        // gameOverPanel.SetActive(true);
    }

    public void GameWin()
    {
        Debug.Log("YOU WIN!");
        char_controller.GetComponent<CharControl>().enabled = false;

        //Time.timeScale = 0f; // Oyunu durdurur

        // Ýstersen burada panel açabilirsin
        // gameOverPanel.SetActive(true);
    }
}