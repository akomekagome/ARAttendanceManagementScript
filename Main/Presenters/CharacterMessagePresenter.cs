using System;
using System.Threading;
using ARAM.Main.Characters;
using ARAM.Main.Managers;
using ARAM.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using Zenject;

namespace ARAM.Main.Presenters
{
    public class CharacterMessagePresenter : MonoBehaviour
    {
        [Inject] private CharacterMessageGateway _characterMessageGateway;
        [Inject] private CharacterMessageTable _characterMessageTable;
        [SerializeField] private TextMeshProUGUI characterMessage;
        [SerializeField] private Transform characterMessageParent;
        private Camera _mainCamera;

        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            _mainCamera = Camera.main;
            characterMessageParent.gameObject.SetActive(false);
            characterMessage.text = "";

            _characterMessageGateway.IsActive
                .Where(x => x != characterMessageParent.gameObject.activeSelf)
                .Subscribe(x => SetActiveText(x, token)); 

            _characterMessageGateway.SendMessageObservable
                .Subscribe(SendMessage);
            
            this.LateUpdateAsObservable()
                .Subscribe(_ => characterMessageParent.LookAt(_mainCamera.transform.position.SetY(characterMessageParent.position.y)));
        }

        private void SendMessage(CharacterMessageType messageType)
        {
            characterMessage.text = _characterMessageTable.GetMessage(messageType);
        }

        private void SetActiveText(bool value, CancellationToken token = default)
        {
            if (value)
                ActiveTextAsync(token).Forget();
            else
                PassiveTextAsync(token).Forget();
        }

        private async UniTaskVoid ActiveTextAsync(CancellationToken token = default)
        {
            characterMessageParent.gameObject.SetActive(true);
            characterMessageParent.localScale = Vector3.zero;
            await characterMessageParent.DOScale(Vector3.one, 1f).SetEase(Ease.Linear).ToUniTask(cancellationToken: token);
        }
        
        private async UniTaskVoid PassiveTextAsync(CancellationToken token = default)
        {
            characterMessageParent.localScale = Vector3.one;
            await characterMessageParent.DOScale(Vector3.zero, 1f).SetEase(Ease.Linear).ToUniTask(cancellationToken: token);
            characterMessageParent.gameObject.SetActive(false);
        }
    }
}