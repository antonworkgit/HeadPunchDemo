using Cinemachine;
using Scripts.CustomYieldInstructions;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cinematic
{
    public sealed class KillCamSampleService : MonoBehaviour
    {
        [SerializeField]
        private List<CamData> _cameras = new();

        private const CamType Default = CamType.Main;
        private static KillCamSampleService _instance;

        private CamType _current;
        private CinemachineVirtualCamera _killCamCached;
        private Coroutine _killCamRoutine;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        private void Start()
        {
            SetActiveCam(Default);

            _killCamCached = _cameras.Find(cd => cd.type == CamType.KillCam).camera;
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        public static void PlayKillCam(GameObject target)
        {
            _instance.StartKillCamRoutine(target);
        }

        private void SetActiveCam(CamType camType)
        {
            foreach (CamData camData in _cameras)
            {
                camData.camera.Priority = camData.type == camType ? camData.priority : 0;
            }

            _current = camType;
        }

        private void StartKillCamRoutine(GameObject target)
        {
            if(_killCamRoutine != null)
                StopCoroutine(_killCamRoutine);

            _killCamRoutine = StartCoroutine(KillCamCoroutine(target));
        }

        private IEnumerator KillCamCoroutine(GameObject target)
        {
            if (target == null)
                yield break;

            CamType previous = _current;

            _killCamCached.LookAt = target.transform;
            _killCamCached.Follow = target.transform;

            // This block is for demo purposes only and should be handled by the state machine without using time scale
            Time.timeScale = 0.5f;
            FirstPersonController firstPersonController = FindObjectOfType<FirstPersonController>();
            Camera weaponCam = GameObject.FindGameObjectWithTag("WeaponCamera").GetComponent<Camera>();
            firstPersonController.enabled = false;
            weaponCam.enabled = false;

            SetActiveCam(CamType.KillCam);

            yield return new WaitUntilDestroyed(target);

            Time.timeScale = 1f;
            firstPersonController.enabled = true;
            weaponCam.enabled = true;

            _killCamCached.LookAt = null;
            _killCamCached.Follow = null;

            SetActiveCam(previous);
        }
    }
}