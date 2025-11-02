using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject titleScreenPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;  // 시간 텍스트 추가

    [Header("게임 상태")]
    private int score = 0;
    private float playTime = 0f;  // 게임 진행 시간
    private bool isPlaying = false;  // 게임 진행 중인지

    void Start()
    {
        ShowTitleScreen();
        UpdateScoreUI();
        UpdateTimeUI();
    }

    void Update()
    {
        // 게임 진행 중일 때만 시간 증가
        if (isPlaying)
        {
            playTime += Time.deltaTime;
            UpdateTimeUI();
        }
    }

    void ShowTitleScreen()
    {
        titleScreenPanel.SetActive(true);
        Time.timeScale = 0f;
        isPlaying = false;
    }

    public void StartGame()
    {
        titleScreenPanel.SetActive(false);
        Time.timeScale = 1f;
        score = 0;
        playTime = 0f;  // 시간 초기화
        isPlaying = true;  // 게임 시작
        UpdateScoreUI();
        UpdateTimeUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // 시간 UI 업데이트
    void UpdateTimeUI()
    {
        if (timeText != null)
        {
            // 시간을 분:초 형식으로 변환
            int minutes = Mathf.FloorToInt(playTime / 60f);
            int seconds = Mathf.FloorToInt(playTime % 60f);

            // 00:00 형식으로 표시
            timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }
}