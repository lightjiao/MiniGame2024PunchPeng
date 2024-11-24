using System.Threading;

namespace PunchPeng
{
    public class Buff
    {
    private CancellationTokenSource _destroyCts;
        protected CancellationTokenSource m_DestroyCts => _destroyCts ??= new CancellationTokenSource();

        protected Player m_Player;
        protected float m_CfgDuration;

        protected float m_TimeElapsed;

        public void Init(Player player)
        {
            m_Player = player;
            OnInit();
        }

        protected virtual void OnInit()
        {
            BuffStart();
        }

        public void Update(float elapseSeconds)
        {
            m_TimeElapsed += elapseSeconds;

            /**
             * 一定执行一次 OnUpdate() 确保整秒的逻辑能正常执行
             * 比如每 1 秒执行一次伤害，配置了 5 秒，上一次update是 4.977 秒，已经执行了 4 次伤害，下一次Update的时候，就会变成 5.01 秒，如果立刻 End ，会导致 第 5 秒的伤害没有
             */
            OnUpdate(elapseSeconds);

            if (m_TimeElapsed > m_CfgDuration)
            {
                BuffEnd();
            }
        }

        protected virtual void OnUpdate(float elapseSeconds)
        {

        }

        private void BuffStart()
        {
            m_TimeElapsed = 0;
            OnBuffStart();
        }

        protected virtual void OnBuffStart()
        {

        }

        private void BuffEnd()
        {
            _destroyCts?.Cancel();
            OnBuffEnd();
        }

        protected virtual void OnBuffEnd()
        {

        }
    }
}