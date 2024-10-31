using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class VfxManager : Singleton<VfxManager>
    {
        //private Dictionary</>
        private class VfxHolder
        {
            public int Uuid;
            public string ResName;
            public GameObject Go;
            public float Duration;
        }

        private int m_UUID;
        private Dictionary<int, VfxHolder> m_VfxHolderDic = new();

        protected override void OnInit()
        {

        }

        public void ReleaseAll()
        {
            foreach (var kv in m_VfxHolderDic)
            {
                DestroyVfx(kv.Key);
            }
        }

        public async UniTask<int> PlayVfx(string resName, Transform parent = null, float duration = 0)
        {
            var go = await ResourceMgr.Inst.InstantiateAsync(resName, parent);
            var holder = new VfxHolder();
            holder.Uuid = ++m_UUID;
            holder.ResName = resName;
            holder.Duration = duration;
            holder.Go = go;

            m_VfxHolderDic[holder.Uuid] = holder;
            if (duration > 0)
            {
                DelayToDestroyVfx(holder).Forget();
            }

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

        private async UniTask DelayToDestroyVfx(VfxHolder holder)
        {
            await UniTask.Delay(holder.Duration.ToMilliSec());
            DestroyVfx(holder.Uuid);
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