using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automate : MonoBehaviour
{
    // 2분 간격으로 회전
    [SerializeField, Range(2, 500)]
    private int rotationCycle = 120;
    public int RotationCycle { get { return rotationCycle; } set { rotationCycle = value; } }

    private float timer = 0;
    public float Timer { get { return timer; } set { timer = value; } }

    [SerializeField]
    private bool stop = false;
    public bool Stop { get { return stop; } set { stop = value; } }

    private static bool solve = false;
    public static bool Solve { get { return solve; } set { solve = value; } }

    public static List<string> moveList = new List<string>() {  };
    private readonly List<string> allMoves = new List<string>
    {
        "U", "D", "L", "R", "F", "B",
        "U'", "D'", "L'", "R'", "F'", "B'",
        "U2", "D2", "L2", "R2", "F2", "B2"
    };

    private CubeState cubeState = null;
    private ReadCube readCube = null;
    private RotateCube rotateCube = null;

    private ReadRoomCube readRoomCube = null;
    private RoomState roomState = null;
    private SynchronizeCube synchronizeCube = null;

    private void Start()
    {
        timer = 0;
    }

    private void Awake()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
        rotateCube = FindObjectOfType<RotateCube>();

        readRoomCube = FindObjectOfType<ReadRoomCube>();
        roomState = FindObjectOfType<RoomState>();
        synchronizeCube = FindObjectOfType<SynchronizeCube>();
    }
    
    private void Update()
    {
        // 자동 해결
        if(solve)
        {  
            if (moveList.Count > 0 && !CubeState.autoRotating && CubeState.started)
            {
                DoMove(moveList[0]);
                moveList.Remove(moveList[0]);
            }
            if (moveList.Count == 0)
            {
                // 맨처음 정면 바라보도록
                rotateCube.target.transform.eulerAngles = Vector3.zero;

                solve = false;
            }
        }
        else
        {
            if (moveList.Count == 0)
            {
                Shuffle();
            }

            if (timer > rotationCycle && !CubeState.autoRotating && CubeState.started)
            {
                timer = 0;

                if (moveList.Count > 0)
                {
                    DoMove(moveList[0]);
                    moveList.Remove(moveList[0]);
                }
            }
            else
            {
                if (!stop && !CubeState.autoRotating && CubeState.started)
                    timer += Time.deltaTime;
            }
        }
        
    }

    public void Shuffle()
    {
        List<string> moves = new List<string>();
        int shuffleLength = Random.Range(10, 30);
        for (int i = 0; i < shuffleLength; i++)
        {
            int randomMove = Random.Range(0, allMoves.Count);
            moves.Add(allMoves[randomMove]);
        }
        moveList = moves;
    }
    
    private void DoMove(string move)
    {
        readCube.ReadState();

        readRoomCube.ReadState();

        // 회전 진행중이다
        CubeState.autoRotating = true;

        if (move == "U")
        {
            RotateSide(cubeState.up, -90);
            synchronizeCube.RotateSide(roomState.up, -90);
        }
        if (move == "U'")
        {
            RotateSide(cubeState.up, 90);
            synchronizeCube.RotateSide(roomState.up, 90);
        }
        if (move == "U2")
        {
            RotateSide(cubeState.up, -180);
            synchronizeCube.RotateSide(roomState.up, -180);
        }
        if (move == "D")
        {
            RotateSide(cubeState.down, -90);
            synchronizeCube.RotateSide(roomState.down, -90);
        }
        if (move == "D'")
        {
            RotateSide(cubeState.down, 90);
            synchronizeCube.RotateSide(roomState.down, 90);
        }
        if (move == "D2")
        {
            RotateSide(cubeState.down, -180);
            synchronizeCube.RotateSide(roomState.down, -180);
        }
        if (move == "L")
        {
            RotateSide(cubeState.left, -90);
            synchronizeCube.RotateSide(roomState.left, -90);
        }
        if (move == "L'")
        {
            RotateSide(cubeState.left, 90);
            synchronizeCube.RotateSide(roomState.left, 90);
        }
        if (move == "L2")
        {
            RotateSide(cubeState.left, -180);
            synchronizeCube.RotateSide(roomState.left, -180);
        }
        if (move == "R")
        {
            RotateSide(cubeState.right, -90);
            synchronizeCube.RotateSide(roomState.right, -90);
        }
        if (move == "R'")
        {
            RotateSide(cubeState.right, 90);
            synchronizeCube.RotateSide(roomState.right, 90);
        }
        if (move == "R2")
        {
            RotateSide(cubeState.right, -180);
            synchronizeCube.RotateSide(roomState.right, -180);
        }
        if (move == "F")
        {
            RotateSide(cubeState.front, -90);
            synchronizeCube.RotateSide(roomState.front, -90);
        }
        if (move == "F'")
        {
            RotateSide(cubeState.front, 90);
            synchronizeCube.RotateSide(roomState.front, 90);
        }
        if (move == "F2")
        {
            RotateSide(cubeState.front, -180);
            synchronizeCube.RotateSide(roomState.front, -180);
        }
        if (move == "B")
        {
            RotateSide(cubeState.back, -90);
            synchronizeCube.RotateSide(roomState.back, -90);
        }
        if (move == "B'")
        {
            RotateSide(cubeState.back, 90);
            synchronizeCube.RotateSide(roomState.back, 90);
        }
        if (move == "B2")
        {
            RotateSide(cubeState.back, -180);
            synchronizeCube.RotateSide(roomState.back, -180);
        }

    }

    private void RotateSide(List<GameObject> side, int angle)
    {
        // automatically rotate the side by the angle
        PivotRotation pr = side[4].transform.parent.transform.parent.GetComponent<PivotRotation>();
        pr.StartAutoRotate(side, angle);
    }
}
