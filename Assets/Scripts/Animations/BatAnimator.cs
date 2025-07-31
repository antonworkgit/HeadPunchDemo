using System;
using UnityEngine;

namespace Scripts.Animations.Bat
{
    public enum BatAnimatorState
    {
        Unknown = -1,
        Idle,
        AttackL,
        AttackR,
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class BatAnimator : MonoBehaviour, IAnimatorStateListener
    {
        private readonly static int Idle = Animator.StringToHash("Idle");

        private readonly static int AttackL = Animator.StringToHash("AttackL");
        private readonly static int AttackR = Animator.StringToHash("AttackR");

        private readonly static int WalkSpeed = Animator.StringToHash("WalkSpeed");

        [SerializeField]
        private Animator _animator;

        public event Action OnAnimationHitEvent;

        public BatAnimatorState State { get; private set; } = BatAnimatorState.Unknown;

        public bool IsAttacking => State is BatAnimatorState.AttackL or BatAnimatorState.AttackR;

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

        private BatAnimatorState FromHash(int hash)
        {
            if (hash == Idle) return BatAnimatorState.Idle;
            else if (hash == AttackL) return BatAnimatorState.AttackL;
            else if (hash == AttackR) return BatAnimatorState.AttackR;

            return BatAnimatorState.Unknown;
        }
    }
}