using ARAM.Main.Characters;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Installers
{
    [CreateAssetMenu(fileName = "CharacterVoiceTableInstaller", menuName = "Installers/CharacterVoiceTableInstaller")]
    public class CharacterVoiceTableInstaller : ScriptableObjectInstaller<CharacterVoiceTableInstaller>
    {
        [SerializeField] private CharacterVoiceTable characterVoiceTable;
        public override void InstallBindings()
        {
            Container.BindInstance(characterVoiceTable);
        }
    }
}