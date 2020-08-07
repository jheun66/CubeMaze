using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform _currentRoom = null;

    private GameController _gameController = null;
   
    private int window = 1 << 9;
    private int cube = 1 << 11;

    private void Awake()
    {
        _gameController = GameObject.Find("GameSystem").GetComponent<GameController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!GameController.clickCube)
        {
            ClickWindow();
            ClickCube();
        }

        // 회전 중에는 방에 자식화를 해야 방과 함께 이동
        if(CubeState.autoRotating)
        {
            this.transform.parent = _currentRoom;
            Vector3 _localPosition = new Vector3(Mathf.Clamp(this.transform.localPosition.x,-3.0f,3.0f), Mathf.Clamp(this.transform.localPosition.y, -3.0f, 3.0f), Mathf.Clamp(this.transform.localPosition.z, -3.0f, 3.0f));
            this.transform.localPosition = _localPosition;
        }
        else
        {
            this.transform.parent = null;
            Vector3 euler = this.transform.eulerAngles;
            euler.x = 0;
            euler.z = 0;
            this.transform.eulerAngles = euler;
        }
    }

    private void ClickWindow()
    {
        RaycastHit hit;
        if (Physics.Raycast(GetMouseRay(), out hit, 30, window))
        {
            if (Input.GetMouseButton(0))
            {
                _gameController.InstantiateQuestion(_currentRoom);
               
            }
        }
    }

    private void ClickCube()
    {
        RaycastHit hit;
        if (Physics.Raycast(GetMouseRay(), out hit, 30, cube))
        {
            if (Input.GetMouseButton(0))
            {
                Move.canMove = false;
                GameController.clickCube = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void ChangeCurrentRoom(Transform transform)
    {
        if(_currentRoom != transform)
        {
            _currentRoom = transform;
            StopAllCoroutines();
            StartCoroutine(_currentRoom.GetComponent<RoomController>().TriggerInTime());
        }
    }

    // 마우스위치로 ray발사 ray반환하는 함수
    private Ray GetMouseRay()
    {
        // 카메라에서 화면좌표로 ray 발사
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }


}
