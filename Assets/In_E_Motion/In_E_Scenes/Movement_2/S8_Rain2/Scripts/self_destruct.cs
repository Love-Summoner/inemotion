using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class self_destruct : MonoBehaviour
{
    public float time;
    void Start()
    {
        
    }

    IEnumerator destroy_self()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    
}
