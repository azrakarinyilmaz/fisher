using UnityEngine;

public class SpawnFlareOnShootExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Shooting biter bitmez
        var logic = animator.GetComponent<EnemyShooter>();
        if (logic != null)
            logic.SpawnFlare();
    }
}