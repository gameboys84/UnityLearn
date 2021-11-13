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
    public int LineCount = 1; //6;
    public int PerLineCount = 1; //8;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private bool m_isWin = false;

    public float m_StartForece = 2.0f;
    public int m_MaxCountBall = 0;
    public int m_LeftBall = 0;

    
    const string m_strGameOver = @"GAME OVER
    Press Space to Restart";
    const string m_strGameFinish = @"CONGRATULATIONS!
    Press Space to Start New Level";

    private const string m_strScore = "Score : {0}, Left : {1}";

    
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
        
        ScoreText.text = string.Format(m_strScore, m_Points, m_LeftBall);
        
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

    void AddPoint(int point)
    {
        m_Points += point;
        m_LeftBall--;
        ScoreText.text = string.Format(m_strScore, m_Points, m_LeftBall);

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

        StartCoroutine(PauseGame());
    }

    private IEnumerator PauseGame()
    {
        Ball.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0f;
        
        yield return null;
    }
}
