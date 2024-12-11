using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PunchPeng
{
    public class UIController : SingletonMono<UIController>
    {
        [Title("GameEntry")]
        public GameObject gameEntry;
        public Button btnStartGame;
        public Button btnQuitGame;
        public Button btnResetScore;

        [Title("GamePreload")]
        public GameObject gamePreload;
        public Image gamePreloadImg;
        private UniTask? loadingTask;

        [Title("GameUI")]
        public GameObject gameUI;
        public GameObject countDown321;
        public GameObject tree;
        public GameObject two;
        public GameObject one;
        public TextMeshProUGUI player1CollectScore;
        public TextMeshProUGUI player2CollectScore;

        [Title("GameSubmit")]
        public GameObject submitPanel;
        public TextMeshProUGUI player1ScoreText;
        public TextMeshProUGUI player2ScoreText;
        public TextMeshProUGUI player1Win;
        public TextMeshProUGUI player2Win;

        private readonly float m_CfgInputCD = 0.618f;
        private float allowInputGameTime;

        protected override void OnAwake()
        {
            GameEvent.Inst.LevelEndPreAction += ShowSubmit;
            btnStartGame.onClick.AddListener(ShowLoading);
            btnQuitGame.onClick.AddListener(QuitGame);
        }

        private void Start()
        {
            ShowGameEntry();
        }

        public void ShowGameEntry()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;

            GameController.Inst.ResetGame();

            gameEntry.SetActiveEx(true);
            gamePreload.SetActiveEx(false);
            gameUI.SetActiveEx(false);
            submitPanel.SetActiveEx(false);
        }

        public void ShowLoading()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;

            loadingTask = LevelController.Inst.LevelLoad();
            gamePreloadImg.sprite = ResourceManager.Inst.Load<Sprite>(LevelController.Inst.CurLevelCfg.PreloadImg);

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

            countDown321.SetActiveEx(false);
        }

        public void ShowSubmit()
        {
            allowInputGameTime = Time.time + m_CfgInputCD;
            gameEntry.SetActiveEx(false);
            gamePreload.SetActiveEx(false);
            gameUI.SetActiveEx(false);
            submitPanel.SetActiveEx(true);
            player1Win.gameObject.SetActiveEx(false);
            player2Win.gameObject.SetActiveEx(false);

            var winPlayerId = ScoreboardManager.Inst.GetWinPlayer();
            if (winPlayerId != 0)
            {
                GameController.Inst.Winner = winPlayerId;
                ShowWinner(winPlayerId).Forget();
            }
        }

        private async UniTask ShowWinner(int winPlayerId)
        {
            allowInputGameTime = Time.time + 999; // pretty hack
            await PlayGameWinUIAnim(winPlayerId);
            allowInputGameTime = Time.time + m_CfgInputCD;
        }

        private void Update()
        {
            if (Time.time < allowInputGameTime)
            {
                return;
            }

            if (gameEntry.activeSelf && Input.anyKeyDown)
            {
                GameController.Inst.ChooseRandomLevelId();
                ShowLoading();
                //if (Input.GetKeyDown(KeyCode.Escape))
                //{
                //    QuitGame();
                //}
                return;
            }

            if (gamePreload.activeSelf && Input.anyKeyDown && loadingTask != null && loadingTask.Value.Status == UniTaskStatus.Succeeded)
            {
                loadingTask = null;
                ShowGameUI();
                LevelController.Inst.LevelStart().Forget();
                return;
            }

            if (submitPanel.activeSelf && Input.anyKeyDown)
            {
                if (GameController.Inst.Winner == 0)
                {
                    GameController.Inst.ChooseRandomLevelId();
                    ShowLoading();
                }
                else
                {
                    ShowGameEntry();
                }

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
            countDown321.SetActiveEx(true);
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

            countDown321.SetActiveEx(false);
        }

        [Button("PlayGameWinUIAnim")]
        private async UniTask PlayGameWinUIAnim(int winPlayerId)
        {
            player1Win.gameObject.SetActive(false);
            player2Win.gameObject.SetActive(false);

            TextMeshProUGUI winTxt = null;
            switch (winPlayerId)
            {
                case 1:
                    winTxt = player1Win;
                    break;
                case 2:
                    winTxt = player2Win;
                    break;
            }

            if (winTxt == null) return;

            winTxt.transform.localScale = Vector3.one * 3;
            winTxt.gameObject.SetActiveEx(true);

            var txtDefaultColor = winTxt.color;

            await winTxt.DOScale(1, 0.618f).SetEase(Ease.InCubic).AsyncWaitForCompletion();
            await winTxt.DOColor(Color.white, 0.1f).SetLoops(3).AsyncWaitForCompletion();

            winTxt.color = txtDefaultColor;
        }
    }
}