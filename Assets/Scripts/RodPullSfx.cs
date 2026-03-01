using UnityEngine;

public class RodPullSfx : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip pullClip;

    [Header("Optional anti-spam")]
    [SerializeField] private float minInterval = 0.08f; // 0 yaparsan her basışta çalar
    private float lastPlayTime = -999f;

    void Reset()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // anti-spam (istersen kapat: minInterval = 0)
            if (Time.time - lastPlayTime < minInterval) return;
            lastPlayTime = Time.time;

            if (source != null && pullClip != null)
                source.PlayOneShot(pullClip);
        }
    }
}