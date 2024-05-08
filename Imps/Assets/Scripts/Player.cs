using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float StunDuration = 30;
    public float MovementSpeed = 7;
    public float RotationSpeed = 10;
    public float JumpHeight = 8;
    public bool IsMoving;

    [Header("Wall Shooting")]
    public float Cooldown = 3;
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public GameObject WallIndicator;
    public AudioSource ThrowWallAudio;

    [Header("Player Input")]
    public PlayerInput PlayerInput;
    private Gamepad _gamepad;

    [Header("Pushing Back")]
    public LayerMask ImpMask;
    public int pushBackForce = 4;

    [Header("Stunning")]
    public GameObject StunUI;
    public bool CanStun; 
    private bool _isOnStunCooldown;
    public float StunCooldown = 20;
    public CinemachineImpulseSource ImpulseSource;
    public AudioSource StunSound;
    public AudioSource StunRechargeSound;

    [Header("Throwing Imps")]
    public bool IsHoldingButton, IsHoldingImp;
    public GameObject Imp = null;
    public float ThrowStrength = 20;
    private bool _canShoot = true;
    public GameObject ThrowUI;

    [Header("Dash")]
    public float DashDistance = 10;
    public float DashCooldown = 2;
    public AudioSource DashAudio;
    private float _isDashingValue;
    private bool _canDash = true;
    public GameObject DashLines;
    public Material CrosshairSprite;

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


        _gamepad = Gamepad.current;
        if (_gamepad != null)
        {
            Debug.Log(_gamepad.name);
            Debug.Log(_gamepad.shortDisplayName);
            Debug.Log(_gamepad.displayName);
        }


        InputUser.PerformPairingWithDevice(_gamepad, PlayerInput.user);

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
    private void OnDash(InputValue value)
    {
        _isDashingValue = value.Get<float>();
    }

    private void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotate();
        Dash();

        if (IsHoldingImp)
        {
            _canShoot = false;
            ThrowUI.SetActive(true);
            Imp.GetComponent<ImpMovement>().Renderer.sharedMaterial = Imp.GetComponent<ImpMovement>()._ghostMat;
            Imp.GetComponent<ImpMovement>().ImpScreamAudio.Play();
            Imp.transform.position = transform.position + (transform.forward * ThrowStrength);
            if (_isShootingValue == 1)
            {
                StartCoroutine(ThrowImp());
                ThrowUI.SetActive(false);
                WallIndicator.SetActive(false);
                _shotFired = true;
            }
        }
        else if (_isShootingValue == 1 && Cooldown >= 3 && _canShoot)
        {
            ThrowWallAudio.Play();
            _shotFired = true;
            Vector3 direction = BulletSpawnPoint.position - Camera.main.transform.forward;
            GameObject go = Instantiate(BulletPrefab, direction, transform.rotation);
            WallIndicator.SetActive(false);
        }



        if (_shotFired)
        {
            Cooldown -= Time.deltaTime;
            if (Cooldown < 0)
            {
                Cooldown = 3;
                _canShoot = true;
                WallIndicator.SetActive(true);
                _shotFired = false;
                return;
            }
        }

        StunningCooldown();

        PushImpsOppositeDirection();

    }

    private void Dash()
    {
        if(_isDashingValue == 1 && _canDash) 
        {
            _characterController.Move(transform.forward * DashDistance);
            DashAudio.Play();
            CrosshairSprite.color = Color.red;
            DashLines.SetActive(true);
            _canDash = false;
        }

        if (!_canDash)
        {
            DashCooldown -= Time.deltaTime;
            if (DashCooldown < 0)
            {
                CrosshairSprite.color = Color.black;
               DashCooldown = 2;
                _canDash = true;
                return;
            }
        }
    }

    private IEnumerator ThrowImp()
    {
        Imp.GetComponent<ImpMovement>().IsBeingHeld = false;
        IsHoldingImp = false;
        Imp.transform.parent = null;
        Imp.GetComponent<ImpMovement>().Renderer.sharedMaterial = Imp.GetComponent<ImpMovement>()._deafultMat;
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
            dir = -dir.normalized;

            // And finally we add force in the direction of dir and multiply it by force. 
            if (!hit.transform.gameObject.GetComponent<ImpMovement>()._isStunned)
            {
                hit.transform.position -= dir * pushBackForce * Time.deltaTime;
                if (hit.rigidbody.GetComponent<ImpMovement>().RollDice() == 1)
                {
                    hit.rigidbody.GetComponent<ImpMovement>().ImpScreamAudio.Play();
                }
            }
        }
    }

    private void StunningCooldown()
    {
        if (_isStunningValue == 1 && StunCooldown >= 20)
        {
            CanStun = true;
            _isOnStunCooldown = true;
           ImpulseSource.GenerateImpulse();
            StunSound.Play();
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
                StunRechargeSound.Play();
                StartCoroutine(FlashUI(StunUI));
                _isOnStunCooldown = false;

                return;
            }
        }

        StunUI.SetActive(!_isOnStunCooldown);
    }

    public IEnumerator FlashUI(GameObject UIElement)
    {
       for(int i = 0;i<=2;i++) 
        {
            UIElement.transform.localScale = Vector3.one * 2;
            yield return new WaitForSeconds(0.15f);
            UIElement.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.3f);
        }

        yield return null;
    }
    private void Rotate()
    {
        transform.Rotate(0, _rotateInput.x * RotationSpeed * Time.deltaTime, 0);
    }

    private void Movement()
    {
        IsMoving = _input != Vector2.zero;
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
