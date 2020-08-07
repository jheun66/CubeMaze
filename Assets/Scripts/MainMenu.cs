using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // 로드전 대기화면보여주고

        this.gameObject.SetActive(false);

        // static 변수들
        {
            Automate2.moveList = new List<string>() { };
            CubeState2.autoRotating = false;
            CubeState2.started = false;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
   
}
