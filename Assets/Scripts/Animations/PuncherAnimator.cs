using System;
using UnityEngine;

namespace Scripts.Animations.Puncher
{
    public enum PuncherAnimatorState
    {
        Unknown = -1,
        Idle,
        Firing
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class PuncherAnimator : MonoBehaviour, IAnimatorStateListener
    {
        private readonly static int Idle = Animator.StringToHash("Idle");

        private readonly static int Fire = Animator.StringToHash("Fire");

        private readonly static int WalkSpeed = Animator.StringToHash("WalkSpeed");

        [SerializeField]
        private Animator _animator;

        public event Action OnAnimationHitEvent;

        public PuncherAnimatorState State { get; private set; } = PuncherAnimatorState.Unknown;

        public bool IsFiring => State is PuncherAnimatorState.Firing;

        private void OnDisable()
        {
            SetIdle();
        }

        public void SetIdle() => _animator.Play(Idle, -1);
        public void PlayFire() => _animator.SetTrigger(Fire);
        public void OnStateEnter(int hash) => State = FromHash(hash);
        public void OnStateExit(int hash) { }

        public void OnAnimationHit()
        {
            OnAnimationHitEvent?.Invoke();
        }

        private PuncherAnimatorState FromHash(int hash)
        {
            if (hash == Idle) return PuncherAnimatorState.Idle;
            else if (hash == Fire) return PuncherAnimatorState.Firing;

            return PuncherAnimatorState.Unknown;
        }
    }
}