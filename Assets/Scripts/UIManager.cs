using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private TextMeshProUGUI gameStateText;
    [SerializeField] private Button nextLevelButton;
    public TextMeshProUGUI finalScoreText;

    public void ShowEndGameUI()
    {
        endGameUI.SetActive(true);
    }
    public void SetGameOverUI()
    {
        ShowEndGameUI();
        gameStateText.text = "Level Complete";
        nextLevelButton.gameObject.SetActive(false);
    }
    public void SetGameWinUI()
    {
        ShowEndGameUI();
        gameStateText.text = "Level Failed";
    }
}
