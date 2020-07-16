using System.Collections;
using System.Collections.Generic;
using ARAM.Main.Freees;
using ARAM.Main.Managers;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Installers
{
    [CreateAssetMenu(fileName = "PlatformDependentDataInstaller", menuName = "Installers/PlatformDependentDataInstaller")]
    public class PlatformDependentDataInstaller : ScriptableObjectInstaller<FreeeDataInstaller>
    {
        [SerializeField] private PlatformDependentData winData;
        [SerializeField] private PlatformDependentData osxData;

        public override void InstallBindings()
        {
#if UNITY_EDITOR_WIN
            Container.BindInstance(winData);
#endif
#if UNITY_EDITOR_OSX
            Container.BindInstance(osxData);
#endif
        }
    }
}
