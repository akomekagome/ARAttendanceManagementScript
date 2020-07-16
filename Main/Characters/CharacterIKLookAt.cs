using System;
using ARAM.Main.Managers;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Characters
{
    public class CharacterIKLookAt : MonoBehaviour
    {
        [Inject] private CameraProvider _cameraProvider;
        [Inject] private Animator _animator;
        private Transform lookAtObj = default;

        [SerializeField] private float lookAtWeight = 1.0f;
        [SerializeField] private float bodyWeight = 0.3f;
        [SerializeField] private float headWeight = 0.8f;
        [SerializeField] private float eyesWeight = 1.0f;
        [SerializeField] private float clampWeight = 0.5f;

        private void Awake()
        {
            lookAtObj = _cameraProvider.MainCamera.transform;
        }

        void OnAnimatorIK(int layerIndex)
        {
            _animator.SetLookAtWeight(lookAtWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
            _animator.SetLookAtPosition(lookAtObj.position);
        }
    }
}