using PunchPeng;
using System;
using System.Collections.Generic;


public class BuffCfg
{
    public int DataId;
    public Type BuffType;
    public float Duration;
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
            Param1 = 10,// 倒计时间隔时间
        }
    };
}
