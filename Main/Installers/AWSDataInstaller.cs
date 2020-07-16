using ARAM.Main.AWS;
using ARAM.Main.Freees;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Installers
{
    [CreateAssetMenu(fileName = "AWSDataInstaller", menuName = "Installers/AWSDataInstaller")]
    public class AWSDataInstaller : ScriptableObjectInstaller<AWSDataInstaller>
    {
        [SerializeField] private AWSAuthData _awsAuthData;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_awsAuthData);
        }
    }
}