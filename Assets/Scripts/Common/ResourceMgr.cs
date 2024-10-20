using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PunchPeng
{
    public class ResourceMgr : Singleton<ResourceMgr>
    {
        protected override void OnInit()
        {
        }

        public async UniTask<GameObject> InstantiateAsync(string path, Transform parent = null)
        {
            var prefab = await Resources.LoadAsync(path) as GameObject;
            if (prefab == null)
            {
                throw new System.Exception($"Prefab {path} is null");
            }

            var go = GameObject.Instantiate(prefab, parent);
            return go;
        }

        public async UniTask LoadSceneAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}