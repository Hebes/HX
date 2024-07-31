//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright ?2011-2017 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using System;

/// <summary>
/// Helper class containing generic functions used throughout the UI library.
/// </summary>
static public class NGUITools
{
    [System.NonSerialized] static AudioListener mListener;


    static bool mLoaded = false;
    static float mGlobalVolume = 1f;

    static float mLastTimestamp = 0f;
    static AudioClip mLastClip;


    /// <summary>
    /// Destroy the specified object, immediately if in edit mode.
    /// </summary>
    public static void Destroy(UnityEngine.Object obj)
    {
        if (obj)
        {
            if (obj is Transform)
            {
                Transform t = (obj as Transform);
                GameObject go = t.gameObject;

                if (Application.isPlaying)
                {
                    t.parent = null;
                    UnityEngine.Object.Destroy(go);
                }
                else UnityEngine.Object.DestroyImmediate(go);
            }
            else if (obj is GameObject)
            {
                GameObject go = obj as GameObject;
                Transform t = go.transform;

                if (Application.isPlaying)
                {
                    t.parent = null;
                    UnityEngine.Object.Destroy(go);
                }
                else UnityEngine.Object.DestroyImmediate(go);
            }
            else if (Application.isPlaying) UnityEngine.Object.Destroy(obj);
            else UnityEngine.Object.DestroyImmediate(obj);
        }
    }


    /// <summary>
    /// Activate the specified object and all of its children.
    /// </summary>
    static void Activate(Transform t, bool compatibilityMode)
    {
        SetActiveSelf(t.gameObject, true);

        if (compatibilityMode)
        {
            // If there is even a single enabled child, then we're using a Unity 4.0-based nested active state scheme.
            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                if (child.gameObject.activeSelf) return;
            }

            // If this point is reached, then all the children are disabled, so we must be using a Unity 3.5-based active state scheme.
            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                Activate(child, true);
            }
        }
    }


    /// <summary>
    /// Unity4 has changed GameObject.active to GameObject.SetActive.
    /// </summary>
    [System.Diagnostics.DebuggerHidden]
    [System.Diagnostics.DebuggerStepThrough]
    static public void SetActiveSelf(GameObject go, bool state)
    {
        go.SetActive(state);
    }


    static int mSizeFrame = -1;
    static Func<Vector2> s_GetSizeOfMainGameView;
    [System.NonSerialized] static bool mCheckedMainViewFunc = false;

    static GameObject mGo;

    static ColorSpace mColorSpace = ColorSpace.Uninitialized;
}