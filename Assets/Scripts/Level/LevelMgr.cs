using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace PunchPeng
{
    public class LevelMgr : Singleton<LevelMgr>
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
        }

        public async UniTask UnLoadCurLevel()
        {
            await SceneManager.UnloadSceneAsync(m_CurLevelName);
            m_CurLevelName = null;

            await UniTask.NextFrame();
        }
    }
}