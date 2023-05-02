using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    [SerializeField] private Health _playerhealth;
    [SerializeField] private GameObject _OnHitEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAtt"))
        {
            _playerhealth.GetDamage(1);
            Instantiate(_OnHitEffect, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        }
    }
}
