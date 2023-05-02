using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyDamageSystem : MonoBehaviour
{
    private bool _isInvincible;
    [SerializeField] private float _timeInvicible;
    private float _invincibleTimer;

    [SerializeField] Slider _hpSlider;
    [SerializeField] private GameObject _damageTxt;
    [SerializeField] private DamageWeakness _damageWeakness;
    [SerializeField] private float _maxHP;
    private float _currentHP;

    //[SerializeField] private Wolf_SimpleFSM _stateMachine;
    private NavMeshAgent _navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        _isInvincible = false;
        _invincibleTimer = _timeInvicible;

        _currentHP = _maxHP;
        _hpSlider.value = _maxHP;

        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        StopInvincible();

        _currentHP = _hpSlider.value;
        Debug.Log(_currentHP);
        if(_currentHP <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            if (!_isInvincible)
            {

                _damageTxt.GetComponent<TextMeshProUGUI>().text = "Hit";
                _damageTxt.SetActive(true);
                IsInvincible();
            }
        }

        else if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            if (!_isInvincible)
            {
                _damageTxt.SetActive(true);
                DamageBullet(other.GetComponentInParent<Bullet>().element);
                IsInvincible();
            }
        }

        else if (other.CompareTag("Spell"))
        {
            Destroy(other.gameObject);
            if (!_isInvincible)
            {

                _damageTxt.GetComponent<TextMeshProUGUI>().text = "Hit";
                _damageTxt.SetActive(true);
                IsInvincible();
            }
        }
    }



    public void DamageBullet(Elements element)
    {
        _invincibleTimer = 0;
        _isInvincible = true;

        //dmg
        _hpSlider.value -= _damageWeakness.CalculateDamageFromElement(0.1f, element);
        

        //Visual Show if normal hit or WeaknessExploit
        if(_damageWeakness.CalculateDamageFromElement(0.1f, element) > 0.1f)
        {
            _damageTxt.GetComponent<TextMeshProUGUI>().text = "Weakness Hit";
        }
        else
        {
            _damageTxt.GetComponent<TextMeshProUGUI>().text = "Hit";
        }

    }


    private void IsInvincible()
    {
        _hpSlider.value -= 0.1f;
        _invincibleTimer = 0;
        _isInvincible = true;
        //StartCoroutine(EnemyMovementOnHit());

    }

    private void StopInvincible()
    {
        if(_invincibleTimer < _timeInvicible)
        {
            _invincibleTimer += Time.deltaTime;
        }

        else
        {
            _damageTxt.SetActive(false);
            _isInvincible = false;
        }
    }

    /*
    IEnumerator EnemyMovementOnHit()
    {
        _navMeshAgent.speed = 0;
        _stateMachine.enabled = false;
        yield return new WaitForSeconds(1f);
        _stateMachine.enabled = true;
    }*/
}
