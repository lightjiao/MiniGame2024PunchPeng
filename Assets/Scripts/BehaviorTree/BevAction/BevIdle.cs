using UnityEngine;

namespace PunchPeng
{
    public class BevIdle : BevDurationAction
    {
        public override void OnBevStart()
        {
            base.OnBevStart();
            m_CfgDuration = Random.Range(1f, 3f);
            m_Player.InputMoveDir = Vector3.zero;
            m_Player.InputAttack = false;
            m_Player.InputRun = false;
            m_Player.InputUseSkill = false;
        }
    }
}