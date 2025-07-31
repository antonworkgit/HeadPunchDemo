using UnityEngine;

namespace Scripts.Animations
{
    public class AnimatorStateReporter : StateMachineBehaviour
    {
        private IAnimatorStateListener _listener;

        public void GetListener(Animator animator)
        {
            if (_listener != null)
                return;

            _listener = animator.gameObject.GetComponent<IAnimatorStateListener>();
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            GetListener(animator);

            _listener.OnStateEnter(stateInfo.shortNameHash);
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            GetListener(animator);

            _listener.OnStateExit(stateInfo.shortNameHash);
        }
    }
}