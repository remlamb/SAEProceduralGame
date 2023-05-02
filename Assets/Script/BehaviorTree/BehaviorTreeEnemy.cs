using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class BehaviorTreeEnemy : MonoBehaviour
{
    private BT_Node _behaviour = new BT_Root("IDLE");
    private BT_Selector _attackSelector = new BT_Selector("AttackSelector");
    private BT_Sequence _attaqueSeqence = new BT_Sequence("BasicAttack");
    private BT_Sequence _specialSeqence = new BT_Sequence("WolfAttack");

    private NavMeshAgent _agent;
    [SerializeField] private TargetVelocity _targetVelocity;
    [SerializeField] private float _distance;
    private Animator _animator;

    private float _patrollTimer = 9f;
    private float _currentPatrollTimer = 0f;
    [SerializeField] private WaypointsManager _waypointsManager;
    private Transform _patrolDestination;

    //Try To switch Sequence with a random int
    private int RandomBehavior;
    [SerializeField] private Transform _wolfTransform;
    [SerializeField] private Transform intialPos;


    [SerializeField] private SteeringBehavior _pursuit;


    [SerializeField] private bool _playerAttackComportement;
    [SerializeField] private bool _wolfAttackComportement;


    // Start is called before the first frame update
    void Start()
    {
        RandomBehavior = Random.Range(0, 2);
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _patrolDestination = _waypointsManager.GetCurrentDestination();


        _attaqueSeqence.AddNode(new BT_Leaf(("Patrolling"), Patrolling));
        _attaqueSeqence.AddNode(new BT_Leaf(("Chasing Target"), PursuitTarget));
        _attaqueSeqence.AddNode(new BT_Leaf(("Attack Target"), AttackTarget));

        _specialSeqence.AddNode(new BT_Leaf("ChasingWolf", ChasingWolf));
        _specialSeqence.AddNode(new BT_Leaf(("AttackWolf"), AttackTarget));
        _specialSeqence.AddNode(new BT_Leaf(("Return to Initial Pos"), ReturnToInitialPos));

        _attackSelector.AddNode(_attaqueSeqence);
        _attackSelector.AddNode(_specialSeqence);
        _behaviour.AddNode(_attackSelector);

        intialPos = transform;

    }

    // Update is called once per frame
    void Update()
    {
        _behaviour.Process();
    }

    public BT_Status ChasingWolf()
    {
        _pursuit.Seek(_wolfTransform, 4f);
        if (!_wolfAttackComportement)
        {
            return BT_Status.FAILURE;
        }
        if (Vector3.Distance(transform.position, _wolfTransform.position) > _distance)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            return BT_Status.SUCCESS;
        }
    }

    public BT_Status ReturnToInitialPos()
    {
        /*
        _agent.isStopped = false;
        _agent.speed = 8f;
        _agent.SetDestination(intialPos.transform.position);
        if (Vector3.Distance(transform.position, intialPos.position) < _distance)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            RandomBehavior = Random.Range(0, 2);
            return BT_Status.SUCCESS;
        }*/


        _agent.isStopped = false;
        _agent.speed = 7.5f;
        _currentPatrollTimer += Time.deltaTime;
        /*
        if (Vector3.Distance(transform.position, _patrolDestination) <= _distance)
        {
            _patrolDestination = _waypointsManager.GetNextPatrolDestination();
        }
        _agent.SetDestination(_patrolDestination);
        */
        _pursuit.Flee(_wolfTransform, 6f);
        //_pursuit.StayAway(_wolfTransform, 6f, 3);

        if (_currentPatrollTimer < 3)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            _currentPatrollTimer = 0;
            return BT_Status.SUCCESS;
        }




    }

    private BT_Status Patrolling()
    {
        _agent.isStopped = false;
        _agent.speed = 7.5f;
        _currentPatrollTimer += Time.deltaTime;
        if (!_playerAttackComportement)
        {
            return BT_Status.FAILURE;
        }
        if (Vector3.Distance(transform.position, _patrolDestination.position) <= _distance)
        {
            _patrolDestination = _waypointsManager.GetNextPatrolDestination();
        }
        _agent.SetDestination(_patrolDestination.position);
        if (_currentPatrollTimer < _patrollTimer)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            _currentPatrollTimer = 0;
            return BT_Status.SUCCESS;
        }
    }

    private BT_Status PursuitTarget()
    {
        /*
        _agent.isStopped = false;
        _agent.speed = 4f;
        _agent.SetDestination(target.transform.position + target.velocity);
        */
        _pursuit.Pursuit(_targetVelocity, 4f);

        if (Vector3.Distance(transform.position, _targetVelocity.transform.position) > _distance)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            RandomBehavior = Random.Range(0, 2);
            return BT_Status.SUCCESS;
        }
    }

    private BT_Status AttackTarget()
    {
        _agent.isStopped = true;
        _animator.Play("Attack");

        return BT_Status.SUCCESS;
    }
}
