namespace PunchPeng
{
    public interface IInputSlowDown
    {
        public float InputSlowDownScale { get; set; }
    }

    public class BuffSlowDown : Buff
    {
        protected override void OnBuffStart()
        {
            base.OnBuffStart();
            if (m_Owner is IInputSlowDown iInputSlowDown)
            {
                iInputSlowDown.InputSlowDownScale += m_Cfg.Param1;
            }
        }

        protected override void OnBuffEnd()
        {
            if (m_Owner is IInputSlowDown iInputSlowDown)
            {
                iInputSlowDown.InputSlowDownScale -= m_Cfg.Param1;
            }
            base.OnBuffEnd();
        }
    }
}