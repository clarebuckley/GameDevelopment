using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private AudioSource audioData;
    private Coroutine changeScene;

    public void ButtonHandlerPlay()
    {
        audioData = GetComponent<AudioSource>();
        audioData.Play();
        changeScene = StartCoroutine(EnterCottageCoroutine());
    }

    public void ButtonHandlerHelp()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Help");
    }

    public void ButtonHandlerBack()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main_Menu");
    }


    IEnumerator EnterCottageCoroutine()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync("Cottage");
    }
}

