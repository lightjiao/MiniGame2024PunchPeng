namespace PunchPeng
{
    public class BevPosibility : BevConditional
    {
        private float m_Posibility;

        public BevPosibility SetPosibility(float posibility)
        {
            m_Posibility = posibility;
            return this;
        }

        public override TaskStatus OnBevUpdate(float deltaTime)
        {
            return MathUtil.InPercent(m_Posibility) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}