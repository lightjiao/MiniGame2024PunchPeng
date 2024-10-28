using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PunchPeng
{
    public class LoadingSwitcher : MonoBehaviour
    {
        void Update()
        {
            if (Input.anyKeyDown)
            {
                GameController.Inst.StartLevelAsync(LevelMgr.PunchPeng).Forget();
            }
        }
    }
}