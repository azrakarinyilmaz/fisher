using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform mouthPoint;
    [SerializeField] private Transform enemy;          // Enemy'nin gerçek rotasyon taşıyan transform'u
    [SerializeField] private GameObject flarePrefab;

    [Header("Shoot")]
    [SerializeField] private float flareSpeed = 7f;
    [SerializeField] private float flareLife = 3f;
    [SerializeField] private float shootCooldown = 1.2f;

    [Header("Optional: Auto shoot")]
    [SerializeField] private bool autoShoot = true;

    private float nextShootTime;

    void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!enemy) enemy = transform; // Inspector'da boşsa kendisini kullan
    }

    void Update()
    {
        if (!autoShoot) return;

        if (Time.time >= nextShootTime)
        {
            TriggerShoot();
            nextShootTime = Time.time + shootCooldown;
        }
    }

    public void TriggerShoot()
    {
        if (!animator) return;
        animator.SetTrigger("shoot");
    }

    public void SpawnFlare()
    {
        if (!flarePrefab || !mouthPoint) return;

        // 1) GÖRÜNTÜ ROTASYONU: enemy ile aynı (spawn anında)
        Quaternion enemyRot = enemy ? enemy.rotation : transform.rotation;

        GameObject flare = Instantiate(flarePrefab, mouthPoint.position, enemyRot);

        // 2) GİDİŞ YÖNÜ: mouthPoint.right
        Vector2 dir = (Vector2)mouthPoint.right;

        Rigidbody2D rb = flare.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.gravityScale = 0f;

            // 3) Fizik rotasyonu bozmasın (enemy ile aynı kalsın)
            rb.freezeRotation = true;

            // 4) Hız ver
            rb.linearVelocity = dir * flareSpeed;
        }
        else
        {
            var mover = flare.GetComponent<FlareMover>();
            if (mover) mover.Init(dir, flareSpeed);
        }

        Destroy(flare, flareLife);
    }
}