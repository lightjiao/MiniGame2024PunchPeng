using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace PunchPeng
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHelper : MonoBehaviour
    {
        private PlayerInput input;

        [ReadOnly] private int m_JoinIndex;
        private int m_PlayerIdx => PlayerInputManagerHelper.Inst.GamePadCnt > 1 ? m_JoinIndex : m_JoinIndex + 1;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            Assert.IsNotNull(input);
            m_JoinIndex = input.playerIndex;
        }

        public void Move(InputAction.CallbackContext ctx)
        {
            PlayerInputManagerHelper.Inst.GetPlayerInputData(m_PlayerIdx).MoveDir = ctx.ReadValue<Vector2>().ToHorizontalVector3();
        }

        public void Attack(InputAction.CallbackContext ctx)
        {
            PlayerInputManagerHelper.Inst.GetPlayerInputData(m_PlayerIdx).IsAttack = !ctx.ReadValue<float>().ApproximatelyZero();
        }

        public void Run(InputAction.CallbackContext ctx)
        {
            PlayerInputManagerHelper.Inst.GetPlayerInputData(m_PlayerIdx).IsRun = !ctx.ReadValue<float>().ApproximatelyZero();
        }
    }
}