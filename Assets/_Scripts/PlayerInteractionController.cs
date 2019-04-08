using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInteractionController : MonoBehaviour
{
    private Image image;
    public Sprite unselected;
    public Sprite selected;
    private Text scoreText;


    void Update()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("DontDestroyOnLoad");
        UI_Controller uiController = GameObject.FindObjectOfType<UI_Controller>();
        image = uiController.getCrosshairImage();
        if (SceneManager.GetActiveScene().name != "Cottage")
        {
            scoreText = uiController.getScore().GetComponent<Text>();
        }
        PhysicsRaycasts();
    }

    private void PhysicsRaycasts()
    {
        Vector3 centreOfScreen = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        float distanceToFireRay = 5;
        Ray centreOfScreenRay = Camera.main.ScreenPointToRay(centreOfScreen);
        RaycastHit hit;

        if (Physics.Raycast(centreOfScreenRay, out hit, distanceToFireRay))
        {
            //Only show selected crosshair when looking at interactive objects
            if (hit.transform.GetComponent("InteractiveObjectBase"))
            {
                image.sprite = selected;
            }
            else
            {
                image.sprite = unselected;
            }

            //Click interacts with only interactable objects
            if (Input.GetMouseButtonDown(0) && hit.transform.GetComponent("InteractiveObjectBase"))
            {
                if (hit.transform.GetComponent<DoorController>())
                {
                    hit.transform.GetComponent<DoorController>().OnInteraction();
                }
                if (hit.transform.GetComponent<BoulderController>())
                {
                    hit.transform.GetComponent<BoulderController>().OnInteraction();
                }
                if (hit.transform.tag == "Zombie")
                {
                    if (hit.transform.GetComponent<AgentController>().state != AgentController.AgentState.Dead)
                    {
                        GameStatsController.Score += 10;
                    }
                   
                    scoreText.text = GameStatsController.Score.ToString();
                    hit.transform.GetComponent<AgentController>().OnInteraction();
                }
                if (hit.transform.GetComponent<WeaponController>())
                {
                    hit.transform.GetComponent<WeaponController>().OnInteraction();
                }
                if (hit.transform.GetComponent<GrandpaController>())
                {
                    hit.transform.GetComponent<GrandpaController>().OnInteraction();
                }
            }
        }
    }


}
