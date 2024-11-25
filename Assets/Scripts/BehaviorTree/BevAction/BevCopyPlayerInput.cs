using static PunchPeng.PlayerInputManagerHelper;

namespace PunchPeng
{
    public class CopyPlayerInputAI : BevDurationAction
    {
        public override void OnStart()
        {
            base.OnStart();
        }

        public override TaskStatus OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            //m_Player.InputMoveDir = PlayerInputActionGen.
            //m_Player.InputMoveDir.Value = PlayerInputManager.Inst.Player1Input.MoveDir;
            //m_Player.InputRun.Value = PlayerInputManager.Inst.Player1Input.IsRun;
            //m_Player.InputAttack.Value = PlayerInputManager.Inst.Player1Input.IsAttack;

            return TaskStatusByDuration;
        }

        public void OnUpdate()
        {
            //m_Player.InputMoveDir.Value = PlayerInputManager.Inst.Player1Input.MoveDir;
            //m_Player.InputRun.Value = PlayerInputManager.Inst.Player1Input.IsRun;
            //m_Player.InputAttack.Value = PlayerInputManager.Inst.Player1Input.IsAttack;
        }
    }
}