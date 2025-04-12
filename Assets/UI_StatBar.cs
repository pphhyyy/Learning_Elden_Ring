using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  


namespace PA {

public class UI_StatBar : MonoBehaviour
{
    private Slider slider;  // 耐力条，耐力越高 这里 slider 显示的值就越大 
    
    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();


    }

    public virtual void SetStat(int newValue)
    {
        slider.value = newValue;

    }

    public virtual void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }
}   

}

