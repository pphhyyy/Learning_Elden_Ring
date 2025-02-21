using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
public class PlayerCamera : MonoBehaviour
{

    public Camera cameraObject;
    public static PlayerCamera instance;

    protected void Awake()
    {
        if(instance == null)
        instance = this;
        else 
        Destroy(gameObject);
    }

    protected void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
}
