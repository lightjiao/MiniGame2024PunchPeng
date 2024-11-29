using System;
using System.Collections.Generic;

namespace PunchPeng
{
    // TODO: 
    // Ability、Buff、Behavior，每一个独特的buff、ability 对 owner 要求的接口都不一样
    // 以buff系统为例，可以尝试把每个buff需要的接口单独声明，但buff里将owner强转为对应的interface，并调用方法
    public class BuffContainer
    {
        public IBuffOwner m_Owner;

        private int UidGen;

        private Dictionary<int, Buff> m_Buffs = new();
        private List<int> m_CacheUids = new();

        public BuffContainer(IBuffOwner owner)
        {
            m_Owner = owner;
        }

        public int AddBuff(int buffDataId)
        {
            var uid = UidGen++;
            var buffCfg = CfgTableBuff.Inst.Get(buffDataId);

            var buff = Activator.CreateInstance(buffCfg.BuffType) as Buff;
            buff.BuffAwake(uid, m_Owner, buffCfg);
            m_Buffs[uid] = buff;

            return uid;
        }

        public void RemoveBuff(int buffUid)
        {
            if (m_Buffs.TryGetValue(buffUid, out var buff))
            {
                buff.BuffEnd();
                m_Buffs.Remove(buffUid);
            }
        }

        public void RemoveAllBuff()
        {
            m_Buffs.KeysCopyTo(m_CacheUids);

            foreach (var buffUid in m_CacheUids)
            {
                RemoveBuff(buffUid);
            }
        }

        public void Update(float elapseSeconds)
        {
            m_Buffs.KeysCopyTo(m_CacheUids);
            foreach (var uid in m_CacheUids)
            {
                var buff = m_Buffs[uid];

                // Unity 的 Awake 在第 0 帧 Start 和 第一次 Update 都在第 1 帧，和 Unity 保持一致
                if (!buff.IsEffecting)
                {
                    buff.BuffStart();
                }

                /**
                 * 一定执行一次 OnUpdate() 确保整秒的逻辑能正常执行
                 * 比如每 1 秒执行一次伤害，配置了 5 秒，上一次update是 4.977 秒，已经执行了 4 次伤害，下一次Update的时候，就会变成 5.01 秒，如果立刻 End ，会导致 第 5 秒的伤害没有
                 */
                buff.BuffUpdate(elapseSeconds);

                if (buff.TimeEnd)
                {
                    //buff.BuffEnd();
                    RemoveBuff(uid);
                }
            }
        }
    }
}