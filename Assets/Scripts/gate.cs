using UnityEngine;

public class gate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogWarning(other.name);
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameWin();
        }

    }
}

