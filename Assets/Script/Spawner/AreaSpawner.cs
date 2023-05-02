using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpawner : MonoBehaviour
{
    [SerializeField] private float _weightMaxToSpawn;
    [SerializeField] private EnemyRandomSpawn[] _spawnRules;

    [SerializeField] private float _posX;
    [SerializeField] private float _posY;
    [SerializeField] private float _posZ;
    private float _currentWeight = 0;

    private EnemyRandomSpawn spawnRule;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    
    private void SpreadEnemy()
    {
        Vector3 randPosition = new Vector3(Random.Range(-_posX, _posX), Random.Range(-_posY, _posY), Random.Range(-_posZ, _posZ)) + this.transform.position;
        GameObject spawned = Instantiate(spawnRule.RandomEnemy(), randPosition, Quaternion.identity);
        _audioSource.Play();
        _currentWeight += spawned.GetComponent<SpawningWeight>().weight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        spawnRule = _spawnRules[Random.Range(0, _spawnRules.Length)];
        do
        {
            SpreadEnemy();
            yield return new WaitForSeconds(.2f);

        } while (_currentWeight < _weightMaxToSpawn);
        Destroy(this);
    }
}
