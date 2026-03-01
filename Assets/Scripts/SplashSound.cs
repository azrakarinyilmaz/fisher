using UnityEngine;

public class SplashSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip splashClip;

    public void PlaySplash()
    {
        audioSource.PlayOneShot(splashClip);
    }
}