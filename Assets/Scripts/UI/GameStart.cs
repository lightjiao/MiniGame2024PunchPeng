using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // 将"GameScene"替换为你的实际游戏场景名称
    }
}