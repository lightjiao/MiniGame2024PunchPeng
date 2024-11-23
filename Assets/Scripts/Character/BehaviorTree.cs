using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class BehaviorTree
    {
        private Player m_Player;
        private List<BevNode> m_BevNodes = new();
        private BevNode m_CurNode;

        private float m_CfgAtkCd = 5;
        private float m_AtkTime = 0;

        public void Init(Player player)
        {
            m_Player = player;

            m_BevNodes = new List<BevNode> {
                new BevIdle(), new BevIdle(), new BevIdle(),
                new BevMove(), new BevMove(),
                new BevRun()
            };

            foreach (var node in m_BevNodes)
            {
                node.Init(m_Player);
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (m_CurNode == null)
            {
                m_CurNode = m_BevNodes.RandomOne();
                m_CurNode.Start();
            }

            m_CurNode.OnUpdate(deltaTime);

            if (m_CurNode.IsFinish == true)
            {
                m_CurNode = null;
            }

            if (AttackPlayerInFrontOfYou() && Time.time - m_AtkTime > m_CfgAtkCd)
            {
                m_AtkTime = Time.time;
                if (MathUtil.InPercent(0.1f))
                {
                    m_Player.InputAttack = true;
                }
            }
        }

        private bool AttackPlayerInFrontOfYou()
        {
            var predictPos = (m_Player.Position + m_Player.Forward);

            foreach (var player in GameController.Inst.PlayerList)
            {
                if (m_Player == player || player.IsDead) continue;
                if ((player.Position - predictPos).SetY(0).magnitude < 0.5f)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public abstract class BevNode
    {
        protected Player m_Player;
        protected float m_CfgDuration;
        protected float m_ElapsedTime;
        public bool IsFinish;

        public virtual void Init(Player player)
        {
            m_Player = player;
        }

        public virtual void Start()
        {
            m_ElapsedTime = 0;
            IsFinish = false;
        }

        public virtual void OnUpdate(float deltaTime)
        {
            m_ElapsedTime += deltaTime;
            if (m_ElapsedTime >= m_CfgDuration)
            {
                Finish();
            }
        }

        public virtual void Finish()
        {
            IsFinish = true;
        }
    }

    public class BevIdle : BevNode
    {
        public override void Start()
        {
            base.Start();
            m_CfgDuration = Random.Range(1f, 3f);
        }
    }

    public class BevMove : BevNode
    {
        protected float m_CfgSpeedUpDuration = 0.2f;
        protected float m_CfgSpeedDownDuration = 0.2f;

        protected Vector3 m_InputDir;
        protected Vector3 m_RealInput;
        protected float m_ReduceSpeedElapesdMoment;

        public override void Start()
        {
            base.Start();
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
                Debug.LogError($"行为树避障初始化检测计算了 100 次");
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

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
                Finish();
            }
        }

        public override void Finish()
        {
            //Debug.Log($"Move Finish log: realInput:{m_RealInput}, m_ElapsedTime:{m_ElapsedTime}, m_ReduceSpeedElapesdMoment:{m_ReduceSpeedElapesdMoment}, m_CfgDuration:{m_CfgDuration}");
            base.Finish();
        }

        private bool PredictMoveInRange()
        {
            var predictPos = m_Player.Position + m_InputDir;
            return predictPos.InRange2D(LevelArea.Inst.Min, LevelArea.Inst.Max);
        }
    }

    public class BevRun : BevMove
    {
        public override void Start()
        {
            base.Start();
            m_CfgDuration = Random.Range(0.5f, 1f);
            m_ReduceSpeedElapesdMoment = m_CfgDuration - m_CfgSpeedDownDuration;
            m_Player.InputRun = true;
        }

        public override void Finish()
        {
            base.Finish();
            m_Player.InputRun = false;
        }
    }

    public class BevAttack : BevNode
    {
        public override void Start()
        {
            base.Start();
            m_Player.InputAttack = true;
        }
    }
}