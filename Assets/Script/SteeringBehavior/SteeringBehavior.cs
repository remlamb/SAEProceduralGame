using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SteeringBehavior : MonoBehaviour
{
    private NavMeshAgent _agent;

    //For StayInDistance
    private float _posPoint;
    private Vector3 _wanderCenter;
    private Vector3 _wanderPoint;
    private float _radius;

    private float _wanderAngle;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Pursuit(TargetVelocity target, float speed)
    {
        _agent.isStopped = false;
        _agent.speed = speed;
        _agent.SetDestination(target.transform.position + target.velocity);
    }

    public void Seek(Transform target, float speed)
    {
        _agent.isStopped = false;
        _agent.speed = speed;
        _agent.SetDestination(target.position);
    }

    public void Flee(Transform target, float speed)
    {
        Vector3 direction = ( transform.position - target.position).normalized;
        _agent.isStopped = false;
        _agent.speed = speed;
        _agent.SetDestination(direction);
    }

    public void StopMovement()
    {
        _agent.isStopped = true;
    }

    public void SetDestination(Vector3 destination, float speed)
    {
        _agent.speed = speed;
        _agent.SetDestination(destination);
    }

    public void StayAway(Transform target, float speed, float stayDistance)
    {

        Vector3 distance = target.position - transform.position;
        Vector3 direction = distance.normalized;
        _agent.isStopped = false;
        _agent.speed = speed;
        RaycastHit hit;
        if (Physics.SphereCast(target.position,  stayDistance, direction, out hit))
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                _agent.SetDestination((target.position + direction) * speed);
            }

        }
        else
        {
            _agent.SetDestination(target.position);
        }
    }

    
    public void StayInDistance(GameObject target, float distanceRadius, float pointSpeed, float agentSpeed)
    {
        _agent.isStopped = false;
        _radius = distanceRadius;
        _posPoint += pointSpeed * Time.deltaTime;
        _wanderCenter = target.transform.position;
        _wanderPoint = _wanderCenter + distanceRadius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * _posPoint), 0,
            Mathf.Cos(Mathf.Deg2Rad * _posPoint)).normalized;

        _agent.speed = agentSpeed;
        _agent.SetDestination(_wanderPoint);
    }

    public void Wander(float angleMin, float angleMax, float distanceRadius, float agentSpeed)
    {
        _wanderAngle += UnityEngine.Random.Range(angleMin, angleMax);
        _wanderCenter = transform.position + transform.forward;
        _wanderPoint = _wanderCenter + distanceRadius * new Vector3(Mathf.Sin(Mathf.Deg2Rad * _wanderAngle), 0,
            Mathf.Cos(Mathf.Deg2Rad * _wanderAngle)).normalized;
        
        transform.LookAt(_wanderPoint);
        _agent.isStopped = false;
        _agent.speed = agentSpeed;
        _agent.SetDestination(_wanderPoint);
    }

    private void OnDrawGizmosSelected()
    {
        //Stay In Distance Gizmos
        float size = 1f;
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(_wanderPoint, new Vector3(size, size, size));
        Gizmos.DrawWireSphere(_wanderCenter, _radius);
    }
}
