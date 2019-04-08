using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public AudioClip impactSound;
    private GameObject healthUI;
    public Sprite threeHalfHearts,threeHearts,twoHalfHearts,twoHearts,oneHalfHearts,oneHeart, halfHeart, noHearts;
    private Image image;
    private AudioSource audioSource;
    //4 points per heart
    public int healthPoints = GameStatsController.Health;
   
    public AudioClip getImpactSound()
    {
        return impactSound;
    }
    public void setImpactSound(AudioClip impactSound)
    {
        impactSound = impactSound;
    }


    // Update is called once per frame
    void Update()
    {
        GameObject healthUI = GameObject.FindGameObjectWithTag("Health");
        audioSource = GetComponent<AudioSource>();
        image = healthUI.GetComponent<Image>();

    }

    public void Impact()
    {
        Debug.Log(GameStatsController.Health);
        audioSource.PlayOneShot(impactSound);
        int health = GameStatsController.Health - 1;
        GameStatsController.Health = health;
        SetHealth(health);
    }

    private void SetHealth(int health)
    {
        switch (health)
        {
            case 12:
                image.sprite = threeHalfHearts;
                break;
            case 10:
                image.sprite = threeHearts;
                break;
            case 8:
                image.sprite = twoHalfHearts;
                break;
            case 6:
                image.sprite = twoHearts;
                break;
            case 4:
                image.sprite = oneHalfHearts;
                break;
            case 2:
                image.sprite = oneHeart;
                break;
            case 0:
                image.sprite = halfHeart;
                break;
            case -2:
                image.sprite = noHearts;
                GameStatsController.Score = GameStatsController.Score / 2;
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("End screen");
                break;
        }
    }
}
