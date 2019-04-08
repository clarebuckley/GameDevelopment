using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    public GameObject gameObjStartL1;
    public GameObject gameObjEndL1;
    public GameObject gameObjStartL2;
    public GameObject gameObjEndL2;
    public GameObject timeTakenText;
    public GameObject timeTakenValue;
    public GameObject score;
    public Image crosshairImage;
    private AudioSource audioSource;
    public AudioClip sound;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Cottage")
        {
            gameObjStartL1.SetActive(true);
            audioSource = GetComponent<AudioSource>();
        }

    }

    public Image getCrosshairImage()
    {
        return crosshairImage;
    }
    public GameObject getScore()
    {
        return score;
    }
    public GameObject getTimeTaken()
    {
        return timeTakenValue;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            Text timeTakenTextVal = timeTakenValue.GetComponent<Text>();
            float t = Time.timeSinceLevelLoad;
            int mins = (int)(t / 60);
            int rest = (int)(t % 60);
            timeTakenTextVal.text = string.Format("{0:D2}:{1:D2}", mins, rest);
            GameStatsController.TimeTaken = timeTakenTextVal.text;
        }
           

        if (SceneManager.GetActiveScene().name == "Level_1")
        {
            if (Input.GetButtonDown("Close dialog"))
            {
                if (gameObjStartL1.activeInHierarchy)
                {
                    gameObjStartL1.SetActive(false);

                }
                if (gameObjEndL1.activeInHierarchy)
                {
                    if (SceneManager.GetActiveScene().name == "Level_1")
                    {
                        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level 2");
                        audioSource.PlayOneShot(sound);
                        gameObjStartL2.SetActive(true);

                        timeTakenText.SetActive(true);
                        timeTakenValue.SetActive(true);
                    }
                    gameObjEndL1.SetActive(false);
                }

            }
        }
        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            if (Input.GetButtonDown("Close dialog"))
            {
                if (gameObjStartL2.activeInHierarchy)
                {
                    gameObjStartL2.SetActive(false);
                }
                if (gameObjEndL2.activeInHierarchy)
                {
                    gameObjEndL2.SetActive(false);
                    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("End screen");
                }

            }
        }

    }


    public void GrandpaInteraction()
    {
        gameObjEndL2.SetActive(true);
    }

}
