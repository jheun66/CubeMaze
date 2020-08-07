using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameController gameController;

    private void OnTriggerEnter()
    {
        if (this.GetComponent<ChangeRoom>().clearQuestion)
        {
            gameController.CompleteGame();
        }
    }
}
