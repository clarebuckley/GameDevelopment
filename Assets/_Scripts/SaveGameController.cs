using System;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using static PlayerStateController;
using static AgentStateController;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;

public class SaveGameController : MonoBehaviour
{
    [Serializable]
    public struct GameState
    {
        public PlayerState playerState;
        public AgentState[] agentStates;
        public Scene scene;
        public GameState(PlayerState playerState, AgentState[] agentStates)
        {
            this.playerState = playerState;
            this.agentStates = agentStates;
            this.scene = SceneManager.GetActiveScene();
        }
    }

    private PlayerStateController player;
    private AgentStateController[] agents;
    private PlayerState playerState;
    private AgentState[] agentStates;
    private static readonly String timestamp = DateTime.Now.ToFileTime().ToString();
    private static readonly String SAVEGAME_FILE = "Assets/Saves/savegame-" + timestamp + ".xml";
    private GameObject playerPrefab;
    [SerializeField] GameObject zombiePrefab;


    private void Update()
    {
        agents = FindObjectsOfType<AgentStateController>();
        agentStates = new AgentState[agents.Length];
        playerPrefab = GameObject.FindGameObjectWithTag("Player");
        player = FindObjectOfType<PlayerStateController>();
        if (SceneManagerHelper.ActiveSceneName != "Cottage")
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Save(SAVEGAME_FILE);
                print("saved.");
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                Load(SAVEGAME_FILE);
                print("loaded.");
            }
        }
    }


    private void Save(string filename)
    {
        playerState = player.ToRecord();
        for (int i = 0; i < agents.Length; i++)
        {
            agentStates[i] = agents[i].ToRecord();
        }

        GameState gs = new GameState(playerState, agentStates);
        XmlDocument xmlDocument = new XmlDocument();
        XmlSerializer serializer = new XmlSerializer(typeof(GameState));
        using (MemoryStream stream = new MemoryStream())
        {
            serializer.Serialize(stream, gs);
            stream.Position = 0;
            xmlDocument.Load(stream);
            xmlDocument.Save(filename);
        }
    }


    private void Load(string filename)
    {
        GameObject playerInScene = GameObject.FindGameObjectWithTag("Player");

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filename);
        string xmlString = xmlDocument.OuterXml;

        GameState gameState;
        PlayerState playerState;
        AgentState[] agentStates;
        using (StringReader read = new StringReader(xmlString))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameState));
            using (XmlReader reader = new XmlTextReader(read))
            {
                gameState = (GameState)serializer.Deserialize(reader);
                playerState = gameState.playerState;
                agentStates = gameState.agentStates;
            }
        }

        GameObject newPlayer = Instantiate(GameObject.FindGameObjectWithTag("Player"));
        //remove script
        FirstPersonController script = newPlayer.GetComponent<FirstPersonController>();
        (script as MonoBehaviour).enabled = false;

        //Set player parameters
        AudioClip impactSound = playerInScene.GetComponent<PlayerHealthController>().getImpactSound();
        newPlayer.GetComponent<PlayerHealthController>().setImpactSound(impactSound);
        Destroy(playerInScene);

        newPlayer.transform.position = playerState.position;
        newPlayer.transform.rotation = playerState.rotation;

        //wait two frames, then load back in script
        StartCoroutine(LoadCorourine(script));
        
        //Load game stats
        GameStatsController.Score = playerState.score;
        GameStatsController.TimeTaken = playerState.timeTaken;
        GameStatsController.Health = playerState.health;

        //Load score
        GameObject canvas = GameObject.FindGameObjectWithTag("DontDestroyOnLoad");
        UI_Controller uiController = GameObject.FindObjectOfType<UI_Controller>();
        Text scoreText = uiController.getScore().GetComponent<Text>();
        scoreText.text = GameStatsController.Score.ToString();


        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (GameObject zombie in zombies)
        {
            //Reset agent targets
            Destroy(zombie);
         //   zombie.GetComponent<AgentController>().SetTarget(newPlayer.transform);
        }

        for (int i = 0; i < agentStates.Length-1; i++)
        {
            GameObject newZombie = Instantiate(zombiePrefab);
            newZombie.transform.position = agentStates[i].position;
            newZombie.transform.rotation = agentStates[i].rotation;

            newZombie.GetComponent<AgentController>().SetTarget(newPlayer.transform);
            GameObject[] waypoints = new GameObject[agentStates[i].waypoints.Length];

            for (int j = 0; j < agentStates[i].waypoints.Length; j++)
            {
                waypoints[j] = GameObject.Find(agentStates[i].waypoints[j]);
            }
            newZombie.GetComponent<AgentController>().waypoints = waypoints;
        }

       
    }

    IEnumerator LoadCorourine(FirstPersonController script)
    {
        yield return new WaitForSeconds(2);
        (script as MonoBehaviour).enabled = true;
    }





}
