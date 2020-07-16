using ARAM.Main.Characters;
using ARAM.Main.Managers;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Installers
{
    [CreateAssetMenu(fileName = "AttendanceUserTableInstaller", menuName = "Installers/AttendanceUserTableInstaller")]
    public class AttendanceUserTableInstaller : ScriptableObjectInstaller<AttendanceUserTableInstaller>
    {
        [SerializeField] private AttendanceUserTable _attendanceUserTable;
        public override void InstallBindings()
        {
            Container.BindInstance(_attendanceUserTable);
        }
    }
}