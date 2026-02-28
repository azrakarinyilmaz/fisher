using UnityEngine;

public class BoatBobbing : MonoBehaviour
{
    public float amplitude = 0.03f;   // ne kadar yukarı-aşağı
    public float frequency = 1.5f;    // dalga hızı
    public float tiltAmplitude = 2f;  // derece olarak hafif sağ-sol eğim
    public float tiltFrequency = 1.2f;

    Vector3 startLocalPos;
    Quaternion startLocalRot;

    void Start()
    {
        startLocalPos = transform.localPosition;
        startLocalRot = transform.localRotation;
    }

    void Update()
    {
        float t = Time.time;

        float y = Mathf.Sin(t * frequency) * amplitude;
        float tilt = Mathf.Sin(t * tiltFrequency + 1.7f) * tiltAmplitude;

        transform.localPosition = startLocalPos + new Vector3(0f, y, 0f);
        transform.localRotation = startLocalRot * Quaternion.Euler(0f, 0f, tilt);
    }
}