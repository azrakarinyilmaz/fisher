using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] stepClips;

    [Header("Timing")]
    [SerializeField] private float stepInterval = 0.35f; // yürüyüş hızı
    [SerializeField] private float moveThreshold = 0.05f;

    [Header("Variation")]
    [SerializeField] private float volumeMin = 0.8f;
    [SerializeField] private float volumeMax = 1.0f;
    [SerializeField] private float pitchMin = 0.95f;
    [SerializeField] private float pitchMax = 1.05f;

    // Movement scriptinden set edeceğiz
    private Vector2 moveInput;
    private float timer;

    void Reset()
    {
        source = GetComponent<AudioSource>();
    }

    void Awake()
    {
        if (source == null) source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
    }

    void Update()
    {
        bool isMoving = moveInput.sqrMagnitude > (moveThreshold * moveThreshold);

        if (!isMoving)
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;
        if (timer >= stepInterval)
        {
            timer = 0f;
            PlayStep();
        }
    }

    void PlayStep()
    {
        if (stepClips == null || stepClips.Length == 0) return;

        source.pitch = Random.Range(pitchMin, pitchMax);
        source.volume = Random.Range(volumeMin, volumeMax);

        int i = Random.Range(0, stepClips.Length);
        source.PlayOneShot(stepClips[i]);
    }

    // Bunu movement scriptinden çağır
    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }
}