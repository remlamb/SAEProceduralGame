using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float _speed = 10f;
    [SerializeField] public Elements element;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();;
    }

    // Update is called once per frame
    void Update()
    {
        _rb.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
        StartCoroutine(TimeOut());
    }


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
