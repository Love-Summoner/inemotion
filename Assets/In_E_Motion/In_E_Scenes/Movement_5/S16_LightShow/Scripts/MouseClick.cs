using extOSC;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public MonoBehaviour script1; // Assign in Inspector
    public MonoBehaviour script2; // Assign in Inspector
    public MonoBehaviour script3; // Assign in Inspector
    public MonoBehaviour script4;// Assing in Inspector

    [Header("OSC Settings")]
    public OSCReceiver Receiver;
    public string Address = "/inemotion/effect";

    private MonoBehaviour activeScript;

    void Start()
    {
        // Initially, set the first script as active
        activeScript = script1;
        EnableScript(activeScript);
        Receiver.Bind(Address, ReceivedMessage);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Press "T" to toggle
        {
            ToggleScripts();
        }
    }

    private void ReceivedMessage(OSCMessage message)
    {
        Debug.LogFormat("Received: {0}", message);
        ToggleScripts();
    }

    void ToggleScripts()
    {
        // Disable the current active script
        EnableScript(null);

        // Toggle to the next script
        if (activeScript == script1)
        {
            activeScript = script2;
        }
        else if (activeScript == script2)
        {
            activeScript = script3;
        }
        else if (activeScript == script3)
        {
            activeScript = script4;
        }
        else
        {
            activeScript = script1;
        }

        // Enable the new active script
        EnableScript(activeScript);
    }

    void EnableScript(MonoBehaviour script)
    {
        // Disable all scripts first
        script1.enabled = false;
        script2.enabled = false;
        script3.enabled = false;
        script4.enabled = false;
        // Then enable the selected script
        if (script != null)
        {
            script.enabled = true;
        }
    }
}
