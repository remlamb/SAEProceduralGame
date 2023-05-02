using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WolfBT : MonoBehaviour
{
    [SerializeField] private float _distanceAroundPlayer;
    [SerializeField] private SteeringBehavior _steeringBehavior;
    [SerializeField] private float _patrolAgentSpeed;
    [SerializeField] private float _patrolPointSpeed;
    [SerializeField] private float _attackDestinationSpeed;

    [SerializeField] private Sensor _sensor;
    private GameObject _player;
    private TargetVelocity _playerVelocity;

    private float _patrollingTimer;
    [SerializeField] private float _patrolDuration;

    private float _animTimer;
    private Animator _animator;
    [SerializeField] private float _attDuration = 1f;

    [SerializeField] private GameObject _attRangeGO;
    [SerializeField] private Collider _attColider;
    [SerializeField] private ParticleSystem _attParticule;

    [SerializeField] private AudioClip _wolfAttClip;
    [SerializeField] private AudioSource _audioSource;


    //BT
    private BT_Node _behaviour = new BT_Root("Root");
    private BT_Sequence _attaqueSeqence = new BT_Sequence("Attack Sequence");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerVelocity = GameObject.FindGameObjectWithTag("Player").GetComponent<TargetVelocity>();

        _attaqueSeqence.AddNode(new BT_Leaf(("Patrolling"), PatrollingAroundPlayer));
        _attaqueSeqence.AddNode(new BT_Leaf(("Patrolling"), SetDirectionPlayer));
        _attaqueSeqence.AddNode(new BT_Leaf(("Chasing Target"), AttackPlayer));
        _behaviour.AddNode(_attaqueSeqence);
    }

    // Update is called once per frame
    void Update()
    {
        _behaviour.Process();
    }

    private BT_Status PatrollingAroundPlayer()
    {
        _animTimer = 0;
        UnActiveAttRange();
        _steeringBehavior.StayInDistance(_player, _distanceAroundPlayer, _patrolPointSpeed, _patrolAgentSpeed);
        transform.LookAt(_player.transform);
        _animator.Play("Moving");
        _patrollingTimer += Time.deltaTime;
        if (_patrollingTimer <= _patrolDuration)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            return BT_Status.SUCCESS;
        }
    }

    private BT_Status SetDirectionPlayer()
    {
        _patrollingTimer = 0;
        _steeringBehavior.Seek(_player.transform, _attackDestinationSpeed);
        transform.LookAt(_player.transform);
        if (!_sensor.PlayerInRange())
        {
            return BT_Status.RUNNING;
        }
        else
        {

            return BT_Status.SUCCESS;
        }
    }

    private BT_Status AttackPlayer()
    {
        _steeringBehavior.StopMovement();
        _animator.Play("Attack");
        _animTimer += Time.deltaTime;

        if (_animTimer < _attDuration)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            return BT_Status.SUCCESS;
        }
    }

    //anim Event

    private void ActiveAttRange()
    {
        _attRangeGO.SetActive(true);
    }

    private void UnActiveAttRange()
    {
        _attRangeGO.SetActive(false);
        _attColider.enabled = false;
    }

    private void AttackFrame()
    {
        _attParticule.Play();
        _attColider.enabled = true;
    }

    private void PlayAttSound()
    {
        _audioSource.clip = _wolfAttClip;
        _audioSource.Play();
    }
}
