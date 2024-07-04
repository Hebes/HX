using UnityEngine;
using UnityEngine.UI;

namespace Framework.Core
{
    public static  class ExpansionInputField
    {
        public static InputField GetInputField(this GameObject gameObject)
        {
            return gameObject.GetComponent<InputField>();
        }
        public static InputField GetInputField(this Transform transform)
        {
            return transform.GetComponent<InputField>();
        }
    }
}
