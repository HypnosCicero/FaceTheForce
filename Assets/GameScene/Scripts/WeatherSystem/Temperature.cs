using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// just make the temperature
/// </summary>
public class Temperature : UnityEvent<float>
{
    public delegate void OnTemperatureChange(float newVal);
    public event OnTemperatureChange OnVariableChange;
    private float temperValue = 25;//…„ œ°„

    public float TemperValue
    {
        get
        {
            return temperValue;
        }
        set
        {
            if (temperValue == value)
            {
                return;
            }

            temperValue = value;
            if (OnVariableChange != null)
            {
                OnVariableChange(value);
            }
        }
    }

    
}
