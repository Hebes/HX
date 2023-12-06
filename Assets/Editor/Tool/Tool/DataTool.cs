using System.Collections.Generic;
using System.IO;

namespace ACEditor
{
    public static class DataTool
    {
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
    }
}
