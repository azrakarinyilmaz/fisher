using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnseenFishManager : MonoBehaviour
{
    [Header("Prefabs")]
    public List<GameObject> FishList = new List<GameObject>();
    public GameObject BossFish;

    [Header("Camera / Spawn Area")]
    public Camera targetCamera;
    public float cameraSafeMargin = 0.8f; // world units inside camera bounds

    [Header("Spawn Timing")]
    public float minSpawnDelay = 1.5f;
    public float maxSpawnDelay = 4.0f;
    public int maxAliveFish = 8;

    [Header("Boss")]
    [Range(0f, 1f)] public float bossSpawnChance = 0.08f;

    [Header("Size (Random Scale)")]
    public float minScale = 0.85f;
    public float maxScale = 1.25f;

    [Header("Lifetime + Fade")]
    public float minLifeTime = 3.0f;
    public float maxLifeTime = 8.0f;
    public float fadeInTime = 0.6f;
    public float fadeOutTime = 0.6f;

    [Header("Optional Isometric Sorting")]
    public bool setSortingByY = false;
    public int sortingBase = 0;
    public int sortingMultiplier = 100;

    private readonly List<GameObject> alive = new List<GameObject>();
    private Coroutine loop;

    void Awake()
    {
        if (targetCamera == null) targetCamera = Camera.main;
    }

    void OnEnable()
    {
        loop = StartCoroutine(SpawnLoop());
    }

    void OnDisable()
    {
        if (loop != null) StopCoroutine(loop);
        loop = null;
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            CleanupDead();
            if (alive.Count >= maxAliveFish) continue;

            GameObject prefab = PickPrefab();
            if (prefab == null) continue;

            Vector3 pos = GetRandomPointInCamera(targetCamera, cameraSafeMargin);
            GameObject fish = Instantiate(prefab, pos, Quaternion.identity);

            // random size
            float s = Random.Range(minScale, maxScale);
            fish.transform.localScale = fish.transform.localScale * s;

            // start invisible
            SetAlphaAllRenderers(fish, 0f);

            // optional iso sorting
            if (setSortingByY) ApplySortingByY(fish);

            alive.Add(fish);

            float life = Random.Range(minLifeTime, maxLifeTime);
            StartCoroutine(FadeLifeCycle(fish, life));
        }
    }

    GameObject PickPrefab()
    {
        bool spawnBoss = (BossFish != null) && (Random.value < bossSpawnChance);
        if (spawnBoss) return BossFish;

        if (FishList == null || FishList.Count == 0) return null;
        return FishList[Random.Range(0, FishList.Count)];
    }

    IEnumerator FadeLifeCycle(GameObject fish, float lifeTime)
    {
        // Fade In
        if (fish != null && fadeInTime > 0f)
            yield return FadeAlphaAllRenderers(fish, 0f, 1f, fadeInTime);
        else
            SetAlphaAllRenderers(fish, 1f);

        // Stay visible
        float stay = Mathf.Max(0f, lifeTime - (fadeInTime + fadeOutTime));
        if (stay > 0f) yield return new WaitForSeconds(stay);

        // Fade Out
        if (fish != null && fadeOutTime > 0f)
            yield return FadeAlphaAllRenderers(fish, 1f, 0f, fadeOutTime);
        else
            SetAlphaAllRenderers(fish, 0f);

        // Destroy
        if (fish != null)
        {
            alive.Remove(fish);
            Destroy(fish);
        }
    }

    IEnumerator FadeAlphaAllRenderers(GameObject root, float from, float to, float duration)
    {
        if (root == null) yield break;

        SpriteRenderer[] srs = root.GetComponentsInChildren<SpriteRenderer>(true);
        float t = 0f;

        // store original colors (so we preserve RGB and just change A)
        Color[] baseColors = new Color[srs.Length];
        for (int i = 0; i < srs.Length; i++)
            baseColors[i] = srs[i] ? srs[i].color : Color.white;

        while (t < duration)
        {
            if (root == null) yield break;

            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            float a = Mathf.Lerp(from, to, k);

            for (int i = 0; i < srs.Length; i++)
            {
                if (!srs[i]) continue;
                Color c = baseColors[i];
                c.a = a;
                srs[i].color = c;
            }

            yield return null;
        }

        // ensure exact final alpha
        SetAlphaAllRenderers(root, to);
    }

    void SetAlphaAllRenderers(GameObject root, float alpha)
    {
        if (root == null) return;

        SpriteRenderer[] srs = root.GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < srs.Length; i++)
        {
            if (!srs[i]) continue;
            Color c = srs[i].color;
            c.a = alpha;
            srs[i].color = c;
        }
    }

    void ApplySortingByY(GameObject root)
    {
        if (root == null) return;

        int order = sortingBase + Mathf.RoundToInt(-root.transform.position.y * sortingMultiplier);
        SpriteRenderer[] srs = root.GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < srs.Length; i++)
        {
            if (!srs[i]) continue;
            srs[i].sortingOrder = order;
        }
    }

    void CleanupDead()
    {
        for (int i = alive.Count - 1; i >= 0; i--)
        {
            if (alive[i] == null) alive.RemoveAt(i);
        }
    }

    Vector3 GetRandomPointInCamera(Camera cam, float marginWorld)
    {
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        Vector3 camPos = cam.transform.position;

        float minX = camPos.x - halfW + marginWorld;
        float maxX = camPos.x + halfW - marginWorld;
        float minY = camPos.y - halfH + marginWorld;
        float maxY = camPos.y + halfH - marginWorld;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        return new Vector3(x, y, 0f);
    }
}