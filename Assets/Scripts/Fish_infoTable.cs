using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Fish_infoTable : MonoBehaviour
{
    [Header("Fish List")]
    public List<FishClass> FishList = new List<FishClass>();

    [Header("UI Texts")]
    public TMP_Text typeText;
    public TMP_Text rarityText;
    public TMP_Text valueText;

    [Header("UI")]
    public GameObject fishPanel;
    public UnityEngine.UI.Image icon;

    private FishClass fish;

    [SerializeField] private CatchTheFish manager;
    [SerializeField] private inventoryManager progress;
    public RectTransform winPanel;
    [SerializeField] private float panelSpeed = 500f;

    void Start()
    {
        //Catched();
         winPanel = fishPanel.GetComponent<RectTransform>();
    }
    void OnEnable()
    {
        if (manager != null)
        {
            manager.OnWin += HandleWin;
        }
    }

    void OnDisable()
    {
        if (manager != null)
        {
            manager.OnWin -= HandleWin;
        }
    }
    void HandleWin()
    {
        // call whatever function you want here
        Catched();
        StopAllCoroutines();
        StartCoroutine(MovePanelRoutine());
    }
    IEnumerator MovePanelRoutine()
    {
        // Start at -200
        Vector2 p = winPanel.anchoredPosition;
        p.y = -200f;
        winPanel.anchoredPosition = p;

        // Move UP to 200
        while (winPanel.anchoredPosition.y != 200f)
        {
            p = winPanel.anchoredPosition;
            p.y = Mathf.MoveTowards(p.y, 200f, panelSpeed * Time.deltaTime);
            winPanel.anchoredPosition = p;
            yield return null;
        }

        yield return new WaitForSeconds(7f);

        // Move DOWN to -200
        while (winPanel.anchoredPosition.y != -200f)
        {
            p = winPanel.anchoredPosition;
            p.y = Mathf.MoveTowards(p.y, -200f, panelSpeed * Time.deltaTime);
            winPanel.anchoredPosition = p;
            yield return null;
        }
    }
    public void Catched()
    {
        if (FishList == null || FishList.Count == 0) return;

        fish = FishList[Random.Range(0, FishList.Count)];

        if (progress != null)
            progress.RegisterCatch(fish);

        if (fishPanel != null)
            fishPanel.SetActive(true);

        if (icon != null)
            icon.sprite = fish.icon;

        if (typeText != null)
            typeText.text = $"Type: {fish.type}";

        if (rarityText != null)
            rarityText.text = $"Rarity: {fish.rarityPercent}%";

        if (valueText != null)
            valueText.text = $"Value: {fish.coin} Coins";

       
    }
    
}