using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float MovementSpeed = 7;
    public float RotationSpeed = 10;
    public float JumpHeight = 8;

    [SerializeField]private float _isJumpingValue;
    private Vector3 _moveVector;
    private float _verticalVel, _gravity = 12;
    private CharacterController _characterController;
    private Vector2 _input;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    private void OnMove(InputValue value)
    {
        _input = value.Get<Vector2>();  
    }

    private void OnJump(InputValue value)
    {
        _isJumpingValue = value.Get<float>();
    }
    // Update is called once per frame
    void Update()
    {
        Movement();

        transform.Rotate(0, _input.x * RotationSpeed * Time.deltaTime, 0);
    }

    private void Movement()
    {
        _moveVector = transform.forward * MovementSpeed * _input.y;
        _moveVector.y = _verticalVel;

        if (_characterController.isGrounded)
        {
            _verticalVel = -0.5f;
        }
        if (_characterController.isGrounded && _isJumpingValue == 1) //Jump
        {
            _verticalVel = JumpHeight;
        }
        else
        {
            _verticalVel -= _gravity * Time.deltaTime;
        }

       _characterController.Move(_moveVector * Time.deltaTime);
    }
}
