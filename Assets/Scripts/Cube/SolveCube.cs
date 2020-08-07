using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;

public class SolveCube : MonoBehaviour
{
    private ReadCube readCube = null;
    private CubeState cubeState = null;
    private bool doOnce = true;

    private void Start()
    {
        doOnce = true;
    }

    private void Awake()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }
    
    private void Update()
    {
        if (CubeState.started && doOnce)
        {
            doOnce = false;
            SolverMakeTable();
        }
    }

    public void SolverMakeTable()
    {
        readCube.ReadState();

        // get the state of the cube as a string
        string moveString = cubeState.GetStateString();
        print(moveString);

        // solve the cube
        string info = "";
        // First time build the tables
        string solution = SearchRunTime.solution(moveString, out info, buildTables: true);

        // Every other time
        //string solution = Search.solution(moveString, out info);

        // convert the solved moves from a string to a list
        List<string> solutionList = StringToList(solution);
        print(solution);

        // Automate the list
        Automate.moveList = solutionList;
        Automate.Solve = true;

        print(info);
    }

    public void Solver()
    {
        readCube.ReadState();

        // get the state of the cube as a string
        string moveString = cubeState.GetStateString();
        //print(moveString);

        // solve the cube
        string info = "";
        // First time build the tables
        //string solution = SearchRunTime.solution(moveString, out info, buildTables: true);

        // Every other time
        string solution = Search.solution(moveString, out info);

        // convert the solved moves from a string to a list
        List<string> solutionList = StringToList(solution);

        // Automate the list
        Automate.moveList = solutionList;
        Automate.Solve = true;

        //print(info);
    }

    List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }
}
