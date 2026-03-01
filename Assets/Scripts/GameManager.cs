using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private LosePanelUI losePanel; // LosePanelUI referansı

    public CharControl char_controller;
    public SceneLoader loader;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Time.timeScale = 1f; 
    }

    public void GameOver()
{
    Debug.Log("GAME OVER");

    // 1) Önce paneli aç (UI kesin çalışsın)
    if (losePanel != null)
        losePanel.Show();
    else
        Debug.LogWarning("LosePanel reference missing! GameManager -> losePanel bağla.");

    // 2) Sonra karakter kontrolünü kapat (null güvenli)
    if (char_controller != null)
    {

        var cc = char_controller.GetComponent<CharControl>();
        if (cc != null) cc.enabled = false;
        else Debug.LogWarning("CharControl component not found on char_controller!");

        char_controller.GetComponent<Animator>().SetTrigger("isDead");

    }
    else
    {
        // char_controller inspector'dan bağlanmadıysa otomatik bulmayı dene
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var cc = player.GetComponent<CharControl>();
            if (cc != null) cc.enabled = false;
            else Debug.LogWarning("Player found but CharControl missing on Player!");
        }
        else
        {
            Debug.LogWarning("char_controller is null AND no GameObject with tag 'Player' found!");
        }
    }

    // İstersen isabetli dursun diye:
    // Time.timeScale = 0f;  (Bunu zaten LosePanelUI.Show içinde yapıyorsun)
}

    public void GameWin()
    {
        Debug.Log("YOU WIN!");
        char_controller.GetComponent<CharControl>().enabled = false;
        //yield return new WaitForSeconds(0.8f);
        loader.LoadMainScene();

        //Time.timeScale = 0f; // Oyunu durdurur

        // �stersen burada panel a�abilirsin
        // gameOverPanel.SetActive(true);
    }
}