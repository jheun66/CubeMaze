using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation2 : MonoBehaviour
{
    private ReadCube2 readCube = null;
    private CubeState2 cubeState = null;

    // 회전을 진행할 side와 pivot
    private List<GameObject> activeSide;
    private Transform activePivot;

    [SerializeField]
    private float speed = 300.0f;
    public float Speed { get { return speed; } set { speed = value; } }

    // side의 회전량 드래그를 놓았을 때(쿼터니언)
    private Quaternion targetQuaternion;

    
    private void Awake()
    {
        readCube = FindObjectOfType<ReadCube2>();
        cubeState = FindObjectOfType<CubeState2>();
    }

    private void LateUpdate()
    {
        // 자동 회전 진행
        if (CubeState2.autoRotating)
        {
            AutoRotate();
        }

    }
    
    private void AutoRotate()
    {
        // target으로의 자연스러운 회전은 rotatetowards 사용
        float step = speed * Time.deltaTime;
        activePivot.localRotation = Quaternion.RotateTowards(activePivot.localRotation, targetQuaternion, step);

        // if within one dgree, set angle to target angle and end the rotation
        if (Quaternion.Angle(activePivot.localRotation, targetQuaternion) <= 1)
        {
            activePivot.localRotation = targetQuaternion;

            // pivotcube와의 부모 관계 해제
            cubeState.PutDown(activeSide, activePivot.parent);

            // 현재 상태 읽고 자동으로 큐브맵 변경
            readCube.ReadState();
            CubeState2.autoRotating = false;
        }
        
    }

    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        cubeState.PickUp(side);

        // 회전이 진행되는 면과 중심
        activeSide = side;
        activePivot = side[4].transform.parent.transform;

        // local 기준 회전, 큰 큐브를 뒤집어도 하얀색이 y축인 것임
        Vector3 localForward = Vector3.zero - activePivot.localPosition;
        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * activePivot.localRotation;
        activeSide = side;
        CubeState2.autoRotating = true;
    }

}
