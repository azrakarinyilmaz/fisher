using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CharControl char_controller;
    public SceneLoader loader;

    private void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");
        char_controller.GetComponent<Animator>().SetTrigger("isDead");
        //char_controller.GetComponent<CharControl>().enabled = false;
        //Destroy(char_controller.gameObject);

        //Time.timeScale = 0f; // Oyunu durdurur

        // Ýstersen burada panel açabilirsin
        // gameOverPanel.SetActive(true);
    }

    public void GameWin()
    {
        Debug.Log("YOU WIN!");
        char_controller.GetComponent<CharControl>().enabled = false;
        //yield return new WaitForSeconds(0.8f);
        loader.LoadMainScene();

        //Time.timeScale = 0f; // Oyunu durdurur

        // Ýstersen burada panel açabilirsin
        // gameOverPanel.SetActive(true);
    }
}