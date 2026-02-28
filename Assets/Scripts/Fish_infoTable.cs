using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fish_infoTable : MonoBehaviour
{
    public List<FishClass> FishList = new List<FishClass>();
    public TMP_Text fishNameText;
    public TMP_Text fishDescText;
    private FishClass fish;
    public GameObject fishPanel;
    public Image icon=null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Start()
    {
        //icon = fishPanel.GetComponentInChildren<Image>();
        Catched();
    }
    void Update()
    {
        
    }
    public void Catched()
    {
        
        if (FishList.Count == 0) { return; }
        fish =FishList[Random.Range(0, FishList.Count)];

        icon.sprite = fish.icon;
        fishNameText.text = fish.f_name;
        fishDescText.text = fish.description;

    }
}
