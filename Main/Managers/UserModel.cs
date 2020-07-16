using System;
using System.Linq;
using ARAM.Main.AWS;
using ARAM.Main.Freees;
using UnityEngine;
using Zenject;
using UniRx;

namespace ARAM.Main.Managers
{
    public class UserModel : MonoBehaviour
    {
        [Inject] private FaceRecognitionProvider _faceRecognitionProvider;
        // [Inject] private FreeeEventHandler _freeeEventHandler;
        [Inject] private SystemMessageRequester _systemMessageRequester;
        [Inject] private AttendanceUserTable _attendanceUserTable;
        public string UserFaceId { get; private set; }
        public string Name { get; private set; }
        public int EmployeeId { get; private set; }
        
        private void Start()
        {
            // _freeeEventHandler.InitObservable
            //     .Subscribe(_ => _freeeEventHandler.PostTimeClocks("clock_in"));
                
            // _freeeEventHandler.InitObservable
            //     .Subscribe(_ => _freeeEventHandler.PostTimeClocks("clock_in", 860334));
            
            // _freeeEventHandler.InitObservable
            //     .Subscribe(_ => _freeeEventHandler.PostTimeClocks("clock_in", 856701));
            
            // _systemMessageRequester.SendMessage("こんにちは");
            // Observable.Timer(TimeSpan.FromSeconds(5))
            //     .Subscribe(_ => _systemMessageRequester.SendMessage("くるー"));
            
            _faceRecognitionProvider.FaceRecognitionObservable
                .Subscribe(x =>
                {
                    UserFaceId = x.First();
                    var userData = _attendanceUserTable.GetUserData(UserFaceId);
                    EmployeeId = userData.employeeId;
                    Name = userData.name;
                })
                .AddTo(this);
        }
    }
}