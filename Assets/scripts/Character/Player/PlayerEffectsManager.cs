using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{


    public class PlayerEffectsManager : CharacterEffectsManager 
    {
        [Header("Debug Delete Later")]
        [SerializeField] TakeStaminaDamageEffect effectToTest;
        [SerializeField] bool processEffect = false;

    private void Update()
    {
      if(processEffect)
      {
        processEffect = false;
        TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
        ProcessInstantEffect(effect);
      }
    }
  }
}
