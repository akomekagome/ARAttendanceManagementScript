using System;
using System.Threading;
using System.Threading.Tasks;
using ARAM.Main.AWS;
using ARAM.Main.Freees;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        [Inject] private FaceRecognitionProvider _faceRecognitionProvider;
        [Inject] private FreeeEventHandler _freeeEventHandler;
        private ReactiveProperty<GameState> _currentGameState = new ReactiveProperty<GameState>();

        public IReadOnlyReactiveProperty<GameState> CurrentGameState => _currentGameState.ToReadOnlyReactiveProperty(GameState.Initializing);

        private void Awake()
        {
            GameLoop(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid GameLoop(CancellationToken token)
        {
            await UniTask.WhenAll(
                _faceRecognitionProvider.InitObservable.Do(_ => Debug.Log("init")).ToUniTask(cancellationToken: token),
                _freeeEventHandler.InitObservable.Do(_ => Debug.Log("init")).ToUniTask(cancellationToken: token)
            );
            
            _currentGameState.SetValueAndForceNotify(GameState.FaceRecognition);

            await _faceRecognitionProvider.FaceRecognitionObservable.ToUniTask(cancellationToken: token);
            
            _currentGameState.SetValueAndForceNotify(GameState.GameUpdate);
        }
    }
}