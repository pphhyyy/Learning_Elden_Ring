using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    public class TitleScreenLoadInputManager : MonoBehaviour
    {
        PlayerControls playerControls;

        [Header("Title Screen Inputs")]
        [SerializeField] bool deletCharacterSlot = false;
        private void Update()
        {
            if (deletCharacterSlot)
            {
                deletCharacterSlot = false;
                TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.UI.X.performed += i => deletCharacterSlot = true;
            }
            playerControls.Enable();
        }

        private void OnDisable() 
        {
            playerControls.Disable();
        }
    }
}


