using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterColorByDay: MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [Header("Tints")]
    [SerializeField] private Color dayColor;
    [SerializeField] private Color nightColor = new Color(0.55f, 0.65f, 1f, 1f);

    [Header("Cycle")]
    [SerializeField] private float dayLengthSeconds = 60f; // full cycle time
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    float t;
    

    void Reset() => tilemap =  GetComponent<Tilemap>();

    void Update()
    {
        if (!tilemap) return;

        t += Time.deltaTime / Mathf.Max(0.01f, dayLengthSeconds);
        float phase = Mathf.PingPong(t, 1f);             // 0..1..0
        float k = curve.Evaluate(phase);                 // smoother transitions

        tilemap.color = Color.Lerp(dayColor, nightColor, k);
    }
}
