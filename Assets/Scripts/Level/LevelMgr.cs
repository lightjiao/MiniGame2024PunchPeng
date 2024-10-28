using ConfigAuto;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PunchPeng
{
    public class LevelMgr : Singleton<LevelMgr>
    {
        protected override void OnInit()
        {
        }

        private string m_CurLevelName;
        private GameObject m_CurCamera;

        public async UniTask LoadLevelAsync(string levelName)
        {
            if (!string.IsNullOrEmpty(m_CurLevelName))
            {
                await UnLoadCurLevel();
            }

            m_CurLevelName = levelName;
            await SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

            var cameraRes = Config_Global.Inst.data.LevelConfig.GetValueOrDefault(levelName)?.Camera;
            if (cameraRes != null)
            {
                m_CurCamera = await ResourceMgr.Inst.InstantiateAsync(cameraRes);
            }
        }

        public async UniTask UnLoadCurLevel()
        {
            await SceneManager.UnloadSceneAsync(m_CurLevelName);
            GameObject.Destroy(m_CurCamera);

            m_CurLevelName = null;
            m_CurCamera = null;

            await UniTask.DelayFrame(1);
        }
    }
}