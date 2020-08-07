using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizeCube : MonoBehaviour
{
    private ReadRoomCube readRoomCube = null;
    private RoomState roomState = null;

    // 회전을 진행할 side와 pivot
    private List<GameObject> activeSide;
    private Transform activePivot;
    
    [SerializeField]
    private float speed = 300.0f;
    
    private Quaternion targetQuaternion;
    
    private void Awake()
    {
        readRoomCube = FindObjectOfType<ReadRoomCube>();
        roomState = FindObjectOfType<RoomState>();
    }

    private void Start()
    {
        speed = FindObjectOfType<PivotRotation>().Speed;
    }

    // pivotRotation에서 같이 호출
    public void SelectFace(int i)
    {
        if (Input.GetMouseButtonDown(0) && !CubeState.autoRotating)
        {
            // 현재 상태 읽어옴
            readRoomCube.ReadState();

            List<List<GameObject>> cubeSides = new List<List<GameObject>>()
            {
                roomState.up,
                roomState.down,
                roomState.left,
                roomState.right,
                roomState.front,
                roomState.back
            };

            // 사이드의 큐브들을 중심의 자식화
            roomState.PickUp(cubeSides[i]);

            // start the side rotation logic
            RotateSide(cubeSides[i]);

        }
    }

    public void RotateSide(List<GameObject> side)
    {
        // 회전이 진행되는 면과 중심
        activeSide = side;
        activePivot = side[4].transform.parent.transform.parent.transform;
    }

    public void SpinSide(Vector3 rotation)
    {
        activePivot.Rotate(rotation, Space.Self);
    }

    public void RotateToRightAngle(Vector3 vec)
    {
        targetQuaternion.eulerAngles = vec;
    }

    public void AutoRotate()
    {
        float step = speed * Time.deltaTime;
        activePivot.localRotation = Quaternion.RotateTowards(activePivot.localRotation, targetQuaternion, step);

        // if within one dgree, set angle to target angle and end the rotation
        if (Quaternion.Angle(activePivot.localRotation, targetQuaternion) <= 1)
        {
            activePivot.localRotation = targetQuaternion;
            
            roomState.PutDown(activeSide, activePivot);
            
            readRoomCube.ReadState();
        }
    }

    // Automate에서 호출
    public void RotateSide(List<GameObject> side, float angle)
    {
        roomState.PickUp(side);

        // 회전이 진행되는 면과 중심
        activeSide = side;
        activePivot = side[4].transform.parent.transform.parent.transform;

        // local 기준 회전, 큰 큐브를 뒤집어도 하얀색이 y축인 것임
        Vector3 localForward = Vector3.zero - activePivot.localPosition;
        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * activePivot.localRotation;
        activeSide = side;
    }

}
