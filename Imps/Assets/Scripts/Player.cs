using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Player : MonoBehaviour
{
    public float StunDuration = 30;
    public float MovementSpeed = 7;
    public float RotationSpeed = 10;
    public float JumpHeight = 8;
    public float Cooldown = 3;
    public GameObject Bullet;
    public Transform BulletSpawnPoint;

    [Header("Pushing Back")]
    public LayerMask ImpMask;
    public int pushBackForce = 4;

    [Header("Stunning")]
    public GameObject StunUI;
    public bool CanStun; 
    private bool _isOnStunCooldown;
    public float StunCooldown = 20;

    [Header("Throwing Imps")]
    public bool IsHoldingButton, IsHoldingImp;
    public GameObject Imp = null;
    public float ThrowStrength = 20;
    [SerializeField]private bool _canShoot = true;
    public GameObject ThrowUI;

    private bool _shotFired;
    private float _isStunningValue;
    private float _isJumpingValue, _isShootingValue;
    private Vector3 _moveVector;
    private float _verticalVel, _gravity = 12;
    private CharacterController _characterController;
    private Vector2 _input, _rotateInput;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        ThrowUI.SetActive(false);
        _canShoot = true;
    }
    private void OnRotate(InputValue value)
    {
        _rotateInput = value.Get<Vector2>();
    }
    private void OnMove(InputValue value)
    {
        _input = value.Get<Vector2>();  
    }
    private void OnShoot(InputValue value) 
    { 
        _isShootingValue = value.Get<float>();
    }
    private void OnJump(InputValue value)
    {
        _isJumpingValue = value.Get<float>();
    }
    private void OnStun(InputValue value) 
    { 
        _isStunningValue = value.Get<float>();
    }
    private void OnHold() 
    {
        IsHoldingButton = !IsHoldingButton;
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotate();


        if (IsHoldingImp)
        {
            _canShoot = false;
            ThrowUI.SetActive(true);
            if (_isShootingValue == 1)
            {
                StartCoroutine(ThrowPlayer());
                ThrowUI.SetActive(false);   
                _shotFired = true;
            }
        }
        else if (_isShootingValue == 1 && Cooldown >= 3 && _canShoot)
        {
            _shotFired = true;
            Vector3 direction = BulletSpawnPoint.position - Camera.main.transform.forward;
            GameObject go = Instantiate(Bullet, direction, transform.rotation);
        }


        if(_shotFired) 
        { 
            Cooldown -= Time.deltaTime;
            if(Cooldown < 0) 
            {
                Cooldown = 3;
                _canShoot = true;
                _shotFired= false;
                return;
            }
        }

        StunningCooldown();

        PushImpsOppositeDirection();

    }
    private IEnumerator ThrowPlayer()
    {
        float t = 0;
        Vector3 startPos = Imp.transform.position;
        Vector3 targetPos = Imp.transform.position + (transform.forward * ThrowStrength);

        while (t < 1)
        {
            t += Time.deltaTime;

            Imp.GetComponent<Rigidbody>().transform.position = Vector3.Lerp(startPos, targetPos, t);

        }

        Imp.GetComponent<ImpMovement>().IsBeingHeld = false;
        IsHoldingImp = false;
        Imp.transform.parent = null;
        Imp = null;
        yield return null;
    }
    private void PushImpsOppositeDirection()
    {
        RaycastHit hit;
        if (Physics.SphereCast(new Vector3(transform.position.x, transform.position.y, transform.position.z), 4,
            Vector3.forward, out hit, 2, ImpMask.value, QueryTriggerInteraction.UseGlobal))
        {
            Vector3 dir = hit.point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            if(!hit.transform.gameObject.GetComponent<ImpMovement>()._isStunned)
            hit.transform.position -= dir * pushBackForce * Time.deltaTime;
        }
    }
    private void StunningCooldown()
    {
        if (_isStunningValue == 1 && StunCooldown >= 20)
        {
            CanStun = true;
            _isOnStunCooldown = true;
        }
        else if (_isStunningValue < 1)
        {
            CanStun = false;
        }

        if(_isOnStunCooldown) 
        {
            StunCooldown -= Time.deltaTime;
            if (StunCooldown < 0)
            {
                StunCooldown = 20;
                _isOnStunCooldown= false;
                return;
            }
        }

        StunUI.SetActive(!_isOnStunCooldown);
    }

    private void Rotate()
    {
        transform.Rotate(0, _rotateInput.x * RotationSpeed * Time.deltaTime, 0);
    }

    private void Movement()
    {
        _moveVector = (transform.forward * _input.y + transform.right * _input.x) * MovementSpeed;    
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
