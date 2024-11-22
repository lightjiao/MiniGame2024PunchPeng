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
        private int m_PlayerIdx => PlayerInputManagerHelper.Inst.GamePadCount > 1 ? m_JoinIndex : m_JoinIndex + 1;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            Assert.IsNotNull(input);
            m_JoinIndex = input.playerIndex;
        }

        public void Move(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<Vector2>().ToHorizontalVector3();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(m_PlayerIdx).MoveDir = value;
            //Debug.Log($"m_PlayerIdx:{m_PlayerIdx} PlayerInputHelper.Move:" + value);
        }

        public void Attack(InputAction.CallbackContext ctx)
        {
            var value = !ctx.ReadValue<float>().ApproximatelyZero();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(m_PlayerIdx).IsAttack = value;
            //Debug.Log($"m_PlayerIdx:{m_PlayerIdx} PlayerInputHelper.Attack:" + value);
        }

        public void Run(InputAction.CallbackContext ctx)
        {
            var value = !ctx.ReadValue<float>().ApproximatelyZero();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(m_PlayerIdx).IsRun = value;
            //Debug.Log($"m_PlayerIdx:{m_PlayerIdx} PlayerInputHelper.Run:" + value);
        }
    }
}