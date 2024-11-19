using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private Dictionary<int, PlayerInput> m_PlayerInputs = new();

        private Dictionary<int, PlayerInputData> m_PlayerInputDatas = new();

        protected override void OnAwake()
        {
            OnAwakeAsync();
        }

        private async void OnAwakeAsync()
        {
            await UniTask.DelayFrame(1); // PlayerInputManager.instance create at onenable
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoin;
            PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
        }

        public void OnUpdate()
        {
            if (GameController.Inst.m_Player1 != null)
            {
                GameController.Inst.m_Player1.InputMoveDir.Value = GetPlayerInputData(1).MoveDir;
                GameController.Inst.m_Player1.InputRun.Value = GetPlayerInputData(1).IsRun;
                GameController.Inst.m_Player1.InputAttack.Value = GetPlayerInputData(1).IsAttack;
            }

            if (GameController.Inst.m_Player2 != null)
            {
                GameController.Inst.m_Player2.InputMoveDir.Value = GetPlayerInputData(2).MoveDir;
                GameController.Inst.m_Player2.InputRun.Value = GetPlayerInputData(2).IsRun;
                GameController.Inst.m_Player2.InputAttack.Value = GetPlayerInputData(2).IsAttack;
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
            Debug.Log("OnPlayerJoin:" + playerInput.playerIndex);
            m_PlayerInputs[playerInput.playerIndex] = playerInput;
        }

        private void OnPlayerLeft(PlayerInput playerInput)
        {
            Debug.Log("OnPlayerLeft:" + playerInput.playerIndex);
            m_PlayerInputs.Remove(playerInput.playerIndex);
        }
    }
}