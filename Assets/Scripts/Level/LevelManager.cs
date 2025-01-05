using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PunchPeng
{
    public class LevelManager : Singleton<LevelManager>
    {
        protected override void OnInit()
        {
        }

        private string m_CurLevelName;

        public async UniTask LoadLevelAsync(string levelName)
        {
            if (!string.IsNullOrEmpty(m_CurLevelName))
            {
                await UnLoadCurLevel();
            }

            m_CurLevelName = levelName;
            await SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            // 这样设置一下避免场景偏黄的问题
            var scene = SceneManager.GetSceneByName(levelName);
            SceneManager.SetActiveScene(scene);
        }

        public async UniTask UnLoadCurLevel()
        {
            await SceneManager.UnloadSceneAsync(m_CurLevelName);
            m_CurLevelName = null;

            await UniTask.NextFrame();

            await Resources.UnloadUnusedAssets();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}