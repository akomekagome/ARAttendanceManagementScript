using System;
using System.Threading;
using ARAM.Main.Characters;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using UniRx;

namespace ARAM.Main.Managers
{
    public class CharacterMessageGateway : MonoBehaviour
    {
        [Inject] private CharacterSpawner _characterSpawner;
        private BoolReactiveProperty _isActive = new BoolReactiveProperty();
        public IReadOnlyReactiveProperty<bool> IsActive => _isActive.ToReadOnlyReactiveProperty();
        // private Subject<CharacterAnimationType> _playAnimationSubject = new Subject<CharacterAnimationType>();
        // public IObservable<CharacterAnimationType> PlayAnimationObservable => _playAnimationSubject.AsObservable();
        // private Subject<CharacterEmotionType> _playEmotionSubject = new Subject<CharacterEmotionType>();
        // public IObservable<CharacterEmotionType> PlayEmotionObservable => _playEmotionSubject.AsObservable();
        private Subject<CharacterMessageType> _sendMessageSubject = new Subject<CharacterMessageType>();
        public IObservable<CharacterMessageType> SendMessageObservable => _sendMessageSubject.AsObservable();
        private CharacterAnimator _characterAnimator;
        private CharacterSpeaker _characterSpeaker;

        private void Awake()
        {
            _characterSpawner.SpawnCharacterObservable
                .FirstOrDefault()
                .Subscribe(x =>
                {
                    _characterAnimator = x.GetComponent<CharacterAnimator>();
                    _characterSpeaker = x.GetComponent<CharacterSpeaker>();
                    _isActive.Value = true;
                })
                .AddTo(this);
        }

        public async UniTask PlayAnimationAsync(CharacterAnimationType animationType, CharacterEmotionType emotionType, CancellationToken token = default)
        {
            _characterAnimator.PlayEmotion(emotionType);
            await _characterAnimator.PlayAnimationAsync(animationType, token);
        }
        
        public async UniTask SendMessageAsync(CharacterMessageType messageType, CancellationToken token = default)
        {
            _sendMessageSubject.OnNext(messageType);
            await _characterSpeaker.PlayVoiceAsync(messageType, token);
        }

        public async UniTask PlayVoiceAndAnimation(CharacterMessageType messageType, CharacterAnimationType animationType, CharacterEmotionType emotionType, CancellationToken token = default)
        {
            await UniTask.WhenAll(
                PlayAnimationAsync(animationType, emotionType, token),
                SendMessageAsync(messageType, token)
                );
        }
    }
}