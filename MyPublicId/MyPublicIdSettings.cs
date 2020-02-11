using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPublicId
{
    [System.Serializable, CreateAssetMenu(fileName = "MyPublicIdSettings", menuName = "MyPublicId/Settings", order = 1)]
    public class MyPublicIdSettings : ScriptableObject
    {
        public string authenticationToken = "";
        public string endpoint = "https://api.mypublicid.com";

        private static MyPublicIdSettings _instance = null;
        public static MyPublicIdSettings instance { get { if (_instance == null) _instance = GetSettingAssets(); return _instance; } }

        private static string GetArg(string name)
        {
            var args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        private static MyPublicIdSettings GetSettingAssets()
        {
            var assets = Resources.LoadAll<MyPublicIdSettings>("MyPublicIdSettings");
            Debug.Assert((assets.Length == 1), "MyPublicId should a total of 1 settings object. Current count is: " + assets.Length);
            return assets[0];
        }
    }
}
