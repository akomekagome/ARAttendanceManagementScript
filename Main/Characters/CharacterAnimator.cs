using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Characters
{
    public class CharacterAnimator : MonoBehaviour
    {
        [Inject] private Animator _animator;

        public async UniTask PlayAnimationAsync(CharacterAnimationType animationType, CancellationToken token = default)
        {
            var animName = animationType.ToString();
            _animator.CrossFadeInFixedTime(animName, 0.2f, (int)CharacterAnimationLayerType.Base);
            await UniTask.WaitWhile(() => !_animator.GetCurrentAnimatorStateInfo((int) CharacterAnimationLayerType.Base).shortNameHash.Equals(Animator.StringToHash(animName)), cancellationToken: token);
            await UniTask.WaitWhile(() => _animator.GetCurrentAnimatorStateInfo((int) CharacterAnimationLayerType.Base).shortNameHash.Equals(Animator.StringToHash(animName)), cancellationToken: token);
        }
        
        public void PlayEmotion(CharacterEmotionType emotionType)
        {
            _animator.Play(emotionType.ToString(), (int)CharacterAnimationLayerType.Face);
        }
    }
}