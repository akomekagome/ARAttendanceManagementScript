using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Characters
{
    public class CharacterSpeaker : MonoBehaviour
    {
        [Inject] private CharacterVoiceTable _characterVoiceTable;
        private AudioSource _audioSource;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public async UniTask PlayVoiceAsync(CharacterMessageType messageType, CancellationToken token = default)
        {
            _audioSource.clip = _characterVoiceTable.GetVoice(messageType);
            _audioSource.Play();
            await UniTask.WaitWhile(() => _audioSource.isPlaying, cancellationToken: token);
        }
    }
}