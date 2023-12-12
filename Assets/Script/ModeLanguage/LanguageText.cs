using UnityEngine;
using UnityEngine.UI;

/*--------脚本描述-----------

描述:
	多语言组件

-----------------------*/

    public class LanguageText : MonoBehaviour
    {
        public string key = "错误";
        private Text m_MeshText;

        private void Awake()
        {
            m_MeshText = GetComponent<Text>();
        }

        private void OnEnable()
        {
            OnSwitchLanguage();
            ManagerLanguage.Instance.OnLanguageChangeEvt += OnSwitchLanguage;
        }

        private void OnDisable()
        {
            ManagerLanguage.Instance.OnLanguageChangeEvt -= OnSwitchLanguage;
        }

        private void OnSwitchLanguage()
        {
            if (m_MeshText != null)
            {
                m_MeshText.text = ManagerLanguage.Instance.GetText(key);
                m_MeshText.font = ManagerLanguage.Instance.font;
            }
        }

        /// <summary>
        /// 设置Key和切换语言
        /// </summary>
        /// <param name="key">关键词</param>
        public void OnSetKeyAndChange(string key)
        {
            this.key = key;
            OnSwitchLanguage();
        }
    }
