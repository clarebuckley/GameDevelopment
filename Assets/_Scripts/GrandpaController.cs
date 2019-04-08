using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandpaController : MonoBehaviour, InteractiveObjectBase
{
    private UI_Controller ui;
    private Animator animController;
    private Coroutine interaction;

    void Awake()
    {
        animController = GetComponent<Animator>();
        GameObject canvas = GameObject.FindGameObjectWithTag("DontDestroyOnLoad");
        ui = GameObject.FindObjectOfType<UI_Controller>();
    }


    public void OnInteraction()
    {
        int awakeHashId = Animator.StringToHash("Awake");
        animController.SetTrigger(awakeHashId);
        interaction = StartCoroutine(StartInteractionCoroutine());
       
    }

    IEnumerator StartInteractionCoroutine()
    {
        //Wait until grandpa stands up
        yield return new WaitForSeconds(3);
        ui.GrandpaInteraction();
    }
}


