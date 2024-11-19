using static PunchPeng.PlayerInputManagerHelper;

namespace PunchPeng
{
    public class CopyPlayerInputAI
    {
        private Player m_Player;

        public void Init(Player player)
        {
            m_Player = player;
        }

        public void OnUpdate()
        {
            //m_Player.InputMoveDir.Value = PlayerInputManager.Inst.Player1Input.MoveDir;
            //m_Player.InputRun.Value = PlayerInputManager.Inst.Player1Input.IsRun;
            //m_Player.InputAttack.Value = PlayerInputManager.Inst.Player1Input.IsAttack;
        }
    }
}