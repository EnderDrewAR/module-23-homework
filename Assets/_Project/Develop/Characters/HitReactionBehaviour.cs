using UnityEngine;

public class HitReactionBehaviour : StateMachineBehaviour
{
    private static readonly int HitKey = Animator.StringToHash("Hit");
    private static readonly int InJumpProcessKey = Animator.StringToHash("InJumpProcess");
    private Character _character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _character = animator.GetComponentInParent<Character>();

        if (_character == null)
            return;

        _character.BeginHitReaction();
        animator.ResetTrigger(HitKey);
        animator.SetBool(InJumpProcessKey, false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_character != null)
            _character.EndHitReaction();
    }
}
