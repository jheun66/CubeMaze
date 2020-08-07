using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automate2 : MonoBehaviour
{
    public static List<string> moveList = new List<string>() {  };
    private readonly List<string> allMoves = new List<string>
    {
        "U", "D", "L", "R", "F", "B",
        "U'", "D'", "L'", "R'", "F'", "B'",
        "U2", "D2", "L2", "R2", "F2", "B2"
    };

    private CubeState2 cubeState = null;
    private ReadCube2 readCube = null;

    private void Awake()
    {
        cubeState = FindObjectOfType<CubeState2>();
        readCube = FindObjectOfType<ReadCube2>();
    }
    
    private void Update()
    {
        if (moveList.Count == 0)
        {
            Shuffle();
        }

        if (!CubeState2.autoRotating && CubeState2.started)
        {
            if (moveList.Count > 0)
            {
                DoMove(moveList[0]);
                moveList.Remove(moveList[0]);
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


        // 회전 진행중이다
        CubeState2.autoRotating = true;

        if (move == "U")
        {
            RotateSide(cubeState.up, -90);
        }
        if (move == "U'")
        {
            RotateSide(cubeState.up, 90);
        }
        if (move == "U2")
        {
            RotateSide(cubeState.up, -180);
        }
        if (move == "D")
        {
            RotateSide(cubeState.down, -90);
        }
        if (move == "D'")
        {
            RotateSide(cubeState.down, 90);
        }
        if (move == "D2")
        {
            RotateSide(cubeState.down, -180);
        }
        if (move == "L")
        {
            RotateSide(cubeState.left, -90);
        }
        if (move == "L'")
        {
            RotateSide(cubeState.left, 90);
        }
        if (move == "L2")
        {
            RotateSide(cubeState.left, -180);
        }
        if (move == "R")
        {
            RotateSide(cubeState.right, -90);
        }
        if (move == "R'")
        {
            RotateSide(cubeState.right, 90);
        }
        if (move == "R2")
        {
            RotateSide(cubeState.right, -180);
        }
        if (move == "F")
        {
            RotateSide(cubeState.front, -90);
        }
        if (move == "F'")
        {
            RotateSide(cubeState.front, 90);
        }
        if (move == "F2")
        {
            RotateSide(cubeState.front, -180);
        }
        if (move == "B")
        {
            RotateSide(cubeState.back, -90);
        }
        if (move == "B'")
        {
            RotateSide(cubeState.back, 90);
        }
        if (move == "B2")
        {
            RotateSide(cubeState.back, -180);
        }

    }

    private void RotateSide(List<GameObject> side, int angle)
    {
        // automatically rotate the side by the angle
        PivotRotation2 pr = side[4].transform.parent.transform.parent.GetComponent<PivotRotation2>();
        pr.StartAutoRotate(side, angle);
    }
}
