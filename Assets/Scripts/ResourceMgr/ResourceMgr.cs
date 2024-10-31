using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class ResourceMgr : Singleton<ResourceMgr>
    {
        protected override void OnInit()
        {
        }

        private Dictionary<string, UniTaskCompletionSource> m_LoadingTcs = new();
        private Dictionary<string, Object> m_LoadedRes = new();

        public async UniTask<T> LoadAsync<T>(string resPath) where T : Object
        {
            if (string.IsNullOrEmpty(resPath))
            {
                return null;
            }
            // TODO loading res lock
            if (!m_LoadedRes.TryGetValue(resPath, out var resObj))
            {
                resObj = await Resources.LoadAsync<T>(resPath);
                if (resObj == null)
                {
                    throw new System.Exception($"ResAsset `{resPath}` is null");
                }
                m_LoadedRes[resPath] = resObj;
            }

            return (T)resObj;
        }

        public async UniTask<GameObject> InstantiateAsync(string path, Transform parent = null)
        {
            var obj = await InstantiateAsync<Transform>(path, parent);
            return obj.gameObject;
        }

        public async UniTask<T> InstantiateAsync<T>(string path, Transform parent = null) where T : Object
        {
            var prefab = await LoadAsync<GameObject>(path);

            var go = GameObject.Instantiate(prefab, parent);
            go.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            go.transform.localScale = Vector3.one;

            return go.GetComponent<T>();
        }
    }
}