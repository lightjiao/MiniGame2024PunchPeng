using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class BevRun : BevMove
    {
        public override void OnStart()
        {
            base.OnStart();
            m_CfgDuration = Random.Range(0.5f, 1f);
            m_ReduceSpeedElapesdMoment = m_CfgDuration - m_CfgSpeedDownDuration;
            m_Player.InputRun = true;
        }

        public override void OnEnd()
        {
            base.OnEnd();
            m_Player.InputRun = false;
        }
    }
}