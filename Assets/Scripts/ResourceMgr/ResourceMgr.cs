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

        public async UniTask<T> InstantiateAsync<T>(string path, Transform parent = null) where T : Object
        {
            // TODO: lock same res
            //await UniTaskLock.Lock(path.GetHashCode());
            var prefab = await Resources.LoadAsync(path) as GameObject;
            if (prefab == null)
            {
                throw new System.Exception($"Prefab {path} is null");
            }

            var go = GameObject.Instantiate(prefab, parent);
            return go.GetComponent<T>();
        }

        public async UniTask LoadSceneAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}