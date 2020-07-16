using System;
using System.Threading;
using ARAM.Main.Characters;
using ARAM.Main.Freees;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Managers
{
    public class CharacterMessageClient : MonoBehaviour
    {
        [Inject] private CharacterMessageGateway _characterMessageGateway;
        [Inject] private FreeeEventHandler _freeeEventHandler;
        private Subject<Unit> _activeAttendancesSubject = new Subject<Unit>();
        public IObservable<Unit> ActiveAttendancesObservable => _activeAttendancesSubject.AsObservable();

        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            CharacterMessageLoopAsync(token).Forget();
        }

        private async UniTaskVoid CharacterMessageLoopAsync(CancellationToken token)
        {
            await _characterMessageGateway.IsActive.Where(x => x).FirstOrDefault().ToUniTask(cancellationToken: token);
            
            await _characterMessageGateway.PlayAnimationAsync(CharacterAnimationType.Jumping,
                CharacterEmotionType.smile, token);
            
            await _characterMessageGateway.PlayVoiceAndAnimation(GetGreetingMessageType(),
                CharacterAnimationType.Salute, CharacterEmotionType.smile, token);
            
            await UniTask.Delay(100, cancellationToken: token);

            await _characterMessageGateway.SendMessageAsync(CharacterMessageType.Progress_Report, token);

            await UniTask.Delay(200, cancellationToken: token);

            await _characterMessageGateway.PlayVoiceAndAnimation(CharacterMessageType.Selection_Confirmation,
                CharacterAnimationType.Thinking, CharacterEmotionType.smile, token);
            
            _activeAttendancesSubject.OnNext(Unit.Default);
            _activeAttendancesSubject.OnCompleted();

            var freeeType = await _freeeEventHandler.PostTimeClockObservable.FirstOrDefault().ToUniTask(cancellationToken: token);
            
            await _characterMessageGateway.PlayVoiceAndAnimation(GetAttendancesMessageType(freeeType),
                GetAttendancesAnimationType(freeeType), CharacterEmotionType.smile, token);
        }
        
        private CharacterAnimationType GetAttendancesAnimationType(FreeeType freeeType)
        {
            switch (freeeType)
            {
                case FreeeType.break_begin:
                    return CharacterAnimationType.Waving;
                case FreeeType.break_end:
                    return CharacterAnimationType.VictoryIdle;
                case FreeeType.clock_in:
                    return CharacterAnimationType.VictoryIdle;
                case FreeeType.clock_out:
                    return CharacterAnimationType.Waving;
            }
            return default;
        }

        private CharacterMessageType GetAttendancesMessageType(FreeeType freeeType)
        {
            switch (freeeType)
            {
                case FreeeType.break_begin:
                    return CharacterMessageType.Attendances_Break_Begin;
                case FreeeType.break_end:
                    return CharacterMessageType.Attendances_Break_End;
                case FreeeType.clock_in:
                    return CharacterMessageType.Attendances_Clock_In;
                case FreeeType.clock_out:
                    return CharacterMessageType.Attendances_Clock_Out;
            }
            return default;
        }

        private CharacterMessageType GetGreetingMessageType()
        {
            var hour = DateTime.Now.Hour;
            if (hour >= 12 && hour < 18)
                return CharacterMessageType.Greeting_Noon;
            else if (hour >= 18 && hour < 24)
                return CharacterMessageType.Greeting_Evening;
            else
                return CharacterMessageType.Greeting_Morning;
        }
    }
}