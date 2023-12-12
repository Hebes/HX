using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    二进制文件生成

-----------------------*/

namespace ACEditor
{
    public static class BinaryData
    {
        public const string binaryDataSavePathKey = "二进制保存文件保存路径";

        /// <summary>
        /// 放置要生成的二进制文件的路径
        /// </summary>
        public static string binaryDataSavePath = string.Empty;

        /// <summary>
        /// 创建二进制文件
        /// </summary>
        public static void CreateByte(string filePath, string[][] data)
        {
            //创建文件
            Debug.Log($"当前路径是{filePath}");
            string className = new FileInfo(filePath).Name.Split('.')[0];
            binaryDataSavePath.GenerateDirectory();
            string path = $"{binaryDataSavePath}/{className}.bytes";
            //写入文件
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                //创建类型
                List<Type> types = GetTypeByFieldType(data);
                //去Byte文件写入数据
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    for (int i = (int)RowType.BEGIN_INDEX; i < data.Length; ++i)//开始读取真实数据
                    {
                        for (int j = 0; j < types.Count; ++j)
                        {
                            //获取数据的bytes
                            Type typeTemp = types[j];
                            string dataTemp = data[i][j];
                            if (string.IsNullOrEmpty(dataTemp))//跳过空的数据
                                continue;
                            byte[] bytes = GetBasicField(typeTemp, dataTemp);
                            //写入数据
                            binaryWriter.Write(bytes);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取字节
        /// </summary>
        /// <param name="type">例如List<Int>的数据在表格也当成string类型看</param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static byte[] GetBasicField(Type type, string data)
        {
            byte[] bytes = null;
            if (type == typeof(int))
                bytes = BitConverter.GetBytes(int.Parse(data));
            else if (type == typeof(float))
                bytes = BitConverter.GetBytes(float.Parse(data));
            else if (type == typeof(bool))
                bytes = BitConverter.GetBytes(bool.Parse(data));
            else if (type == typeof(string) ||
                type == typeof(List<string>) ||
                type == typeof(List<int>) ||
                type == typeof(List<float>)
                //TODO自己定义的类型
                )
            {
                byte[] dataBytes = Encoding.Default.GetBytes(data);
                List<byte> lengthBytes = BitConverter.GetBytes(dataBytes.Length).ToList();
                lengthBytes.AddRange(dataBytes);
                bytes = lengthBytes.ToArray();
            }

            //try
            //{
            //    if (type == typeof(int))
            //        bytes = BitConverter.GetBytes(int.Parse(data));
            //    else if (type == typeof(float))
            //        bytes = BitConverter.GetBytes(float.Parse(data));
            //    else if (type == typeof(bool))
            //        bytes = BitConverter.GetBytes(bool.Parse(data));
            //    else if (type == typeof(string) ||
            //        type == typeof(List<string>) ||
            //        type == typeof(List<int>) ||
            //        type == typeof(List<float>)
            //        //TODO自己定义的类型
            //        )
            //    {
            //        byte[] dataBytes = Encoding.Default.GetBytes(data);
            //        List<byte> lengthBytes = BitConverter.GetBytes(dataBytes.Length).ToList();
            //        lengthBytes.AddRange(dataBytes);
            //        bytes = lengthBytes.ToArray();
            //    }
            //}
            //finally
            //{
            //    Debug.LogError($"当前类型是：{type.Name}.{data}数据转换错误,请检查类型");
            //}


            if (bytes == null) throw new Exception($"{nameof(UnityEngine.Object.name)}.GetBasicField: 其类型未配置或不是基础类型 Type:{type} Data:{data}");
            return bytes;
        }

        private static List<Type> GetTypeByFieldType(string[][] data)
        {
            List<Type> types = new List<Type>();
            string[] temp = data[(int)RowType.FIELD_TYPE];//获取类型
            for (int i = 0; i < temp.Length; ++i)
            {
                if (temp[i] == "int") types.Add(typeof(int));
                else if (temp[i] == "bool") types.Add(typeof(bool));
                else if (temp[i] == "float") types.Add(typeof(float));
                else if (temp[i] == "string") types.Add(typeof(string));
                else if (temp[i] == "List<int>") types.Add(typeof(List<int>));
                else if (temp[i] == "List<string>") types.Add(typeof(List<string>));
                else if (temp[i] == "List<float>") types.Add(typeof(List<float>));
            }
            return types;
        }
    }
}
