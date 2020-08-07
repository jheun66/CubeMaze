using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;

    private Vector3 previousMousePosition;
    private Vector3 mouseDelta;

    // Cube를 얼마나 회전 시킬지 담을 게임오브젝트
    public GameObject target = null;

    [SerializeField]
    private float sensitivity = 0.1f;
    [SerializeField]
    private float speed = 200.0f;


    // 마우스 우클릭으로 전체 큐브 회전(각 면의 회전은 PivotRotation에서)
    private void Update()
    {
        if(GameController.clickCube)
        {
            Swipe();
            Drag();
        }
    }

    private void Drag()
    {
        if (Input.GetMouseButton(1))
        {
            // while the mouse is held down the cube can be moved around its cetral axis to provide visual feedback
            mouseDelta = Input.mousePosition - previousMousePosition;
            mouseDelta *= sensitivity; // reduction of rotation speed

            // y축은 드래그 되게
            this.transform.rotation = Quaternion.Euler(0, -mouseDelta.x, 0) * this.transform.rotation;
        }
        else
        {
            // automatically move to the target position
            if (this.transform.rotation != target.transform.rotation)
            {
                float step = speed * Time.deltaTime;
                // target으로의 자연스러운 회전은 rotatetowards 사용
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, target.transform.rotation, step);
            }
        }

        previousMousePosition = Input.mousePosition;
    }

    private void Swipe()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // get the 2D position of the first mouse click
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //print(firstPressPos);

        }
        if (Input.GetMouseButtonUp(1))
        {
            // get the 2D position of the first mouse click
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            // create a vector from the first and second click positions
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            currentSwipe.Normalize();
            if (LeftSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 90, 0, Space.World);
            }
            else if (RightSwipe(currentSwipe))
            {
                target.transform.Rotate(0, -90, 0, Space.World);
            }
            else if (UpLeftSwipe(currentSwipe))
            {
                target.transform.Rotate(90, 0, 0, Space.World);
            }
            else if (UpRightSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 0, -90, Space.World);
            }
            else if (DownLeftSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 0, 90, Space.World);
            }
            else if (DownRightSwipe(currentSwipe))
            {
                target.transform.Rotate(-90, 0, 0, Space.World);
            }
        }
    }

    bool LeftSwipe(Vector2 swipe)
    {
        return currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    bool RightSwipe(Vector2 swipe)
    {
        return currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    bool UpLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.x < 0 && currentSwipe.y > 0;
    }

    bool UpRightSwipe(Vector2 swipe)
    {
        return currentSwipe.x > 0 && currentSwipe.y > 0;
    }

    bool DownLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.x < 0 && currentSwipe.y < 0;
    }

    bool DownRightSwipe(Vector2 swipe)
    {
        return currentSwipe.x > 0 && currentSwipe.y < 0;
    }

}
