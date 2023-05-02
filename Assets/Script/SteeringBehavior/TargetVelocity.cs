using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetVelocity : MonoBehaviour
{
    [SerializeField] private Vector3 _velocity;
    public Vector3 velocity{get => _velocity;}

    private Vector3 _oldPosition;

    private void FixedUpdate()
    {
        _velocity = (transform.position - _oldPosition)/Time.deltaTime;
        Debug.DrawRay(transform.position, _velocity, Color.cyan);
        _oldPosition = transform.position;
    }

}
