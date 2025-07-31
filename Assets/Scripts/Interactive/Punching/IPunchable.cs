using UnityEngine;

namespace Scripts.Interactive.Punching
{
    public interface IPunchable
    {
        void ReceivePunch(PunchData punchData);
    }
}
