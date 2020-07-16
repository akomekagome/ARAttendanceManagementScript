using System;
using System.Collections;
using System.Threading;
using ARAM.Main.Managers;
using TMPro;
using UnityEngine;
using Zenject;
using UniRx;

namespace ARAM.Main.Presenters
{
    public class SystemMessagePresenter : MonoBehaviour
    {
        [Inject] private SystemMessageRequester _systemMessageRequester;
        [SerializeField] private TextMeshProUGUI textMeshProUgui;

        private void Awake()
        {
            textMeshProUgui.text = "";
            
            _systemMessageRequester.SendMessageObservable
                .Subscribe(x =>
                {
                    textMeshProUgui.text = x;
                })
                .AddTo(this);
            
            _systemMessageRequester.DeleteMessageObservable
                .Subscribe(_ =>
                {
                    textMeshProUgui.text = "";
                })
                .AddTo(this);
        }
    }
}