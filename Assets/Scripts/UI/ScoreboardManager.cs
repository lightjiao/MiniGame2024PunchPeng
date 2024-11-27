using System.Collections.Generic;

namespace PunchPeng
{
    public class ScoreboardManager : Singleton<ScoreboardManager>
    {
        public Dictionary<int, int> m_PlayerScores = new();

        public bool HasScore => m_PlayerScores.Count > 0;

        protected override void OnInit()
        {
            UpdateScoreModules();
            GameEvent.Inst.PlayerDeadPostAction += OnPlayerDeadToChangeScore;
        }

        private void OnPlayerDeadToChangeScore(int attacker, int deadPlayer)
        {
            if (deadPlayer < 0) return;

            if (deadPlayer == 1)
            {
                AddPlayerScore(2);
            }
            if (deadPlayer == 2)
            {
                AddPlayerScore(1);
            }

            UpdateScoreModules();
        }

        private void AddPlayerScore(int playerId)
        {
            if (!m_PlayerScores.ContainsKey(playerId))
            {
                m_PlayerScores[playerId] = 0;
            }

            m_PlayerScores[playerId]++;
        }

        public void ResetAllScore()
        {
            m_PlayerScores.Clear();
        }

        private void UpdateScoreModules()
        {
            UIController.Inst.player1ScoreText.text = m_PlayerScores.GetValueOrDefault(1).ToString(); ;
            UIController.Inst.player2ScoreText.text = m_PlayerScores.GetValueOrDefault(2).ToString(); ;
        }
    }
}