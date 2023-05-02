using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Random Spawn", menuName = "Enemy Random Spawn")]
public class EnemyRandomSpawn : ScriptableObject
{

    [SerializeField] private GameObject[] _enemyListFirstType;
    [SerializeField] private float _percentageFirstType;
    [SerializeField] private GameObject[] _enemyListSecondType;
    [SerializeField] private float _percentageSecondType;
    [SerializeField] private GameObject[] _enemyListThirdType;
    [SerializeField] private float _percentageThirdType;

    public GameObject RandomEnemy()
    {
        //gestion du pourcentage de spawn d'un ennemi 
        float pourcentage = Random.Range(0, 101);
        //Debug.Log(pourcentage);

        if (pourcentage <= _percentageFirstType)
        {
            return _enemyListFirstType[Random.Range(0, _enemyListFirstType.Length)];
        }
        else if (pourcentage > _percentageFirstType && pourcentage <= _percentageFirstType + _percentageSecondType)
        {
            return _enemyListSecondType[Random.Range(0, _enemyListSecondType.Length)];
        }
        else
        {
            return _enemyListThirdType[Random.Range(0, _enemyListThirdType.Length)];
        }
    }
}
