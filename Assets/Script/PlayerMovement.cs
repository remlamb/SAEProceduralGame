using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    private InputWrapper _inputs;
    private Rigidbody _rb;
    private Vector2 _velocity;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotateSmoothing = 1000f;
    private Animator _animator;

    [Header("Dashing System")]
    private bool _isDashing;
    [SerializeField] private float _dashAmout = 5f;
    [SerializeField] private LayerMask _dashLayerMask;
    private float _dashingTimer;
    [SerializeField] private float _timeBetweenDash;

    private Vector2 _lastDirection;

    [SerializeField] private GameObject _playerClone;
    [SerializeField] private ParticleSystem _dashParticule;

    [SerializeField] private AudioSource _movementAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _inputs = GetComponent<InputWrapper>();
        _animator = GetComponent<Animator>();

        _dashingTimer = _timeBetweenDash;
        _lastDirection = new Vector2(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        _velocity = _inputs._move * _speed;
        IsMovingAnimation();
        Rotate();
        DashSetup();
    }

    void FixedUpdate()
    {
        _rb.velocity = new Vector3(_velocity.x, 0, _velocity.y);

        //Dashing movement
        DashMovement();
    }

    private void IsMovingAnimation()
    {
        if (_inputs._move.x > 0.1 || _inputs._move.x < -0.1 || _inputs._move.y > 0.1 || _inputs._move.y < -0.1)
        {
            _animator.SetBool("isMoving", true);
            _lastDirection = new Vector2(_inputs._move.x, _inputs._move.y).normalized;
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    private void Rotate()
    {
        Vector3 playerDirection = Vector3.right * _inputs._move.x + Vector3.forward * _inputs._move.y;
        if(playerDirection.sqrMagnitude > 0.0f)
        {
            Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, _rotateSmoothing * Time.deltaTime);
        }
    }

    private void DashSetup()
    {

        if(_dashingTimer < _timeBetweenDash)
        {
            _dashingTimer += Time.deltaTime;
        }

        if (_inputs.useDash && _dashingTimer >= _timeBetweenDash)
        {
            _isDashing = true;
        }
    }

    private void DashMovement()
    {
        if (_isDashing)
        {
            Instantiate(_playerClone, transform.position, transform.rotation);
            _movementAudioSource.Play();
            Vector3 dashPosition = new Vector3();
            //If the player is moving dashing forward
            if (_inputs._move.x > 0.1 || _inputs._move.x < -0.1 || _inputs._move.y > 0.1 || _inputs._move.y < -0.1)
            {
                //Raycast for collision, stop dash at collision point
                dashPosition = new Vector3(transform.position.x + _inputs._move.x * _dashAmout, transform.position.y, transform.position.z + _inputs._move.y * _dashAmout);
                RaycastHit hit;
                bool RayHit = Physics.Raycast(transform.position, new Vector3(_inputs._move.x, 0, _inputs._move.y), out hit, _dashAmout, _dashLayerMask);
                if (RayHit)
                {
                    dashPosition = hit.point;
                }
            }
            //if the player isnt moving dashing backward
            else
            {
                dashPosition = new Vector3(transform.position.x + -1 *(_lastDirection.x * _dashAmout), transform.position.y, transform.position.z + -1*(_lastDirection.y * _dashAmout));
                RaycastHit backDashHit;
                bool RayHitBack = Physics.Raycast(transform.position, dashPosition.normalized, out backDashHit, _dashAmout, _dashLayerMask);
                if (RayHitBack)
                {
                    dashPosition = backDashHit.point;
                }
            }
            _rb.MovePosition(dashPosition);
            Instantiate(_dashParticule, dashPosition, transform.rotation);
            _dashingTimer = 0;
            _isDashing = false;
        }
    }
}
