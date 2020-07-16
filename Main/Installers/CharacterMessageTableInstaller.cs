using ARAM.Main.AWS;
using ARAM.Main.Characters;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Installers
{
    [CreateAssetMenu(fileName = "CharacterMessageTableInstaller", menuName = "Installers/CharacterMessageTableInstaller")]
    public class CharacterMessageTableInstaller : ScriptableObjectInstaller<CharacterMessageTableInstaller>
    {
        [SerializeField] private CharacterMessageTable _characterMessageTable;
        public override void InstallBindings()
        {
            Container.BindInstance(_characterMessageTable);
        }
    }
}