using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerHealth playerHealth;
    private int currentScore;
    private void Awake()
    {
        
    }
    void Start()
    {
        currentScore = 0;
    }
    void Update()
    {
        if (playerHealth.IsDeath) GameOver();
    }
    public void AddScore(int score)
    {
        currentScore += score;
        scoreText.text = "Score:" + currentScore;
    }
    public void GameOver()
    {
        uiManager.SetGameOverUI();
    }
    public void GameWin()
    {
        uiManager.SetGameWinUI();
    }
}
