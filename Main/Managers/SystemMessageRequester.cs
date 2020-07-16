using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Managers
{
    public class SystemMessageRequester : MonoBehaviour
    {
        private Subject<string> _sendMessageSubject = new Subject<string>();
        public IObservable<string> SendMessageObservable => _sendMessageSubject.AsObservable();
        private Subject<Unit> _deleteMessageSubject = new Subject<Unit>();
        public IObservable<Unit> DeleteMessageObservable => _deleteMessageSubject.AsObservable();
        

        public void SendMessage(string message)
        {
            _sendMessageSubject.OnNext(message);
        }
        
        public void DeleteMessage()
        {
            _deleteMessageSubject.OnNext(Unit.Default);
        }
    }
}