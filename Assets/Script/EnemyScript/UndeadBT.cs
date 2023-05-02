using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UndeadBT : MonoBehaviour
{

    private BT_Node _behaviour = new BT_Root("Root");
    private BT_Selector _selector = new BT_Selector("Chasing Selector");
    private BT_Sequence _attaqueSeqence = new BT_Sequence("Attack Sequence");
    private BT_Sequence _idleSeqence = new BT_Sequence("IDLE Sequence");

    private Animator _animator;
    [SerializeField] private SteeringBehavior _steeringBehavior;
    [SerializeField] private Sensor _sensor;

    private TargetVelocity _targetVelocity;

    [SerializeField] private float _distance;
    private float _idleTimer;
    private float _animTimer;
    [SerializeField] private float _timeExhausted;

    [SerializeField] private AudioClip _zombieScreamClip;
    [SerializeField] private AudioClip _undeadAttClip;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private float _speed;

    [SerializeField] private GameObject _attRangeGO;
    [SerializeField] private ParticleSystem _attParticule;
    [SerializeField] private Collider _attColider;

    [SerializeField] private float _attduration = 3f;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        _targetVelocity = GameObject.FindGameObjectWithTag("Player").GetComponent<TargetVelocity>();

        _animTimer = 0;
        _idleTimer = 0;

        _attaqueSeqence.AddNode(new BT_Leaf(("Player Detected"), PlayerDetected));
        _attaqueSeqence.AddNode(new BT_Leaf(("Patrolling"), PursuitPlayer));
        _attaqueSeqence.AddNode(new BT_Leaf(("Chasing Target"), AttackPlayer));
        _attaqueSeqence.AddNode(new BT_Leaf(("Attack Target"), Exhausted));

        _idleSeqence.AddNode(new BT_Leaf("IDLE", Idle));

        _selector.AddNode(_attaqueSeqence);
        _selector.AddNode(_idleSeqence);
        _behaviour.AddNode(_selector);

    }

    // Update is called once per frame
    void Update()
    {
        _behaviour.Process();
    }

    public BT_Status Idle()
    {
        _animTimer = 0;
        _animator.Play("Zombie Idle");
        if (_sensor.PlayerInRange())
        {
            return BT_Status.FAILURE;
        }
        if (!_sensor.PlayerInRange())
        {
            return BT_Status.RUNNING;
        }
        else
        {
            return BT_Status.SUCCESS;
        }
    }

    private BT_Status PlayerDetected()
    {
        _idleTimer = 0;
        _animator.Play("Zombie Scream");
        _animTimer += Time.deltaTime;
       // _steeringBehavior.SB_Pursuit(_targetVelocity, 4f);

        if (!_sensor.PlayerInRange())
        {
            return BT_Status.FAILURE;
        }
        if (_animTimer <= _animator.GetCurrentAnimatorStateInfo(0).length)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            return BT_Status.SUCCESS;
        }
    }

    private BT_Status PursuitPlayer()
    {
        _animTimer = 0;
        transform.LookAt(_targetVelocity.gameObject.transform);
        _steeringBehavior.Seek(_targetVelocity.gameObject.transform, _speed);
        _animator.Play("Drunk Walk");
        if (Vector3.Distance(transform.position, _targetVelocity.transform.position) > _distance)
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
        _animator.Play("Zombie Attack");
        _animTimer += Time.deltaTime;

        if (_animTimer <= _attduration)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            UnActiveAttRange();
            return BT_Status.SUCCESS;
        }
    }

    private BT_Status Exhausted()
    {
        _animTimer = 0;
        _steeringBehavior.StopMovement();
        _animator.Play("Exhausted");
        _idleTimer += Time.deltaTime;

        if (_idleTimer <= _timeExhausted)
        {
            return BT_Status.RUNNING;
        }
        else
        {
            return BT_Status.SUCCESS;
        }
    }

    //anim Event
    private void PlayZombieScream()
    {
        _audioSource.clip = _zombieScreamClip;
        _audioSource.Play();
    }

    private void PlayUndeadAttSound()
    {
        _audioSource.clip = _undeadAttClip;
        _audioSource.Play();
    }

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

}
