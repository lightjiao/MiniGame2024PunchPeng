using PunchPeng;
using System;
using System.Collections.Generic;

public class BuffCfg
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
        m_BuffDict = new Dictionary<int, BuffCfg>();
        foreach (var item in list)
        {
            if (m_BuffDict.ContainsKey(item.DataId))
            {
                throw new Exception($"Duplicated DataId:{item.DataId}");
            }
            m_BuffDict[item.DataId] = item;
        }
    }

    public BuffCfg Get(int id)
    {
        m_BuffDict.TryGetValue(id, out var buff);
        return buff;
    }

    private Dictionary<int, BuffCfg> m_BuffDict;
    private readonly List<BuffCfg> list = new()
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
        }
    };
}
