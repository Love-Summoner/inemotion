using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using nuitrack;

public class Sc1_UserTracker : MonoBehaviour
{
    void Start()
    {
        NuitrackManager.onUserTrackerUpdate += HandleUserTracking;
    }

    void HandleUserTracking(nuitrack.UserFrame frame)
    {
        foreach (var user in frame.Users)
        {
            Debug.Log("User ID: " + user.ID + ", Position: " + user.Real.X + ", " + user.Real.Y);
        }
    }

    void OnDestroy()
    {
        NuitrackManager.onUserTrackerUpdate -= HandleUserTracking;
    }
}
