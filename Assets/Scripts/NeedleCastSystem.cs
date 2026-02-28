using UnityEngine;
using UnityEngine.InputSystem;

public class NeedleCastSystem : MonoBehaviour
{
    public float rotationSpeed = 120f;
    public float minAngle = -90f;
    public float maxAngle = 90f;

    float currentAngle;

    void Awake()
    {
        SetToMinAngle();
    }

    void OnEnable()
    {
        SetToMinAngle();
    }

    void SetToMinAngle()
    {
        currentAngle = minAngle;
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    void Update()
    {
        if (!Application.isPlaying) return;

        // NEW INPUT SYSTEM
        var mouse = Mouse.current;
        if (mouse == null) return;

        bool holding = mouse.leftButton.isPressed;
        if (!holding) return;

        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle > maxAngle) currentAngle = maxAngle;

        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}