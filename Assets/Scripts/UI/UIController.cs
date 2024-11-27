using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PunchPeng
{
    public class UIController : SingletonMono<UIController>
    {
        [Title("GameEntry")]
        public GameObject gameEntry;
        public Button btnStartGame;
        public Button btnResetScore;

        [Title("GamePreload")]
        public GameObject gamePreload;
        public Image gamePreloadImg;
        private UniTask? loadingTask;

        [Title("GameUI")]
        public GameObject gameUI;
        public GameObject counddoan321;
        public GameObject tree;
        public GameObject two;
        public GameObject one;

        [Title("GameSubmit")]
        public GameObject submitPanel;
        public TextMeshProUGUI player1ScoreText;
        public TextMeshProUGUI player2ScoreText;

        private readonly float m_CfgInputCD = 0.618f;
        private float allowInputGameTime;

        protected override void OnAwake()
        {
            GameEvent.Inst.BeforeLevelEnd += ShowSubmit;
            btnStartGame.onClick.AddListener(ShowLoading);
            btnResetScore.onClick.AddListener(ClickResetScore);
        }

        private void Start()
        {
            ShowGameEntry();
        }

        public void ShowGameEntry()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;
            btnResetScore.gameObject.SetActiveEx(ScoreboardManager.Inst.HasScore);
            gameEntry.SetActiveEx(true);
            gamePreload.SetActiveEx(false);
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

            GameFlowController.Inst.ChooseLevel();
            loadingTask = LevelController.Inst.LevelPreload();
            gamePreloadImg.sprite = ResourceMgr.Inst.Load<Sprite>(LevelController.Inst.CurLevelCfg.PreloadImg);

            gameEntry.SetActiveEx(false);
            gamePreload.SetActiveEx(true);
            gameUI.SetActiveEx(false);
            submitPanel.SetActiveEx(false);
        }

        public void ShowGameUI()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;
            gameEntry.SetActiveEx(false);
            gamePreload.SetActiveEx(false);
            gameUI.SetActiveEx(true);
            submitPanel.SetActiveEx(false);

            counddoan321.SetActiveEx(false);
        }

        public void ShowSubmit()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;
            gameEntry.SetActiveEx(false);
            gamePreload.SetActiveEx(false);
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

            if (gamePreload.activeSelf && Input.anyKeyDown && loadingTask != null && loadingTask.Value.Status == UniTaskStatus.Succeeded)
            {
                loadingTask = null;
                ShowGameUI();
                LevelController.Inst.LevelStart().Forget();
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