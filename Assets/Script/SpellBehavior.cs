using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehavior : MonoBehaviour
{
    [SerializeField] private float _duration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(TimeOut());  
    }

    private IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(_duration);
        Destroy(gameObject);
    }
}
