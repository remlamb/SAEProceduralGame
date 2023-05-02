using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _healthMax = 10;
    private float _currentHealth;



    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _healthMax;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetDamage(float damages)
    {
        _currentHealth -= damages;
    }

    public float GetHealthMax()
    {
        return _healthMax;
    }


    public float GetCurrentHealth()
    {
        return _currentHealth;
    }
}
