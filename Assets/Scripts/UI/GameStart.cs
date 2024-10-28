using ConfigAuto;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PunchPeng
{
    public class GameStart : MonoBehaviour
    {
        public void StartGame()
        {
            GameController.Inst.StartLevelAsync(Config_Global.Inst.data.LevelPunchPengScene).Forget();
        }
    }
}