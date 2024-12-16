using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class ScoreboardManager : Singleton<ScoreboardManager>
    {
        public Dictionary<int, int> m_PlayerScores = new();
        public Dictionary<int, int> m_PlayerCoinScores = new();
        public bool HasScore => m_PlayerScores.Count > 0;

        protected override void OnInit()
        {
            UpdateScoreModules();
            UpdateCollectScoreModules();
            GameEvent.Inst.PlayerDeadPostAction += OnPlayerDeadToChangeScore;
            GameEvent.Inst.PlayerCollectCoinPostAction += OnPlayerCollectCoinToChangeScore;
        }

        private void OnPlayerDeadToChangeScore(int attacker, int deadPlayer)
        {
            if (deadPlayer < 0 ) return;

            if (deadPlayer == 1 )
            {
                AddPlayerScore(2);
            }
            if (deadPlayer == 2 )
            {
                AddPlayerScore(1);
            }

            UpdateScoreModules();
        }
        private void OnPlayerCollectCoinToChangeScore(int collector)
        {
            if (collector < 0) return;
            if (collector == 1)
            {
                AddPlayerCoinScore(1);
            }
            if (collector == 2)
            {
                AddPlayerCoinScore(2);
            }
            if (m_PlayerCoinScores[collector] >= 5)
            {
                AddPlayerScore(collector);
                UpdateScoreModules();
            }

            UpdateCollectScoreModules();
        }
        
        private void AddPlayerScore(int playerId)
        {
            if (!m_PlayerScores.ContainsKey(playerId))
            {
                m_PlayerScores[playerId] = 0;
            }
            m_PlayerScores[playerId]++;
        }
        
        private void AddPlayerCoinScore(int playerId)
        {
            if (!m_PlayerCoinScores.ContainsKey(playerId))
            {
                m_PlayerCoinScores[playerId] = 0;
            }

            m_PlayerCoinScores[playerId]++;
        }

        public void ResetAllScore()
        {
            m_PlayerScores.Clear();
            m_PlayerCoinScores.Clear();
        }

        public void ResetCollectScore()
        {
            m_PlayerCoinScores.Clear();
            UpdateCollectScoreModules();
        }

        public int GetWinPlayer()
        {
            var winpoint = 6;
            foreach (var kv in m_PlayerScores)
            {
                if (kv.Value >= winpoint)
                {
                    return kv.Key;
                }
            }

            return 0;
        }

        private void UpdateScoreModules()
        {
            UIController.Inst.player1ScoreText.text = m_PlayerScores.GetValueOrDefault(1).ToString(); ;
            UIController.Inst.player2ScoreText.text = m_PlayerScores.GetValueOrDefault(2).ToString(); ;
        }
        
        private void UpdateCollectScoreModules()
        {
            System.Text.StringBuilder pb1 = new System.Text.StringBuilder();
            pb1.Append("Player 1's punch bots: ");
            System.Text.StringBuilder pb2 = new System.Text.StringBuilder();
            pb2.Append("Player 2's punch bots: ");
            
            UIController.Inst.player1CollectScore.text = pb1.Append(m_PlayerCoinScores.GetValueOrDefault(1).ToString()).ToString();
            UIController.Inst.player2CollectScore.text = pb2.Append(m_PlayerCoinScores.GetValueOrDefault(2).ToString()).ToString();
        }
    }
}