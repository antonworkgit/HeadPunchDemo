using System;
using UnityEngine;

namespace Scripts.Animations.Gloves
{
    public enum GlovesAnimatorState
    {
        Unknown = -1,
        Idle,
        AttackL,
        AttackR,
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class GlovesAnimator : MonoBehaviour, IAnimatorStateListener
    {
        private readonly static int Idle = Animator.StringToHash("Idle");

        private readonly static int AttackL = Animator.StringToHash("AttackL");
        private readonly static int AttackR = Animator.StringToHash("AttackR");

        private readonly static int WalkSpeed = Animator.StringToHash("WalkSpeed");

        [SerializeField]
        private Animator _animator;

        public event Action OnAnimationHitEvent;

        public GlovesAnimatorState State { get; private set; } = GlovesAnimatorState.Unknown;

        public bool IsAttacking => State is GlovesAnimatorState.AttackL or GlovesAnimatorState.AttackR;

        private void OnDisable()
        {
            SetIdle();
        }

        public void SetIdle() => _animator.Play(Idle, -1);

        public void OnStateEnter(int hash) => State = FromHash(hash);
        public void OnStateExit(int hash) { }

        public void OnAnimationHit()
        {
            OnAnimationHitEvent?.Invoke();
        }

        public void PlayAttackL() => _animator.SetTrigger(AttackL);
        public void PlayAttackR() => _animator.SetTrigger(AttackR);

        private GlovesAnimatorState FromHash(int hash)
        {
            if (hash == Idle) return GlovesAnimatorState.Idle;
            else if (hash == AttackL) return GlovesAnimatorState.AttackL;
            else if (hash == AttackR) return GlovesAnimatorState.AttackR;

            return GlovesAnimatorState.Unknown;
        }
    }
}
