using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour, InteractiveObjectBase
{
  
    public void OnInteraction()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level 1");
    }
}
