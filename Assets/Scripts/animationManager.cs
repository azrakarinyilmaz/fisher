using UnityEngine;

public class animationManager : MonoBehaviour
{
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Haandlepull()
    {
        animator.SetBool("pull", true);
    }
    public void Haandlethrow()
    {
        animator.SetBool("throw", true);
    }
}
