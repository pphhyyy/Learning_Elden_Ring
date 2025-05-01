using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar staminaBar;

        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }

        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(newValue);
        }

        public void SetMaxHealthValue(int maxValue)
        {
            healthBar.SetMaxStat(maxValue);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxValue)
        {
            staminaBar.SetMaxStat(maxValue);
        }
    }
}
