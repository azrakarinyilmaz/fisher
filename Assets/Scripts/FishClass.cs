using UnityEngine;

public class FishClass : MonoBehaviour
{
    public string f_name;
    public Sprite icon;

    public string type;
    [Range(0, 100)] public int rarityPercent;
    public int coin;
}