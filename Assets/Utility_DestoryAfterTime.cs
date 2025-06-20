using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    public class Utility_DestoryAfterTime : MonoBehaviour
    {
        [SerializeField] float timeUntilDestoryed = 5;

        private void Awake()
        {
            Destroy(gameObject, timeUntilDestoryed);
        }
    }
}

