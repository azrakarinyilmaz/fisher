using UnityEngine;

public class NormalStandCleanup : StateMachineBehaviour
{
    public string[] resetTriggers;
    public string[] falseBools;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset triggers
        if (resetTriggers != null)
            foreach (var t in resetTriggers)
                animator.ResetTrigger(t);

        // Force bools false
        if (falseBools != null)
            foreach (var b in falseBools)
                animator.SetBool(b, false);
    }
}