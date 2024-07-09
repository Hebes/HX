using System.Collections;

namespace Framework.Core
{
    public interface IBehaviour
    {

    }

    public interface IUpdate : IBehaviour
    {
        void CoreUpdate();
    }
    
    public interface IFixedUpdate : IBehaviour
    {
        public void OnFixedUpdate();
    }
}
