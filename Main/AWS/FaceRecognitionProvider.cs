using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UniRx;
using MySister.Scripts.Common.Define;
using OpenCvSharp;
using Zenject;
using System.Linq;
using System.Threading;
using ARAM.Main.Managers;
using ARAM.Main.OpenCv;
using Cysharp.Threading.Tasks;

namespace ARAM.Main.AWS
{
    
    public class FaceRecognitionProvider : MonoBehaviour
    {
        // [Inject] private AmazonConginitoProvider AmazonConginitoProvider;
        [Inject] private AWSAuthData _awsAuthData;
        [Inject] private PlatformDependentData _platformDependentData;
        [Inject] private SystemMessageRequester _systemMessageRequester;

        private Subject<List<string>> faceRecognitionSubject = new Subject<List<string>>();
        public IObservable<List<string>> FaceRecognitionObservable => faceRecognitionSubject.AsObservable();
        // private ReactiveCollection<string> _faceIds;
        // public IReadOnlyReactiveCollection<string> FaceIds => _faceIds;
        private Subject<Unit>　_initSubject = new Subject<Unit>();
        public IObservable<Unit> InitObservable => _initSubject.AsObservable();
        
        private AmazonRekognitionGateway rekoGate;
        private OpenCvGateway cvGate;
        private WebCamClient webCamClient;
        
        private void Start()
        {
            // if(!AmazonConginitoProvider.LogIn())
            //     return;

            cvGate = new OpenCvGateway();
            webCamClient = new WebCamClient(_platformDependentData.cameraIndex);
            rekoGate = new AmazonRekognitionGateway(_awsAuthData);
            
            // var faceids = await FaceRecoAsync();
            //
            // DebugExtensions.DebugShowList(faceids);
            // faceRecognitionSubject.OnNext(faceids);
            
            _initSubject.OnNext(Unit.Default);
            _initSubject.OnCompleted();
        }

        public async UniTask<bool> FaceRecognitionAsync(CancellationToken token = default)
        {
            _systemMessageRequester.SendMessage("認証中...");
            
            Mat mat;
            while ((mat = DetectingFace()) == null)
                await UniTask.DelayFrame(10, cancellationToken: token);

            var faceIds = await AuthenticateFace(mat, token);
            var isSucceeded = faceIds != null && faceIds.Any();

            if (isSucceeded)
            {
                _systemMessageRequester.DeleteMessage();
                faceRecognitionSubject.OnNext(faceIds);
                faceRecognitionSubject.OnCompleted();
            }
            else
            {
                _systemMessageRequester.SendMessage("顔認証に失敗しました。顔を中心に捉えた状態でボタンを押してください");
            }

            return isSucceeded;
        }

        private Mat DetectingFace()
        {
            var mat = webCamClient.GetMatData();

            // if (mat != null && cvGate.DetectFaceInImage(mat, OpenCvSettings.CascadePath))
            //     cvGate.ShowMat(mat);
            
            return (mat != null && cvGate.DetectFaceInImage(mat, OpenCvSettings.CascadePath)) ? mat : null;
        }

        private async UniTask<List<string>> AuthenticateFace(Mat mat, CancellationToken token = default)
        {
            var tex = webCamClient.GetTexture2D();
            var jpgBytes = tex.EncodeToJPG();
            Destroy(tex);
            var stream = new MemoryStream(jpgBytes);
            
            return await rekoGate.AuthenticateFaceAsync(
                stream,
                AmazonRekognitionSettings.FaceCollectionId,
                token);
        }
    }
}

