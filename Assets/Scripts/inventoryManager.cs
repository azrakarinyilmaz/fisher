using System.Collections.Generic;
using UnityEngine;

public class inventoryManager : MonoBehaviour
{
    [Header("Money")]
    [SerializeField] private int coins = 0;
    public int Coins => coins;

    [Header("Rarity rule")]
    [Range(0f, 100f)]
    [SerializeField] private float rareThresholdPercent = 70f;

    // how many of each fish you’ve caught (by id/name)
    private Dictionary<string, int> caughtCounts = new Dictionary<string, int>();

    // list of fish ids that are considered "max rare" and have been caught at least once
    public HashSet<string> rareCaught = new HashSet<string>();

    /// <summary>Add a fish catch to inventory + money.</summary>
    public void RegisterCatch(FishClass fish)
    {
        if (fish == null) return;

        coins += fish.coin;

        string key = GetFishKey(fish);

        if (caughtCounts.ContainsKey(key)) caughtCounts[key]++;
        else caughtCounts[key] = 1;

        if (fish.rarityPercent > rareThresholdPercent)
            rareCaught.Add(key);

        Debug.Log($"Caught {key} | +{fish.coin} coins | Total={coins} | Rare(>{rareThresholdPercent})={rareCaught.Contains(key)}");
    }

    public int GetCaughtCount(FishClass fish)
    {
        if (fish == null) return 0;
        string key = GetFishKey(fish);
        return caughtCounts.TryGetValue(key, out int count) ? count : 0;
    }

    public int RareCaughtCount() => rareCaught.Count;

    private string GetFishKey(FishClass fish)
    {
        // Best: fish.id if you have it.
        // For now: use type (or name). Make sure it's unique.
        return fish.type;
    }
}