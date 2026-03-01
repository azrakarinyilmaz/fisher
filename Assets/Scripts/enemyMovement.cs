using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyYMove : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;

    private Rigidbody2D rb;
    private int direction = -1; // 1 = yukarý, -1 = aþaðý

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(0f, direction * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("boundry")){
            direction *= -1;

            // Collider içinde kalmasýn diye minik itme
            rb.position += new Vector2(0f, direction * 0.05f);
            transform.Rotate(0f, 0f, 180f);
        }
        else if (other.CompareTag("Player"))
        {
            //other.GetComponent<CharControl>().enabled = false;
            GameManager.Instance.GameOver();
            speed = 0;
        }
            
        // Sýnýr collider'ýna girince

    }
}