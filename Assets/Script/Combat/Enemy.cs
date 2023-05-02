using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Health _health;

    [SerializeField] private GameObject _hitParticule;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _slurpClip;


    [SerializeField] private ParticleSystem _onDeathParticleSystem;

    private Collider _collider;
    private bool _deathEffectLaunch;

    // Start is called before the first frame update
    void Start()
    {
        _health = GetComponent<Health>();
        _deathEffectLaunch = false;
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_health.GetCurrentHealth() <= 0)
        {
            OnDeath();
            
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            _health.GetDamage(1);
            Instantiate(_hitParticule, other.transform.position, this.transform.rotation);
            OnHitAudio();
        }

        else if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            _health.GetDamage(1);
            Instantiate(_hitParticule, other.transform.position, this.transform.rotation);
            OnHitAudio();
        }

        else if (other.CompareTag("Spell"))
        {
            Destroy(other.gameObject);
            _health.GetDamage(0.5f);
            Instantiate(_hitParticule, other.transform.position, this.transform.rotation);
            OnHitAudio();
        }
    }


    private void OnHitAudio()
    {
        _audioSource.clip = _slurpClip;
        _audioSource.Play();
    }

    private void OnDeath()
    {
        if (!_deathEffectLaunch)
        {
            _collider.enabled = false;
            StartCoroutine(DestroyOnDeath());
            Instantiate(_onDeathParticleSystem, transform.position, transform.rotation);
            _onDeathParticleSystem.Play();
            _deathEffectLaunch = true;
        }
    }

    private IEnumerator DestroyOnDeath()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}
