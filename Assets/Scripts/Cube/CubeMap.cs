using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMap : MonoBehaviour
{
    private CubeState cubeState = null;

    private Transform up;
    private Transform down;
    private Transform left;
    private Transform right;
    private Transform front;
    private Transform back;

    private void Awake()
    {
        cubeState = FindObjectOfType<CubeState>();
        up = this.transform.Find("Up");
        down = this.transform.Find("Down");
        left = this.transform.Find("Left");
        right = this.transform.Find("Right");
        front = this.transform.Find("Front");
        back = this.transform.Find("Back");
    }
    
    public void Set()
    {
        UpdateMap(cubeState.front, front);
        UpdateMap(cubeState.back, back);
        UpdateMap(cubeState.left, left);
        UpdateMap(cubeState.right, right);
        UpdateMap(cubeState.up, up);
        UpdateMap(cubeState.down, down);
    }

    private void UpdateMap(List<GameObject> face, Transform side)
    {
        int i = 0;
        foreach (Transform map in side)
        {
            if (face[i].name[0] == 'F')
            {
                map.GetComponent<Image>().color = Color.red;
            }
            if (face[i].name[0] == 'B')
            {
                map.GetComponent<Image>().color = new Color(1, 0.3098039f, 0, 1);
            }
            if (face[i].name[0] == 'U')
            {
                map.GetComponent<Image>().color = Color.white;
            }
            if (face[i].name[0] == 'D')
            {
                map.GetComponent<Image>().color = Color.yellow;
            }
            if (face[i].name[0] == 'L')
            {
                map.GetComponent<Image>().color = Color.green;
            }
            if (face[i].name[0] == 'R')
            {
                map.GetComponent<Image>().color = Color.blue;
            }
            i++;
        }
    }
}
