using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameController : MonoBehaviour
{
    private AudioSource musicData;
    [SerializeField] private AudioSource zombieSound;
    [SerializeField] Animator animController;
    private bool triggered = false;

    private void Start()
    {
       
    }

    public void OnTriggerEnter(Collider collider)
    {
        animController = GetComponent<Animator>();
        if (collider.CompareTag("Player") && !triggered)
        {
            zombieSound.Play();
            
            musicData = GetComponent<AudioSource>();
            musicData.PlayDelayed(zombieSound.clip.length);
            triggered = true;
        }

    }
}
