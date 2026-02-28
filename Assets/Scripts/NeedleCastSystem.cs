using UnityEngine;

public class NeedleCastSystem : MonoBehaviour
{
    public float rotationSpeed = 200f;   // derece/saniye
    public float minAngle = -90f;
    public float maxAngle = 90f;

    public Transform boat;
    public Transform bobber;

    public float nearDistance = 2f;      // yeşil
    public float midDistance = 4f;       // sarı
    public float farDistance = 6f;       // turuncu
    public float maxDistance = 8f;       // kırmızı

    float currentAngle;
    bool rotating = false;

    void Start()
    {
        currentAngle = minAngle;
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rotating = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            rotating = false;
            CastByAngle();
        }

        if (rotating)
        {
            currentAngle += rotationSpeed * Time.deltaTime;

            if (currentAngle > maxAngle)
                currentAngle = minAngle;

            transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
        }
    }

    void CastByAngle()
    {
        float zone = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);
        float distance = 0f;

        if (zone <= 0.25f)
            distance = nearDistance;
        else if (zone <= 0.5f)
            distance = midDistance;
        else if (zone <= 0.75f)
            distance = farDistance;
        else
            distance = maxDistance;

        Vector3 targetPos = boat.position + boat.up * distance;
        bobber.position = targetPos;
    }
}