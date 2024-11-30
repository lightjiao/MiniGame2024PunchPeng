using System.Threading;

namespace PunchPeng
{
    public interface IBuffOwner
    {

    }

    public class Buff
    {
        private CancellationTokenSource _destroyCts;
        protected CancellationTokenSource m_DestroyCts => _destroyCts ??= new CancellationTokenSource();

        public int m_Uid;
        public int DataId => m_Cfg.DataId;
        public bool IsEffecting { get; private set; }
        public bool TimeEnd => m_Cfg.Duration > 0 && m_TimeElapsed >= m_Cfg.Duration;

        protected IBuffOwner m_Owner;
        protected BuffCfg m_Cfg;

        protected float m_TimeElapsed;

        public void BuffAwake(int uid, IBuffOwner player, BuffCfg cfg)
        {
            m_Uid = uid;
            m_Owner = player;
            m_Cfg = cfg;
            m_TimeElapsed = 0;
            OnBuffAwake();
        }

        protected virtual void OnBuffAwake()
        {
        }

        public void BuffStart()
        {
            if (IsEffecting) return;
            IsEffecting = true;
            OnBuffStart();
        }

        protected virtual void OnBuffStart()
        {

        }

        public void BuffUpdate(float elapseSeconds)
        {
            m_TimeElapsed += elapseSeconds;
            OnBuffUpdate(elapseSeconds);
        }

        protected virtual void OnBuffUpdate(float elapseSeconds)
        {

        }

        public void BuffEnd()
        {
            if (!IsEffecting) return;
            IsEffecting = false;

            _destroyCts?.Cancel();
            OnBuffEnd();
        }

        protected virtual void OnBuffEnd()
        {

        }
    }
}