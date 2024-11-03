using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PunchPeng
{
    public class UIController : SingletonMono<UIController>
    {
        public GameObject gameStartPanel;
        public Button startBtn;
        public Button ResetScoreBtn;

        public GameObject loadingPanel;
        public GameObject submitPanel;
        public TextMeshProUGUI player1ScoreText;
        public TextMeshProUGUI player2ScoreText;

        private readonly float m_CfgInputCD = 0.618f;
        private float allowInputGameTime;

        protected override void OnAwake()
        {
            GameEvent.Inst.OnGameEnd += ShowSubmit;
            startBtn.onClick.AddListener(ClickStartGame);
            ResetScoreBtn.onClick.AddListener(ClickResetScore);
        }

        private void Start()
        {
            ShowGameStart();
        }

        public void ClickStartGame()
        {
            ShowLoading();
        }

        private void ClickResetScore()
        {
            ScoreboardManager.Inst.ResetAllScore();
            ResetScoreBtn.gameObject.SetActiveEx(ScoreboardManager.Inst.HasScore);
        }

        public void ShowGameStart()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;
            ResetScoreBtn.gameObject.SetActiveEx(ScoreboardManager.Inst.HasScore);
            gameStartPanel.SetActive(true);
            loadingPanel.SetActive(false);
            submitPanel.SetActive(false);
        }

        public void ShowLoading()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;
            gameStartPanel.SetActive(false);
            loadingPanel.SetActive(true);
            submitPanel.SetActive(false);
        }

        public void ShowSubmit()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;
            gameStartPanel.SetActive(false);
            loadingPanel.SetActive(false);
            submitPanel.SetActive(true);
        }

        void Update()
        {
            if (Time.time < allowInputGameTime)
            {
                return;
            }

            if (gameStartPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ShowLoading();
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }

                return;
            }
            if (loadingPanel.activeSelf && Input.anyKeyDown)
            {
                loadingPanel.SetActive(false);
                GameEvent.Inst.OnGameStart?.Invoke();
                return;
            }
            else if (submitPanel.activeSelf && Input.anyKeyDown)
            {
                ShowGameStart();
                return;
            }
        }
    }
}