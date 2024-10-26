using UnityEngine;
using System;
using System.Linq;
using PunchPeng;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScoreboardManager : MonoBehaviour
{
    public TextMeshProUGUI Player1ScoreText;
    public TextMeshProUGUI Player2ScoreText;
    public TextMeshProUGUI[] playerScores;

    private int[] scores = new int[2];
    public static Action<int, int> OnPlayerDead;

    void Start()
    {
        SetPlayerName(0, "Player1");
        SetPlayerName(1, "Player2");

        // 设置初始分数
        for (int i = 0; i < 2; i++)
        {
            scores[i] = 0;
            UpdateScore(i, scores[i]);
        }
        UpdateScoreModules();
    }

    private void OnPlayerDeadToChangeScore(int deadPlayer, int attacker)
    {
        // cal score
        if (deadPlayer >= 0 && deadPlayer < scores.Length)
        {
            if (attacker == -1 && scores[deadPlayer] > 0)
            {
                scores[deadPlayer]--;
                UpdateScore(deadPlayer, scores[deadPlayer]);
            }
            else if (attacker >= 0 && attacker < scores.Length)
            {
                scores[attacker]++;
                UpdateScore(attacker, scores[attacker]);
            }
            UpdateScoreModules();
        }
    }

    private void UpdateScoreModules()
    {
        int module1Score = scores[0];
        int module2Score = scores[1];
        
        // draw ui
        Player1ScoreText.text = module1Score.ToString();
        Player2ScoreText.text = module2Score.ToString();
    }
    
    private void Update()
    {
        
    }

    public void SetPlayerName(int playerIndex, string name)
    {
        //playerNames[playerIndex].text = name;
    }

    public void UpdateScore(int playerIndex, int score)
    {
        playerScores[playerIndex].text = score.ToString();
    }
}