﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.Management;
using UnityEngine.XR;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Unity.XR.Oculus
{
    public class OculusLoader : XRLoaderHelper
#if UNITY_EDITOR
    , IXRLoaderPreInit
#endif
    {
        private static List<XRSessionSubsystemDescriptor> s_SessionSubsystemDescriptors = new List<XRSessionSubsystemDescriptor>();
        private static List<XRDisplaySubsystemDescriptor> s_DisplaySubsystemDescriptors = new List<XRDisplaySubsystemDescriptor>();
        private static List<XRInputSubsystemDescriptor> s_InputSubsystemDescriptors = new List<XRInputSubsystemDescriptor>();
        private static List<XRExperienceSubsystemDescriptor> s_ExperienceSubsystemDescriptors = new List<XRExperienceSubsystemDescriptor>();

        public XRSessionSubsystem sessionSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRSessionSubsystem>();
            }
        }

        public XRDisplaySubsystem displaySubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRDisplaySubsystem>();
            }
        }

        public XRInputSubsystem inputSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRInputSubsystem>();
            }
        }

        public XRExperienceSubsystem experienceSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRExperienceSubsystem>();
            }
        }

        public override bool Initialize()
        {
            OculusSettings settings = GetSettings();
            if (settings != null)
            {
                UserDefinedSettings userDefinedSettings;
                userDefinedSettings.sharedDepthBuffer = (ushort)(settings.SharedDepthBuffer ? 1 : 0);
                userDefinedSettings.dashSupport = (ushort)(settings.DashSupport ? 1 : 0);
                userDefinedSettings.stereoRenderingMode = (ushort) settings.GetStereoRenderingMode();
                SetUserDefinedSettings(userDefinedSettings);
            }


            CreateSubsystem<XRSessionSubsystemDescriptor, XRSessionSubsystem>(s_SessionSubsystemDescriptors, "oculus session");
            CreateSubsystem<XRDisplaySubsystemDescriptor, XRDisplaySubsystem>(s_DisplaySubsystemDescriptors, "oculus display");
            CreateSubsystem<XRInputSubsystemDescriptor, XRInputSubsystem>(s_InputSubsystemDescriptors, "oculus input");
            CreateSubsystem<XRExperienceSubsystemDescriptor, XRExperienceSubsystem>(s_ExperienceSubsystemDescriptors, "oculus experience");


            Debug.Log(displaySubsystem);
            Debug.Log(inputSubsystem);
            Debug.Log(sessionSubsystem);
            Debug.Log(experienceSubsystem);


            return sessionSubsystem != null && displaySubsystem != null && inputSubsystem != null && experienceSubsystem != null;
        }

        public override bool Start()
        {
            StartSubsystem<XRSessionSubsystem>();
            StartSubsystem<XRDisplaySubsystem>();
            StartSubsystem<XRInputSubsystem>();
            StartSubsystem<XRExperienceSubsystem>();

            return true;
        }

        public override bool Stop()
        {
            StopSubsystem<XRSessionSubsystem>();
            StopSubsystem<XRDisplaySubsystem>();
            StopSubsystem<XRInputSubsystem>();
            StopSubsystem<XRExperienceSubsystem>();


            return true;

        }

        public override bool Deinitialize()
        {
            DestroySubsystem<XRSessionSubsystem>();
            DestroySubsystem<XRDisplaySubsystem>();
            DestroySubsystem<XRInputSubsystem>();
            DestroySubsystem<XRExperienceSubsystem>();

            return true;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct UserDefinedSettings
        {
            public ushort sharedDepthBuffer;
            public ushort dashSupport;
            public ushort stereoRenderingMode;
        }

        [DllImport("XRSDKOculus", CharSet=CharSet.Auto)]
        static extern void SetUserDefinedSettings(UserDefinedSettings settings);

        public OculusSettings GetSettings()
        {
            OculusSettings settings = null;
#if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.TryGetConfigObject<OculusSettings>("Unity.XR.Oculus.Settings", out settings);
#else
            settings = OculusSettings.s_Settings;
#endif
            return settings;
        }

#if UNITY_EDITOR
        public string GetPreInitLibraryName(BuildTarget buildTarget, BuildTargetGroup buildTargetGroup)
        {
            return "XRSDKOculus";
        }
#endif
    }
}
