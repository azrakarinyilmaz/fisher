using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image icon;

    void Awake()
    {
        if (icon == null)
            icon = GetComponentInChildren<Image>(true);
    }

    public void Set(Sprite sprite)
    {
        if (icon == null) return;
        icon.sprite = sprite;
        icon.enabled = (sprite != null);
    }
}