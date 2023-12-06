using UnityEngine;
using UnityEngine.UI;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	Image拓展

-----------------------*/

namespace Core
{
    public static class ExpansionImage
    {
        public static Image GetImage(this GameObject gameObject)
        {
            return gameObject.GetComponent<Image>();
        }

        public static Image GetImage(this Transform transform)
        {
            return transform.GetComponent<Image>();
        }
    }
}
