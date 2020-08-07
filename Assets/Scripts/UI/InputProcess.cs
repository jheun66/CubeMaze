using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputProcess : MonoBehaviour
{
    public GameObject _answer = null;
    private Text _text = null;

    private Transform _player = null;
    
    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
        _text = _answer.GetComponent<Text>();
    }

    public void ProcessAnswer()
    {
        Transform currentRoom = _player.GetComponent<PlayerController>()._currentRoom;
        string _correctAnswer = currentRoom.GetComponent<RoomController>().RoomAnswer;
        if (_text.text == _correctAnswer)
        {
            currentRoom.GetComponent<RoomController>().Clear = true;

            FindObjectOfType<AudioManager>().Play("CorrectAnswer");
            Destroy(this.gameObject);
            Move.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("IncorrectAnswer");
            Destroy(this.gameObject);
            Move.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
