using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Image icon;

    private FishClass fish;

    void Start()
    {
        Catched();
    }

    public void Catched()
    {
        if (FishList == null || FishList.Count == 0) return;

        fish = FishList[Random.Range(0, FishList.Count)];

        if (fishPanel != null)
            fishPanel.SetActive(true);

        if (icon != null)
            icon.sprite = fish.icon;

        if (typeText != null)
            typeText.text = $"Type: {fish.type}";

        if (rarityText != null)
            rarityText.text = $"Rarity: {fish.rarityPercent}%";

        if (valueText != null)
            valueText.text = $"Value: {fish.coin} ";
    }
}