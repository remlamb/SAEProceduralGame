using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Slider _playerHPSlider;
    void Start()
    {
        _playerHPSlider.maxValue = _playerHealth.GetHealthMax();
    }

    // Update is called once per frame
    void Update()
    {
        _playerHPSlider.value = _playerHealth.GetCurrentHealth();
    }
}
