using System;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreenController : MonoBehaviour
{
    [SerializeField] private GameObject score;
    [SerializeField] private GameObject timeTaken;
    [SerializeField] private GameObject endScore;
    [SerializeField] private GameObject highScore;
    private static readonly String dateTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
    private AudioSource audioData;

    [Serializable]
    public struct ScoreState
    {
        public String score;
        public String dateTime;
        public ScoreState(String score, String dateTime)
        {
            this.score = score;
            this.dateTime = dateTime;

        }
    }



    void Awake()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("DontDestroyOnLoad");
        UI_Controller uiController = GameObject.FindObjectOfType<UI_Controller>();
        Text scoreText = score.GetComponent<Text>();
        Text timeTakenText = timeTaken.GetComponent<Text>();
        Text endScoreText = endScore.GetComponent<Text>();
        scoreText.text = GameStatsController.Score.ToString();
        timeTakenText.text = GameStatsController.TimeTaken;
        if (timeTakenText.text != "--:--")
        {
            String timeTakenFormatted = timeTakenText.text.Replace(":", ".");
            Double endScoreVal = Convert.ToDouble(GameStatsController.Score) - (Convert.ToDouble(timeTakenFormatted) * 15);
            endScoreText.text = Math.Round(endScoreVal).ToString();
        }
        else
        {
            endScoreText.text = 1.ToString();
        }

        audioData = GetComponent<AudioSource>();
        audioData.Play();
        SaveScore(endScoreText.text);
        LoadHighScores();
    }

    public void Update()
    {
        if (!Cursor.visible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ButtonHandlerBack()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main_Menu");
        GameObject canvas = GameObject.FindGameObjectWithTag("DontDestroyOnLoad");
        GameStatsController.Score = 0;
        GameStatsController.Health = 16;
        Destroy(canvas);
    }

    private void SaveScore(String score)
    {
        ScoreState ss = new ScoreState(score, dateTime);
        XmlDocument xmlDocument = new XmlDocument();
        XmlSerializer serializer = new XmlSerializer(typeof(ScoreState));
        using (MemoryStream stream = new MemoryStream())
        {
            serializer.Serialize(stream, ss);
            stream.Position = 0;
            xmlDocument.Load(stream);
            xmlDocument.Save("Assets/Saves/Scores/" + dateTime + ".xml");
        }
    }

    private void LoadHighScores()
    {
        String[] files = Directory.GetFiles("Assets/Saves/Scores/");
        String highScoreText = "High scores: " + System.Environment.NewLine;

        XmlDocument xmlDocument = new XmlDocument();
        foreach (String fileName in files)
        {
            Debug.Log(fileName);
            if (!fileName.Contains("meta"))
            {
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;
                ScoreState scoreState;
                String score, date;

                using (StringReader read = new StringReader(xmlString))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ScoreState));
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        scoreState = (ScoreState)serializer.Deserialize(reader);
                        score = scoreState.score;
                        date = scoreState.dateTime;
                    }
                }

                highScoreText = highScoreText + " Date: " + date + "  Score: " + score + System.Environment.NewLine;
                Text addTo = highScore.GetComponent<Text>();
                addTo.text = highScoreText;
            }
        }





    }
}
