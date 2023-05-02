using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Wolf_SimpleFSM : MonoBehaviour
{
    enum MachineState
    {
        PATROL,
        CHASE,
        ATTACK,
    }

    private MachineState _state;

    private NavMeshAgent _navMeshAgent;
    [SerializeField] private GameObject _target;
    [SerializeField] private WaypointsManager _waypointsManager;
    [SerializeField] private float _attDistance;

    [SerializeField] private TargetVelocity _targetVelocity;
    [SerializeField] private float _speed;
    private Rigidbody _rb;

    private Transform _patrolDestination;

    private Animator _animator;
    private float _timerPatrolling;
    private float _patrolDuration;
    private bool _attackPhase;

    // Start is called before the first frame update
    void Start()
    {
        _state = MachineState.PATROL;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _patrolDestination = _waypointsManager.GetCurrentDestination();
        _timerPatrolling = 0;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _patrolDuration = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        _state = ManageTransitions();
        ManageActions();
    }

    public void GoToStatePatrol()
    {
        _state = MachineState.PATROL;
    }
    public void GoToStateChase()
    {
        _state = MachineState.CHASE;
    }
    public void GoToStateATTACK()
    {
        _state = MachineState.ATTACK;
    }


    private void ManageActions()
    {
        switch (_state)
        {
            case MachineState.PATROL:
                _navMeshAgent.isStopped = false;
                _navMeshAgent.speed = 7.5f;
                _timerPatrolling += Time.deltaTime;
                if (Vector3.Distance(transform.position, _patrolDestination.position) <= 2)
                {
                    _patrolDestination = _waypointsManager.GetNextPatrolDestination();
                }
                _navMeshAgent.SetDestination(_patrolDestination.position);
                break;

            case MachineState.CHASE:
                _timerPatrolling = 0f;
                _navMeshAgent.speed = 4f;
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_targetVelocity.transform.position + _targetVelocity.velocity);
                break;

            case MachineState.ATTACK:
                _attackPhase = true;
                _patrolDuration = Random.Range(1, 5);
                _navMeshAgent.isStopped = true;
                _animator.Play("Attack");
                Debug.Log("Wolf is Attacking");

                StartCoroutine(ResetAttackPhase());
                break;


            default:
                break;
        }
    }

    private MachineState ManageTransitions()
    {
        switch (_state)
        {
            case MachineState.PATROL:
                if (_timerPatrolling >= _patrolDuration)
                    return MachineState.CHASE;
                break;

            case MachineState.CHASE:
                if (Vector3.Distance(_target.transform.position, transform.position) <= _attDistance)
                    return MachineState.ATTACK;

                break;

            case MachineState.ATTACK:
                if (Vector3.Distance(_target.transform.position, transform.position) > _attDistance)
                    return MachineState.CHASE;
                else if (!_attackPhase)
                    return MachineState.PATROL;

                break;

            default:
                Debug.LogError("Illegal state, so go back patrolling");
                break;
        }
        return _state;
    }
    IEnumerator ResetAttackPhase()
    {
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
            _attackPhase = false;
    }
}
