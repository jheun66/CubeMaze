using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    public bool clearQuestion = false;

    private Transform _player = null;

    [SerializeField]
    private Transform _currentRoom = null;

    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        clearQuestion = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 방 체크하기
        CheckRoom();
        
    }

    private void CheckRoom()
    {
        this._currentRoom = _player.GetComponent<PlayerController>()._currentRoom;
        clearQuestion = this._currentRoom.GetComponent<RoomController>().Clear;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 문제를 풀고 큐브가 움직이지 않을때만
        if(clearQuestion && !CubeState.autoRotating)
        {
            MoveToNextRoom();
        }
    }

    private void MoveToNextRoom()
    {
        Debug.Log("다음 방으로 이동");
        FindObjectOfType<AudioManager>().Play("ChangeRoom");
        // world 공간에서 현재 방의 위치와 트리거의 위치를 가지고 방향을 
        Vector3 vec =  this.transform.position - this._currentRoom.transform.position;
        vec.Normalize();        

        _player.position = this._currentRoom.transform.position + vec * 55;
    }
}
