using UnityEngine;

namespace PunchPeng
{
    public class PlayerInputManager : SingletonMono<PlayerInputManager>
    {
        public struct PlayerInput
        {
            public Vector3 MoveDir;
            public bool IsRun;
            public bool IsAttack;
        }

        public PlayerInput Player1Input = new();
        public PlayerInput Player2Input = new();

        public PlayerControls m_PlayerControls;

        protected override void OnAwake()
        {
            m_PlayerControls = new();
        }

        private void OnEnable()
        {
            m_PlayerControls.Enable();
        }

        private void OnDisable()
        {
            m_PlayerControls.Disable();
        }

        public void OnUpdate()
        {
            Check1PInput();
            Check2PInput();
        }

        public void Check1PInput()
        {
            var rawInput = new Vector3(Input.GetAxis("Player1_Horizontal"), 0, Input.GetAxis("Player1_Vertical"));
            var squreInput = Vector3Util.SquareToCircle(rawInput);
            Player1Input.MoveDir = squreInput;
            Player1Input.IsRun = Input.GetButton("Player1_Run");
            Player1Input.IsAttack = Input.GetButtonDown("Player1_Attack");

            if (GameController.Inst.m_Player1 != null)
            {
                GameController.Inst.m_Player1.InputMoveDir.Value = Player1Input.MoveDir;
                GameController.Inst.m_Player1.InputRun.Value = Player1Input.IsRun;
                GameController.Inst.m_Player1.InputAttack.Value = Player1Input.IsAttack;
            }
        }

        public void Check2PInput()
        {
            var rawInput = new Vector3(Input.GetAxis("Player2_Horizontal"), 0, Input.GetAxis("Player2_Vertical"));
            var squreInput = Vector3Util.SquareToCircle(rawInput);
            Player2Input.MoveDir = m_PlayerControls.GamePlay.Player2Move.ReadValue<Vector2>().ToHorizontalVector3();
            Player2Input.IsRun = !m_PlayerControls.GamePlay.Player2Run.ReadValue<float>().ApproximatelyZero();
            Player2Input.IsAttack = !m_PlayerControls.GamePlay.Player2Attack.ReadValue<float>().ApproximatelyZero();

            if (GameController.Inst.m_Player2 != null)
            {
                GameController.Inst.m_Player2.InputMoveDir.Value = Player2Input.MoveDir;
                GameController.Inst.m_Player2.InputRun.Value = Player2Input.IsRun;
                GameController.Inst.m_Player2.InputAttack.Value = Player2Input.IsAttack;
            }
        }
    }
}