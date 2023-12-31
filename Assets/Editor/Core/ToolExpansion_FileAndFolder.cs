﻿using System.Collections.Generic;
using System.IO;
using Unity.CodeEditor;
using UnityEditor;
using UnityEngine;

namespace Tool
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public static class ToolExpansion_FileAndFolder
    {

        //***************************文件***************************
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void FileDelete(this string filePath)
        {
            File.Delete(filePath);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void FileCreat(this string filePath)
        {
            if (File.Exists(filePath)) return;
            Debug.Log("文件不存在,开始创建!");
            File.Create(filePath);
            ToolExpansion_AssetDatabase.Refresh();
        }

        /// <summary>
        /// 检查文件
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        public static bool FileChack(this string filePath)
        {
            return File.Exists(filePath);//是否存在这个文件
        }

        /// <summary>
        /// 生成文件并写入内容
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        public static void CreatCSharpScript(string folderPath, string fileName, string content)
        {
            //创建并写入内容
            string filePath = $"{folderPath}/{fileName}";
            if (!File.Exists(filePath))
            {
                Debug.Log("文件不存在,进行创建...");
                using (StreamWriter writer = File.CreateText(filePath))//生成文件
                {
                    writer.Write(content);
                    Debug.Log("内容写入成功!");
                }
            }
            folderPath.ACAssetDatabaseRefresh();
        }

        /// <summary>
        /// 文件以追加写入的方式
        /// https://wenku.baidu.com/view/a8fdb767fd4733687e21af45b307e87100f6f85b.html
        /// 显示IO异常请在创建文件的时候Close下
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">内容</param>
        private static void FileWriteContent(this string path, string content)
        {
            byte[] myByte = System.Text.Encoding.UTF8.GetBytes(content);
            using (FileStream fsWrite = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                fsWrite.Write(myByte, 0, myByte.Length);
            }
        }

        /// <summary>
        /// 生成文件并写入内容
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        public static void CreatScript(string folderPath, string fileName, string content)
        {
            //创建并写入内容
            string filePath = $"{folderPath}/{fileName}";
            if (!File.Exists(filePath))
            {
                Debug.Log("文件不存在,进行创建...");
                using (StreamWriter writer = File.CreateText(filePath))//生成文件
                {
                    writer.Write(content);
                    Debug.Log("内容写入成功!");
                }
            }
            folderPath.ACAssetDatabaseRefresh();
        }

        /// <summary>
        /// 创建文件并写入
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="str"></param>
        public static void CreateFileText(this string filePath, string str)
        {
            using (StreamWriter writer = File.CreateText(filePath)) { writer.Write(str); Debug.Log("内容写入成功!"); }
        }

        //***************************文件夹***************************
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static void ACFolderCreat(this string folderPath)
        {
            if (Directory.Exists(folderPath)) return;
            Debug.Log("文件不存在,开始创建!");
            Directory.CreateDirectory(folderPath);//创建
            ToolExpansion_AssetDatabase.Refresh();
        }


        /// <summary>
        /// 通过路径检文件夹是否存在，如果不存在则创建
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        public static bool ACFolderChack(this string folderPath)
        {
            return Directory.Exists(folderPath);//是否存在这个文件
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="folderPath"></param>
        public static void OpenFolder(string folderPath)
        {
            System.Diagnostics.Process.Start(folderPath);
        }

        //***************************TXT***************************

        /// <summary>
        /// txt读取
        /// </summary>
        /// <param name="txtPath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> TxtRead(this string txtPath)
        {
            Dictionary<string, string> DesDic = new Dictionary<string, string>();
            FileStream fileStream = File.Open(txtPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            StreamReader reader = new StreamReader(fileStream);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] strings = line.Split(',');
                DesDic.Add(strings[0], strings[1]);
            }
            reader.Close();
            fileStream.Close();
            return DesDic;
        }

        //***************************其他***************************

        /// <summary>
        /// 开打.sln路径,
        /// </summary>
        public static void ACOSOpenFile(this string path)
        {
            CodeEditor.OSOpenFile(CodeEditor.CurrentEditorInstallation, Path.Combine(Application.dataPath, path));
        }

        /// <summary>
        /// 打开路径
        /// </summary>
        /// <param name="folderPath">路径</param>
        public static void ACOpenPath(this string folderPath)
        {
            if (!Directory.Exists(folderPath)) return;
            EditorUtility.RevealInFinder(folderPath);
        }


        /// <summary>
        /// 删除文件夹
        /// </summary>
        public static void ACFolderDelete(this string folderPath)
        {
            Directory.Delete(folderPath);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public static void ACFolderDelete(this string folderPath, bool recursive)
        {
            Directory.Delete(folderPath, recursive);
        }

        public static string[] ACFolderGetFiles(this string folderPath)
        {
            return Directory.GetFiles(folderPath);
        }    }

    //****************************打开程序***************************

    //System.Diagnostics.Process.Start("explorer.exe", Application.persistentDataPath);
}
