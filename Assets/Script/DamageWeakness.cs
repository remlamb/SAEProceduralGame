using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Weakness", menuName = "Damage Weakness")]
public class DamageWeakness : ScriptableObject
{
    [System.Serializable]
    public struct Weakness
    {
        public Elements elementType;
        public int percentageToTake;
    }

    public Weakness weakness;

    public float CalculateDamageFromElement(float damage, Elements element)
    {
        if(weakness.elementType == element)
        {
            return (damage * weakness.percentageToTake);
        }
        return damage;
    }
}
