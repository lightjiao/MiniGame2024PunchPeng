namespace PunchPeng
{
    public class BevAttack : BevAction
    {
        public override void OnStart()
        {
            base.OnStart();
            m_Player.InputAttack = true;
        }
    }
}