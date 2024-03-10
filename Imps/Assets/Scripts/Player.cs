using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float MovementSpeed = 7;
    public float RotationSpeed = 10;

    private Vector3 _moveVector;
    [SerializeField]private float _verticalVel, _gravity = 12;
    private CharacterController _characterController;
    private Vector2 _input, _leftInput;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    private void OnMove(InputValue value)
    {
        _input = value.Get<Vector2>();  
    }

    private void OnRotate(InputValue value) 
    { 
        _leftInput = value.Get<Vector2>();
    }
    // Update is called once per frame
    void Update()
    {
        Movement();

        transform.Rotate(0, _leftInput.x * RotationSpeed * Time.deltaTime, 0);
    }

    private void Movement()
    {
        _moveVector = transform.forward * MovementSpeed * _input.y;
        _moveVector.y = _verticalVel;

        if (_characterController.isGrounded)
        {
            _verticalVel = -0.5f;
        }
        else
        {
            _verticalVel -= _gravity * Time.deltaTime;
        }

       _characterController.Move(_moveVector * Time.deltaTime);
    }
}