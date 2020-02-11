using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MyPublicId
{
    public class RequestHandler : SingletonMonoBehaviour<RequestHandler>
    {
        public delegate void OnProcessRequestComplete(UnityWebRequest request, string response, bool bSucceeded);

        private static string buildQueryString(Dictionary<string, string> data)
        {
            string ret = "";
            bool isFirst = true;
            foreach(KeyValuePair<string,string> elem in data)
            {
                if (!isFirst)
                    ret += "&";
                else
                    isFirst = false;
                ret += elem.Key + "=" + elem.Value;
            }
            return ret;
        }

        public static UnityWebRequest SendRequest(string action, string method, Dictionary<string, string> data, OnProcessRequestComplete onComplete)
        {
            return RequestHandler.Instance.InternalSendRequest(action, method, data, onComplete);
        }

        private UnityWebRequest InternalSendRequest(string action, string method, Dictionary<string, string> data, OnProcessRequestComplete onComplete)
        {
            var request = new UnityWebRequest(MyPublicIdSettings.instance.endpoint + action, method);
            if (method != "GET")
            {
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(buildQueryString(data)));
                request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            }
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("x-api-key", MyPublicIdSettings.instance.authenticationToken);

            StartCoroutine(WaitForRequest(request, onComplete));

            return request;
        }
        
        private static IEnumerator WaitForRequest(UnityWebRequest request, OnProcessRequestComplete onComplete) {
            yield return request.SendWebRequest();
            
            if (!request.isNetworkError && !request.isHttpError) {
                onComplete(request, request.downloadHandler.text, true);
            } else if (!request.isNetworkError) {
                onComplete(request, request.downloadHandler.text, false);
            } else {
                onComplete(request, request.error, false);
            }
        }
    }
}
