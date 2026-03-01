using UnityEngine;
using UnityEngine.Tilemaps;

public class CharControl : MonoBehaviour
{
    [SerializeField] private FootstepSounds footsteps;
    [Header("Maps")]
    [SerializeField] private Tilemap walkableMap;
    [SerializeField] private Tilemap wallsMap; // opsiyonel

    [Header("Move")]
    [SerializeField] private float stepTime = 0.25f; // 0 yaparsan an�nda tile tile z�plar

    [Header("Animator (BlendTree)")]
    [SerializeField] private Animator animator; // MoveX/MoveY

    private bool moving;
    private Vector2Int facing = Vector2Int.up;
    private Vector2Int heldDir = Vector2Int.zero;

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (walkableMap == null)
        {
            Debug.LogError("[GridMoveHold_NoDelay] walkableMap not assigned!", this);
            enabled = false;
            return;
        }

        // Snap
        Vector3Int cell = walkableMap.WorldToCell(transform.position);
        transform.position = walkableMap.GetCellCenterWorld(cell);

        SetFacing(facing);
    }

    void Update()
    {
        heldDir = ReadHeldDirection();
        if (footsteps != null)
        {
            footsteps.SetMoveInput(new Vector2(heldDir.x, heldDir.y)); 
        }
        

        if (heldDir == Vector2Int.zero) return;

        if (heldDir != facing)
        {
            facing = heldDir;
            SetFacing(facing);
        }

        if (!moving)
        {
            TryStep(heldDir);
        }
    }

    Vector2Int ReadHeldDirection()
    {
        // Ayn� anda iki tu�: dikey �ncelik (istersen yatay �ncelik yapar�z)
        bool up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (up && !down) return Vector2Int.up;
        if (down && !up) return Vector2Int.down;
        if (left && !right) return Vector2Int.left;
        if (right && !left) return Vector2Int.right;

        return Vector2Int.zero;
    }

    void SetFacing(Vector2Int dir)
    {
        if (animator == null) return;
        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);
    }

    void TryStep(Vector2Int dir)
    {
        Vector3Int current = walkableMap.WorldToCell(transform.position);
        Vector3Int targetCell = current + new Vector3Int(dir.x, dir.y, 0);

        if (!walkableMap.HasTile(targetCell)) return;
        if (wallsMap != null && wallsMap.HasTile(targetCell)) return;

        Vector3 targetPos = walkableMap.GetCellCenterWorld(targetCell);

        if (stepTime <= 0f)
        {
            transform.position = targetPos;
            return;
        }

        StartCoroutine(MoveSmooth(targetPos));
        
    }

    System.Collections.IEnumerator MoveSmooth(Vector3 target)
    {
        moving = true;

        Vector3 start = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / stepTime;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
        moving = false;
    }
}