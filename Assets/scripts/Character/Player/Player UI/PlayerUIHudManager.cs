using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA{
public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] UI_StatBar staminaBar;
    public void SetNewStaminaValue(float oldValue , float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxStaminaValue(int maxValue)
    {
        staminaBar.SetMaxStat(maxValue);
    }
}
}
