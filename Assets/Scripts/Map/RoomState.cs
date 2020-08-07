using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomState : MonoBehaviour
{
    // 각 방향의 큐브면 리스트 (ReadCube에서 넣어줌)
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    public void PickUp(List<GameObject> cubeSide)
    {
        foreach (GameObject face in cubeSide)
        {
            // 선택된 면의 4번째 인덱스를 제외하고 나머지들을 4번째 인덱스의 자식으로 붙임
            if (face != cubeSide[4])
            {
                face.transform.parent.transform.parent.transform.parent = cubeSide[4].transform.parent.transform.parent;
            }
        }
    }

    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        // 반대로 해제
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[4])
            {
                littleCube.transform.parent.transform.parent.transform.parent = pivot.transform.parent;
            }
        }
    }

}
