﻿using UnityEngine;
using NuitrackSDK.Calibration;


namespace NuitrackSDK.Pointer
{
    public class PointerPassing : MonoBehaviour
    {
        [SerializeField] Transform pointerObject;
        [Header("For Rigged model")]
        [SerializeField] Transform leftHand;
        [SerializeField] Transform rightHand;
        [SerializeField] PointerRotation leftHandRotator;
        [SerializeField] PointerRotation rightHandRotator;

        [Header("For SkeletonAvatar")]
        [SerializeField] bool attachToSkelJoint = false;

        public static int hand = 0;

        nuitrack.PublicNativeImporter.ButtonCallback buttonsCallback;
        nuitrack.PublicNativeImporter.ControllerCalibrationCallback controllerCalibrationCallback;

        public delegate void ClickAction(int buttonID, int eventID);
        public static event ClickAction OnPressed;

        public delegate void CalibrationAction(int handID, float progress);
        public static event CalibrationAction OnCalibration;

        void Start()
        {
            VVRInput.Init();

            buttonsCallback = ButtonsCallback;
            nuitrack.PublicNativeImporter.nuitrack_OnButtonUpdate(buttonsCallback);

            controllerCalibrationCallback = ControllerCalibrationCallback;
            nuitrack.PublicNativeImporter.nuitrack_OnControllerCalibration(controllerCalibrationCallback);
            DoBLEScan(Quaternion.identity);
        }

        void Update()
        {
            if (attachToSkelJoint && NuitrackManager.sensorsData[0].Users.Current != null && NuitrackManager.sensorsData[0].Users.Current.Skeleton != null)
            {
                UserData.SkeletonData skeleton = NuitrackManager.sensorsData[0].Users.Current.Skeleton;

                if (hand % 2 == 0)
                {
                    Vector3 rightHandPos = Vector3.up * CalibrationInfo.FloorHeight + CalibrationInfo.SensorOrientation * skeleton.GetJoint(nuitrack.JointType.RightHand).Position;
                    pointerObject.position = rightHandPos;
                }
                else
                {
                    Vector3 leftHandPos = Vector3.up * CalibrationInfo.FloorHeight + CalibrationInfo.SensorOrientation * skeleton.GetJoint(nuitrack.JointType.LeftHand).Position;
                    pointerObject.position = leftHandPos;
                }
            }

            //Debug.Log ("STICK VALUE: " + x + " " + y);
        }

        void ButtonsCallback(int buttonID, int eventID)
        {
            if (OnPressed != null)
                OnPressed(buttonID, eventID);
        }

        void ControllerCalibrationCallback(int handID, float progress)
        {
            if (OnCalibration != null)
                OnCalibration(handID, progress);

            if (handID == -1) //Controller not found
            {
                Destroy(gameObject);
            }

            if (handID < 0 || progress < 0.001f)
            {
                return;
            }

            if (progress > 99.999f)
            {
                hand = handID;
            }

            if (handID % 2 == 0)
            {
                if (!attachToSkelJoint)
                {
                    rightHandRotator.transform.rotation = Quaternion.identity;
                    rightHandRotator.enabled = true;
                    leftHandRotator.enabled = false;
                    pointerObject.SetParent(rightHand, false);
                    ResetPosition();
                }
            }
            else
            {
                if (!attachToSkelJoint)
                {
                    leftHandRotator.transform.rotation = Quaternion.identity;
                    rightHandRotator.enabled = false;
                    leftHandRotator.enabled = true;
                    pointerObject.SetParent(leftHand, false);
                    ResetPosition();
                }
            }
        }

        void ResetPosition()
        {
            pointerObject.localPosition = Vector3.zero;
            pointerObject.localRotation = Quaternion.identity;
        }

        //Search Controllers
        void DoBLEScan(Quaternion rot)
        {
            Debug.Log("Unity BLESCANNING");
            nuitrack.PublicNativeImporter.nuitrack_doBLEScanning();
        }
    }
}
