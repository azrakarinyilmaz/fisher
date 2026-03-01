using UnityEngine;
using System.Collections;

public class CatchTheFish : MonoBehaviour
{
 

    [Header("References (assign in Inspector)")]
    [SerializeField] private GameObject bar;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform fish;
    [SerializeField] private Transform safeZone;

    [SerializeField] private Collider2D fishCollider;
    [SerializeField] private Collider2D safeZoneCollider;

    [Header("Safe Zone X Range (LOCAL)")]
    [SerializeField] private float safeMinLocalX = -4f;
    [SerializeField] private float safeMaxLocalX = 4f;

    [Header("Fish X Range (LOCAL)")]
    [SerializeField] private float fishMinLocalX = 0f;
    [SerializeField] private float fishMaxLocalX = 7f;

    [Header("Fish Control (space spam)")]
    [SerializeField] private float impulsePerPress = 2.5f;
    [SerializeField] private float fishDrag = 7f;
    [SerializeField] private float fishMaxSpeed = 7f;

    [Header("Fail Rule")]
    [SerializeField] private float failAfterSecondsOutside = 5f;

    [Header("Safe Zone Stress Motion")]
    [SerializeField] private float accelNormalMin = 0.6f;
    [SerializeField] private float accelNormalMax = 1.8f;

    [SerializeField] private float accelBurstMin = 3.0f;
    [SerializeField] private float accelBurstMax = 7.0f;

    [SerializeField] private float speedCapNormal = 1.2f;
    [SerializeField] private float speedCapBurst = 3.4f;

    [SerializeField] private float safeDamping = 2.6f;

    [Header("Random Event Timing")]
    [SerializeField] private float eventIntervalMin = 0.35f;
    [SerializeField] private float eventIntervalMax = 0.95f;

    [SerializeField] private float burstDurationMin = 0.18f;
    [SerializeField] private float burstDurationMax = 0.45f;

    [Header("Rare Downside (fixed left velocity)")]
    [SerializeField] private float downsideChance = 0.08f;
    [SerializeField] private float downsideFixedSpeed = 1.1f;
    [SerializeField] private float downsideDurationMin = 0.20f;
    [SerializeField] private float downsideDurationMax = 0.55f;

    [Header("Runtime (read-only)")]
    [SerializeField] private bool running = true;
    [SerializeField] private float timeOutside = 0f;

    private float fishVel;
    private float safeVel;
    private float safeAccel;

    public Animator animator;

    private GameObject currentBar=null;

    private enum SafeMode
    {
        Normal,
        Burst,
        Downside
    }

    [SerializeField] private SafeMode mode = SafeMode.Normal;

    private float modeEndTime;
    private float nextEventTime;

    public System.Action OnWin;
    public System.Action OnLose;

    void Start()
    {
        //NewRound();
    }

    public void NewRound()
    {
        animator.SetBool("roll", true);
        currentBar = Instantiate(bar, spawnPoint.position, spawnPoint.rotation);
        AutoAssignReferences(currentBar);
        running = true;

        fishVel = 0f;
        safeVel = 0f;
        safeAccel = 0f;

        timeOutside = 0f;

        if (fish != null)
        {
            SetLocalX(fish, fishMinLocalX);
        }

        if (safeZone != null)
        {
            SetLocalX(safeZone, safeMinLocalX);
        }

        mode = SafeMode.Normal;
        PickNormalAcceleration();

        nextEventTime = Time.time + Random.Range(eventIntervalMin, eventIntervalMax);
        modeEndTime = 0f;
    }
    void AutoAssignReferences(GameObject barInstance)
    {
        // Find children by name (must match EXACTLY in prefab)
        fish = barInstance.transform.Find("ip");
        safeZone = barInstance.transform.Find("safe_zone");

        if (fish != null)
            fishCollider = fish.GetComponent<Collider2D>();

        if (safeZone != null)
            safeZoneCollider = safeZone.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (running == false)
        {
            return;
        }

        if (fish == null || safeZone == null)
        {
            return;
        }

        float dt = Time.deltaTime;

        UpdateSafeZone(dt);
        UpdateFish(dt);

        bool inZone = IsFishInSafeZoneByBounds();

        if (inZone == true)
        {
            timeOutside = 0f;
        }
        else
        {
            timeOutside += dt;
        }

        if (timeOutside >= failAfterSecondsOutside)
        {
            Lose("Outside safe zone too long");
            return;
        }

        float safeLocalX = safeZone.localPosition.x;

        // WIN only if safe zone reached end AND fish is currently in the zone
        if (safeLocalX >= safeMaxLocalX - 0.0001f)
        {
            if (fish.localPosition.x == 7 && inZone)
            {
                Win("Safe zone reached end while fish inside");
                return;
            }
        }
        
    }

