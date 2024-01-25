using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
{
   

    public class InputAndroid : IInput
    {
        public void Init()
        {
            LoadPrefab().Forget();
        }

        public async UniTask LoadPrefab()
        {
            GameObject gameObject = await CoreResource.LoadAsync<GameObject>(SettingCore.jpyStickPanelPath);
            GameObject gameObject1= gameObject.Instantiate();
            gameObject1.AddComponent<JpyStickPanelView>();
        }
    }
}
