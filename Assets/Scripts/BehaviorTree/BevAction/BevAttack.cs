namespace PunchPeng
{
    public class BevAttack : BevAction
    {
        public override void OnBevStart()
        {
            base.OnBevStart();
            m_Player.InputAttack = true;
        }
    }
}