using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MyPublicId
{
    namespace DataModels
    {
        [System.Serializable]
        public class FMyPublicId_Error
        {
            public int code = -1;
            public string message = "-1";
        }

        [System.Serializable]
        public class FMyPublicId_Authorize
        {
            public bool active = false;
            public int country = -1;
        }
    }
    public class API
    {
        public delegate void MyPublicId_Empty();
        public delegate void MyPublicId_Error(int ErrorCode, string ErrorMessage);

        private static void onEmpty(UnityWebRequest request, string response, bool bSucceeded, MyPublicId_Empty SuccessDelegate, MyPublicId_Error ErrorDelegate)
        {
            if (bSucceeded) {
                if (SuccessDelegate == null)
                    return;
                SuccessDelegate();
            } else {
                if (ErrorDelegate == null)
                    return;
                DataModels.FMyPublicId_Error data = JsonUtility.FromJson<DataModels.FMyPublicId_Error>(response);
                if (data.message != null) {
                    ErrorDelegate(data.code, data.message);
                } else {
                    ErrorDelegate(1000, "Unknown error occured!");
                }
            }
        }

        public delegate void MyPublicId_Authorize(bool UserActive, int CountryId);

        public static void Authorize(string tokenName, string token, MyPublicId_Authorize SuccessDelegate, MyPublicId_Error ErrorDelegate)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token_name", tokenName);
            data.Add("token", token);

            RequestHandler.SendRequest("/game/authorize", "POST", data, (UnityWebRequest request, string response, bool bSucceeded) => {
                onAuthorize(request, response, bSucceeded, SuccessDelegate, ErrorDelegate);
            });
        }

        private static void onAuthorize(UnityWebRequest request, string response, bool bSucceeded, MyPublicId_Authorize SuccessDelegate, MyPublicId_Error ErrorDelegate)
        {
            if (bSucceeded) {
                if (SuccessDelegate == null)
                    return;
                DataModels.FMyPublicId_Authorize data = JsonUtility.FromJson<DataModels.FMyPublicId_Authorize>(response);
                SuccessDelegate(data.active, data.country);
            } else {
                if (ErrorDelegate == null)
                    return;
                DataModels.FMyPublicId_Error data = JsonUtility.FromJson<DataModels.FMyPublicId_Error>(response);
                if (data.message != "-1") {
                    ErrorDelegate(data.code, data.message);
                } else {
                    ErrorDelegate(1000, "Unknown error occured!");
                }
            }
        }

        public static void Ban(string tokenName, string token, MyPublicId_Empty SuccessDelegate, MyPublicId_Error ErrorDelegate)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token_name", tokenName);
            data.Add("token", token);

            RequestHandler.SendRequest("/game/ban", "PUT", data, (UnityWebRequest request, string response, bool bSucceeded) => {
                onEmpty(request, response, bSucceeded, SuccessDelegate, ErrorDelegate);
            });
        }

        public static void UnBan(string tokenName, string token, MyPublicId_Empty SuccessDelegate, MyPublicId_Error ErrorDelegate)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token_name", tokenName);
            data.Add("token", token);

            RequestHandler.SendRequest("/game/ban", "DELETE", data, (UnityWebRequest request, string response, bool bSucceeded) => {
                onEmpty(request, response, bSucceeded, SuccessDelegate, ErrorDelegate);
            });
        }
    }
}