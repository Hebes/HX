
/*--------�ű�����-----------

����:
	�¼��ӿ�

-----------------------*/

using System.Collections.Generic;

namespace Core
{
    public interface IEvent : IID
    {
        /// <summary>
        /// ��������
        /// </summary>
        public string MethodName { get; set; }
    }
}

