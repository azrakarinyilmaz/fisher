using System.Collections.Generic;
using UnityEngine;

public class inventoryManager : MonoBehaviour
{
    public static inventoryManager Instance;

    [Header("Money")]
    [SerializeField] private int coins = 0;
    public int Coins => coins;

    [Header("Rarity rule")]
    [Range(0f, 100f)]
    [SerializeField] private float rareThresholdPercent = 70f;

    private Dictionary<string, int> caughtCounts = new Dictionary<string, int>();
    public HashSet<string> rareCaught = new HashSet<string>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public void RegisterCatch(FishClass fish)
    {
        if (fish == null) return;

        string key = fish.f_name;

        coins += fish.coin;

        if (caughtCounts.ContainsKey(key))
        {
            Debug.Log($"Caught AGAIN {key} | +{fish.coin} coins | Total={coins}");
            return;
        }

        caughtCounts[key] = 1;

        if (fish.rarityPercent >= rareThresholdPercent)
            rareCaught.Add(key);

        Debug.Log($"Caught NEW {key} | +{fish.coin} coins | Total={coins}");
    }

    public int GetCaughtCount(string fishName)
    {
        return caughtCounts.TryGetValue(fishName, out int count) ? count : 0;
    }

    public int RareCaughtCount() => rareCaught.Count;
}