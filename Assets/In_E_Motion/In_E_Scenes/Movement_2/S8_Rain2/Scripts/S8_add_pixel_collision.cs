using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S8_add_pixel_collision : MonoBehaviour
{
    void Start()
    {
        ParticleSystem rain = GameObject.Find("cover_screen").GetComponent<ParticleSystem>();

        rain.trigger.AddCollider(GetComponent<BoxCollider>());
    }
}
