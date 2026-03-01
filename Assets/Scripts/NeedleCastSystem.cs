using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NeedleCastSystem : MonoBehaviour
{
    [Header("Angles")]
    public float minAngle = -90f;
    public float maxAngle = 90f;
    private bool justStarted;
    public Animator animator;
    public GameObject parent;
  

    [Header("Time to go from min to max (seconds)")]
    public float sweepDuration = 1.0f;  // 0.6 hızlı, 1.0 ideal

    float t = 0f;        // 0..1
    int dir = 1;         // +1 ileri, -1 geri

    void OnEnable()
    {
        // İstersen her seferinde -90'dan başlasın:
        // t = 0f; dir = 1; ApplyRotation();

        ApplyRotation(); // kaldığı yerden devam etsin
    }
    private void Start()
    {
        justStarted = true;
        animator = GameObject.Find("pulled_phase").GetComponent<Animator>();
    }

    void Update()
    {
        if (!Application.isPlaying) return;

        var mouse = Mouse.current;
        if (mouse == null) return;

        if (!mouse.leftButton.isPressed) {

            if (!justStarted){ 
                animator.SetTrigger("throw");
                StartCoroutine(Wait());
                
                return; 
            }
        }

        else justStarted = false;


        float step = Time.unscaledDeltaTime / Mathf.Max(0.01f, sweepDuration);
        t += dir * step;

        // Sınırda yön değiştir (ping-pong)
        if (t >= 1f) { t = 1f; dir = -1; }
        if (t <= 0f) { t = 0f; dir = 1; }

        ApplyRotation();
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        Destroy(parent);
    }

    void ApplyRotation()
    {
        float angle = Mathf.Lerp(minAngle, maxAngle, t);
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}