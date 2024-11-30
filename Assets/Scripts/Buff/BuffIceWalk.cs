namespace PunchPeng
{
    public interface IIceWalkAccScale
    {
        public float SpeedUpReduction { get; set; }
        public float SpeedDownScale { get; set; }
    }

    public class BuffIceWalk : Buff
    {
        protected override void OnBuffStart()
        {
            base.OnBuffStart();

            if (m_Owner is IIceWalkAccScale iIceWalkParameter)
            {
                iIceWalkParameter.SpeedUpReduction += m_Cfg.Param1;
                iIceWalkParameter.SpeedDownScale += m_Cfg.Param2;
            }
        }

        protected override void OnBuffEnd()
        {
            if (m_Owner is IIceWalkAccScale iIceWalkParameter)
            {
                iIceWalkParameter.SpeedUpReduction -= m_Cfg.Param1;
                iIceWalkParameter.SpeedDownScale -= m_Cfg.Param2;
            }

            base.OnBuffEnd();
        }
    }
}