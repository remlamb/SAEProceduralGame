using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputWrapper : MonoBehaviour
{
    public Vector2 _move;

    public bool useFire;
    public bool useMelee;
    public bool useSpell;
    public bool useDash;
    public Vector2 _aiming;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        if (value.isPressed) //or (ctx.Started)
        {
            useFire = true;
        }
        else
        {
            useFire = false;
        }
    }

    public void OnMelee(InputValue value)
    {
        if (value.isPressed) //or (ctx.Started)
        {
            useMelee = true;
        }
        else
        {
            useMelee = false;
        }
    }

    public void OnSpell(InputValue value)
    {
        if (value.isPressed) //or (ctx.Started)
        {
            useSpell = true;
        }
        else
        {
            useSpell = false;
        }
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed) //or (ctx.Started)
        {
            useDash = true;
        }
        else
        {
            useDash = false;
        }
    }

    public void OnAiming(InputValue value)
    {
        _aiming = value.Get<Vector2>();
    }
}
