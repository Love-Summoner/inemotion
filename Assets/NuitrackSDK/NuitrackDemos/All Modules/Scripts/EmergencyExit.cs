﻿using UnityEngine;

namespace NuitrackSDK.NuitrackDemos
{
    public class EmergencyExit : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Escape)) Application.Quit();
        }
    }
}