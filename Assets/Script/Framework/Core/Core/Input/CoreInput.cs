using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Framework.Core;

namespace Framework.Core
{
    /// <summary>
    /// 按键事件
    /// </summary>
    public enum EInputKeyBoardType
    {
        Attack,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
    }

    [CreateCore(typeof(CoreInput), 9)]
    public class CoreInput : ICore
    {
        public static CoreInput Instance { get; private set; }
        private List<IInput> _inputList;        //组合输入


        public void Init()
        {
            Instance = this;
            _inputList = new List<IInput>();
        }

        public IEnumerator AsyncEnter()
        {
            yield return null;
        }

        public IEnumerator Exit()
        {
            yield break;
        }


        public static void AddInputType(IInput input)
        {
            Instance._inputList.AddNotContainElement(input);
            input.Init();
        }

        public T GetInputType<T>() where T : IInput
        {
            return Instance._inputList.GetContainElement<IInput, T>();
        }

        
    }
}
