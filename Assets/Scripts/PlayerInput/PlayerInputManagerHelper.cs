using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace PunchPeng
{
    [Serializable]
    public class PlayerInputData
    {
        public Vector3 MoveDir;
        public bool Run;
        public bool Attack;
        public bool UseSkill;
    }

    public class PlayerInputManagerHelper : SingletonMono<PlayerInputManagerHelper>
    {
        [ShowInInspector]
        public int GamePadCount => m_PlayerInputs.Count;

        private Dictionary<int, PlayerInput> m_PlayerInputs = new();
        private Dictionary<int, PlayerInputData> m_PlayerInputDatas = new();
        private PlayerInputKeyboard m_KeyboardInput;

        protected override void OnAwake()
        {
            m_KeyboardInput = new();
            m_KeyboardInput.GamePlay.AddCallbacks(new KeyboardInputEventReciever());
        }

        private void OnEnable()
        {
            m_KeyboardInput.Enable();
        }

        private void OnDisable()
        {
            m_KeyboardInput.Disable();
        }

        private void Update()
        {
            if (LevelController.Inst.m_Player1 != null)
            {
                LevelController.Inst.m_Player1.InputMoveDir = GetPlayerInputData(0).MoveDir;
                LevelController.Inst.m_Player1.InputRun = GetPlayerInputData(0).Run;
                LevelController.Inst.m_Player1.InputAttack = GetPlayerInputData(0).Attack;
                LevelController.Inst.m_Player2.InputUseSkill = GetPlayerInputData(0).UseSkill;

                PlayerInputLogger.Inst.AppendInput(1, Time.frameCount, GetPlayerInputData(0));
            }

            if (LevelController.Inst.m_Player2 != null)
            {
                LevelController.Inst.m_Player2.InputMoveDir = GetPlayerInputData(1).MoveDir;
                LevelController.Inst.m_Player2.InputRun = GetPlayerInputData(1).Run;
                LevelController.Inst.m_Player2.InputAttack = GetPlayerInputData(1).Attack;
                LevelController.Inst.m_Player2.InputUseSkill = GetPlayerInputData(1).UseSkill;

                PlayerInputLogger.Inst.AppendInput(2, Time.frameCount, GetPlayerInputData(1));
            }
        }

        private void LateUpdate()
        {
        }

        public PlayerInputData GetPlayerInputData(int playerIdx)
        {
            if (!m_PlayerInputDatas.TryGetValue(playerIdx, out var data))
            {
                data = new PlayerInputData();
                m_PlayerInputDatas[playerIdx] = data;
            }
            return data;
        }

        /// <summary>
        /// 记录InputSystem 的BUG:
        /// 如果用代码来实现事件监听，则必须在第一帧去访问 PlayerInputManager 保证它在 OnEnable 的时候实例化
        /// 但有的设备会在第一帧自动被识别到，导致代码监听事件因时序问题获取不到
        /// 所以这里必须使用 UnityEvent 的方式或者 SendMessage 的方式来访问 OnPlayerJoin 的回调，避免时序问题丢失设备
        /// 
        /// InputSystem 有一个 InputSystem.inputsetting.asset 的文件，在 ProjectSetting 里可以设置，
        /// 里面的 Supported Devices 要么留空，要么一定要包含你所需要的设备，不然可能出现鼠标不在这个列表里导致鼠标点击不生效
        /// </summary>
        /// <param name="playerInput"></param>
        public void UnityEventOnPlayerJoin(PlayerInput playerInput)
        {
            var deviceNames = playerInput.devices.Select(x => x.name).ToArray().ToStringEx();
            Log.Info($"UnityEventOnPlayerJoin(): idx:{playerInput.playerIndex}, deviceNames:{deviceNames}");

            m_PlayerInputs[playerInput.playerIndex] = playerInput;
        }

        public void UnityEventOnPlayerLeft(PlayerInput playerInput)
        {
            var deviceNames = playerInput.devices.Select(x => x.name).ToArray().ToStringEx();
            Log.Info($"UnityEventOnPlayerLeft(): idx:{playerInput.playerIndex}, deviceNames:{deviceNames}");
            m_PlayerInputs.Remove(playerInput.playerIndex);
        }
    }

    public class KeyboardInputEventReciever : @PlayerInputKeyboard.IGamePlayActions
    {
        public void OnPlayer1Attack(CallbackContext context)
        {
            var value = !context.ReadValue<float>().ApproximatelyZero();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(0).Attack = value;
        }

        public void OnPlayer1Move(CallbackContext context)
        {
            var value = context.ReadValue<Vector2>().ToHorizontalVector3();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(0).MoveDir = value;
        }

        public void OnPlayer1Run(CallbackContext context)
        {
            var value = !context.ReadValue<float>().ApproximatelyZero();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(0).Run = value;
        }

        public void OnPlayer1UseSkill(CallbackContext context)
        {
            var value = !context.ReadValue<float>().ApproximatelyZero();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(0).UseSkill = value;
        }

        public void OnPlayer2Attack(CallbackContext context)
        {
            var value = !context.ReadValue<float>().ApproximatelyZero();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(1).Attack = value;
        }

        public void OnPlayer2Move(CallbackContext context)
        {
            var value = context.ReadValue<Vector2>().ToHorizontalVector3();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(1).MoveDir = value;
        }

        public void OnPlayer2Run(CallbackContext context)
        {
            var value = !context.ReadValue<float>().ApproximatelyZero();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(1).Run = value;
        }

        public void OnPlayer2UseSkill(CallbackContext context)
        {
            var value = !context.ReadValue<float>().ApproximatelyZero();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(1).UseSkill = value;
        }
    }
}