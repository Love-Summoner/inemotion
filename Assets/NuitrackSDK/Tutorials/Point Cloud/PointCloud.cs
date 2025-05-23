﻿using UnityEngine;


namespace NuitrackSDK.Tutorials.PointCloud
{
    [AddComponentMenu("NuitrackSDK/Tutorials/Point Cloud/Point Cloud")]
    public class PointCloud : MonoBehaviour
    {
        [SerializeField] Material depthMat = null, colorMat = null; //materials for depth and color output

        ulong lastFrameID = ulong.MaxValue;

        [SerializeField] int hRes;
        int frameStep;

        [SerializeField] Color defaultColor;
        [SerializeField] GameObject pointMesh;
        [SerializeField] float meshScaling = 1f;
        float depthToScale; //Size of the "points", depending on the range (than the cubes further, the larger they are)
        Texture2D depthTexture, rgbTexture;
        Color[] depthColors;
        Color[] rgbColors;
        GameObject[] points;

        bool initialized = false;

        void Start()
        {
            if (!initialized) Initialize();
        }

        void Initialize()
        {
            initialized = true;

            nuitrack.OutputMode mode = NuitrackManager.DepthSensor.GetOutputMode(); //Returns the structure in which there is resolution, FPS and FOV of the sensor

            frameStep = mode.XRes / hRes;
            if (frameStep <= 0) frameStep = 1; // frameStep must be bigger than 0
            hRes = mode.XRes / frameStep;

            InitMeshes(
              ((mode.XRes / frameStep)), //Width
              ((mode.YRes / frameStep)), //Height
              mode.HFOV);
        }

        void InitMeshes(int cols, int rows, float hfov)
        {
            depthColors = new Color[cols * rows];
            rgbColors = new Color[cols * rows];
            points = new GameObject[cols * rows];

            depthTexture = new Texture2D(cols, rows, TextureFormat.RFloat, false);
            depthTexture.filterMode = FilterMode.Point;
            depthTexture.wrapMode = TextureWrapMode.Clamp;
            depthTexture.Apply();

            rgbTexture = new Texture2D(cols, rows, TextureFormat.ARGB32, false);
            rgbTexture.filterMode = FilterMode.Point;
            rgbTexture.wrapMode = TextureWrapMode.Clamp;
            rgbTexture.Apply();

            depthMat.mainTexture = depthTexture;
            colorMat.mainTexture = rgbTexture;

            int pointId = 0;

            for (int i = 0; i < rgbTexture.height; i++)
            {
                for (int j = 0; j < rgbTexture.width; j++)
                {
                    points[pointId++] = Instantiate(pointMesh, transform);
                }
            }
        }

        void Update()
        {
            if (NuitrackManager.DepthFrame != null)
            {
                nuitrack.DepthFrame depthFrame = NuitrackManager.DepthFrame;
                nuitrack.ColorFrame colorFrame = NuitrackManager.ColorFrame;

                bool haveNewFrame = (lastFrameID != depthFrame.ID);
                if (haveNewFrame)
                {
                    ProcessFrame(depthFrame, colorFrame);
                    lastFrameID = depthFrame.ID;
                }
            }
        }

        void ProcessFrame(nuitrack.DepthFrame depthFrame, nuitrack.ColorFrame colorFrame)
        {
            int pointIndex = 0;

            for (int i = 0; i < depthFrame.Rows; i += frameStep)
            {
                for (int j = 0; j < depthFrame.Cols; j += frameStep)
                {
                    //take depth from the frame and put it into the depthColors array
                    depthColors[pointIndex].r = depthFrame[i, j] / 16384f;

                    //take from the rgb frame a color and put it into an array rgbColors
                    Color rgbCol = defaultColor;
                    if (colorFrame != null)
                    {
                        int colorJ = Mathf.Clamp(j * colorFrame.Cols / depthFrame.Cols, 0, colorFrame.Cols - 1);
                        int colorI = Mathf.Clamp(i * colorFrame.Rows / depthFrame.Rows, 0, colorFrame.Rows - 1);

                        rgbCol = new Color32(colorFrame[colorI, colorJ].Red, colorFrame[colorI, colorJ].Green, colorFrame[colorI, colorJ].Blue, 255);
                    }

                    rgbColors[pointIndex] = rgbCol;

                    //Hide the "point" if its depth is 0
                    if (depthFrame[i, j] == 0)
                        points[pointIndex].SetActive(false);
                    else
                    {
                        points[pointIndex].GetComponent<Renderer>().material.color = rgbCol;

                        //change of position of the point (cube) in Z (depth)
                        Vector3 newPos = NuitrackManager.DepthSensor.ConvertProjToRealCoords(j, i, depthFrame[i, j]).ToVector3();

                        points[pointIndex].SetActive(true);
                        points[pointIndex].transform.position = newPos;

                        float distancePoints = Vector3.Distance(newPos, NuitrackManager.DepthSensor.ConvertProjToRealCoords(j + 1, i, depthFrame[i, j]).ToVector3()); //Distance between points
                        depthToScale = distancePoints * depthFrame.Cols / hRes; //calculate the size of cubes as a function of depth

                        points[pointIndex].transform.localScale = Vector3.one * meshScaling * depthToScale;
                    }

                    ++pointIndex;
                }
            }

            depthTexture.SetPixels(depthColors);
            rgbTexture.SetPixels(rgbColors);

            depthTexture.Apply();
            rgbTexture.Apply();
        }
    }
}