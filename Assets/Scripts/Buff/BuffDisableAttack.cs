namespace PunchPeng
{
    public class BuffDisableAttack : Buff
    {
        protected override void OnBuffStart()
        {
            base.OnBuffStart();

            if (m_Owner is ICanAttack iCanAttack)
            {
                iCanAttack.CanAttack--;
            }
        }

        protected override void OnBuffEnd()
        {
            if (m_Owner is ICanAttack iCanAttack)
            {
                iCanAttack.CanAttack++;
            }
            base.OnBuffEnd();
        }
    }
}