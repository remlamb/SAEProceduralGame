using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float _speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, -1 * _speed, _rb.velocity.z);
    }
}
