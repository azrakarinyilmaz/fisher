using UnityEngine;
using System.Collections;

public class NextRoundScheduler : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CatchTheFish manager;
    [SerializeField] private Animator animator;

    [Header("Animator State")]
    [SerializeField] private string standStateName = "normal_stand 0"; // usually THIS
    [SerializeField] private int animatorLayer = 0;

    [Header("Random delay to start next round")]
    [SerializeField] private float minDelay = 1.5f;
    [SerializeField] private float maxDelay = 3.5f;

    private Coroutine pendingNextRoundCo;

    void OnEnable()
    {
        if (manager != null)
        {
            manager.OnWin += OnRoundEnded;
            manager.OnLose += OnRoundEnded;
        }

    
        TryScheduleNextRound();
    }

    void OnDisable()
    {
        if (manager != null)
        {
            manager.OnWin -= OnRoundEnded;
            manager.OnLose -= OnRoundEnded;
        }

        StopPending();
    }

    private void OnRoundEnded()
    {
       
        TryScheduleNextRound();
    }

    private void TryScheduleNextRound()
    {
        
        if (pendingNextRoundCo != null) return;

        // (Optional but recommended) If your manager has a flag, block scheduling while round is active.
        // Example:
        // if (manager != null && manager.IsRoundActive) return;

        pendingNextRoundCo = StartCoroutine(WaitStandThenRandomThenNewRound());
    }

    private IEnumerator WaitStandThenRandomThenNewRound()
    {
        // 1) Wait until stand
        yield return new WaitUntil(IsInStandState);

        // 2) Random delay, but ONLY count time while still in stand
        float target = Random.Range(minDelay, maxDelay);
        float elapsed = 0f;

        while (elapsed < target)
        {
            if (!IsInStandState())
            {
                yield return new WaitUntil(IsInStandState);
                continue; // don't count time while not standing
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 3) Fire next round
        pendingNextRoundCo = null;
        NewRound();
    }

    private bool IsInStandState()
    {
        if (animator == null) return false;
        var info = animator.GetCurrentAnimatorStateInfo(animatorLayer);

        // If your stand state is in Base Layer, you can also use:
        // return info.IsName("Base Layer.normal_stand");

        return info.IsName(standStateName);
    }

    private void StopPending()
    {
        if (pendingNextRoundCo != null)
        {
            StopCoroutine(pendingNextRoundCo);
            pendingNextRoundCo = null;
        }
    }

    private void NewRound()
    {
        if (manager != null)
            manager.NewRound();

        Debug.Log("NewRound() called");
    }
}