using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //[SerializeField] private CinemachineVirtualCamera _cameraToFocus;
    [SerializeField] private GameObject _cameraToFocus;
    [SerializeField] private GameObject[] _allCam;
    [SerializeField] private GameObject[] _allWaypoint;
    [SerializeField] private GameObject _currentWayPoint;

    [SerializeField] private GameObject[] _enemiesAlive;
    [SerializeField] private GameObject _barrier;

    // Start is called before the first frame update
    void Start()
    {
        _allCam = GameObject.FindGameObjectsWithTag("VirtualCamera");
        _allWaypoint = GameObject.FindGameObjectsWithTag("WayPoint");
    }

    // Update is called once per frame
    void Update()
    {
        _enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy");
        if (_enemiesAlive.Length > 0)
        {
            _barrier.SetActive(true);
        }
        else
        {
            _barrier.SetActive(false);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var cam in _allCam)
            {
                cam.SetActive(false);
            }
            //_cameraToFocus.Priority = 11;
            _cameraToFocus.SetActive(true);

            foreach (var wayPoint in _allWaypoint)
            {
                wayPoint.SetActive(false);
            }
            _currentWayPoint.SetActive(true);
        }
    }

}
