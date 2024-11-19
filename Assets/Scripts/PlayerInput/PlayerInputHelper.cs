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

        [ReadOnly] private int m_InputIndex;
        [ReadOnly] private PlayerInputData m_PlayerInputData;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            Assert.IsNotNull(input);
            m_InputIndex = input.playerIndex;
            m_PlayerInputData = PlayerInputManagerHelper.Inst.GetPlayerInputData(m_InputIndex);
        }

        public void Move(InputAction.CallbackContext ctx)
        {
            m_PlayerInputData.MoveDir = ctx.ReadValue<Vector2>().ToHorizontalVector3();
            Debug.Log("MoveInput:" + m_PlayerInputData.MoveDir);
            // TODO: 特殊处理键盘输入的gravity逻辑
        }

        public void Attack(InputAction.CallbackContext ctx)
        {
            m_PlayerInputData.IsAttack = !ctx.ReadValue<float>().ApproximatelyZero();
            Debug.Log("Attack:" + m_PlayerInputData.IsAttack);
        }

        public void Run(InputAction.CallbackContext ctx)
        {
            m_PlayerInputData.IsRun = !ctx.ReadValue<float>().ApproximatelyZero();
            Debug.Log("Run：" + m_PlayerInputData.IsRun);
        }
    }
}