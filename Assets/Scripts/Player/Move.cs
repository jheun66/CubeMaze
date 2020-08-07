using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour, IAction
{
    private Animator _animator = null;

    public static bool canMove = true;

    // 점프에 사용
    private Rigidbody _rb = null;
    //private CapsuleCollider _cCol = null;
    //private LayerMask playerLayer = 10;

    [SerializeField, Range(5.0f, 10.0f)]
    private float _speed = 5.0f;
    [SerializeField, Range(10.0f, 30.0f)]
    private float _jumpForce = 20.0f;


    private float _speedH = 80.0f;

    private float _yaw = 0.0f;      // y 축회전
                                    // roll 은 z축 물론 엔진마다 xyz축이 다름 하지만 roll pitch yaw의 회전은 동일

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _rb = this.GetComponent<Rigidbody>();
        //_cCol = this.GetComponent<CapsuleCollider>();

    }

    private void Start()
    {
        _speed = 5.0f;
        _jumpForce = 20.0f;
    }
    
    private void FixedUpdate()
    {
        if(canMove)
        {
            Moving();
            Flying();
            Rotating();
        }
    }
    

    private void Rotating()
    {
        _yaw = Input.GetAxis("Mouse X") * _speedH * Time.deltaTime;
        
        // 캐릭터 y축 회전 후 
        this.transform.Rotate(Vector3.up * _yaw);
        
    }

    private Vector3 exPos;
    private void Moving()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //_rb 로 안하면 벽뚫기 문제 발생할수 있음.
            if (Input.GetKey(KeyCode.W))
            {
                _rb.MovePosition(this.transform.position + this.transform.forward * Time.deltaTime * _speed * 2);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _rb.MovePosition(this.transform.position - this.transform.forward * Time.deltaTime * _speed * 2);
            }

            if (Input.GetKey(KeyCode.A))
            {
                _rb.MovePosition(this.transform.position - this.transform.right * Time.deltaTime * _speed * 2);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _rb.MovePosition(this.transform.position + this.transform.right * Time.deltaTime * _speed * 2);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                _rb.MovePosition(this.transform.position + this.transform.forward * Time.deltaTime * _speed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _rb.MovePosition(this.transform.position - this.transform.forward * Time.deltaTime * _speed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                _rb.MovePosition(this.transform.position - this.transform.right * Time.deltaTime * _speed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _rb.MovePosition(this.transform.position + this.transform.right * Time.deltaTime * _speed);
            }

        }

        // y축 제외하기
        Vector2 vec1 = new Vector2(exPos.x, exPos.z);
        Vector2 vec2 = new Vector2(this.transform.position.x, this.transform.position.z);

        float dis = Vector2.Distance(vec1, vec2);
        float speed = dis / Time.fixedDeltaTime;

        _animator.SetFloat("Speed", speed);
        exPos = this.transform.position;

    }

    //private bool isGround = false;

    private void Flying()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(this.transform.up * _jumpForce, ForceMode.Force);
        }
    }

    public void Begin(object initValue)
    {
        canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void End()
    {
        canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    
}
