using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelRotationUI : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private RectTransform _rectTransform;

    void Start()
    {
        //_rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        _rectTransform.rotation = new Quaternion(_rectTransform.rotation.x, 0, _rectTransform.rotation.z, 1);
    }
}
