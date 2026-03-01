using System.Collections;
using UnityEngine;

public class Dusman1ProximitySound : MonoBehaviour
{
    private Transform player;
    private AudioSource source;

    [Header("Distance Settings")]
    [SerializeField] private float hearRadius = 8f;
    [SerializeField] private float hysteresis = 1f;

    [Header("Fade Settings")]
    [SerializeField] private float fadeTime = 0.4f;
    [SerializeField] private float maxVolume = 1f;

    private bool isActive = false;
    private Coroutine fadeRoutine;

    void Start()
    {
        // Player'ı isme göre bul
        GameObject p = GameObject.Find("Karakter_arka");
        if (p != null)
            player = p.transform;

        // AudioSource al
        source = GetComponent<AudioSource>();

        if (source != null)
        {
            source.playOnAwake = false;
            source.loop = true;
            source.volume = 0f;
        }
    }

    void Update()
    {
        if (player == null || source == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        float enterDist = hearRadius;
        float exitDist = hearRadius + hysteresis;

        if (!isActive && distance <= enterDist)
        {
            isActive = true;
            StartFade(true);
        }
        else if (isActive && distance >= exitDist)
        {
            isActive = false;
            StartFade(false);
        }
    }

    void StartFade(bool fadeIn)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(fadeIn));
    }

    IEnumerator Fade(bool fadeIn)
    {
        if (fadeIn && !source.isPlaying)
            source.Play();

        float start = source.volume;
        float end = fadeIn ? maxVolume : 0f;

        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(start, end, t / fadeTime);
            yield return null;
        }

        source.volume = end;

        if (!fadeIn)
            source.Stop();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearRadius);
    }
}