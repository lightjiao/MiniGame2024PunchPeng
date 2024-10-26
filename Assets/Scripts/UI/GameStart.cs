using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PunchPeng
{
    public class GameStart : MonoBehaviour
    {
        public void StartGame()
        {
            GameController.Inst.StartLevelAsync(LevelMgr.PunchPeng).Forget();
        }
    }
}