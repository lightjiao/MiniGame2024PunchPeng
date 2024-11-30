using PunchPeng;
using System;
using System.Collections.Generic;

public struct BuffCfg
{
    public int DataId;
    public Type BuffType;
    public float Duration;
    public float Interval;

    public float Param1;
    public float Param2;
    public float Param3;
    public float Param4;
}

public class CfgTableBuff : Singleton<CfgTableBuff>
{
    protected override void OnInit()
    {
        m_CfgDict = new Dictionary<int, BuffCfg>();

        foreach (var cfg in m_CfgList)
        {
            if (m_CfgDict.ContainsKey(cfg.DataId))
            {
                throw new Exception($"Duplicated DataId:{cfg.DataId}");
            }
            m_CfgDict[cfg.DataId] = cfg;
        }
    }

    public bool Get(int id, out BuffCfg cfg)
    {
        return m_CfgDict.TryGetValue(id, out cfg);
    }

    private Dictionary<int, BuffCfg> m_CfgDict;

    private readonly List<BuffCfg> m_CfgList = new()
    {
        new() {
            DataId = 1,
            BuffType = typeof(BuffAICountDownAttack),
            Duration = -1,
            Interval = 7,
        },
        new() {
            DataId = 2,
            BuffType = typeof(BuffDisableAttack),
            Duration = 2,
        },
        new() {
            DataId = 3,
            BuffType = typeof(BuffIceWalk),
            Duration = -1,
            Param1 = 0.95f, // 加速的减益
            Param2 = 0.99f, // 减速缩放
        }
    };
}
