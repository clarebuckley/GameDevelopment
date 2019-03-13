using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameController : MonoBehaviour
{
    private AudioSource audioData;
    public Animator animator;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            audioData = GetComponent<AudioSource>();
            audioData.Play();
            animator.SetBool("gameStarted", true) ;
        }

    }
}
