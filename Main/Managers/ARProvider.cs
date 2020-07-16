using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace ARAM.Main.Managers
{
    
    public class ARProvider : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private SystemMessageRequester _systemMessageRequester;
        [Inject] private UserModel _userModel;
        [SerializeField] private ARRaycastManager _arRaycastManager;
        private List<ARRaycastHit> _hitResults = new List<ARRaycastHit>();
        private Subject<Vector3> _spawnCharacterSubject = new Subject<Vector3>();
        public IObservable<Vector3> SpawnCharacterObservable => _spawnCharacterSubject.AsObservable();
        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            PlaneDetection(token).Forget();
        }

        private async UniTaskVoid PlaneDetection(CancellationToken token)
        {
            await _gameStateManager.CurrentGameState
                .Where(x => x == GameState.GameUpdate)
                .FirstOrDefault()
                .ToUniTask(cancellationToken: token);
            
            _systemMessageRequester.SendMessage($"{GetGreetingMessage()}、{_userModel.Name}さん、召喚する場所をタップしてください");
            
            // while (Input.GetMouseButtonDown(0) && _arRaycastManager.Raycast(Input.GetTouch(0).position, _hitResults))
            // {
            //     _spawnCharacterSubject.OnNext(_hitResults[0].pose.position);
            //     _spawnCharacterSubject.OnCompleted();
            //     await UniTask.DelayFrame(1, cancellationToken: token);
            // }

            await UniTask.Delay(3000, cancellationToken: token);
            _spawnCharacterSubject.OnNext(Vector3.zero);
            _spawnCharacterSubject.OnCompleted();

            _systemMessageRequester.DeleteMessage();
        }
        
        private string GetGreetingMessage()
        {
            var hour = DateTime.Now.Hour;
            if (hour >= 12 && hour < 18)
                return "こんにちは";
            else if (hour >= 18 && hour < 24)
                return "こんばんは";
            else
                return "おはようございます";
        }
    }
}
