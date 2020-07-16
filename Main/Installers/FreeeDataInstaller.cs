using ARAM.Main.Freees;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Installers
{
    [CreateAssetMenu(fileName = "FreeeDataInstaller", menuName = "Installers/FreeeDataInstaller")]
    public class FreeeDataInstaller : ScriptableObjectInstaller<FreeeDataInstaller>
    {
        [SerializeField] private FreeeAuthData _freeeAuthData;
            
        public override void InstallBindings()
        {
            Container.BindInstance(_freeeAuthData);
        }
    }
}
