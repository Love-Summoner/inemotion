using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class LogGenerator : MonoBehaviour
{
    public TMP_Text displayText;  // Reference to TextMeshPro text component
    public float logInterval = 1f; // Interval between log updates
    public int maxLines = 30; // Maximum number of lines in the log

    private string[] logMessages;

    // Color options for the log messages
    private Color[] logColors = new Color[]
    {
        Color.white,
        Color.green,
        Color.yellow,
        Color.red,
        Color.cyan,
        Color.magenta,
        new Color(1f, 0.5f, 0f),  // Orange
    };

    // Font style options
    private FontStyle[] fontStyles = new FontStyle[]
    {
        FontStyle.Normal,
        FontStyle.Bold,
        FontStyle.Italic,
        FontStyle.BoldAndItalic
    };

    void Start()
    {
        // Predefined log messages for simulation
        logMessages = new string[]
        {
            "System Booted: Starting services...",
            "CPU Usage: 12.5%",
            "RAM Usage: 2048MB / 8192MB",
            "Disk Usage: 45%",
            "Network: Downloading updates...",
            "System Alert: High memory usage detected.",
            "Application crashed unexpectedly.",
            "Rebooting system...",
            "User logged in: Admin",
            "Error: Could not initialize service X.",
            "Update complete: System is up-to-date.",
            "Battery level: 75%",
            "Warning: Low disk space!",
            "System performance is normal.",
            "System Error: Out of memory.",
            "Update Required: Version 1.2.3",
            "Hardware Test: Passed.",
            "Error: Disk read failure.",
            "Connected to Network: Wi-Fi",
            "System time sync successful.",
            "User logged out: Admin"
        };

        // Start generating logs
        StartCoroutine(GenerateLogs());
    }

    IEnumerator GenerateLogs()
    {
        while (true)
        {
            string newLog = GenerateRandomLog();
            displayText.text = newLog + "\n" + displayText.text;

            // Limit the log lines to `maxLines` so it doesn't overflow
            string[] lines = displayText.text.Split('\n');
            if (lines.Length > maxLines)
            {
                displayText.text = string.Join("\n", lines, 0, maxLines);
            }

            yield return new WaitForSeconds(logInterval);
        }
    }

    string GenerateRandomLog()
    {
        // Randomly select a log message and simulate the data
        string log = logMessages[Random.Range(0, logMessages.Length)];

        // Simulate data like CPU usage, RAM usage, etc.
        if (log.Contains("CPU Usage"))
        {
            log = $"CPU Usage: {Random.Range(10, 90)}%";
        }
        else if (log.Contains("RAM Usage"))
        {
            log = $"RAM Usage: {Random.Range(1000, 8000)}MB / 8192MB";
        }
        else if (log.Contains("Disk Usage"))
        {
            log = $"Disk Usage: {Random.Range(10, 100)}%";
        }
        else if (log.Contains("Network"))
        {
            log = $"Network: Downloading {Random.Range(10, 500)} MB...";
        }
        else if (log.Contains("Battery"))
        {
            log = $"Battery level: {Random.Range(50, 100)}%";
        }
        else if (log.Contains("Error"))
        {
            log = $"Error: {Random.Range(1, 100)} - Random system error!";
        }
        else if (log.Contains("Update"))
        {
            log = $"Update: Version {Random.Range(1, 10)}.{Random.Range(0, 10)}.{Random.Range(0, 10)} completed.";
        }

        // Randomize the color and font style
        string coloredLog = $"<color=#{ColorUtility.ToHtmlStringRGB(GetRandomColor())}>{log}</color>";
        coloredLog = ApplyRandomStyle(coloredLog);

        return coloredLog;
    }

    // Get a random color from the logColors array
    Color GetRandomColor()
    {
        return logColors[Random.Range(0, logColors.Length)];
    }

    // Apply a random font style to the log
    string ApplyRandomStyle(string log)
    {
        FontStyle randomStyle = fontStyles[Random.Range(0, fontStyles.Length)];
        if (randomStyle == FontStyle.Bold)
        {
            return $"<b>{log}</b>";
        }
        else if (randomStyle == FontStyle.Italic)
        {
            return $"<i>{log}</i>";
        }
        else if (randomStyle == FontStyle.BoldAndItalic)
        {
            return $"<b><i>{log}</i></b>";
        }
        else
        {
            return log; // No style applied
        }
    }
}
