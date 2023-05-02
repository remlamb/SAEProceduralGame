using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class PlayerAttack : MonoBehaviour
{
    private InputWrapper _inputs;
    private Animator _playerAnimator;
    private bool _isUsingAtt;

    [Header("Shooting")]
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Bullet _bulletMax;
    [SerializeField] private Transform _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPointPlayer;

    private float _timerBullet;
    [SerializeField] private float _timeBetweenBullet;

    [SerializeField] private GameObject _ShooterGO;
    [SerializeField] private float _aimingRotateSmoothing = 1000f;
    [SerializeField] private GameObject _gunModel;

    [Header("Melee")]
    [SerializeField] private GameObject _swordModel;
    private float _timerMeleeAtt;
    [SerializeField] private float _timeBetweenMeleeAtt;

    [Header("Spell")]
    [SerializeField] private GameObject _spellPrefab;
    [SerializeField] private GameObject _spellMaxPrefab;
    private float _timerSpell;
    [SerializeField] private float _timeBetweenSpell;
    [SerializeField] private float _spellRange;

    [SerializeField] private Transform _originOverlapSphereDetection;
    [SerializeField] private float _radiusDetectionSphere;
    [SerializeField] private LayerMask _enemyLayerMask;

    [Header("Combo")]
    [SerializeField] private int _currentCombo;


    [Header("RaycastShooting")]
    [SerializeField] private int _rangeMax;
    [SerializeField] private int _aimingRangeDetection;

    [Header("ComboUI")]
    [SerializeField] private GameObject _comboUI;
    [SerializeField] private List<GameObject> _comboLogo;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _slashClip;
    [SerializeField] private AudioClip _shootingClip;


    // Start is called before the first frame update
    void Start()
    {
        _inputs = GetComponent<InputWrapper>();
        _playerAnimator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _timerBullet = _timeBetweenBullet;
        _timerMeleeAtt = _timeBetweenMeleeAtt;
        _timerSpell = _timeBetweenSpell;

        _currentCombo = 1;
        _isUsingAtt = false;
        _swordModel.SetActive(false);
        SetGunInactive();
    }

    // Update is called once per frame
    void Update()
    {
        Shooting();
        MeleeAtt();
        //SpellAtt();
        SpellAttWithDetection();
        ResetIsUsingAtt();
        ComboUIManager();
    }

    private void Shooting()
    {
        //Aiming with left Stick
        Vector3 aimingDirecion = Vector3.right * _inputs._aiming.x + Vector3.forward * _inputs._aiming.y;
        if (aimingDirecion.sqrMagnitude > 0.0f)
        {
            Quaternion newRotation = Quaternion.LookRotation(aimingDirecion, Vector3.up);
            _ShooterGO.transform.rotation = Quaternion.RotateTowards(_ShooterGO.transform.rotation, newRotation, _aimingRotateSmoothing * Time.deltaTime);
        }

        //Shooting cadence
        if (_timerBullet <= _timeBetweenBullet)
        {
            _timerBullet += Time.deltaTime;
            //_playerAnimator.SetBool("isShooting", false);
        }

        //Spawning
        if (_inputs.useFire && _timerBullet >= _timeBetweenBullet && !_isUsingAtt)
        {
            _isUsingAtt = true;
            SetSwordInactive();


            _playerAnimator.SetBool("isShooting", true);
            _playerAnimator.SetBool("isAttacking", false);
            _playerAnimator.SetBool("isAttackingMax", false);
            if (_currentCombo < 3)
            {
                Transform spawnPosition = _bulletSpawnPoint;
                //StartCoroutine(RiffleShoot(spawnPosition));
                _timerBullet = 0;
            }
            if (_currentCombo >= 3)
            {
                //Instantiate(_bulletMax, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
                _timerBullet = 0;
                //_currentCombo = 0;
            }
            //_currentCombo++;
        }
    }
    
    private IEnumerator RiffleShoot(Transform spawnPosition)
    {
        Instantiate(_bullet, spawnPosition.position, spawnPosition.rotation);
        Instantiate(_bullet, spawnPosition.position, spawnPosition.rotation * Quaternion.Euler(0, 10, 0));
        Instantiate(_bullet, spawnPosition.position, spawnPosition.rotation * Quaternion.Euler(0, -10, 0));
        yield return new WaitForSeconds(0.03f);
        Instantiate(_bullet, spawnPosition.position, spawnPosition.rotation * Quaternion.Euler(0, 5, 0));
        Instantiate(_bullet, spawnPosition.position, spawnPosition.rotation * Quaternion.Euler(0, -5, 0));
    }

    public IEnumerator PlayerRiffleShoot()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = _shootingClip;
            _audioSource.Play();
        }
            
        if (_currentCombo < 3)
        {
            Instantiate(_bullet, _bulletSpawnPointPlayer.position, _bulletSpawnPointPlayer.rotation);
            yield return new WaitForSeconds(0.03f);
            Instantiate(_bullet, _bulletSpawnPointPlayer.position, _bulletSpawnPointPlayer.rotation * Quaternion.Euler(0, 5, 0));
            Instantiate(_bullet, _bulletSpawnPointPlayer.position, _bulletSpawnPointPlayer.rotation * Quaternion.Euler(0, -5, 0));

        }
        else
        {
            Instantiate(_bulletMax, _bulletSpawnPointPlayer.position, _bulletSpawnPointPlayer.rotation);
        }
    }


    private void MeleeAtt()
    {
        if (_timerMeleeAtt < _timeBetweenMeleeAtt)
        {
            _timerMeleeAtt += Time.deltaTime;
            //_playerAnimator.SetBool("isAttacking", false);
            //_playerAnimator.SetBool("isAttackingMax", false);
        }

        if (_inputs.useMelee && _timerMeleeAtt >= _timeBetweenMeleeAtt && !_isUsingAtt)
        {
            _isUsingAtt = true;
            SetGunInactive();
            _playerAnimator.SetBool("isShooting", false);

            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = _slashClip;
                _audioSource.Play();
            }



            if (_currentCombo < 3)
            {
                _playerAnimator.SetBool("isAttacking", true);
                _timerMeleeAtt = 0;
                //_currentCombo++;
            }

            else if(_currentCombo >= 3)
            {
                _playerAnimator.SetBool("isAttacking", false);
                _playerAnimator.SetBool("isAttackingMax", true);
                _timerMeleeAtt = 0;
                //_currentCombo = 1;
            }
        }
    }

    private void SpellAtt()
    {
        if (_timerSpell < _timeBetweenSpell)
        {
            _timerSpell += Time.deltaTime;
        }

        if (_inputs.useSpell && _timerSpell >= _timeBetweenSpell && !_isUsingAtt)
        {
            _isUsingAtt = true;
            Vector2 playerDirection = _inputs._move.normalized;
            if (_currentCombo < 3)
            {
                Instantiate(_spellPrefab,transform.position + new Vector3(playerDirection.x, 0,playerDirection.y) * _spellRange, transform.rotation);
                _timerSpell = 0;
                _currentCombo++;
            }

            else if (_currentCombo >= 3)
            {
                Instantiate(_spellMaxPrefab, transform.position + new Vector3(playerDirection.x, 0, playerDirection.y) * _spellRange, transform.rotation);
                _timerSpell = 0;
                _currentCombo = 1;
            }
        }
    }

    private void SpellAttWithDetection()
    {
        if (_timerSpell < _timeBetweenSpell)
        {
            _timerSpell += Time.deltaTime;
        }

        if (_inputs.useSpell && _timerSpell >= _timeBetweenSpell && !_isUsingAtt)
        {
            _isUsingAtt = true;
            SetSwordInactive();
            Vector2 playerDirection = _inputs._move.normalized;
            Vector3 magiePosition = transform.position + new Vector3(playerDirection.x, 0, playerDirection.y) * _spellRange;
            Collider[] hitColliders = Physics.OverlapSphere(_originOverlapSphereDetection.position, _radiusDetectionSphere, _enemyLayerMask);
            if (hitColliders.Length != 0)
            {
                magiePosition = hitColliders[0].gameObject.transform.position;
            }

            if (_currentCombo < 3)
            {
                Instantiate(_spellPrefab, magiePosition, transform.rotation);
                _timerSpell = 0;
                _currentCombo++;
            }

            else if (_currentCombo >= 3)
            {
                if (hitColliders.Length != 0)
                {
                    foreach (Collider hit in hitColliders)
                    {
                        Instantiate(_spellMaxPrefab, hit.gameObject.transform.position, transform.rotation);
                    }
                }
                else
                {
                    Instantiate(_spellMaxPrefab, magiePosition, transform.rotation);
                }
                //Instantiate(_spellMaxPrefab, magiePosition, transform.rotation);
                _timerSpell = 0;
                _currentCombo = 1;
            }
        }
    }

    private void ResetIsUsingAtt()
    {
        if(_timerMeleeAtt >= _timeBetweenMeleeAtt && _timerBullet >= _timeBetweenBullet && _timerSpell >= _timeBetweenSpell)
        {
            _isUsingAtt = false;
        }
    }

    //AnimationEvent
    private void SetSwordActive()
    {
        _swordModel.SetActive(true);
    }

    private void SetSwordInactive()
    {
        _swordModel.SetActive(false);
    }

    private void SetGunActive()
    {
        _gunModel.SetActive(true);
    }

    private void SetGunInactive()
    {
        _gunModel.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_originOverlapSphereDetection.position, _radiusDetectionSphere);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(_bulletSpawnPointPlayer.position.x, _bulletSpawnPoint.position.y, -1*(_bulletSpawnPointPlayer.position.z - _rangeMax / 2)), new Vector3(_aimingRangeDetection, 2, _rangeMax));
    }

    public void SetAttBoolFalse()
    {
        _playerAnimator.SetBool("isAttacking", false);
        _playerAnimator.SetBool("isAttackingMax", false);
        _playerAnimator.SetBool("isShooting", false);
    }

    public void AddCurrentCombo()
    {
        if(_currentCombo < 3)
        {
            _currentCombo++;
        }
        else
        {
            _currentCombo = 1;
        }
    }

    private void ComboUIManager()
    {
        /*
        if (_isUsingAtt)
        {
            _comboUI.SetActive(true);
            _comboText.text = Convert.ToString(_currentCombo);
        }
        else
        {
            _comboUI.SetActive(false);
        }*/
        //_comboText.text = Convert.ToString(_currentCombo -1 );
        foreach (var logo in _comboLogo)
        {
            logo.SetActive(false);
        }
        _comboLogo[_currentCombo -1].SetActive(true);
    }
}
