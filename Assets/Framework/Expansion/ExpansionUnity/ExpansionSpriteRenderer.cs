﻿using UnityEngine;

namespace Framework.Core
{
    public static  class ExpansionSpriteRenderer
    {

        public static SpriteRenderer GetSpriteRenderer(this GameObject gameObject)
        {
            return gameObject.GetComponent<SpriteRenderer>();
        }
        public static SpriteRenderer GetSpriteRenderer(this Transform transform)
        {
            return transform.GetComponent<SpriteRenderer>();
        }
    }
}
