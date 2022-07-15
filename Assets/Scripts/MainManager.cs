using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text UpperText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    public string enteredName;
    public string bestPlayerName;
    public int bestPlayerScore;
    public int bestScore;


    private bool m_GameOver = false;


    // Start is called before the first frame update
    private void Awake()
    {
        enteredName = MenuManager.Instance.savedName;

    }
    void Start()
    {
        LoadBest();
        UpperText.text = "BestScore: " + bestPlayerName + " : " + bestPlayerScore;
        const float step = 0.6f; 
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
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
        ScoreText.text = $"Score : {enteredName} :  {m_Points}";
        

        if (!m_Started)
        {
            
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
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
        ScoreText.text = $"Score : {enteredName } :  {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (m_Points > bestPlayerScore)
        {

            bestPlayerScore = m_Points;
            bestPlayerName = enteredName;
            SaveBest();
            UpperText.text = "BestScore: " + bestPlayerName + " : " + bestPlayerScore;
            Debug.Log(bestPlayerScore);
        }

    }
    public void ScoreUpperText()
    {
        
       
        
    }

    [System.Serializable]
    public class SaveData
    {
        public string bestPlayerName;
        public int bestPlayerScore;
    }
    public void SaveBest()
    {
        SaveData best = new SaveData();
        best.bestPlayerName = bestPlayerName;
        best.bestPlayerScore = bestPlayerScore;
        string json = JsonUtility.ToJson(best);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

    }
    public void LoadBest()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData best = JsonUtility.FromJson<SaveData>(json);
            bestPlayerName = best.bestPlayerName;
            bestPlayerScore = best.bestPlayerScore;
        }

    }
}  