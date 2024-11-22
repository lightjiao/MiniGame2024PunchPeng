using Cysharp.Threading.Tasks;
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
        public bool IsRun;
        public bool IsAttack;
    }

    // TODO: 记录玩家游玩时的数据，并随机赋给AI
    // AI 随机拷贝当前玩家的移动数据
    public class PlayerInputManagerHelper : SingletonMono<PlayerInputManagerHelper>
    {
        public int GamePadCnt => m_PlayerInputs.Count;
        public bool KeyboardCtrlIsDisable;

        private Dictionary<int, PlayerInput> m_PlayerInputs = new();
        private Dictionary<int, PlayerInputData> m_PlayerInputDatas = new();
        private PlayerInputKeyboard m_KeyboardInput;

        protected override void OnAwake()
        {
            m_KeyboardInput = new();
            m_KeyboardInput.GamePlay.Move.performed += KeyboardMove;
            m_KeyboardInput.GamePlay.Move.canceled += KeyboardMove;
            m_KeyboardInput.GamePlay.Run.performed += KeyboardRun;
            m_KeyboardInput.GamePlay.Run.canceled += KeyboardRun;
            m_KeyboardInput.GamePlay.Attack.performed += KeyboardAttack;
            m_KeyboardInput.GamePlay.Attack.canceled += KeyboardAttack;

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
                GameController.Inst.m_Player1.InputMoveDir.Value = GetPlayerInputData(0).MoveDir;
                GameController.Inst.m_Player1.InputRun.Value = GetPlayerInputData(0).IsRun;
                GameController.Inst.m_Player1.InputAttack.Value = GetPlayerInputData(0).IsAttack;
            }

            if (GameController.Inst.m_Player2 != null)
            {
                GameController.Inst.m_Player2.InputMoveDir.Value = GetPlayerInputData(1).MoveDir;
                GameController.Inst.m_Player2.InputRun.Value = GetPlayerInputData(1).IsRun;
                GameController.Inst.m_Player2.InputAttack.Value = GetPlayerInputData(1).IsAttack;
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

        private void KeyboardMove(CallbackContext ctx)
        {
            GetPlayerInputData(0).MoveDir = m_KeyboardInput.GamePlay.Move.ReadValue<Vector2>().ToHorizontalVector3();
            //Debug.Log("KeyboardMove:" + GetPlayerInputData(0).MoveDir);
        }

        private void KeyboardRun(CallbackContext ctx)
        {
            GetPlayerInputData(0).IsRun = !m_KeyboardInput.GamePlay.Run.ReadValue<float>().ApproximatelyZero();
            //Debug.Log("KeyboardRun:" + GetPlayerInputData(0).IsRun);
        }

        private void KeyboardAttack(CallbackContext ctx)
        {
            GetPlayerInputData(0).IsAttack = !m_KeyboardInput.GamePlay.Attack.ReadValue<float>().ApproximatelyZero();
            //Debug.Log("KeyboardAttack:" + GetPlayerInputData(0).IsAttack);
        }
    }
}