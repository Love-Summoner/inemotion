using UnityEngine;
using NuitrackSDK;

public class NativeAvatar : MonoBehaviour
{
    string message = "";
    public nuitrack.JointType[] typeJoint;
    GameObject[] CreatedJoint;
    public GameObject PrefabJoint;

    void Start()
    {
        CreatedJoint = new GameObject[typeJoint.Length];
        for (int q = 0; q < typeJoint.Length; q++)
        {
            CreatedJoint[q] = Instantiate(PrefabJoint);
            CreatedJoint[q].transform.SetParent(transform);
        }
        message = "Skeleton created";
    }

    void Update()
    {
        if (NuitrackManager.Users.Current != null && NuitrackManager.Users.Current.Skeleton != null)
        {
            message = "Skeleton found";

            for (int q = 0; q < typeJoint.Length; q++)
            {
                UserData.SkeletonData.Joint joint = NuitrackManager.Users.Current.Skeleton.GetJoint(typeJoint[q]);
                CreatedJoint[q].transform.localPosition = joint.Position;
            }
        }
        else
        {
            message = "Skeleton not found";
        }
    }

    // Display the message on the screen
    void OnGUI()
    {
        //GUI.color = Color.red;
        //GUI.skin.label.fontSize = 50;
        //GUILayout.Label(message);
    }
}