using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private GameObject _currentQuestion = null;
    private GameObject _canvas = null;

    public static bool clickCube = false;

    public GameObject exitWindow;
    public Renderer exitRenderer;
    public Color exitColor;

    // 중앙 센터로 가기 위한 문
    public GameObject fWindow;
    public Renderer fRenderer;
    public Color fColor;
    public GameObject bWindow;
    public Renderer bRenderer;
    public Color bColor;
    public GameObject uWindow;
    public Renderer uRenderer;
    public Color uColor;
    public GameObject dWindow;
    public Renderer dRenderer;
    public Color dColor;
    public GameObject rWindow;
    public Renderer rRenderer;
    public Color rColor;
    public GameObject lWindow;
    public Renderer lRenderer;
    public Color lColor;

    public GameObject PauseMenuUI = null;
    public static bool GameIsPaused = false;

    public GameObject EndPanel = null;
    public static bool GameIsCompleted = false;

    private void Awake()
    {
        _canvas = GameObject.Find("GameCanvas");
    }

    private void Start()
    {
        exitColor = exitRenderer.material.color;

        fColor = fRenderer.material.color;
        bColor = bRenderer.material.color;
        uColor = uRenderer.material.color;
        dColor = dRenderer.material.color;
        rColor = rRenderer.material.color;
        lColor = lRenderer.material.color;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !GameIsCompleted)
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }


        if(_currentQuestion != null && Input.GetKey(KeyCode.Return))
        {
            _canvas.GetComponentInChildren<InputProcess>().ProcessAnswer();
        }


        MakeExit();
        MakeCenterWindow();

    }

    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;

        // static 변수들
        {
            Automate.Solve = false;
            Automate.moveList = new List<string>() { };
            CubeState.autoRotating = false;
            CubeState.started = false;
            CubeState.solvedCube = false;
            Move.canMove = true;
            GameController.clickCube = false;
            GameIsPaused = false;
            GameIsCompleted = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


   
    public void CompleteGame()
    {
        StartCoroutine(ActiveEndPannel());
    }

    public IEnumerator ActiveEndPannel()
    {
        yield return new WaitForSeconds(2);

        GameIsCompleted = true;
        EndPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;
    }
    


    public void InstantiateQuestion(Transform currentRoom)
    {
        GameObject question = currentRoom.GetComponent<RoomController>()._roomQuestion;
        if (_currentQuestion == null && !currentRoom.GetComponent<RoomController>().Clear)
        {
            // 플레이어의 위치를 파악하고 그 방 이름에 맞는 혹은 번호에 맞는 Question Instat..
            _currentQuestion = Instantiate(question, _canvas.transform);
            Move.canMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void MakeExit()
    {
        if(CubeState.solvedCube)
        {
            exitWindow.SetActive(true);
            exitRenderer.material.color = exitColor * 1;
        }
        else
        {
            exitWindow.SetActive(false);
            exitRenderer.material.color = exitColor * 0;
        }
    }

    private void MakeCenterWindow()
    {
        if(fWindow.transform.parent.transform.parent.GetComponent<RoomController>().Clear
            && bWindow.transform.parent.transform.parent.GetComponent<RoomController>().Clear
            && uWindow.transform.parent.transform.parent.GetComponent<RoomController>().Clear
            && dWindow.transform.parent.transform.parent.GetComponent<RoomController>().Clear
            && rWindow.transform.parent.transform.parent.GetComponent<RoomController>().Clear
            && lWindow.transform.parent.transform.parent.GetComponent<RoomController>().Clear)
        {
            fWindow.SetActive(true);
            fRenderer.material.color = fColor * 1;
            bWindow.SetActive(true);
            bRenderer.material.color = bColor * 1;
            uWindow.SetActive(true);
            uRenderer.material.color = uColor * 1;
            dWindow.SetActive(true);
            dRenderer.material.color = dColor * 1;
            rWindow.SetActive(true);
            rRenderer.material.color = rColor * 1;
            lWindow.SetActive(true);
            lRenderer.material.color = lColor * 1;
        }
        else
        {
            fWindow.SetActive(false);
            fRenderer.material.color = fColor * 0;
            bWindow.SetActive(false);
            bRenderer.material.color = bColor * 0;
            uWindow.SetActive(false);
            uRenderer.material.color = uColor * 0;
            dWindow.SetActive(false);
            dRenderer.material.color = dColor * 0;
            rWindow.SetActive(false);
            rRenderer.material.color = rColor * 0;
            lWindow.SetActive(false);
            lRenderer.material.color = lColor * 0;
        }
    }


}
