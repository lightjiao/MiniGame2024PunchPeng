using UnityEngine;
using UnityEngine.UI;

namespace PunchPeng
{
    public class UIController : MonoBehaviour
    {
        public GameObject gameStartPanel;
        public GameObject loadingPanel;
        public GameObject submitPanel;
        public Camera uiCamera; // 添加 UI Camera 的引用
        
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
            if (loadingPanel.activeSelf && Input.anyKeyDown)
            {
                ShowSubmit();
            }
            else if (submitPanel.activeSelf && Input.anyKeyDown)
            {
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
    }
}