using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace ZB.TextFile
{
    public enum AccessStyle
    {
        /// <summary>
        /// �б�, ���� ���� (PC, �����) / ���� �ܺο� ����������
        /// </summary>
        PersistentDataPath,

        /// <summary>
        /// //�б�, ���� ���� (PC) / ���� ���ο� ����������
        /// </summary>
        DataPath
    }

    public class TXTUtility
    {
        public static T Json_2_T<T>(string stringData)
        {
            T jsonData = JsonUtility.FromJson<T>(stringData);
            return jsonData;
        }
        public static string T_2_Json<T>(T jsonData)
        {
            string stringData = JsonUtility.ToJson(jsonData, true);
            return stringData;
        }

        public static string Read(string path, AccessStyle accessStyle)
        {
            string fullPath = Path.Combine(GetBasePath(accessStyle), path);

            if (File.Exists(fullPath))
            {
                return File.ReadAllText(fullPath);
            }

            Debug.LogError("ZB.TextFile.Parser.Read / ����Է¿��� / " + fullPath);
            return null;
        }
        public static string[] ReadAll(string path, AccessStyle accessStyle)
        {
            string fullPath = Path.Combine(GetBasePath(accessStyle), path);

            if (File.Exists(fullPath))
            {
                return File.ReadAllLines(fullPath);
            }

            Debug.LogError("ZB.TextFile.Parser.Read / ����Է¿��� / " + fullPath);
            return null;
        }
        public static bool FileExists(string path, AccessStyle accessStyle)
        {
            string fullPath = Path.Combine(GetBasePath(accessStyle), path);

            return File.Exists(fullPath);
        }
        public static void Write(string path, string data, AccessStyle accessStyle = AccessStyle.PersistentDataPath, bool canOverwrite = true)
        {
            string fullPath = Path.Combine(GetBasePath(accessStyle), path);
            if (!File.Exists(fullPath) ||
                File.Exists(fullPath) && canOverwrite)
            {
                File.WriteAllText(fullPath, data);
            }
        }

        public static string GetBasePath(AccessStyle accesStyle)
        {
            string result = "";
            switch (accesStyle)
            {
                case AccessStyle.PersistentDataPath:
                    result = Application.persistentDataPath;
                    break;
                case AccessStyle.DataPath:
                    result = Application.dataPath;
                    break;
            }
            return result;
        }
    }
}