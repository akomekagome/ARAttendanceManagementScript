using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARAM.Main.Characters
{
    [System.Serializable]
    public struct CharacterMessageData
    {
        public CharacterMessageType characterMessageType;
        public string message;
    }
    
    [CreateAssetMenu(fileName = "CharacterMessageTable", menuName = "ARAM/Data/CharacterMessageTable")]
    public class CharacterMessageTable : ScriptableObject
    {
        [SerializeField] private List<CharacterMessageData> characterMessageTable;

        public string GetMessage(CharacterMessageType messageType)
            => characterMessageTable.FirstOrDefault(x => x.characterMessageType == messageType).message;
    }
}