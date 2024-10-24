using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace PunchPeng
{
    public class LevelMgr : Singleton<LevelMgr>
    {
        public const string TestLevelScene = "TestLevel";

        protected override void OnInit()
        {
        }

        private string m_CurLevelName;

        public async UniTask LoadLevelAsync(string levelName)
        {
            if (!string.IsNullOrEmpty(m_CurLevelName))
            {
                await SceneManager.UnloadSceneAsync(m_CurLevelName);
                m_CurLevelName = null;
            }

            m_CurLevelName = levelName;
            await SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        }
    }
}