using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene2Contoller : MonoBehaviour
{
    public GameObject gameOverImage;
    public GameObject gameWinImage;
    public GameObject backGround;
    public GameObject textString;
    private void Start()
    {
        backGround.SetActive(false);
        gameOverImage.SetActive(false);
        gameWinImage.SetActive(false);
    }
    //public IEnumerator BadGameOver()
    public void BadGameOver()
    {
        textString.SetActive(false);
        //yield return 1;
        Time.timeScale = 0;//‘›Õ£”Œœ∑
        gameOverImage.SetActive(true);
        
    }
    public void WinGameOver() 
    {
        textString.SetActive(false);
        Time.timeScale = 0;//‘›Õ£”Œœ∑
        gameWinImage.SetActive(true);
    }
    public void BackTheMenu()
    {
        textString.SetActive(false);
        gameOverImage.SetActive(false);
        gameWinImage.SetActive(false);
        backGround.SetActive(true);
    }
}
