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
        private Dictionary<string, GameObject> m_LoadedRes = new();

        public async UniTask<GameObject> InstantiateAsync(string path, Transform parent = null)
        {
            var obj = await InstantiateAsync<Transform>(path, parent);
            return obj.gameObject;
        }

        public async UniTask<T> InstantiateAsync<T>(string path, Transform parent = null) where T : Object
        {
            // TODO loading res lock

            if (!m_LoadedRes.TryGetValue(path, out var prefab))
            {
                prefab = await Resources.LoadAsync(path) as GameObject;
                if (prefab == null)
                {
                    throw new System.Exception($"Prefab {path} is null");
                }
                m_LoadedRes[path] = prefab;
            }

            var go = GameObject.Instantiate(prefab, parent);

            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            return go.GetComponent<T>();
        }
    }
}