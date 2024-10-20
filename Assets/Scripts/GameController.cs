using UnityEngine;
using ConfigAuto;

namespace PunchPeng
{
    public class GameController : MonoBehaviour
    {
        private GameObject m_player;

        // Start is called before the first frame update
        private async void Start()
        {
            await LevelMgr.Inst.LoadLevelAsync(LevelMgr.TestLevelScene);
            m_player = await ResourceMgr.Inst.InstantiateAsync(Config_Player.Inst.PlayerPrefab);
        }
    }
}