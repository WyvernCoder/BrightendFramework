using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Networking;

namespace ProSheep
{
    public class Brightend : MonoBehaviour
    {
        private static Brightend _singleton;

        /// <summary>
        /// Check validate of Brightend instance.
        /// If it's not valid, then instantiate it into scene.  
        /// </summary>
        /// <returns>Always true</returns>
        private static Brightend CheckValidate()
        {
            Brightend result = null;

            if (_singleton == null)
            {
                result = Instantiate(new GameObject("Brightend")).AddComponent<Brightend>();
            }
            else
            {
                result = _singleton;
            }

            return result;
        }

        private void OnEnable()
        {
            if (_singleton != null && _singleton != this)
            {
                Destroy(this);
            }
            else
            {
                DontDestroyOnLoad(this);
                _singleton = this;
            }
        }


        /// <summary>
        /// Send a request with JSON body or path params.
        /// </summary>
        /// <param name="endpoint">v1/userdata/id/666 OR v1/user/login</param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="callback">code, msg, data</param>
        public void SendRequest(string endpoint, RequestType type, string value, ContentType contentType,
            UnityAction<int, string, JToken> callback = null)
        {
            StartCoroutine(IE_SendRequest(endpoint, type, value, contentType, callback));
        }

        private static IEnumerator IE_SendRequest(string endpoint, RequestType type, string value,
            ContentType contentType,
            UnityAction<int, string, JToken> callback = null)
        {
            // CONTENT TYPE
            var _contentType = "";
            switch (contentType)
            {
                case ContentType.EMPTY:
                    _contentType = "";
                    break;
                case ContentType.JSON:
                    _contentType = "application/json";
                    break;
                case ContentType.XFORM:
                    _contentType = "application/x-www-form-urlencoded";
                    break;
            }

            // MAKING INITIAL URL
            var url = FreeDownloadManager.ServerURL;
            url = new Uri(url, endpoint);

            // INITIAL SECTION
            UnityWebRequest request = null;
            var bodyRaw = Encoding.UTF8.GetBytes(value);

            // PREPARE SECTION
            switch (type)
            {
                case RequestType.POST:
                {
                    request = new UnityWebRequest(url, "POST");

                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", _contentType);

                    break;
                }
                case RequestType.GET:
                {
                    request = new UnityWebRequest(url, "GET");
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", _contentType);
                    break;
                }
                case RequestType.PUT:
                {
                    request = new UnityWebRequest(url, "PUT");
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", _contentType);
                    break;
                }
                case RequestType.DELETE:
                {
                    request = new UnityWebRequest(url, "DELETE");
                    break;
                }
                case RequestType.GET_ENDPOINT:
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
                yield return null;
            }

            // RESULT SECTION
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Request 200: {request.url} \n{request.downloadHandler.text}");

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
                        $"Received message isn't a valid format! \n{request.url}\n{e}\n{request.downloadHandler.text}");
                }

