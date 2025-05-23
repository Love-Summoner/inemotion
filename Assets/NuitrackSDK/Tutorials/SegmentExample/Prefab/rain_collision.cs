using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rain_collision : MonoBehaviour
{
    int index;
    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "S8_Lite-Raining")
        {
            GetComponent<rain_collision>().enabled = false;
        }
        else
        {
            ParticleSystem ps = GameObject.Find("cover_screen").GetComponent<ParticleSystem>();
            index = ps.trigger.colliderCount;
            ps.trigger.SetCollider(index, transform);
        }
    }
    void onDisable()
    {
        ParticleSystem ps = GameObject.Find("cover_screen").GetComponent<ParticleSystem>();
        ps.trigger.RemoveCollider(index);
    }

    
}
