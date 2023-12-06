using UnityEngine;
using UnityEngine.UI;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	Core拓展

-----------------------*/

namespace Core
{
    public static  class ExpansionSlider
    {
        public static Slider GetSlider(this GameObject gameObject)
        {
            return gameObject.GetComponent<Slider>();
        }

        public static Slider GetSlider(this Transform transform)
        {
            return transform.GetComponent<Slider>();
        }
    }
}
