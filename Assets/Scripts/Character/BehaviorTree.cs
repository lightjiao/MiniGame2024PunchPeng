using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class BehaviorTree
    {
        private Player m_Player;
        private List<BevNode> m_BevNodes = new();
        private BevNode m_CurNode;

        public void Init(Player player)
        {
            m_Player = player;
            m_BevNodes = new List<BevNode> { new BevIdle(), new BevMove(), new BevRun() };
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
            m_CfgDuration = Random.Range(2, 4);
        }
    }

    public class BevMove : BevNode
    {
        protected Vector3 m_InputDir;

        public override void Start()
        {
            base.Start();
            m_CfgDuration = Random.Range(1, 3);
            m_InputDir = Vector3Ex.Rand2DDir();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            InputMove();

            var predictPos = m_Player.Position + m_InputDir;

            // check obstacle to finish
            //if (predictPos.x < LevelArea.Inst.MinX || predictPos.x > LevelArea.Inst.MaxX || predictPos.z < LevelArea.Inst.MinZ || predictPos.z > LevelArea.Inst.MaxZ)
            //{
            //    Debug.Log("BevMove predict pos finish");
            //    Finish();
            //}
        }

        protected virtual void InputMove()
        {
            m_Player.PlayerInputMoveDir.Value = m_InputDir;
        }
    }

    public class BevRun : BevMove
    {
        public override void Start()
        {
            base.Start();
            m_CfgDuration = Random.Range(1, 4);
        }

        protected override void InputMove()
        {
            base.InputMove();
            m_Player.PlayerInputRun.Value = true;
        }
    }

    public class BevAttack : BevNode
    {

    }
}