using UnityEngine;

public class CatchTheFish : MonoBehaviour
{
    [Header("Green Zone (random)")]
    [SerializeField] private float zoneMinLow = 10f;     // min possible start of zone
    [SerializeField] private float zoneMaxLow = 70f;     // max possible start of zone
    [SerializeField] private float zoneMinWidth = 10f;   // min zone width
    [SerializeField] private float zoneMaxWidth = 30f;   // max zone width

    [Header("Value (0..100)")]
    [SerializeField] private float value = 50f;          // current value
    [SerializeField] private float pressImpulse = 6f;    // add on each Space press
    [SerializeField] private float decayPerSec = 18f;    // subtract per second when not pressing
    [SerializeField] private float clampMin = 0f;
    [SerializeField] private float clampMax = 100f;

    [Header("Win / Lose Timers")]
    [SerializeField] private float successSeconds = 2.0f; // must stay inside zone this long
    [SerializeField] private float failSeconds = 3.0f;    // if outside accumulates this long -> lose


    [Header("Runtime (read-only)")]
    [SerializeField] private float zoneMin;
    [SerializeField] private float zoneMax;
    [SerializeField] private float inZoneTime;
    [SerializeField] private float outZoneTime;
    [SerializeField] private bool isRunning = true;

    public System.Action OnWin;
    public System.Action OnLose;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NewRound();
    }

    public void NewRound()
    {
        // Create a random zone [zoneMin, zoneMax] within 0..100
        float start = Random.Range(zoneMinLow, zoneMaxLow);
        float width = Random.Range(zoneMinWidth, zoneMaxWidth);

        zoneMin = start;
        zoneMax = Mathf.Min(start + width, clampMax);

        // Ensure zoneMin <= zoneMax and not too tiny
        if (zoneMax - zoneMin < 5f) zoneMax = Mathf.Min(zoneMin + 5f, clampMax);

        value = Mathf.Clamp(value, clampMin, clampMax);
        inZoneTime = 0f;
        outZoneTime = 0f;
        isRunning = true;

        // Debug
        Debug.Log($"Green Zone: {zoneMin:0.0} - {zoneMax:0.0} | Start Value: {value:0.0}");
    }

    void Update()
    {
        if (!isRunning) return;

        float dt = Time.deltaTime;

        // Space spam impulse
        if (Input.GetKeyDown(KeyCode.Space))
            value += pressImpulse;

        // Natural decay
        value -= decayPerSec * dt;
        value = Mathf.Clamp(value, clampMin, clampMax);

        // Zone check
        bool inZone = (value >= zoneMin && value <= zoneMax);

        if (inZone)
        {
            inZoneTime += dt;
            outZoneTime = Mathf.Max(0f, outZoneTime - dt); // optional forgiveness
        }
        else
        {
            outZoneTime += dt;
            inZoneTime = Mathf.Max(0f, inZoneTime - dt * 0.5f); // optional partial decay
        }
        /*
        // Win/Lose
        if (inZoneTime >= successSeconds)
        {
            isRunning = false;
            Debug.Log("WIN (kept value in green zone long enough)");
            OnWin?.Invoke();
        }
        else if (outZoneTime >= failSeconds)
        {
            isRunning = false;
            Debug.Log("LOSE (failed to keep value in green zone)");
            OnLose?.Invoke();
        }*/
    }

    // Useful getters if you want to display UI later
    public float GetValue01() => Mathf.InverseLerp(clampMin, clampMax, value);
    public (float min, float max) GetZone() => (zoneMin, zoneMax);
    public float GetInZoneProgress01() => Mathf.Clamp01(inZoneTime / Mathf.Max(0.01f, successSeconds));
}
