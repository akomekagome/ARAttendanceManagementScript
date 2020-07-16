using System;
using System.Linq;
using System.Threading;
using ARAM.Main.AWS;
using ARAM.Main.Managers;
using Cysharp.Threading.Tasks;
using UniRx;

using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ARAM.Main.Presenters
{
    public class FaceRecognitionPresenter : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private FaceRecognitionProvider _faceRecognitionProvider;
        [SerializeField] private Button faceRecognitionButton;
        
        private void Start()
        {
            PushButtonAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid PushButtonAsync(CancellationToken token)
        {
            faceRecognitionButton.gameObject.SetActive(false);
            
            await _gameStateManager.CurrentGameState
                .Where(x => x == GameState.FaceRecognition)
                .FirstOrDefault()
                .ToUniTask(cancellationToken: token);
            
            faceRecognitionButton.gameObject.SetActive(true);

            while (true)
            {
                await faceRecognitionButton.OnClickAsObservable()
                    .FirstOrDefault()
                    .ToUniTask(cancellationToken: token);

                if(await _faceRecognitionProvider.FaceRecognitionAsync(token))
                    break;
            }
            
            faceRecognitionButton.gameObject.SetActive(false);
        }
    }
}