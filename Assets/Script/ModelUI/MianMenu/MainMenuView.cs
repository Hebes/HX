using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FieldEdge
{
    /// <summary>
    /// 主菜单界面
    /// </summary>
    public class MainMenuView : UIBase
    {

        public override void UIAwake()
        {
            base.UIAwake();

            InitUIBase(EUIType.Fixed, EUIMode.HideOther, EUILucenyType.ImPenetrable);

            UIComponent UIComponent = panelGameObject.GetComponent<UIComponent>();

            GameObject T_StartGame = UIComponent.Get<GameObject>("T_StartGame");
            GameObject T_Load = UIComponent.Get<GameObject>("T_Load");
            GameObject T_Setting = UIComponent.Get<GameObject>("T_Setting");
            GameObject T_Exit = UIComponent.Get<GameObject>("T_Exit");

            T_StartGame.GetButton().onClick.AddListener(StartGame);
            T_Load.GetButton().onClick.AddListener(Load);
            T_Setting.GetButton().onClick.AddListener(Setting);
            T_Exit.GetButton().onClick.AddListener(Exit);
        }

        private void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
            CoreBehaviour.StopAllCoroutines();      //停止所有协程
        }

        private void Setting()
        {

        }

        private void Load()
        {

        }

        private void StartGame()
        {
            LoadScene().Forget();
        }

        private async UniTask LoadScene()
        {
            await ManagerScene.LoadSceneAsync(ConfigScenes.unityHome);
            CloseUIForm();
        }
    }
}
