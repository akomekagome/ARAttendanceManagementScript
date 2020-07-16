using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARAM.Main.Characters
{
    [System.Serializable]
    public struct CharacterVoiceData
    {
        public CharacterMessageType characterMessageType;
        public AudioClip audioClip;
    }
    
    [CreateAssetMenu(fileName = "CharacterVoiceTable", menuName = "ARAM/Data/CharacterVoiceTable")]
    public class CharacterVoiceTable : ScriptableObject
    {
        [SerializeField] private List<CharacterVoiceData> characterVoiceTable;

        public AudioClip GetVoice(CharacterMessageType messageType)
            => characterVoiceTable.FirstOrDefault(x => x.characterMessageType == messageType).audioClip;
    }
}