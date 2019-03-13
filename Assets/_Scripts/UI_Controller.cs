using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    public GameObject gameObjStart;
    public GameObject gameObjEnd;

    void Start()
    {
        gameObjStart.SetActive(true);
    }

    void Update()
    {
        if (Input.GetButtonDown("Close dialog"))
        {
            if (gameObjStart.activeInHierarchy)
            {
                gameObjStart.SetActive(false);
  
            }
            if (gameObjEnd.activeInHierarchy)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level 2");
                gameObjEnd.SetActive(false);
            }
            
        }
    }
}
