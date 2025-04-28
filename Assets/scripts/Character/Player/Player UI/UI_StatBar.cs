using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PA
{

    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;  // 耐力条，耐力越高 这里 slider 显示的值就越大 
        private RectTransform rectTransform;

        [Header("Bar Options")]
        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthScaleMultiplier = 1;
 
        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();

        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;

        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if(scaleBarLengthWithStats)
            {
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier , rectTransform.sizeDelta.y);
                // 按照 ui 条 在他们自己的 layout goup 中的位置 更新 位置
                PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
            }
        }
    }

}