                callback?.Invoke(_code, _msg, _data);
            }
            else
            {
                Debug.LogError($"Receive error: {request.error}\n{request.url}\n{request.downloadHandler.text}");
                callback?.Invoke((int)request.responseCode, request.downloadHandler.text, new JObject());
            }

            request.Dispose();

            yield break;
        }

        public enum RequestType
        {
            POST,
            GET,
            GET_ENDPOINT,
            PUT,
            DELETE
        }

        public enum ContentType
        {
            EMPTY,
            JSON,
            XFORM
        }

        /// <summary>
        /// This class contains User and UserData information. 
        /// </summary>
        public static class UserServiceOnline
        {
            /// <summary>
            /// Fetch user information from backend. 
            /// </summary>
            /// <param name="username"></param>
            /// <param name="callback">isSuccessful, UserInformationJson</param>
            public static void FetchUser(string username, UnityAction<bool, JObject> callback, bool useCache = true)
            {
                /*
{
  "avatar": "useruploads/avatars/test_user.png",
  "createBy": "system",
  "createTime": "2025-12-06T12:28:14",
  "delFlag": "0",
  "deptid": null,
  "loginDate": "2025-12-01T08:00:00",
  "loginIp": "127.0.0.1",
  "nickname": "测试账号",
  "password": "pass",
  "phonenumber": "13700000003",
  "remark": "停用测试账号",
  "sex": "0",
  "status": "1",
  "updateBy": "system",
  "updateTime": "2025-12-06T12:28:14",
  "userId": 8,
  "username": "test_user",
  "usertype": 0
}
                 */
                
                // Use cache
                if (useCache && UserServiceCache.IsCached())
                {
                    callback?.Invoke(true, UserServiceCache.CachedUser);
                    return;
                }

                // Fetch from server
                CheckValidate().SendRequest("/v1/user/username/", RequestType.GET_ENDPOINT, username,
                    ContentType.EMPTY,(code, msg, data) =>
                    {
                        if (code == 200)
                        {
                            callback?.Invoke(true, (JObject)data);
                        }
                        else
                        {
                            callback?.Invoke(false, null);
                        }
                    });
            }

            /// <summary>
            /// Fetch UserData information from backend. 
            /// </summary>
            /// <param name="username"></param>
            /// <param name="callback">isSuccessful, UserInformationJson</param>
            public static void FetchUserData(string username, UnityAction<bool, JObject> callback, bool useCache = true)
            {
                /*
{
  "createBy": "linehey999",
  "createTime": "2025-12-06T12:37:06",
  "deptid": null,
  "id": 3,
  "unlockStatus": "{\"level_array\": [{\"stars\": 1, \"unlock\": 1, \"process\": 0.75, \"level_uid\": \"D7DF161DF5D03C75468C0B882DA288C1\"}, {\"stars\": 0, \"unlock\": 0, \"process\": 0, \"level_uid\": \"9EA609884AC13C6822ABBD81109A8A56\"}]}",
  "updateBy": "linehey999",
  "updateTime": "2025-12-06T12:37:06",
  "username": "test_user"
}
                 */
                
                // Use cache
                if (useCache && UserServiceCache.IsCached())
                {
                    callback?.Invoke(true, UserServiceCache.CachedUserData);
                    return;
                }

                // Fetch from server
                CheckValidate().SendRequest("/v1/userdata/username/", RequestType.GET_ENDPOINT, username,
                    ContentType.EMPTY,
                    (code, msg, data) =>
                    {
                        if (code == 200)
                        {
                            callback?.Invoke(true, (JObject)data);
                        }
                        else
                        {
                            callback?.Invoke(false, null);
                        }
                    });
            }

            /// <summary>
            /// Fetch user information from backend. 
            /// </summary>
            /// <param name="username"></param>
            /// <param name="password"></param>
            /// <param name="callback">isSuccessful, UserInformationJson</param>
            public static void FetchUser(string username, string password, UnityAction<bool, JObject> callback, bool useCache = true)
            {
                /*
{
  "avatar": "useruploads/avatars/test_user.png",
  "createBy": "system",
  "createTime": "2025-12-06T12:28:14",
  "delFlag": "0",
  "deptid": null,
  "loginDate": "2025-12-01T08:00:00",
  "loginIp": "127.0.0.1",
  "nickname": "测试账号",
  "password": "pass",
  "phonenumber": "13700000003",
  "remark": "停用测试账号",
  "sex": "0",
  "status": "1",
  "updateBy": "system",
  "updateTime": "2025-12-06T12:28:14",
  "userId": 8,
  "username": "test_user",
  "usertype": 0
}
                 */
                
                // Use cache
                if (useCache && UserServiceCache.IsCached())
                {
                    callback?.Invoke(true, UserServiceCache.CachedUser);
                    return;
                }

                // Fetch from server
                var body = $"username={username}&password={password}";
                CheckValidate().SendRequest("/v1/user/login", RequestType.POST, body.ToString(),
                    ContentType.XFORM,
                    (code, msg, data) =>
                    {
                        if (code == 200)
                        {
                            callback?.Invoke(true, (JObject)data);
                        }
                        else
                        {
                            callback?.Invoke(false, null);
                        }
                    });
            }
        }

        public static class UserServiceCache
        {
            public static JObject CachedUser;
            public static JObject CachedUserData;

            /// <summary>
            /// Download User and UserData from server, and cached to CachedUser and CachedUserData.
            /// It will smash CachedUser and CachedUserData once start download. 
            /// </summary>
            /// <param name="username"></param>
            /// <param name="password"></param>
            /// <param name="callback">isSucceed, CachedUser, CachedUserData</param>
            public static void Download(string username, string password, UnityAction<bool, JObject, JObject> callback, bool useCache = true)
            {
                Flush();

                UserServiceOnline.FetchUser(username, password, (isSucceed, data) =>
                {
                    if (isSucceed)
                    {
                        UserServiceOnline.FetchUserData(username, (_isSucceed, _data) =>
                        {
                            if (_isSucceed)
                            {
                                CachedUser = data;
                                CachedUserData = _data;
                                callback?.Invoke(true, data, _data);
                            }
                            else
                            {
                                callback?.Invoke(false, null, null);
                            }
                        });
                    }
                    else
                    {
                        callback?.Invoke(false, null, null);
                    }
                }, useCache);
            }

            /// <summary>
            /// Set CachedUser and CachedUserData both to null. 
            /// </summary>
            public static void Flush()
            {
                CachedUser = null;
                CachedUserData = null;
            }

            public static bool IsCached()
            {
                return CachedUser != null && CachedUserData != null;
            }

            public static string GetUsername()
            {
                if (CachedUser == null)
                    return "";

                return (string)CachedUser["username"];
            }

            public static string GetNickname()
            {
                if (CachedUser == null)
                    return "";

                return (string)CachedUser["nickname"];
            }

            /// <summary>
            /// Get Level Star
            /// </summary>
            /// <param name="level_uid"></param>
            /// <returns>-1 if null</returns>
            public static int GetLevelStar(string level_uid)
            {
                if (CachedUser == null || CachedUserData == null)
                    return -1;

                var target_level = FindLevelByUid(level_uid);
                if (target_level == null)
                {
                    return -1;
                }

                return (int)target_level["stars"];
            }

            /// <summary>
            /// Get Level Unlock
            /// </summary>
            /// <param name="level_uid"></param>
            /// <returns></returns>
            public static bool GetLevelUnlock(string level_uid)
            {
                if (CachedUser == null || CachedUserData == null)
                    return false;

                var target_level = FindLevelByUid(level_uid);
                if (target_level == null)
                {
                    Debug.LogError($"Can't find level! \n{level_uid}");
                    return false;
                }

                var result = (int)target_level["unlock"];

                return (result == 1);
            }

            /// <summary>
            /// Get Level Process
            /// </summary>
            /// <param name="level_uid"></param>
            /// <returns>Less than 0 if null</returns>
            public static float GetLevelProcess(string level_uid)
            {
                if (CachedUser == null || CachedUserData == null)
                    return -1;

                var target_level = FindLevelByUid(level_uid);
                if (target_level == null)
                {
                    return -1;
                }

                return (float)target_level["process"];
            }

            /// <summary>
            /// Find level by level_uid.
            /// </summary>
            /// <param name="uid"></param>
            /// <returns>null if not found</returns>
            private static JObject FindLevelByUid(string uid)
            {
                if (CachedUser == null || CachedUserData == null)
                    return null;
                
                var levelArrayObj = JObject.Parse((string)CachedUserData["unlock_status"]);
                var levelArray = (JArray)levelArrayObj["level_array"];

                JObject result = null;
                
                try
                {
                    foreach (JObject level in levelArray)
                    {
                        if ((string)level["level_uid"] == uid)
                        {
                            result = level;
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Can't find level by Uid! \n {e}");
                }

                return result;
            }
        }

        public static class LevelServiceOnline
        {
            /// <summary>
            /// Fetch all official levels from backend. 
            /// </summary>
            /// <param name="callback">isSuccessful, UserInformationJson</param>
            public static void FetchLevelList(UnityAction<bool, JArray> callback, bool useCache = true)
            {
                /*
[
  {
    "categoryId": 1,
    "cover": "gameassets/covers/level_forest_advanture.png",
    "createBy": "linehey999",
    "createTime": "2025-12-06T12:38:58",
    "defaultUnlock": 1,
    "describe": "新手教程",
    "id": 1,
    "json": "{}",
    "label": "森林探险",
    "maxStars": 3,
    "uid": "D7DF161DF5D03C75468C0B882DA288C1",
    "updateBy": "linehey999",
    "updateTime": "2025-12-06T12:38:58"
  },
  {
    "categoryId": 1,
    "cover": "gameassets/covers/level_desert_examing.png",
    "createBy": "linehey999",
    "createTime": "2025-12-06T12:38:58",
    "defaultUnlock": 0,
    "describe": "沙漠挑战",
    "id": 2,
    "json": "{}",
    "label": "沙漠试炼",
    "maxStars": 3,
    "uid": "9EA609884AC13C6822ABBD81109A8A56",
    "updateBy": "linehey999",
    "updateTime": "2025-12-06T12:38:58"
  }
]
                 */
                
                // Use cache
                if (useCache && LevelServiceCache.IsCached())
                {
                    callback?.Invoke(true, LevelServiceCache.CachedLevel);
                    return;
                }

                // Fetch from server
                CheckValidate().SendRequest("/v1/levels", RequestType.GET, "",
                    ContentType.EMPTY,
                    (code, msg, data) =>
                    {
                        if (code == 200)
                        {
                            callback?.Invoke(true, (JArray)data);
                        }
                        else
                        {
                            callback?.Invoke(false, null);
                        }
                    });
            }
        }

        public static class LevelServiceCache
        {
            public static JArray CachedLevel;

            /// <summary>
            /// Download all official levels from server, and cache to local. 
            /// </summary>
            /// <param name="callback"></param>
            public static void Download(UnityAction<bool, JArray> callback, bool useCache = true)
            {
                Flush();
                
                LevelServiceOnline.FetchLevelList((isSucceed, levelArray) =>
                {
                    if (isSucceed)
                    {
                        CachedLevel = levelArray;
                        callback?.Invoke(true, CachedLevel);
                    }
                    else
                    {
                        Debug.LogError($"Can't  get official level list : {levelArray}!");
                        callback?.Invoke(false, null);
                    }
                }, useCache);
            }

            public static void Flush()
            {
                CachedLevel = null;
            }

            public static bool IsCached()
            {
                return CachedLevel != null;
            }

            /// <summary>
            /// Find level uid by Label. 
            /// </summary>
            /// <param name="level_uid"></param>
            /// <returns>null if not found</returns>
            public static JObject GetLevelByLabel(string label)
            {
                if (CachedLevel == null)
                    return null;

                JObject targetLevel = null;
                foreach (JObject level in CachedLevel)
                {
                    if ((string)level["label"] == label)
                    {
                        targetLevel = level;
                        break;
                    }
                }

                return targetLevel;
            }
        }

        public static class DictServiceOnline
        {
            public static string StringDatabaseTableName = "DefaultStringTableCollection";

            public enum LanguageType
            {
                zh,
                en
            }

            /// <summary>
            /// Download dictionaries from Database, then insert the data to Localization System. 
            /// </summary>
            public static void UpdateLocalizationSystemFromOnline(bool useCache = true)
            {
                foreach (LanguageType lang in Enum.GetValues(typeof(LanguageType)))
                {
                    FetchDictList(lang, (isSuccessfully, jsonArray) =>
                    {
                        if (isSuccessfully)
                        {
                            try
                            {
                                foreach (JObject jObj in jsonArray)
                                {
                                    var dict_key = (string)jObj["dict_value"];
                                    var dict_value = (string)jObj["dict_label"];
                                    var dict_lang = (string)jObj["dict_language"];

                                    AddEntryToLocalizationSystem(dict_key, dict_value,
                                        Enum.Parse<LanguageType>(dict_lang));
                                }

                                RefreshAllString();
                            }
                            catch (Exception e)
                            {
                                Debug.LogError($"Localization Inserting Error! \n{e}");
                            }
                        }
                        else
                        {
                            Debug.LogError($"Can't fetch dictionaries from server! ");
                        }
                    }, useCache);
                }
            }

            /// <summary>
            /// Fetch all official levels from backend. 
            /// </summary>
            /// <param name="username"></param>
            /// <param name="callback">isSuccessful, UserInformationJson</param>
            public static void FetchDictList(LanguageType localeType, UnityAction<bool, JArray> callback, bool useCache = true)
            {
                /*
[{
    "createBy": "linehey",
    "createTime": "2025-12-06T00:02:49",
    "deptid": null,
    "dictCode": 1,
    "dictLabel": "登录",
    "dictLanguage": "zh",
    "dictType": "language",
    "dictValue": "#LOGIN",
    "lengthList": null,
    "status": "0",
    "updateBy": "linehey",
    "updateTime": "2025-12-06T00:04:21"
}, {
    "createBy": "linehey999",
    "createTime": "2025-12-06T00:04:26",
    "deptid": null,
    "dictCode": 3,
    "dictLabel": "今日公告",
    "dictLanguage": "zh",
    "dictType": "language",
    "dictValue": "#MOTD_TITLE",
    "lengthList": null,
    "status": "0",
    "updateBy": null,
    "updateTime": "2025-12-06T00:05:22"
}]
                 */

                // Use cache
                if (useCache && DictServiceCached.IsDictCached)
                {
                    callback?.Invoke(true, DictServiceCached.DictCached);
                    return;
                }

                // Fetch from server
                CheckValidate().SendRequest("/v1/dict/language/", RequestType.GET_ENDPOINT,
                    Enum.GetName(typeof(LanguageType), localeType),
                    ContentType.EMPTY,
                    (code, msg, data) =>
                    {
                        if (code == 200)
                        {
                            callback?.Invoke(true, (JArray)data);
                        }
                        else
                        {
                            callback?.Invoke(false, null);
                        }
                    });
            }

            /// <summary>
            /// Add entry to the localization system. 
            /// </summary>
            /// <param name="key">#CHAPTER_01</param>
            /// <param name="value">沙地陷阱</param>
            /// <param name="localeType">zh</param>
            private static void AddEntryToLocalizationSystem(string key, string value, LanguageType localeType)
            {
                var localeName = Enum.GetName(typeof(LanguageType), localeType);

                // Check validate of target locale
                Locale locale = LocalizationSettings.AvailableLocales.Locales.Find(searchingLocale =>
                    searchingLocale.Identifier.Code == localeName);
                if (locale == null)
                {
                    var _localePrint = "";
                    foreach (var _locale in LocalizationSettings.AvailableLocales.Locales)
                    {
                        _localePrint += $"{_locale.Identifier.Code}, ";
                    }

                    Debug.LogError(
                        $"Can't find locale in your project: {localeName}! \nProject Locales:{_localePrint}");
                    return;
                }

                // Find default string table
                var targetTable = LocalizationSettings.StringDatabase.GetTable(StringDatabaseTableName, locale);
                if (targetTable == null)
                {
                    Debug.LogError($"Can't find String Table in your project: {StringDatabaseTableName} ");
                    return;
                }

                // Create or Update entry
                var entry = targetTable.GetEntry(key);
                if (entry == null)
                {
                    targetTable.AddEntry(key, value);
                }
                else
                {
                    entry.Value = value;
                }

                // Refresh all localize string
                //Debug.Log($"Localization Updated. \nAdded entry with Key: {key} and Value: {value}. ");
            }

            public static void RefreshAllString()
            {
                var localeList = LocalizationSettings.AvailableLocales.Locales;
                var currentLocaleIndex = localeList.IndexOf(LocalizationSettings.SelectedLocale);

                var currentLocale = localeList[currentLocaleIndex];

                var locale = localeList[(currentLocaleIndex + 1) % localeList.Count];
                LocalizationSettings.SelectedLocale = locale;

                LocalizationSettings.SelectedLocale = currentLocale;

                Debug.Log($"Localization String Refreshed. ");
            }

            public static LocaleIdentifier ToggleLocale()
            {
                var localeList = LocalizationSettings.AvailableLocales.Locales;
                var currentLocaleIndex = localeList.IndexOf(LocalizationSettings.SelectedLocale);

                var locale = localeList[(currentLocaleIndex + 1) % localeList.Count];
                LocalizationSettings.SelectedLocale = locale;
                return locale.Identifier;
            }
        }
        
        public static class DictServiceCached
        {
            public static JArray DictCached;
            public static bool IsDictCached => DictCached != null;
        }
    }
}