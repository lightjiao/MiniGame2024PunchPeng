using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PunchPeng
{
    public class PlayerInputLogger : Singleton<PlayerInputLogger>
    {
        private struct PlayerInputLogData
        {
            public int PlayerId;
            public int FrameCnt;

            public Vector3 MoveDir;
            public bool IsRun;
            public bool IsAttack;
        }

        private List<PlayerInputLogData> m_PlayerInputDatas = new();

        protected override void OnInit()
        {
            GameEvent.Inst.OnGameEnd += OnGameEnd;
        }

        public void AppendInput(int playerId, int frameCnt, PlayerInputData inputData)
        {
            //if (inputData.MoveDir.ApproximatelyZero())
            //{
            //    return;
            //}

            //var logData = new PlayerInputLogData();
            //logData.PlayerId = playerId;
            //logData.FrameCnt = frameCnt;
            //logData.MoveDir = inputData.MoveDir;
            //logData.IsRun = inputData.Run;
            //logData.IsAttack = inputData.Attack;

            //m_PlayerInputDatas.Add(logData);
        }

        private void OnGameEnd()
        {
            //var json = JsonUtil.ToJson(m_PlayerInputDatas);
            //WriteToCurFile(json);
            //m_PlayerInputDatas.Clear();
        }

        private async void WriteToCurFile(string str)
        {
            var curDir = Directory.GetCurrentDirectory();
            var fileName = "PlayerInputLog.json";
            var filePath = Path.Combine(curDir, fileName);
            try
            {
                await File.AppendAllTextAsync(filePath, str);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}