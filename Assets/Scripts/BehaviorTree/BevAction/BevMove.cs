using UnityEngine;

namespace PunchPeng
{
    public class BevMove : BevDurationAction
    {
        protected float m_CfgSpeedUpDuration = 0.2f;
        protected float m_CfgSpeedDownDuration = 0.2f;

        protected Vector3 m_InputDir;
        protected Vector3 m_RealInput;
        protected float m_ReduceSpeedElapesdMoment;


        public override void OnBevStart()
        {
            base.OnBevStart();
            m_CfgDuration = Random.Range(1f, 2f);
            m_ReduceSpeedElapesdMoment = m_CfgDuration - m_CfgSpeedDownDuration;

            var i = 100;
            while (i > 0)
            {
                m_InputDir = Vector3Util.Rand2DDir();
                if (PredictMoveInRange())
                {
                    break;
                }
                i--;
            }
            if (i == 0)
            {
                Log.Error($"行为树避障初始化检测计算了 100 次");
            }
        }

        public override TaskStatus OnBevUpdate(float deltaTime)
        {
            base.OnBevUpdate(deltaTime);

            if (m_ElapsedTime < m_CfgSpeedUpDuration)
            {
                m_RealInput = m_InputDir * Mathf.Clamp01(m_ElapsedTime / m_CfgSpeedUpDuration);
            }
            else if (m_ElapsedTime > m_ReduceSpeedElapesdMoment)
            {
                var speedDownPct = Mathf.Clamp01((m_ElapsedTime - m_ReduceSpeedElapesdMoment) / m_CfgSpeedDownDuration);
                m_RealInput = m_InputDir * (1 - speedDownPct);
            }
            else
            {
                m_RealInput = m_InputDir;
            }

            if (m_ElapsedTime < m_ReduceSpeedElapesdMoment && !PredictMoveInRange())
            {
                m_ReduceSpeedElapesdMoment = m_ElapsedTime;
            }

            m_Player.InputMoveDir = m_RealInput;

            if (m_ElapsedTime >= m_ReduceSpeedElapesdMoment && m_RealInput.ApproximatelyZero())
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        public override void OnBevEnd()
        {
            //Debug.Log($"Move Finish log: realInput:{m_RealInput}, m_ElapsedTime:{m_ElapsedTime}, m_ReduceSpeedElapesdMoment:{m_ReduceSpeedElapesdMoment}, m_CfgDuration:{m_CfgDuration}");
            base.OnBevEnd();
        }

        private bool PredictMoveInRange()
        {
            var predictPos = m_Player.Position + m_InputDir;
            return predictPos.InRange2D(LevelArea.Inst.Min, LevelArea.Inst.Max);
        }
    }
}