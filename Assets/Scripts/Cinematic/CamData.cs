using Cinemachine;
using System;

namespace Scripts.Cinematic
{
    [Serializable]
    public class CamData
    {
        public CamType type;
        public CinemachineVirtualCamera camera;
        public int priority;
    }
}