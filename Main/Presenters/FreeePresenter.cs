using System;
using System.Collections.Generic;
using System.Linq;
using ARAM.Main.Freees;
using ARAM.Main.Managers;
using ARAM.Scripts.Main.Views;
using ARAM.Utils;
using Freee;
using UniRx;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Presenters
{
    public class FreeePresenter : MonoBehaviour
    {
        [Inject] private UserModel _userModel;
        [Inject] private FreeeEventHandler _freeeEventHandler;
        [Inject] private CharacterMessageClient _characterMessageClient;
        [SerializeField] private List<FreeeButton> freeeButtons;

        private void Start()
        {
            freeeButtons.ForEach(x => x.gameObject.SetActive(false));

            freeeButtons
                .Select(x => x.Button.OnClickAsObservable().Select(_ => x.FreeeType))
                .Merge()
                .Subscribe(x =>
                {
                    _freeeEventHandler.PostTimeClocks(x.ToString(), _userModel.EmployeeId);
                    freeeButtons.ForEach(x2 => x2.gameObject.SetActive(false));
                })
                .AddTo(this);
            
            _characterMessageClient.ActiveAttendancesObservable
                .Subscribe(_ => _freeeEventHandler.GetTimeClocksAvailableTypes(_userModel.EmployeeId, ActiveButton));
        }

        private void ActiveButton(bool success, string response)
        {
            if (!success)
            {
                var msg = JsonUtility.FromJson<Message>(response).message;
                return;
            }

            var availableTypes = JsonUtility.FromJson<TimeClocksAvailableTypes>(response).available_types;
            DebugExtensions.DebugShowList(availableTypes);

            foreach (var button in freeeButtons.Where(x => availableTypes.Contains(x.FreeeType.ToString())))
                button.gameObject.SetActive(true);
        }
    }
}