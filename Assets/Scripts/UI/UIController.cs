using UnityEngine;
using UnityEngine.UI;

namespace PunchPeng
{
    public class UIController : MonoBehaviour
    {
        public GameObject gameStartPanel;
        public Button startBtn;
        public GameObject loadingPanel;
        public GameObject submitPanel;
        public Camera uiCamera; // 添加 UI Camera 的引用
        
        private float inputCooldown = 0.5f; // 输入间隔时间，单位为秒
        private float timeSinceLastInput = 0;

        private void Awake()
        {
            GameEvent.Inst.OnGameEnd += ShowSubmit;
            startBtn.onClick.AddListener(ClickStartGame);
        }

        private void Start()
        {
            ShowGameStart();
        }

        public void ShowGameStart()
        {
            gameStartPanel.SetActive(true);
            loadingPanel.SetActive(false);
            submitPanel.SetActive(false);
        }

        public void ShowLoading()
        {
            gameStartPanel.SetActive(false);
            loadingPanel.SetActive(true);
            submitPanel.SetActive(false);
        }

        public void ShowSubmit()
        {
            gameStartPanel.SetActive(false);
            loadingPanel.SetActive(false);
            submitPanel.SetActive(true);
        }

        void Update()
        {
            timeSinceLastInput += Time.deltaTime;
            
            if (gameStartPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Return) && timeSinceLastInput >= inputCooldown)
                {
                    timeSinceLastInput = 0;
                    ShowLoading();
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && timeSinceLastInput >= inputCooldown)
                {
                    timeSinceLastInput = 0;
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                }
            }
            if (loadingPanel.activeSelf && Input.anyKeyDown && timeSinceLastInput >= inputCooldown)
            {
                timeSinceLastInput = 0;
                loadingPanel.SetActive(false);
                GameEvent.Inst.OnGameStart?.Invoke();
            }
            else if (submitPanel.activeSelf && Input.anyKeyDown && timeSinceLastInput >= inputCooldown)
            {
                timeSinceLastInput = 0;
                ShowGameStart();
            }

            // 如果有基于射线检测的 UI 交互，可以在这里进行调整
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // 处理按钮点击等交互
                }
            }
        }

        public void ClickStartGame()
        {
            ShowLoading();
        }
    }
}