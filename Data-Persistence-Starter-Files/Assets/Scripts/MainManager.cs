using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    int[] pointCountArray = new [] {1,1,2,2,5,5};
    [Range(1, 6)]
    public int LineCount = 1; //6;
    [Range(1, 6)]
    public int PerLineCount = 1; //8;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    // private int m_Points;
    
    private bool m_GameOver = false;
    private bool m_isWin = false;

    public float m_StartForece = 1.0f;
    public int m_MaxCountBall = 0;
    public int m_LeftBall = 0;

    
    const string m_strGameOver = @"GAME OVER
    Press Space to Restart";
    const string m_strGameFinish = @"CONGRATULATIONS!

    Press Space to Start New Level";

    private const string m_strScore = "Level : {0}, Score : {1}, Left : {2}";
    private const string m_strHighScore = "HighLevel : {0}, HighScore : {1}, TotalScore : {2}";

    
    // Start is called before the first frame update
    void Start()
    {
        Ball.gameObject.SetActive(true);
        Time.timeScale = 1f;
        
        const float step = 0.6f;
        // int perLine = Mathf.FloorToInt(4.0f / step);
        LineCount = Math.Min(LineCount, pointCountArray.Length);

        m_MaxCountBall = PerLineCount * LineCount;
        m_LeftBall = m_MaxCountBall;

        UpdateScore();
        
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < PerLineCount; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * m_StartForece, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void UpdateScore()
    {
        var runtimeScore = PersistentData.Instance.runtimeData;
        ScoreText.text = string.Format(m_strScore, runtimeScore.curLevel, runtimeScore.curScore, m_LeftBall);
        var highScore = PersistentData.Instance.data;
        HighText.text = string.Format(m_strHighScore, highScore.highLevel, highScore.highScore, highScore.totalScore);
    }

    void AddPoint(int point)
    {
        var points = PersistentData.Instance.AddScore(point);
        m_LeftBall--;

        UpdateScore();

        if (m_LeftBall == 0)
        {
            GameOver(true);
        }
    }

    public void GameOver(bool isWin)
    {
        m_isWin = isWin;
        m_GameOver = true;
        GameOverText.GetComponent<Text>().text = m_isWin ? m_strGameFinish : m_strGameOver;
        GameOverText.SetActive(true);

        PersistentData.Instance.GameOver(isWin);
        
        PersistentData.Instance.SaveData();

        StartCoroutine(PauseGame());
    }

    private IEnumerator PauseGame()
    {
        Ball.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0f;
        
        yield return null;
    }

    public void ClearScore()
    {
        PersistentData.Instance.ClearScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
