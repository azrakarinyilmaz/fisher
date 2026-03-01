using UnityEngine;

public class gaugeSpawner : StateMachineBehaviour
{
    public GameObject uiPrefab;

    [Header("Exact RectTransform Values")]
    public Vector2 anchoredPos = new Vector2(-350f, 180f);
    public Vector2 size = new Vector2(600f, 300f);
    public Vector2 pivot = new Vector2(0.5f, 0.5f);

    private GameObject spawnedUI;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!uiPrefab) return;

        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (!canvas) { Debug.LogWarning("No Canvas found!"); return; }

        spawnedUI = Object.Instantiate(uiPrefab);
        RectTransform rt = spawnedUI.GetComponent<RectTransform>();
        if (!rt) { Debug.LogWarning("Prefab is not UI (no RectTransform)"); Object.Destroy(spawnedUI); return; }

        // UI için kritik: parent set ederken worldPositionStays = false
        rt.SetParent(canvas.transform, false);

        // Birebir ayný davranýþ için anchor'ý center'a sabitle
        rt.anchorMin = new Vector2(1f, 0f);
        rt.anchorMax = new Vector2(1f, 0f);

        rt.pivot = pivot;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;

        rt.localRotation = Quaternion.identity;
        rt.localScale = Vector3.one;
    }

   
}