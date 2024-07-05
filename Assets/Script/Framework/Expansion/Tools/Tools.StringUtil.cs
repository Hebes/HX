using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using zhaorh.UI;

// <summary>
// @Author: zrh
// @Date: 2022,12,06,16:52
// @Description:
// </summary>

namespace zhaorh
{
    public partial class Tools
    {
        /// <summary>
        /// 设置字体颜色
        /// </summary>
        /// <returns>The to color.</returns>
        /// <param name="hex">Hex.</param>
        public static Color HexToColor (string hex)
        {
            hex = hex.Replace ("0x", "");//如果字符串已格式化 0xFFFFFF
            hex = hex.Replace ("#", "");//如果字符串已格式化 #FFFFFF
            byte a = 255;//假设完全可见，除非以十六进制指定
            byte r = byte.Parse (hex.Substring (0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse (hex.Substring (2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse (hex.Substring (4, 2), System.Globalization.NumberStyles.HexNumber);
            //仅当字符串有足够的字符时才使用alpha
            if (hex.Length == 8) {
                a = byte.Parse (hex.Substring (6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32 (r, g, b, a);
        }
        /// <summary>
        /// 竖线和逗号分割得到二维数组,竖线是一级分隔符号，逗号是二级分隔符号
        /// </summary>
        /// <returns>The int32 array2d.</returns>
        /// <param name="str">String.</param>
        public static int[][] ToInt32Array2d (string str)
        {
	        if (string.IsNullOrEmpty (str) || "-1".Equals (str.Trim ())) {
		        return new int[0][];
	        }
	        str = str.Trim ().Replace ("｜", "|").Replace ("，", ",").Replace ("；", ",").Replace (";", ",");
	        string[] array = str.Split (new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
	        var res = new int[array.Length][];
	        for (var i = 0; i < array.Length; ++i) {
		        res [i] = ToInt32Array (array [i]);
	        }
	        return res;
        }
        /// <summary>
        /// 用来打印list信息
        /// </summary>
        /// <param name="intArray"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ToString<T> (List<T> intArray)
        {
            if (intArray == null) {
                return "NULL";
            }
            StringBuilder sb = new StringBuilder ();
            for (int i = 0; i < intArray.Count; i++) {
                if (i > 0) {
                    sb.Append (",");
                }
                sb.Append (intArray [i]);
            }
            return sb.ToString ();
        }
        /// <summary>
        /// 整数数组转字符串
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="intArray">Int array.</param>
        public static string ToString (int[] intArray)
        {
	        if (intArray == null) {
		        return "NULL";
	        }
	        StringBuilder sb = new StringBuilder ();
	        for (int i = 0; i < intArray.Length; i++) {
		        if (i > 0) {
			        sb.Append (",");
		        }
		        sb.Append (intArray [i]);
	        }
	        return sb.ToString ();
        }
        /// <summary>
        /// 长整形数组转字符串
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="longArray">Long array.</param>
        public static string ToString (long[] longArray)
        {
	        if (longArray == null) {
		        return "NULL";
	        }
	        StringBuilder sb = new StringBuilder ();
	        for (int i = 0; i < longArray.Length; i++) {
		        if (i > 0) {
			        sb.Append (",");
		        }
		        sb.Append (longArray [i]);
	        }
	        return sb.ToString ();
        }
        public static string ToString (string[] strArray)
        {
	        if (strArray == null) {
		        return "NULL";
	        }
	        StringBuilder sb = new StringBuilder ();
	        for (int i = 0; i < strArray.Length; i++) {
		        if (i > 0) {
			        sb.Append (",");
		        }
		        sb.Append (strArray [i]);
	        }
	        return sb.ToString ();
        }
        public static int toInt (string str, int def = 0)
        {
	        int val = def;
	        int.TryParse (str, out val);
	        return val;
        }
        /// <summary>
        /// 字符串按逗号分割为 float 数组，兼容全半角逗号及分号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float[] ToSingleArray (string str)
        {
            if (string.IsNullOrEmpty (str) || "-1".Equals (str.Trim ())) {
                return new float[0];
            }
            str = str.Trim ().Replace ("，", ",").Replace ("；", ",").Replace (";", ",");
            string[] array = str.Split (new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            float[] res = new float[array.Length];
            try {
                for (var i = 0; i < res.Length; ++i){
                    res [i] = float.Parse (array [i]);
                }
            } catch (Exception ex) {
                Debug.LogException (ex);
                Debug.LogErrorFormat ("{0} 不是一个float[]类型", str);
            }
            return res;
        }
        public static string toXML<T> (T toSerialize)
        {
            if (toSerialize == null) {
                return "NULL";
            }
            if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.IPhonePlayer){
                return toSerialize.GetType().FullName;
            }
            try {
                XmlSerializer xmlSerializer = new XmlSerializer (toSerialize.GetType ());

                using (StringWriter textWriter = new StringWriter ()) {
                    xmlSerializer.Serialize (textWriter, toSerialize);
                    return textWriter.ToString ();
                }
            } catch (Exception ex) {
                return "toXML|err|for|" + toSerialize.GetType () + "|errInfo:" + ex;
            }
        }
        public static float toFloat (string str, float def = 0.0f)
        {
            float val = def;
            float.TryParse (str, out val);
            return val;
        }
        /// <summary>
        /// 字符串按逗号分割为 int 数组，兼容全半角逗号及分号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] ToInt32Array (string str)
        {
            if (string.IsNullOrEmpty (str) || "-1".Equals (str.Trim ())) {
                return new int[0];
            }
            str = str.Trim ().Replace ("，", ",").Replace ("；", ",").Replace (";", ",");
            string[] array = str.Split (new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            int[] res = new int[array.Length];
            try {
                for (int i = 0; i < res.Length; ++i){
                    res [i] = int.Parse (array [i]);
                }
            } catch (Exception ex) {
                Debug.LogException (ex);
                Debug.LogErrorFormat ("{0} 不是一个int[]类型{1}", str,res.Length);
                //throw;
            }
            return res;
        }
        /// <summary>
        /// 字符串按逗号分割为 long 数组 ，兼容全半角逗号及分号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long[] ToInt64Array (string str)
        {
	        if (string.IsNullOrEmpty (str) || "-1".Equals (str.Trim ())) {
		        return new long[0];
	        }
	        str = str.Trim ().Replace ("，", ",").Replace ("；", ",").Replace (";", ",");
	        string[] array = str.Split (new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
	        long[] res = new long[array.Length];
	        try {
		        for (var i = 0; i < res.Length; ++i){
			        res [i] = long.Parse (array [i]);
		        }
	        } catch (Exception ex) {
		        Debug.LogException (ex);
		        Debug.LogErrorFormat ("{0} 不是一个long[]类型", str);
	        }
	        return res;
        }
        /// <summary>
		/// 尝试parse2统一富文本。
		/// </summary>
		/// <returns>The parse2 unity rich text.</returns>
		/// <param name="str">String.</param>
		public static string TryParse2UnityRichText (string str)
		{
			if (string.IsNullOrEmpty (str) || !str.Contains ("#") || str.Length < 4) {
				return str;
			}
			for(int i=0;i<10;i++){
				str = str.Replace("{"+i+"}","[[["+i+"]]]");
			}
			string richSpaceStr = "<color=$ffffff00>k</color>";
			string richSpaceStrReal = "<color=#ffffff00>k</color>";
			if(str.Contains(" ")){//为避免空格导致排版换行，用富文本来潜规则处理
				//Debug.LogError("文本内容有空格" + str);
				str = str.Replace(" ", richSpaceStr);
			}
			//至少井号加一位颜色加字符，才有可能是需要转换颜色的文本，因此文本长度至少是3
			string[] arr = str.Split ('#');
			int startIndex = 1;
			if (str.StartsWith ("#")) {
				startIndex = 0;
			}
			for (int i = startIndex; i < arr.Length; i++) {
				string tmp = arr [i];
				if (tmp.Length < 4) {
					continue;
				}
				int aa = tmp.IndexOf ("{");
				int bb = tmp.IndexOf ("}");
				if (aa > 0 && bb > aa) {//是 #Y{汉字} 这样的格式，必须有大括号，才需要转换
					string yanseStr = tmp.Substring (0, aa);

					//Debug.LogError ("|TryParse2UnityRichText颜色是" + yanseStr + "|文本是|" + tmp + "|总文本是" + str);
					//<color=yellow>RICH</color> 
					if (yanseStr.Length == 1) {						
						if ("GBPOR".Contains (yanseStr)) {
							StringBuilder sb = new StringBuilder (tmp.Length);
							switch (yanseStr) {
							case "G"://绿色
								{
									sb.Append ("<color=#00ff00ff>");
										sb.Append (tmp.Substring (aa + 1, bb - aa - 1).Replace(richSpaceStr," "));
									sb.Append ("</color>");
									sb.Append (tmp.Substring (bb + 1));
									break;
								}
							case "B"://蓝色
								{
									sb.Append ("<color=#0000ffff>");
										sb.Append (tmp.Substring (aa + 1, bb - aa - 1).Replace(richSpaceStr," "));
									sb.Append ("</color>");
									sb.Append (tmp.Substring (bb + 1));
									break;
								}
							case "P"://紫色
								{
									sb.Append ("<color=#ff00ffff>");
										sb.Append (tmp.Substring (aa + 1, bb - aa - 1).Replace(richSpaceStr," "));
									sb.Append ("</color>");
									sb.Append (tmp.Substring (bb + 1));
									break;
								}
							case "O"://橙色
								{
									sb.Append ("<color=#ffcc00ff>");
										sb.Append (tmp.Substring (aa + 1, bb - aa - 1).Replace(richSpaceStr," "));
									sb.Append ("</color>");
									sb.Append (tmp.Substring (bb + 1));
									break;
								}
							case "R"://红色
								{
									sb.Append ("<color=#ff0000ff>");
										sb.Append (tmp.Substring (aa + 1, bb - aa - 1).Replace(richSpaceStr," "));
									sb.Append ("</color>");
									sb.Append (tmp.Substring (bb + 1));
									break;
								}
							default:
								{
									//不转换
									break;
								}
							}
							if (sb.Length > 0) {
								string newTmp = sb.ToString ();
								//Debug.LogError ("TryParse2UnityRichText|" + i + "|" + tmp + "|转换后得到|" + newTmp);
								arr [i] = newTmp;
							}

						} else {
							//不是大些字母颜色的，则跳过
						}

					} else if (IsHexString (yanseStr) && (yanseStr.Length == 6 || yanseStr.Length == 8)) {
						if (yanseStr.Length == 6) {
							StringBuilder sb = new StringBuilder (tmp.Length);

							sb.Append ("<color=#").Append (yanseStr).Append ("ff>");
							sb.Append (tmp.Substring (aa + 1, bb - aa - 1).Replace(richSpaceStr," "));
							sb.Append ("</color>");
							sb.Append (tmp.Substring (bb + 1));
							string newTmp = sb.ToString ();
							//Debug.LogError (tmp + "|转换后得到|" + newTmp);
							arr [i] = newTmp;
						}
						if (yanseStr.Length == 8) {
							StringBuilder sb = new StringBuilder (tmp.Length);
							sb.Append ("<color=#").Append (yanseStr).Append (">");
							sb.Append (tmp.Substring (aa + 1, bb - aa - 1).Replace(richSpaceStr," "));
							sb.Append ("</color>");
							sb.Append (tmp.Substring (bb + 1));
							string newTmp = sb.ToString ();
							//Debug.LogError (tmp + "|转换后得到|" + newTmp);
							arr [i] = newTmp;
						}
					} else {
						//也不处理
					}

				}
			}
			for (int i = startIndex; i < arr.Length; i++) {
				string tmp = arr [i];
				if (tmp.Length > 0 && !tmp.StartsWith ("<")) {
					//Debug.LogWarning (i + "|需要补回井号|" + tmp);
					arr [i] = "#" + tmp;
				}
			}
			string strNew= string.Join ("", arr);
			for(int i=0;i<10;i++){
				strNew = strNew.Replace("[[["+i+"]]]","{"+i+"}");
			}
			strNew = strNew.Replace(richSpaceStr, richSpaceStrReal);
			return strNew;
		}
        
        public static  bool IsHexString (string str)
        {
	        if (string.IsNullOrEmpty (str)) {
		        return true;
	        }
	        if (str.Length % 2 == 1) {
		        //Debug.LogError ("16进制字符串的长度必须是2的倍数|" + str.Length);
		        return false;
	        }
	        for (var i = 0; i < str.Length; i++) {
		        var current = str [i];
		        if (!(Char.IsDigit (current) || (current >= 'a' && current <= 'f') || (current >= 'A' && current <= 'F'))) {
			        //Debug.LogError ("有非法字符" + current + "|不是16进制了|" + str);
			        return false;
		        }
	        }
	        return true;
        }
        /// <summary>
        /// 字符串按逗号分割为 string 数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string[] ToStringArray (string str, string def = "")
        {
	        if (string.IsNullOrEmpty (str) || "-1".Equals (str.Trim ())) {
		        return new string[0];
	        }

	        str = str.Trim().Replace("，", ",").Replace("；", ",").Replace(";", ",");

	        string[] arr= str.Split (',');
	        for(int i=0;i< arr.Length;i++){
		        arr [i] = TryParse2UnityRichText (arr[i]);
	        }
	        return arr;
        }
        /// <summary>
        /// 输入形如 x;y|a;b 的字符文本，获取对应的 set[x] -> y; set[a] -> b 键值映射列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<KeyValuePair<int, int>> ToListOfKeyPair (string str)
        {
	        var list = new List<KeyValuePair<int, int>> ();
	        if (string.IsNullOrEmpty (str)) {
		        return list;
	        }

	        str = str.Trim ().Replace ("｜", "|").Replace ("，", ",").Replace ("；", ",").Replace (";", ",");
	        var array = str.Split ('|');

	        for (var i = 0; i < array.Length; ++i) {
		        var itemSet = array [i].Split (',');
		        list.Add (new KeyValuePair<int, int> (itemSet [0].ToInteger (), itemSet [1].ToInteger ()));
	        }
	        return list;
        }
        /// <summary>
        /// 替换括号中的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="bracketsStr"></param>
        /// <returns></returns>
        public static string ReplaceBracketsStr(string str, string bracketsStr)
        {
	        if (str.IndexOf("(") > 0)
	        { 
		        int count;
		        string subStr = "";
		        string result = "";
		        count = str.IndexOf(")") - str.IndexOf("(");
		        subStr = str.Substring(str.IndexOf("(") + 1, count - 1);
		        result = str.Replace(subStr, bracketsStr);
		        result = result.Replace("(Clone)", "");
		        return result;
	        }
	        else
	        {
		        return str;
	        }
        }
        
        public static string ObjectArrayToString (params object[] args)
        {
	        if (args == null) {
		        return "NULL";
	        }
	        StringBuilder sb = new StringBuilder ();
	        for (int i = 0; i < args.Length; i++) {
		        if (i > 0) {
			        sb.Append (",");
		        }
		        sb.Append (args [i]);
	        }
	        return sb.ToString ();
        }
        private static string bytes2Hex (byte[] ba)
        {
	        string hex = BitConverter.ToString (ba);
	        return hex.Replace ("-", "");
        }
        private static byte[] hex2Bytes (string input)
        {
	        if (string.IsNullOrEmpty (input))
		        return null;
	        var offset = input.Length % 2;
	        if (offset == 1)
		        input = "0" + input;
	        int i;
	        var list = new List<byte> ();
	        for (i = 0; i < input.Length; i += 2) {
		        var temp = input.Substring (i, 2);
		        byte bv;
		        var success = byte.TryParse (temp, System.Globalization.NumberStyles.HexNumber, null, out bv);
		        if (!success)
			        throw new ArgumentOutOfRangeException ();
		        list.Add (bv);
	        }
	        return list.ToArray ();
        }
        
        /// <summary>
        ///竖线和逗号分割得到二维数组,竖线是一级分隔符号，逗号是二级分隔符号
        /// </summary>
        /// <returns>The int64 array2d.</returns>
        /// <param name="str">String.</param>
        public static long[][] ToInt64Array2d (string str)
        {
	        if (string.IsNullOrEmpty (str) || "-1".Equals (str.Trim ())) {
		        return new long[0][];
	        }
	        str = str.Trim ().Replace ("｜", "|").Replace ("，", ",").Replace ("；", ",").Replace (";", ",");
	        string[] array = str.Split (new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
	        var res = new long[array.Length][];
	        for (var i = 0; i < array.Length; ++i) {
		        res [i] = ToInt64Array (array [i]);
	        }
	        return res;
        }
    }
}