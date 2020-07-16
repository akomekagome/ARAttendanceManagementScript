using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARAM.Main.Managers
{
    [System.Serializable]
    public struct AttendanceUserData
    {
        public int employeeId;
        public string faceId; 
        public string name;
    }
    
    [CreateAssetMenu(fileName = "AttendanceUserTable", menuName = "ARAM/Data/AttendanceUserTable")]
    public class AttendanceUserTable : ScriptableObject
    {
        [SerializeField] private List<AttendanceUserData> attendanceUserTable;

        public AttendanceUserData GetUserData(string faceId)
            => attendanceUserTable.FirstOrDefault(x => x.faceId == faceId);
    }
}