using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using extOSC;

public class S8_lightning_flash : MonoBehaviour
{
    public GameObject thunderflash;
    private Color start_color, mid_color;
    private SpriteRenderer flash;

    [Header("OSC Settings")]
    public OSCReceiver Receiver;
    public string Address = "/inemotion/effect";

    private void Start()
    {
        Receiver.Bind(Address, ReceivedMessage);
        flash = thunderflash.GetComponent<SpriteRenderer>();
        start_color = flash.color;
        mid_color = new Color(start_color.r, start_color.g, start_color.b, 1);
    }
    private void ReceivedMessage(OSCMessage message)
    {
        Debug.LogFormat("Received: {0}", message);
        StartCoroutine(thunder_event());

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            StartCoroutine(thunder_event());
        }
    }
    IEnumerator thunder_event()
    {
        float t = 0;
        yield return new WaitForSeconds(.05f);
        while (t <= 1)
        {
            flash.color = Color.Lerp(start_color, mid_color, t);
            t += .2f;
            yield return new WaitForSeconds(.01f);
        }
        flash.color = mid_color;
        yield return new WaitForSeconds(.1f);
        t = 0;
        while (t <= 1)
        {
            flash.color = Color.Lerp(mid_color, start_color, t);
            t += .1f;
            yield return new WaitForSeconds(.01f);
        }
        flash.color = start_color;
    }
}
