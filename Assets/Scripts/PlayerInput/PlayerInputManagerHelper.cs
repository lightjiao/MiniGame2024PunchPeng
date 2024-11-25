using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
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

    // TODO: 记录玩家游玩时的数据，并随机赋给AI
    // AI 随机拷贝当前玩家的移动数据
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
            var keyboardInput = new KeyboardInput();
            m_KeyboardInput.GamePlay.AddCallbacks(keyboardInput);

            OnAwakeAsync();
        }

        private async void OnAwakeAsync()
        {
            await UniTask.DelayFrame(1); // PlayerInputManager.instance create at onenable
            PlayerInputManager.instance.EnableJoining();
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoin;
            PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
        }

        private void OnEnable()
        {
            m_KeyboardInput.Enable();
        }

        private void OnDisable()
        {
            m_KeyboardInput.Disable();
        }

        public void OnUpdate()
        {
            if (GameController.Inst.m_Player1 != null)
            {
                GameController.Inst.m_Player1.InputMoveDir = GetPlayerInputData(0).MoveDir;
                GameController.Inst.m_Player1.InputRun = GetPlayerInputData(0).Run;
                GameController.Inst.m_Player1.InputAttack = GetPlayerInputData(0).Attack;
                GameController.Inst.m_Player2.InputUseSkill = GetPlayerInputData(0).UseSkill;

                PlayerInputLogger.Inst.AppendInput(1, Time.frameCount, GetPlayerInputData(0));
            }

            if (GameController.Inst.m_Player2 != null)
            {
                GameController.Inst.m_Player2.InputMoveDir = GetPlayerInputData(1).MoveDir;
                GameController.Inst.m_Player2.InputRun = GetPlayerInputData(1).Run;
                GameController.Inst.m_Player2.InputAttack = GetPlayerInputData(1).Attack;
                GameController.Inst.m_Player2.InputUseSkill = GetPlayerInputData(1).UseSkill;

                PlayerInputLogger.Inst.AppendInput(2, Time.frameCount, GetPlayerInputData(1));
            }
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

        private void OnPlayerJoin(PlayerInput playerInput)
        {
            //Debug.Log("OnPlayerJoin:" + playerInput.playerIndex);
            m_PlayerInputs[playerInput.playerIndex] = playerInput;
        }

        private void OnPlayerLeft(PlayerInput playerInput)
        {
            //Debug.Log("OnPlayerLeft:" + playerInput.playerIndex);
            m_PlayerInputs.Remove(playerInput.playerIndex);
        }
    }

    public class KeyboardInput : @PlayerInputKeyboard.IGamePlayActions
    {
        public void OnAttack(CallbackContext context)
        {
            var value = !context.ReadValue<float>().ApproximatelyZero();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(0).Attack = value;
            //Debug.Log("KeyboardAttack:" + value);
        }

        public void OnMove(CallbackContext context)
        {
            var value = context.ReadValue<Vector2>().ToHorizontalVector3();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(0).MoveDir = value;
            //Debug.Log("KeyboardMove:" + value);
        }

        public void OnRun(CallbackContext context)
        {
            var value = !context.ReadValue<float>().ApproximatelyZero();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(0).Run = value;
            //Debug.Log("KeyboardRun:" + value);
        }

        public void OnUseSkill(CallbackContext context)
        {
            var value = !context.ReadValue<float>().ApproximatelyZero();
            PlayerInputManagerHelper.Inst.GetPlayerInputData(0).UseSkill = value;
        }
    }
}