using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    // prefab 넣어주기
    public GameObject _roomQuestion = null;

    private GameObject _room = null;
    private DialogueTrigger _dialogueTrigger = null;
    

    [SerializeField]
    private string _roomAnswer = null;
    public string RoomAnswer { get { return _roomAnswer; } }

    [SerializeField]
    private bool _clear = false;
    public bool Clear { get { return _clear; } set { _clear = value; } }

    private void Start()
    {
        _clear = false;
    }

    private void Awake()
    {
        _room = this.gameObject;
        _dialogueTrigger = _room.GetComponent<DialogueTrigger>();
    }
    

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            other.GetComponent<PlayerController>().ChangeCurrentRoom(this.transform);
        }
    }

    public IEnumerator TriggerInTime()
    {
        // 독백할깨 남아있으면 시간지나고 출력
        while(_dialogueTrigger.index < _dialogueTrigger.dialogue.Length && !Clear)
        {
            // 10초 간격으로 대화창 트리거발동
            yield return new WaitForSeconds(_dialogueTrigger.delayTime[_dialogueTrigger.index]);
            _dialogueTrigger.TriggerDialogue();
        }
    }

}
