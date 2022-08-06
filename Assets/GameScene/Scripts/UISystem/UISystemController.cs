using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UISystemController : MonoBehaviour
{
    public GameObject uniStorm;
    public void ResetScene() 
    {
        //SceneManager.LoadScene("MainScenes");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadSceneAsync("MainScenes");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        uniStorm.SetActive(true);
    }
    public void BackScene() 
    {
        SceneManager.LoadScene("StartScene");
    }
}