    void UpdateFish(float dt)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fishVel += impulsePerPress;
        }

        fishVel -= fishDrag * dt;
        fishVel = Mathf.Clamp(fishVel, -fishMaxSpeed, fishMaxSpeed);

        float newX = fish.localPosition.x + fishVel * dt;

        if (newX < fishMinLocalX)
        {
            newX = fishMinLocalX;
            fishVel = 0f;
        }

        if (newX > fishMaxLocalX)
        {
            newX = fishMaxLocalX;
            fishVel = 0f;
        }

        SetLocalX(fish, newX);
    }

    void UpdateSafeZone(float dt)
    {
        if (mode != SafeMode.Downside)
        {
            if (Time.time >= nextEventTime)
            {
                StartRandomEvent();
                nextEventTime = Time.time + Random.Range(eventIntervalMin, eventIntervalMax);
            }
        }

        if (mode == SafeMode.Burst)
        {
            if (Time.time >= modeEndTime)
            {
                mode = SafeMode.Normal;
                PickNormalAcceleration();
            }
        }
        else if (mode == SafeMode.Downside)
        {
            if (Time.time >= modeEndTime)
            {
                mode = SafeMode.Normal;
                PickNormalAcceleration();
                nextEventTime = Time.time + Random.Range(eventIntervalMin, eventIntervalMax);
            }
        }

        float speedCap = speedCapNormal;
        if (mode == SafeMode.Burst)
        {
            speedCap = speedCapBurst;
        }

        if (mode == SafeMode.Downside)
        {
            safeVel = -Mathf.Abs(downsideFixedSpeed);
        }
        else
        {
            safeVel += safeAccel * dt;

            float damp = safeDamping * dt;
            safeVel = safeVel * (1f / (1f + damp));

            safeVel = Mathf.Clamp(safeVel, -speedCap, speedCap);
        }

        float newX = safeZone.localPosition.x + safeVel * dt;

        if (newX < safeMinLocalX)
        {
            newX = safeMinLocalX;
            safeVel = 0f;

            mode = SafeMode.Normal;
            PickNormalAcceleration();

            nextEventTime = Time.time + Random.Range(eventIntervalMin, eventIntervalMax);
        }

        if (newX > safeMaxLocalX)
        {
            newX = safeMaxLocalX;
            safeVel = 0f;
        }

        SetLocalX(safeZone, newX);
    }

    void StartRandomEvent()
    {
        float r = Random.value;

        if (r < downsideChance)
        {
            mode = SafeMode.Downside;
            safeAccel = 0f;
            modeEndTime = Time.time + Random.Range(downsideDurationMin, downsideDurationMax);
        }
        else
        {
            mode = SafeMode.Burst;
            PickBurstAcceleration();
            modeEndTime = Time.time + Random.Range(burstDurationMin, burstDurationMax);
        }
    }

    void PickNormalAcceleration()
    {
        float a = Random.Range(accelNormalMin, accelNormalMax);
        safeAccel = Mathf.Abs(a);
    }

    void PickBurstAcceleration()
    {
        float a = Random.Range(accelBurstMin, accelBurstMax);
        safeAccel = Mathf.Abs(a);
    }

    bool IsFishInSafeZoneByBounds()
    {
        if (fishCollider == null)
        {
            return false;
        }

        if (safeZoneCollider == null)
        {
            return false;
        }

        if (fishCollider.bounds.Intersects(safeZoneCollider.bounds))
        {
            return true;
        }

        return false;
    }

    void SetLocalX(Transform t, float x)
    {
        Vector3 p = t.localPosition;
        p.x = x;
        t.localPosition = p;
    }

    void Win(string reason)
    {
        running = false;
        Debug.Log("WIN: " + reason);

        // ALWAYS destroy spawned bar
        if (currentBar != null)
        {
            Destroy(currentBar);
            currentBar = null;
        }

        // Optional event
        OnWin?.Invoke();
        animator.SetTrigger("pull");

        // Optional: clear refs so Update() doesn't keep running on null stuff
        fish = null;
        safeZone = null;
        fishCollider = null;
        safeZoneCollider = null;
    }

    void Lose(string reason)
    {
        running = false;
        Debug.Log("LOSE: " + reason);
        

        // ALWAYS destroy spawned bar
        if (currentBar != null)
        {
            Destroy(currentBar);
            currentBar = null;
        }

        // Optional event
        OnLose?.Invoke();
        animator.SetBool("roll", false);
        fish = null;
        safeZone = null;
        fishCollider = null;
        safeZoneCollider = null;
    }
}