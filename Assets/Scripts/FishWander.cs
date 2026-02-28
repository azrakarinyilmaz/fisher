using UnityEngine;

public class FishWanderInsideCamera : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1.2f;
    public float minDirectionTime = 5f;
    public float maxDirectionTime = 10f;

    private float changeTimer;
    private float nextChangeTime;

    [Header("Camera Bounds")]
    public Camera targetCamera;
    public float safeMargin = 0.5f;

    [Header("Rotation")]
    public bool rotateToMove = true;
    public float rotateLerp = 3; // higher = snappier
    public float angleOffsetDeg = 0f; // if your art isn't perfectly "up", tweak this (+/-)


    private Vector2 direction;

    void Start()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        PickNewDirection();
        changeTimer = 0f;
        nextChangeTime = Random.Range(minDirectionTime, maxDirectionTime);
        ApplyRotationInstant();
    }

    void Update()
    {
        changeTimer += Time.deltaTime;
        if (changeTimer >= nextChangeTime)
        {
            changeTimer = 0f;
            PickNewDirection();
        }

        Move();
        KeepInsideCamera();
        ApplyRotationSmooth();
    }

    void Move()
    {
        Vector3 next = transform.position + (Vector3)(direction * speed * Time.deltaTime);

       

        transform.position = next;
    }

    void KeepInsideCamera()
    {
        float halfH = targetCamera.orthographicSize;
        float halfW = halfH * targetCamera.aspect;

        Vector3 camPos = targetCamera.transform.position;

        float minX = camPos.x - halfW + safeMargin;
        float maxX = camPos.x + halfW - safeMargin;
        float minY = camPos.y - halfH + safeMargin;
        float maxY = camPos.y + halfH - safeMargin;

        Vector3 pos = transform.position;
        bool hitEdge = false;

        if (pos.x < minX) { pos.x = minX; hitEdge = true; }
        if (pos.x > maxX) { pos.x = maxX; hitEdge = true; }
        if (pos.y < minY) { pos.y = minY; hitEdge = true; }
        if (pos.y > maxY) { pos.y = maxY; hitEdge = true; }

        transform.position = pos;

        if (hitEdge)
        {
            PickNewDirection();

            // Reset "time passed since latest tilt/direction change"
            changeTimer = 0f;

            // Optional but recommended: pick a fresh interval so it doesn't feel patterned
            nextChangeTime = Random.Range(minDirectionTime, maxDirectionTime);
        }
    }

    void PickNewDirection()
    {
        direction = Random.insideUnitCircle.normalized;
        if (direction.sqrMagnitude < 0.0001f) direction = Vector2.up;
    }

    // --- Rotation (sprite default faces UP) ---
    void ApplyRotationSmooth()
    {
        if (!rotateToMove) return;
        if (direction.sqrMagnitude < 0.0001f) return;

        // angle where 0° means "up"
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f + angleOffsetDeg;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 1f - Mathf.Exp(-rotateLerp * Time.deltaTime));
    }

    void ApplyRotationInstant()
    {
        if (!rotateToMove) return;
        if (direction.sqrMagnitude < 0.0001f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f + angleOffsetDeg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}