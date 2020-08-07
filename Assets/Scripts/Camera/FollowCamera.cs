using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform head = null;
    
    // 플레이어가 바라보는 방향으로 회전하고 위해
    private Transform _player = null;

    [SerializeField]
    private Vector3 _offset;
    private bool changeView = false;

    // zoom 관련
    private float _minFOV = 40.0f;
    private float _maxFOV = 60.0f;
    private float _zoomDistance = 60.0f;        // 최초의 FOV(Field Of View)
    private float _sensitiveDistance = 50.0f;
    private float _damping = 5.0f;
    private Camera _camera = null;
    
    
    // 흔들림 효과
    [SerializeField]
    private float shakeAmount;


    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _camera = Camera.main;
    }

    private void Start()
    {
        _offset = new Vector3(0, 1.5f, -3.0f);
        this.transform.position = head.position;
        
        // 마우스 커서 lock
        Cursor.lockState = CursorLockMode.Locked;

        shakeAmount = 0.06f;

        _zoomDistance = _camera.fieldOfView;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            if(changeView)
                changeView = false;
            else
                changeView = true;
        }
    }

    private void LateUpdate()
    {
        if(Move.canMove && !GameController.GameIsPaused && !GameController.GameIsCompleted)
        {
            // 카메라 위치 고정
            FixCameraPos();

            // 카메라 회전
            Rotating();
            
        }
        else
        {
            if(GameController.clickCube)
            {
                this.transform.position = new Vector3(-7, 7, -7);
                this.transform.eulerAngles = new Vector3(40, 45, 0);
            }
        }

        Zooming();
    }

    private void Zooming()
    {
        // zoom을 카메라가 직접움직여서 진행되는게 아니라 FOV를 이용해 진행 FOV가 줄면 가까워 보임, 대신 너무 땡기면 이상하게 보임
        // 휠돌리면 감도만큼 FOV 감소, 최대 최소는 Clamp를 이용해 잘라주고, Lerp를 이용해 부드럽게 이동
        _zoomDistance -= Input.GetAxis("Mouse ScrollWheel") * _sensitiveDistance;
        _zoomDistance = Mathf.Clamp(_zoomDistance, _minFOV, _maxFOV);
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _zoomDistance, Time.deltaTime * _damping);
    }

    private void FixCameraPos()
    {
        if(changeView)
        {
            Quaternion quaternion = _player.transform.rotation;
            Vector3 offset = quaternion * _offset;

            this.transform.position = head.position + offset;
        }
        else
        {
            this.transform.position = head.position;
        }
        if (CubeState.autoRotating)
        {
            this.transform.position += UnityEngine.Random.insideUnitSphere * shakeAmount;
        }
    }

    private float _pitch = 0.0f;    // x 축회전
    private float _speedV = 100.0f;


    private void Rotating()
    {

        _pitch -= Input.GetAxis("Mouse Y") * _speedV * Time.deltaTime;

        _pitch = Mathf.Clamp(_pitch, -80, 30);
        
        this.transform.eulerAngles = new Vector3(_pitch, _player.transform.eulerAngles.y, 0);
    }



}
