using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderController : MonoBehaviour, InteractiveObjectBase
{

    public GameObject gameObj;

    void Awake()
    {
        gameObj.SetActive(false);
    }

    public void OnInteraction()
    {
        gameObj.SetActive(true);
    }
}

