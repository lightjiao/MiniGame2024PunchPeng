using UnityEngine;
using TMPro;

namespace PunchPeng
{

    public class ScoreboardManager : MonoBehaviour
    {
        public TextMeshProUGUI player1ScoreText;
        public TextMeshProUGUI player2ScoreText;
        //public TextMeshProUGUI[] playerScores;

        public int[] scores = new int[2];

        private void Awake()
        {
            SetPlayerName(0, "Player1");
            SetPlayerName(1, "Player2");

            // 设置初始分数
            for (int i = 0; i < 2; i++)
            {
                scores[i] = 0;
                //UpdateScore(i, scores[i]);
            }

            UpdateScoreModules();
            GameEvent.Inst.OnPlayerDead += OnPlayerDeadToChangeScore;
        }

        private void OnPlayerDeadToChangeScore(int attacker, int deadPlayer)
        {
            // cal score
            Debug.Log("player's index");
            var deadPlayerIndex = deadPlayer - 1;
            var attackerIndex = attacker - 1;
            Debug.Log(deadPlayer);
            Debug.Log(attacker);
            if (deadPlayer >= 0 && deadPlayer <= scores.Length)
            {
                if (attacker <0 && scores[deadPlayerIndex] > 0)
                {
                    Debug.Log("attacked");
                    Debug.Log(deadPlayer);
                    scores[deadPlayerIndex]--;
                }
                else if (attacker >= 0 && attackerIndex < scores.Length)
                {
                    Debug.Log("attack");
                    Debug.Log(attacker);
                    scores[attackerIndex]++;
                }
                UpdateScoreModules();
            }
        }

        private void UpdateScoreModules()
        {
            int module1Score = scores[0];
            int module2Score = scores[1];

            // draw ui
            player1ScoreText.text = module1Score.ToString();
            player2ScoreText.text = module2Score.ToString();
        }
        
        private void SetPlayerName(int playerIndex, string playerName)
        {
            //playerNames[playerIndex].text = playerName;
        }


        public void UpdateScore(int playerIndex, int score)
        {
            //playerScores[playerIndex].text = score.ToString();
        }
    }
}