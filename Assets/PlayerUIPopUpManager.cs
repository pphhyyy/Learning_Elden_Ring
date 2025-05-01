using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace PA
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("YOU DIED Pop Up")]
        [SerializeField] GameObject youDiedPopUpGameObject;
        [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI youDiedPopUpText;
        [SerializeField] CanvasGroup youDiedPopUpCanvasGroup; // 允许我们设置 alpha 值以实现淡入淡出效果

        public void SendYouDiedPopUp()
        {
            Debug.Log("SendYouDiedPopUp");
            // 激活后处理效果
            youDiedPopUpGameObject.SetActive(true);
            youDiedPopUpBackgroundText.characterSpacing = 0;

            // 拉伸弹出窗口
            StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 19f));
            // 淡入弹出窗口
            StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
            // 等待，然后淡出弹出窗口
            StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup,2,5));
        }

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if (duration > 0f)
            {
                text.characterSpacing = 0; // 重置字符间距
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                    yield return null;
                }
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if (duration > 0f)
            {
                canvas.alpha = 0; // 设置初始透明度为 0
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime); // 逐渐增加透明度
                    yield return null;
                }
            }

            canvas.alpha = 1; // 确保最终透明度为 1
            yield return null;
        }

        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if (duration > 0f)
            {
                while(delay > 0)
                {
                    // 这里的等待 时间和上面 fadein 的时间一致 
                    delay = delay - Time.deltaTime;
                    yield return null;
                }
                
                

                canvas.alpha = 1; // 设置初始透明度为 1
                float timer = 0;

                yield return new WaitForSeconds(delay); // 等待指定的延迟时间

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime); // 逐渐减少透明度
                    yield return null;
                }
            }

            canvas.alpha = 0; // 确保最终透明度为 0
            yield return null;
        }
    }
}

