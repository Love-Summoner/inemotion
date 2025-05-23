using extOSC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning_randomizer : MonoBehaviour
{
    public float speed;
    private LineRenderer linerenderer;

    [Header("OSC Settings")]
    public OSCReceiver Receiver;
    public string Address = "/inemotion/effect";

    private void Start()
    {
        Receiver.Bind(Address, ReceivedMessage);
    }
    void Awake()
    {
        linerenderer = GetComponent<LineRenderer>();
    }

    public void lightning_strike(float side)
    {
        linerenderer.enabled = true;
        float change = -(linerenderer.GetPosition(0).y - linerenderer.GetPosition(5).y)/linerenderer.positionCount;

        for (int i = 0; i < linerenderer.positionCount; i++) 
        {
            if (i != 0 && i != 5)
            {
                if (i % 2 == 0)
                    linerenderer.SetPosition(i, new Vector3(Random.Range(.5f, 4f - i*.5f) * side, change * i * Random.Range(.7f, 1f) + 8, 10));
                else
                    linerenderer.SetPosition(i, new Vector3(Random.Range(-4f + i * .5f, -.5f) * side, change * i * Random.Range(.7f, 1f) + 8, 10));
            }
            else
            {
                if (i % 2 == 0)
                    linerenderer.SetPosition(i, new Vector3(Random.Range(.5f, 4f - i) * side, 8, 10));
                else
                    linerenderer.SetPosition(i, new Vector3(Random.Range(-4f + i, -.5f) * side, -4.5f, 10));
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(lighting_event());
        }
    }

    private void ReceivedMessage(OSCMessage message)
    {
        Debug.LogFormat("Received: {0}", message);
        StartCoroutine(lighting_event());

    }

    IEnumerator repeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(speed);
        }
    }
    IEnumerator lighting_event()
    {
        float side = (Random.Range(0, 2) == 1) ? 1 : -1;
        linerenderer.enabled = true;
        lightning_strike(side);
        yield return new WaitForSeconds(speed/2);
        lightning_strike(side*-1);
        yield return new WaitForSeconds(speed/2);
        linerenderer.enabled = false;
    }
}
