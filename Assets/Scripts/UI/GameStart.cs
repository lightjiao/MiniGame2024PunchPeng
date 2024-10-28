using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PunchPeng
{
    public class GameStart : MonoBehaviour
    {

        public UIController uiController;

        public void StartGame()
        {
            uiController.ShowLoading();
        }
        
       /* public void StartGame()
        {
            UILayerSwitcher.ShowPanel(2);
            //GameController.Inst.StartLevelAsync(LevelMgr.Loading).Forget();
        }*/
    }
}