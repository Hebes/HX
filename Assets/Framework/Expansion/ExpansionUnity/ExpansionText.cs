﻿using UnityEngine;
using UnityEngine.UI;

namespace Framework.Core
{
    public static class ExpansionText
    {
        public static Text GetText(this GameObject gameObject)
        {
            return gameObject.GetComponent<Text>();
        }
        public static Text GetText(this Transform transform)
        {
            return transform.GetComponent<Text>();
        }
    }
}
