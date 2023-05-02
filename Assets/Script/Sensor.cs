using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _detectionRadius;
    [SerializeField] private float _playerDetectionRadius;
    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(GameObject.FindGameObjectWithTag("Player").transform.position, _playerDetectionRadius);
    }


    public bool PlayerInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectionRadius, _playerLayerMask);
        if (hitColliders.Length != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool InRangeOfPlayer(GameObject MyEnemy)
    {
        Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, _playerDetectionRadius, _enemyLayerMask);
        if (hitColliders.Length != 0)
        {
            foreach (var collider in hitColliders)
            {
                if (collider.gameObject.name == MyEnemy.name)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }
}
