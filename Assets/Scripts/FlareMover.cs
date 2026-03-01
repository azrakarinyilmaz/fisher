using UnityEngine;

public class FlareMover : MonoBehaviour
{
    private Vector2 dir;
    private float speed;

    public void Init(Vector2 direction, float spd)
    {
        dir = direction.normalized;
        speed = spd;
    }

    void Update()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //other.GetComponent<CharControl>().enabled = false;
            GameManager.Instance.GameOver();
            Destroy(gameObject); // Ýstersen flare yok olsun
        }
    }
}