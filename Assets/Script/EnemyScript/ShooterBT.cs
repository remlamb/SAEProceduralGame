using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShooterBT : MonoBehaviour
{

    [SerializeField] private SteeringBehavior _steeringBehavior;
    [SerializeField] private Sensor _sensor;
    private GameObject _player;
    private TargetVelocity _playerVelocity;
    private Animator _animator;

    private BT_Node _behaviour = new BT_Root("Root");
    private BT_Sequence _attaqueSeqence = new BT_Sequence("Attack Sequence");
    private BT_Sequence _reloadSeqence = new BT_Sequence("Reload");

    private BT_Selector _selector = new BT_Selector("Selector");

    [SerializeField] private float _attaqueDuration;
    private float _attaqueTimer;

    [SerializeField] private WaypointsManager _waypointsManager;
    private Transform _currentWaypoint;
    [SerializeField] private float _wayPointDistance;
    [SerializeField] private float _fleeSpeed;
    [SerializeField] private float _movingSpeed;

    [SerializeField] private GameObject _gun;
    private bool _hasReload;
    [SerializeField] private float _reloadDuration;
    private float _reloadTimer;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _spawnBullet;
    [SerializeField] private GameObject _LoadAttGO;
    [SerializeField] private GameObject _CrossSignGO;
    [SerializeField] private AudioSource _weaponAudioSource;
    [SerializeField] private AudioClip _shootingClip;
    [SerializeField] private AudioClip _onAimClip;

    // Start is called before the first frame update
    void Start()
    {
        _hasReload = true;
        _animator = GetComponent<Animator>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _waypointsManager = GameObject.FindGameObjectWithTag("WayPoint").GetComponent<WaypointsManager>();
        _playerVelocity = GameObject.FindGameObjectWithTag("Player").GetComponent<TargetVelocity>();

        _attaqueSeqence.AddNode(new BT_Leaf("Take a Position", MoveToAWaypoint));
        _attaqueSeqence.AddNode(new BT_Leaf(("Look at Player"), Attack));
        _reloadSeqence.AddNode(new BT_Leaf("Reloading", Reload));

        _selector.AddNode(_attaqueSeqence);
        _selector.AddNode(_reloadSeqence);

        _behaviour.AddNode(_selector);
        _currentWaypoint = _waypointsManager.GetRandomDestination();
    }

    // Update is called once per frame
    void Update()
    {
        _behaviour.Process();
    }

    private BT_Status Attack()
    {
        _steeringBehavior.StopMovement();
        transform.LookAt(_player.transform);
        _attaqueTimer += Time.deltaTime;
        _animator.Play("Fire");

        if (_attaqueTimer <= _attaqueDuration)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            _hasReload = false;
            _currentWaypoint = _waypointsManager.GetRandomDestination();
            return BT_Status.SUCCESS;
        }
    }

    private BT_Status Reload()
    {
        _steeringBehavior.StopMovement();
        _attaqueTimer = 0;
        _reloadTimer += Time.deltaTime;
        _animator.Play("Reload");

        if (_hasReload)
        {
            return BT_Status.FAILURE;
        }

        if (_reloadTimer <= _reloadDuration)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            _hasReload = true;
            return BT_Status.SUCCESS;
        }
    }

    private BT_Status MoveToAWaypoint()
    {
        _steeringBehavior.Seek(_currentWaypoint, _movingSpeed);
        _attaqueTimer = 0;
        _reloadTimer = 0;

        _animator.Play("Run");

        if (!_hasReload)
        {
            return BT_Status.FAILURE;
        }
        if (Vector3.Distance(transform.position, _currentWaypoint.position) >= _wayPointDistance)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            return BT_Status.SUCCESS;
        }
    }

    private void AnimEventHideGun()
    {
        _gun.SetActive(false);
    }

    private void AnimEventShowGun()
    {
        _gun.SetActive(true);
    }

    private void RiffleShoot()
    {
        _weaponAudioSource.clip = _shootingClip;
        _weaponAudioSource.Play();
        Instantiate(_bullet, _spawnBullet.position, _spawnBullet.rotation);
    }

    private void LoadAttackFeedback()
    {
        _LoadAttGO.SetActive(true);
        _CrossSignGO.SetActive(true);
        _weaponAudioSource.clip = _onAimClip;
        _weaponAudioSource.Play();
    }
    private void UnActiveLoadAttackFeedback()
    {
        _LoadAttGO.SetActive(false);
        _CrossSignGO.SetActive(false);
    }
}
