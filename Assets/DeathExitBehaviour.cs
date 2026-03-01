using UnityEngine;

public class DeathExitBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject character = animator.gameObject;
        Destroy(character);

        //GameManager.Instance.GameOver();
    }
}