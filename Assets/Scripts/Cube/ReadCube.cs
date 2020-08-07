using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCube : MonoBehaviour
{
    // 기준 Rays 위치
    private Transform tUp;
    private Transform tDown;
    private Transform tLeft;
    private Transform tRight;
    private Transform tFront;
    private Transform tBack;

    // 각 방향별 Ray를 담을 리스트
    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();

    private int layerMask = 1 << 10;     // this layerMask is for the face of Cube
    private CubeState cubeState = null;
    private CubeMap cubeMap = null;

    // RayStart Prefab
    public GameObject emptyGO = null;

    private void Awake()
    {
        Transform Rays = this.transform.Find("Rays");
        tUp = Rays.Find("Up");
        tDown = Rays.Find("Down");
        tLeft = Rays.Find("Left");
        tRight = Rays.Find("Right");
        tFront = Rays.Find("Front");
        tBack = Rays.Find("Back");
        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetRayTrasform();
        ReadState();
        CubeState.started = true;
    }
   
    private void SetRayTrasform()
    {
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 90, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
        backRays = BuildRays(tBack, new Vector3(0, 270, 0));
    }

    // direction은 rayTransform에서의 z축이 원하는 방향을 가리키도록하는 오일러 회전 값(로컬)
    private List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        // The ray count is used to name the rays so we can be sure they are in the right order.
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();
        // This creates 9 rays in the shape of the side of the cube with
        // Ray 0 at the top left and Ray 8 at the bottom right:
        // |0|1|2|
        // |3|4|5|
        // |6|7|8|

        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3 startPos = new Vector3(rayTransform.localPosition.x + x,
                                               rayTransform.localPosition.y + y,
                                               rayTransform.localPosition.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
            }
        }

        // z축이 큐브를 바라보도록 회전
        rayTransform.localRotation = Quaternion.Euler(direction);

        return rays;
    }

    public void ReadState()
    {
        // set the state of each position in the list of sides so we know
        // what color in what position
        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);

        if(cubeState.GetStateString() == "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB")
        {
            CubeState.solvedCube = true;
        }
        else
        {
            CubeState.solvedCube = false;
        }

        cubeMap.Set();
    }

    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTrasform)
    {
        List<GameObject> faceHit = new List<GameObject>();

        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;

            // Does the ray intersect any objects in the layerMask?
            if (Physics.Raycast(ray, rayTrasform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, rayTrasform.forward * hit.distance, Color.yellow);
                faceHit.Add(hit.collider.gameObject);
                //print(hit.collider.gameObject.name);
            }
            else
            {
                Debug.DrawRay(ray, rayTrasform.forward * 1000, Color.green);
            }
        }

        return faceHit;
    }

}
