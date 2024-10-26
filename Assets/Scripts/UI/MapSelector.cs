using UnityEngine;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour
{
    public Button map1Button;
    public Button map2Button;
    // 添加更多地图按钮...

    public Text selectedMapText;
    public Image mapImage;
    public Button confirmButton;

    private void Start()
    {
        map1Button.onClick.AddListener(() => SelectMap("地图 1", "Map1Image"));
        map2Button.onClick.AddListener(() => SelectMap("地图 2", "Map2Image"));
        // 为更多地图按钮添加点击事件...

        confirmButton.onClick.AddListener(ConfirmMapSelection);
    }

    private void SelectMap(string mapName, string imageResourceName)
    {
        selectedMapText.text = mapName + " 被选中";
        mapImage.sprite = Resources.Load<Sprite>(imageResourceName);
    }

    private void ConfirmMapSelection()
    {
        // 在这里添加地图跳转的逻辑
        Debug.Log("确认选择，进行地图跳转");
    }
}