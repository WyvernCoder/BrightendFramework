using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProSheep;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DictEditorWindow : EditorWindow
{
    private string dictKey1 = "#LOGIN";
    private string updateBy = "unity_editor";

    private string dictValue_1 = "登录";
    private string dictLanguage_1 = "zh";

    private string dictValue_2 = "Login";
    private string dictLanguage_2 = "en";

    private Texture2D banner;

    private void OnEnable()
    {
        banner = Resources.Load<Texture2D>("Editor/managarmr");
    }

    [MenuItem("Brightend Tools/Dict Utility")]
    public static void ShowWindow()
    {
        GetWindow<DictEditorWindow>("Dict Utility");
    }

    async void OnGUI()
    {
        GUILayout.Label(banner, GUILayout.Height(100), GUILayout.ExpandWidth(true));

        GUILayout.Space(5);

        dictKey1 = EditorGUILayout.TextField("Dict Key", dictKey1);
        updateBy = EditorGUILayout.TextField("Update By", updateBy);

        GUILayout.Space(5);

        dictValue_1 = EditorGUILayout.TextField("Dict Value 1", dictValue_1);
        dictLanguage_1 = EditorGUILayout.TextField("Dict Language", dictLanguage_1);


        GUILayout.Space(5);
        dictValue_2 = EditorGUILayout.TextField("Dict Value 2", dictValue_2);
        dictLanguage_2 = EditorGUILayout.TextField("Dict Language", dictLanguage_2);

        GUILayout.Space(10);

        if (GUILayout.Button("Add Entry"))
        {
            await AddEntry(dictKey1, new string[] { dictValue_1, dictValue_2 },
                new string[] { dictLanguage_1, dictLanguage_2 }, updateBy);
        }

        // TODO: Update Entry
        // if (GUILayout.Button("Update Entry"))
        // {
        //     //UpdateEntry(new string[], dictValue1, dictLanguage1, updateBy);
        // }
    }

    // Example stub functions — replace with your backend call logic
    private async Task AddEntry(string key, string[] value, string[] language, string updater)
    {
        var body = JArray.Parse(
            @"[{""dictLabel"":""DEF"",""dictValue"":""#DEF"",""dictType"":""language"",""dictLanguage"":""zh"",""status"":""0"",""createBy"":""unity_editor""},{""dictLabel"":""DEF"",""dictValue"":""#DEF"",""dictType"":""language"",""dictLanguage"":""en"",""status"":""0"",""createBy"":""unity_editor""}]");
        body[0]["dict_label"] = value[0];
        body[0]["dict_value"] = key;
        body[0]["dict_language"] = language[0];
        body[0]["create_by"] = updater;
        body[1]["dict_label"] = value[1];
        body[1]["dict_value"] = key;
        body[1]["dict_language"] = language[1];
        body[1]["create_by"] = updater;

        await SendRequestEditor("v1/dict/add", Brightend.RequestType.POST, body.ToString());
    }

    private static async Task SendRequestEditor(string endpoint, Brightend.RequestType type, string value = "")
    {
        // MAKING INITIAL URL
        var url = FreeDownloadManager.ServerURL;
        url = new Uri(url, endpoint);

        // INITIAL SECTION
        UnityWebRequest request = null;
        var bodyRaw = Encoding.UTF8.GetBytes(value);

        // PREPARE SECTION
        switch (type)
        {
            case Brightend.RequestType.POST:
            {
                request = new UnityWebRequest(url, "POST");

                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                break;
            }
            case Brightend.RequestType.GET:
            {
                request = new UnityWebRequest(url, "GET");
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                break;
            }
            case Brightend.RequestType.PUT:
            {
                request = new UnityWebRequest(url, "PUT");
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                break;
            }
            case Brightend.RequestType.DELETE:
            {
                request = new UnityWebRequest(url, "DELETE");
                break;
            }
            case Brightend.RequestType.GET_ENDPOINT:
            {
                request = new UnityWebRequest(url.ToString() + $"{value}", "GET");
                request.downloadHandler = new DownloadHandlerBuffer();
                break;
            }
            default:
            {
                throw new NullReferenceException($"Not supported RequestType! ");
                //break;
            }
        }

        // SEND SECTION
        var asyn = request.SendWebRequest();
        while (!asyn.isDone)
        {
            Debug.Log($"Sending Process: {asyn.progress * 100f}%");
            await Task.Yield();
        }

        // RESULT SECTION
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Request 200: {request.url}");

            var _code = -1;
            var _msg = "";
            JToken _data = null;

            try
            {
                var _receive = JToken.Parse(request.downloadHandler.text);
                if (_receive == null)
                    throw new NullReferenceException("Received message isn't a valid JSON! ");
                _code = _receive["code"].Value<int>();
                _msg = _receive["msg"].Value<string>();
                _data = _receive["data"];
            }
            catch (Exception e)
            {
                Debug.LogWarning(
                    $"Received message isn't a valid format! \n{request.url}\n{e.Message}\n{request.downloadHandler.text}");
            }
        }
        else
        {
            Debug.LogError($"Receive error: {request.error}\n{request.url}");
        }
        
        request.Dispose();
        
        return;
    }
}
