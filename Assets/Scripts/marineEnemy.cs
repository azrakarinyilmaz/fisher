using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyXMove : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;

    private Rigidbody2D rb;
    public int direction = 1; // 1 = sað, -1 = sol

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * speed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("boundry"))
        {
            direction *= -1;

            // Collider içinde kalmasýn diye minik itme
            rb.position += new Vector2(direction * 0.05f, 0f);

            // Sprite yön deðiþtirsin istiyorsan:
            transform.Rotate(0f, 180f, 0f);
            // Eðer 2D top-down ise ve z rotation kullanýyorsan:
            // transform.Rotate(0f, 0f, 180f);
        }
        else if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
            speed = 0;
        }
    }
}