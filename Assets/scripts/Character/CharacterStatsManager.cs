using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA{
public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;
    [Header("Stamina Regeneration")]
    [SerializeField] float staminaRegenAmount = 2;
    private float staminaRegenarationTimer = 0 ;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2;

    protected virtual void Awake()
    {
      character = GetComponent<CharacterManager>();
    }

    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;

        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina); 
    }

    public virtual void RegenarateStamina()
        {
            if(!character.IsOwner)
            return;
            if(character.characterNetworkManager.isSprinting.Value)
            return;
            if(character.isPerfromingAction)
            return;
            
            staminaRegenarationTimer += Time.deltaTime;

            if(staminaRegenarationTimer >= staminaRegenerationDelay)
            {
                // 休息超过  staminaRegenerationDelay 后才能开始恢复体力
                if(character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    // 每隔 0.1 秒 恢复一点体力
                    staminaTickTimer += Time.deltaTime;
                    if(staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0 ;
                        character.characterNetworkManager.currentStamina.Value += staminaRegenAmount;
                    }
                }
            }
        }

    public virtual void ResetStaminaRegenTimer(float oldValue,float newValue)
    {
        // 只有消耗了耐力，才需要恢复耐力
        if(newValue < oldValue)
        staminaRegenarationTimer = 0;
    }
}
}