using System;
using ARAM.Main.Freees;
using UnityEngine;
using UnityEngine.UI;

namespace ARAM.Scripts.Main.Views
{
    public class FreeeButton : MonoBehaviour
    {
        [SerializeField] private FreeeType _freeeType;
        public FreeeType FreeeType => _freeeType;
        public Button Button { get; private set; }

        private void Awake()
        {
            this.Button = GetComponent<Button>();
        }
    }
}