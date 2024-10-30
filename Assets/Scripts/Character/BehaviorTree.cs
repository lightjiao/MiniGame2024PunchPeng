using ConfigAuto;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Drawing;
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
            m_BevNodes = new List<BevNode> { new BevIdle(), new BevIdle(), new BevIdle(), new BevMove(), new BevMove(), new BevRun() };
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
            m_CfgDuration = Random.Range(1f, 3f);
        }
    }

    public class BevMove : BevNode
    {
        protected Vector3 m_InputDir;

        public override void Start()
        {
            base.Start();
            m_CfgDuration = Random.Range(1f, 2f);

            var i = 100;
            while (i > 0)
            {
                m_InputDir = Vector3Util.Rand2DDir();
                if (CheckMoveInRange())
                {
                    break;
                }
                i--;
            }
            if (i == 0)
            {
                Debug.LogError($"行为树避障初始化检测计算了 {i} 次");
            }
            StartSmoothAsync().Forget();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (!CheckMoveInRange())
            {
                Finish();
            }
        }

        public override void Finish()
        {
            base.Finish();
            StopSmoothAsync().Forget();
        }

        private bool CheckMoveInRange()
        {
            var predictPos = m_Player.Position + m_InputDir;
            return predictPos.InRange2D(LevelArea.Inst.Min, LevelArea.Inst.Max);
        }

        private async UniTask StartSmoothAsync()
        {
            var smoothFrame = Config_Global.Inst.data.TargetFrameRate / 6;
            for (var i = 0; i <= smoothFrame; i++)
            {
                m_Player.PlayerInputMoveDir.Value = m_InputDir * i / smoothFrame;
                await UniTask.NextFrame();
            }
        }

        private async UniTask StopSmoothAsync()
        {
            var smoothFrame = Config_Global.Inst.data.TargetFrameRate / 6;
            for (var i = smoothFrame; i >= 0; i--)
            {
                m_Player.PlayerInputMoveDir.Value = m_InputDir * i / smoothFrame;
                await UniTask.NextFrame();
            }
        }
    }

    public class BevRun : BevMove
    {
        public override void Start()
        {
            base.Start();
            m_CfgDuration = Random.Range(0.5f, 1f);
            m_Player.PlayerInputRun.Value = true;
        }

        public override void Finish()
        {
            base.Finish();
            m_Player.PlayerInputRun.Value = false;
        }
    }

    public class BevAttack : BevNode
    {

    }
}