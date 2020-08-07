using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeUIScript : MonoBehaviour
{
    private CanvasGroup canvasGroup = null;
    // Start is called before the first frame update
    private void Awake()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.clickCube)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;

        }
    }

    public void clickCloseButton()
    {
        GameController.clickCube = false;
        Move.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;

    }
}
