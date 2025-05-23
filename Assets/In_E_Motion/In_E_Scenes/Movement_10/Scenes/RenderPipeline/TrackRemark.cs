using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NuitrackSDK;
using NuitrackSDK.SensorEnvironment;

public class TrackRemark : MonoBehaviour
{
    public GameObject markPrefab;
    private RectTransform rectTransform;
    [SerializeField] nuitrack.JointType rootJoint1= nuitrack.JointType.LeftHand;
    [SerializeField] nuitrack.JointType rootJoint2 = nuitrack.JointType.RightHand;
    
    // Start is called before the first
    // frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (NuitrackManager.Users.Current != null && NuitrackManager.Users.Current.Skeleton != null)
            ProcessSkeleton(NuitrackManager.Users.Current.Skeleton);
    }

    void ProcessSkeleton(UserData.SkeletonData skeleton)
    {
        Vector3 rootPos = Quaternion.Euler(0f, 180f, 0f) * skeleton.GetJoint(rootJoint1).Position * 10;
        CreateMark(rootPos);
        Vector3 rootPos2 = Quaternion.Euler(0f, 180f, 0f) * skeleton.GetJoint(rootJoint2).Position * 10;
        CreateMark(rootPos2);
    }

    private void CreateMark(Vector3 pos)
    {
        Vector2 v2 = pos;
        //GameObject mark = Instantiate(markPrefab, rectTransform);
        //RectTransform markRect = mark.GetComponent<RectTransform>();
        transform.position = v2;
    }
}
