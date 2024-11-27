using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class BevRun : BevMove
    {
        public override void OnBevStart()
        {
            base.OnBevStart();
            m_CfgDuration = Random.Range(0.5f, 2f);
            m_ReduceSpeedElapesdMoment = m_CfgDuration - m_CfgSpeedDownDuration;
            m_Player.InputRun = true;
        }

        public override void OnBevEnd()
        {
            base.OnBevEnd();
            m_Player.InputRun = false;
        }
    }
}