using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

namespace PunchPeng
{
    public class UIController : SingletonMono<UIController>
    {
        public GameObject gameEntry;
        public Button btnStartGame;
        public Button btnResetScore;

        public GameObject loadingPanel;
        private Image loadingImg;

        public GameObject gameUI;
        public GameObject counddoan321;
        public GameObject tree;
        public GameObject two;
        public GameObject one;

        public GameObject submitPanel;
        public TextMeshProUGUI player1ScoreText;
        public TextMeshProUGUI player2ScoreText;

        private readonly float m_CfgInputCD = 0.618f;
        private float allowInputGameTime;

        protected override void OnAwake()
        {
            GameEvent.Inst.OnGameEnd += ShowSubmit;
            btnStartGame.onClick.AddListener(ShowLoading);
            btnResetScore.onClick.AddListener(ClickResetScore);
        }

        private void Start()
        {
            ShowGameStart();
        }

        public void ShowGameStart()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;
            btnResetScore.gameObject.SetActiveEx(ScoreboardManager.Inst.HasScore);
            gameEntry.SetActiveEx(true);
            loadingPanel.SetActiveEx(false);
            gameUI.SetActiveEx(false);
            submitPanel.SetActiveEx(false);
        }

        private void ClickResetScore()
        {
            ScoreboardManager.Inst.ResetAllScore();
            btnResetScore.gameObject.SetActiveEx(ScoreboardManager.Inst.HasScore);
        }

        public void ShowLoading()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;
            gameEntry.SetActiveEx(false);
            loadingPanel.SetActiveEx(true); // TODO: 加载 img
            gameUI.SetActiveEx(false);
            submitPanel.SetActiveEx(false);
        }

        public void ShowGameUI()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;
            gameEntry.SetActiveEx(false);
            loadingPanel.SetActiveEx(false);
            gameUI.SetActiveEx(true);
            submitPanel.SetActiveEx(false);

            counddoan321.SetActiveEx(false);
        }

        public void ShowSubmit()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;
            gameEntry.SetActiveEx(false);
            loadingPanel.SetActiveEx(false);
            gameUI.SetActiveEx(false);
            submitPanel.SetActiveEx(true);
        }

        private void Update()
        {
            if (Time.time < allowInputGameTime)
            {
                return;
            }

            if (gameEntry.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ShowLoading();
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    QuitGame();
                }
                return;
            }

            if (loadingPanel.activeSelf && Input.anyKeyDown)
            {
                ShowGameUI();
                GameEvent.Inst.OnGameStart?.Invoke();
                return;
            }
            else if (submitPanel.activeSelf && Input.anyKeyDown)
            {
                ShowLoading();
                return;
            }
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public async UniTask PlayCoundDown321Anim()
        {
            counddoan321.SetActiveEx(true);
            one.SetActiveEx(false);
            two.SetActiveEx(false);
            tree.SetActiveEx(false);
            one.transform.localScale = Vector3.one;
            two.transform.localScale = Vector3.one;
            tree.transform.localScale = Vector3.one;

            tree.SetActiveEx(true);
            await tree.transform.DOScale(3, 0.5f).AsyncWaitForCompletion();
            tree.SetActiveEx(false);

            two.SetActiveEx(true);
            await two.transform.DOScale(3, 0.5f).AsyncWaitForCompletion();
            two.SetActiveEx(false);

            one.SetActiveEx(true);
            await one.transform.DOScale(3, 0.5f).AsyncWaitForCompletion();
            one.SetActiveEx(false);

            counddoan321.SetActiveEx(false);
        }
    }
}