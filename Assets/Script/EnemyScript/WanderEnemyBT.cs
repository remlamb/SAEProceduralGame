using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEnemyBT : MonoBehaviour
{
    [SerializeField] private SteeringBehavior _steeringBehavior;

    [SerializeField] private float _angleMin;
    [SerializeField] private float _angleMax;
    [SerializeField] private float _speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _steeringBehavior.Wander(_angleMin, _angleMax, 6, _speed);
    }
}
