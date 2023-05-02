using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAGO : MonoBehaviour
{
    private GameObject _gameObjectToFollow;    
    [SerializeField] private Vector3 _AddToGOPos;
    // Start is called before the first frame update
    void Start()
    {
        _gameObjectToFollow = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _gameObjectToFollow.transform.position + _AddToGOPos;
    }
}
