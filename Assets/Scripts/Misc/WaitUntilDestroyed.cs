using UnityEngine;

namespace Scripts.CustomYieldInstructions
{
    public sealed class WaitUntilDestroyed : CustomYieldInstruction
    {
        private readonly GameObject _target;

        public override bool keepWaiting => _target != null;
        
        public WaitUntilDestroyed(GameObject target)
        {
            _target = target;
        }
    }
}