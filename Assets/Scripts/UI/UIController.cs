using UnityEngine;
using UnityEngine.UI;

namespace PunchPeng
{
    public class UIController : MonoBehaviour
    {
        public GameObject gameStartPanel;
        public GameObject loadingPanel;
        public GameObject submitPanel;

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
        }
    }
}