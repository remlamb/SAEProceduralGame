using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaypointsManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _waypoints;

    private int _idxCurrentWaypoint = 0;

    [SerializeField] private GameObject _player;
    //[SerializeField] private LayerMask _allGameObjectLayerMask;
    
    public Transform GetCurrentDestination()
    {
        if(_idxCurrentWaypoint < _waypoints.Count && _waypoints.Count > 0)
            return _waypoints[_idxCurrentWaypoint];
        else
            return null;
        
    }

    public Transform GetNextPatrolDestination()
    {
        _idxCurrentWaypoint++;
        if(_idxCurrentWaypoint >= _waypoints.Count)
            _idxCurrentWaypoint = 0;

        return GetCurrentDestination();

    }

    public Transform GetRandomDestination()
    {
        int idx = Random.Range(0, _waypoints.Count);
        return _waypoints[idx];
    }

    /*
    public void PointAroundPlayer(float range)
    {
        RaycastingGameObjectAround(new Vector3(0, 0, 1), range, 0, new Vector3(0, 0, range));
        RaycastingGameObjectAround(new Vector3(1, 0, 0), range, 1, new Vector3(range, 0, 0));
        RaycastingGameObjectAround(new Vector3(0, 0, -1), range, 2, new Vector3(0, 0, -1*range));
        RaycastingGameObjectAround(new Vector3(-1, 0, 0), range, 3, new Vector3(-1*range, 0, 0)) ;
    }


    private void RaycastingGameObjectAround(Vector3 direction, float range, int index, Vector3 vectorToAdd)
    {
        RaycastHit hit;
        bool RayHit = Physics.Raycast(_player.transform.position, direction, out hit, range, _allGameObjectLayerMask);
        if (RayHit)
        {
            _waypoints[index].position = hit.point - direction;
        }
        else
        {
            _waypoints[index].position = _player.transform.position + vectorToAdd;
        }
    }*/

}
