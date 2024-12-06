using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class VfxManager : Singleton<VfxManager>
    {
        private class VfxHolder
        {
            public int Uuid;
            public string ResName;
            public GameObject Go;
            public float Duration;
            public float ElapsedTime;
        }

        private int m_UUID;
        private Dictionary<int, VfxHolder> m_VfxHolderDic = new();
        private List<int> m_KeysCache = new();

        protected override void OnInit()
        {

        }

        public void OnUpdate(float deltaTime)
        {
            m_VfxHolderDic.KeysCopyTo(m_KeysCache);
            foreach (var uid in m_KeysCache)
            {
                var holder = m_VfxHolderDic[uid];
                if (holder.Duration <= 0) continue;

                holder.ElapsedTime += deltaTime;
                if (holder.ElapsedTime >= holder.Duration)
                {
                    DestroyVfx(uid);
                }
            }
        }

        public void ReleaseAll()
        {
            foreach (var kv in m_VfxHolderDic)
            {
                GameObjectUtil.DestroyGo(kv.Value.Go);
            }
            m_VfxHolderDic.Clear();
        }

        public async UniTask<int> PlayVfx(string resName, Transform parent = null, float duration = 0)
        {
            var go = await ResourceManager.Inst.InstantiateAsync(resName, parent);
            var holder = new VfxHolder();
            holder.Uuid = ++m_UUID;
            holder.ResName = resName;
            holder.Duration = duration;
            holder.Go = go;

            m_VfxHolderDic[holder.Uuid] = holder;

            return holder.Uuid;
        }

        public async UniTask<int> PlayVfx(string resName, Vector3 position, float duration = 0)
        {
            var uuid = await PlayVfx(resName, null, duration);
            if (m_VfxHolderDic.TryGetValue(uuid, out var vfxHolder))
            {
                vfxHolder.Go.transform.position = position;
            }

            return uuid;
        }

        public void DestroyVfx(int id)
        {
            if (false == m_VfxHolderDic.TryGetValue(id, out VfxHolder vfxHolder))
            {
                return;
            }

            m_VfxHolderDic.Remove(id);
            GameObjectUtil.DestroyGo(vfxHolder.Go);
        }
    }
}