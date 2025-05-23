using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NuitrackSDK;
using NuitrackSDK.SensorEnvironment;

public class Draw : MonoBehaviour
{
    private Color lastColor = Color.red; //tracking previous color
    public float colorMargin = 0.8f; // margin of change

    public GameObject markPrefab;
    private RectTransform rectTransform;
    [SerializeField] nuitrack.JointType rootJoint1= nuitrack.JointType.LeftHand;
    [SerializeField] nuitrack.JointType rootJoint2 = nuitrack.JointType.RightHand;
    
    // Start is called before the first frame update
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
        Vector3 rootPos = Quaternion.Euler(0f, 180f, 0f) * skeleton.GetJoint(rootJoint1).Position;
        CreateMark(rootPos);
        rootPos = Quaternion.Euler(0f, 180f, 0f) * skeleton.GetJoint(rootJoint2).Position;
        CreateMark(rootPos);
    }

    private IEnumerator deleteDelay(GameObject mark, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(mark);
    }

    private Color getColor(Color prevColor)
    {
        
        float r = Mathf.Clamp(prevColor.r + Random.Range(-colorMargin, colorMargin), 0f, 1f);
        float g = Mathf.Clamp(prevColor.g + Random.Range(-colorMargin, colorMargin), 0f, 1f);
        float b = Mathf.Clamp(prevColor.b + Random.Range(-colorMargin, colorMargin), 0f, 1f);

        return new Color(r, g, b);
        //return new Color(1f, 1f, 1f); //test, delete later
    }

    private void CreateMark(Vector3 pos)
    {
        Vector3 worldpos = pos * 700;
        Vector3 screenpos = Camera.main.WorldToScreenPoint(worldpos);
        Vector2 canvaspos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenpos, Camera.main, out canvaspos);

        // inst circle prefab
        GameObject mark = Instantiate(markPrefab, rectTransform);

        // set circle position
        RectTransform markRect = mark.GetComponent<RectTransform>();
        markRect.anchoredPosition = canvaspos;

        // randomize circle size
        float randsize = Random.Range(50f, 150f); //change values if needed
        markRect.sizeDelta = new Vector2(randsize, randsize);

        // get color within margin of prev
        Color newColor = getColor(lastColor);

        // update last color
        //lastColor = getColor(newColor);

        Image markimage = mark.GetComponent<Image>();
        if (markimage != null)
        {
            markimage.color = newColor;
        }

        StartCoroutine(deleteDelay(mark, 2f));

    }
}
