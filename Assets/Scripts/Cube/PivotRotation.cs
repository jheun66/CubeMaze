using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    private ReadCube readCube = null;
    private CubeState cubeState = null;
    private SynchronizeCube synchronizeCube = null;
    private Automate _automate = null;

   

    // 회전을 진행할 side와 pivot
    private List<GameObject> activeSide;
    private Transform activePivot;
   
    private bool dragging = false;

    // 마우스 위치 저장용
    private Vector3 mouseRef;

    [SerializeField]
    private float sensitivity = 0.4f;
    [SerializeField]
    private float speed = 300.0f;
    public float Speed { get { return speed; } set { speed = value; } }

    // side의 회전량 드래그하는 동안(오일러)
    private Vector3 rotation;
    // side의 회전량 드래그를 놓았을 때(쿼터니언)
    private Quaternion targetQuaternion;

    // Face Layer
    private int layerMask = 1 << 10;         

    private void Awake()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
        synchronizeCube = FindObjectOfType<SynchronizeCube>();

        _automate = FindObjectOfType<Automate>();
    }
    
    private void Update()
    {
        if (GameController.clickCube && _automate.Stop)
        {  // 현재 누른 위치의 Cube Face 확인
            SelectFace();
        }
    }
    
    private void LateUpdate()
    {
        if (dragging && !CubeState.autoRotating)
        {
            // 드래그 하는 동안 회전
            SpinSide();

            // 마우스 왼쪽 버튼을 누르면 현재 회전량을 바탕으로 TargetQuaternion을 구하고 자동회전 준비
            if (Input.GetMouseButtonUp(0))
            {
                RotateToRightAngle();
            }
        }
        // 자동 회전 진행
        if (CubeState.autoRotating)
        {
            AutoRotate();
        }

    }

    private void SelectFace()
    {
        if (Input.GetMouseButtonDown(0) && !CubeState.autoRotating)
        {
            // 현재 상태 읽어옴
            readCube.ReadState();

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                GameObject face = hit.collider.gameObject;
               
                List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    cubeState.up,
                    cubeState.down,
                    cubeState.left,
                    cubeState.right,
                    cubeState.front,
                    cubeState.back
                };

                // If the face hit exists whitin a side
                for (int i=0;i<cubeSides.Count;i++)
                {
                    if (cubeSides[i].Contains(face))
                    {
                        // 사이드의 큐브들을 중심의 자식화
                        cubeState.PickUp(cubeSides[i]);

                        // start the side rotation logic
                        RotateSide(cubeSides[i]);

                        synchronizeCube.SelectFace(i);
                    }
                }
            }
        }
    }

    public void RotateSide(List<GameObject> side)
    {
        // 회전이 진행되는 면과 중심
        activeSide = side;
        activePivot = side[4].transform.parent.transform;

        // 눌렀을 때 현재 마우스 위치 저장
        mouseRef = Input.mousePosition;
        dragging = true;
    }

    private void SpinSide()
    {
        // ActiveSide가 회전할 오일러 회전값 초기화
        rotation = Vector3.zero;

        // 오프셋 = 현재 위치 - 이전 mouseRef
        Vector3 mouseOffset = Input.mousePosition - mouseRef;
        
        // 마우스 오프셋이 양수면 시계방향으로 회전, 음수면 반시계로 회전
        // activeSide가 무엇인지에 따라
        // Y축 기준 오일러 회전값 조정
        if (activeSide == cubeState.up)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (activeSide == cubeState.down)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        // Z축 기준 오일러 회전값 조정
        if (activeSide == cubeState.left)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (activeSide == cubeState.right)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        // X축 기준 오일러 회전값 조정
        if (activeSide == cubeState.front)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (activeSide == cubeState.back)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }

        // rotate
        activePivot.Rotate(rotation, Space.Self);

        // mouseRef 수정
        mouseRef = Input.mousePosition;

        synchronizeCube.SpinSide(rotation);
    }
    
    public void RotateToRightAngle()
    {
        Vector3 vec = activePivot.localEulerAngles;
        // 반올림
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        targetQuaternion.eulerAngles = vec;
        CubeState.autoRotating = true;
        FindObjectOfType<AudioManager>().Play("RotateSound");

        dragging = false;


        synchronizeCube.RotateToRightAngle(vec);
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
            CubeState.autoRotating = false;
        }

        synchronizeCube.AutoRotate();
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
        CubeState.autoRotating = true;
        FindObjectOfType<AudioManager>().Play("RotateSound");
    }

}
