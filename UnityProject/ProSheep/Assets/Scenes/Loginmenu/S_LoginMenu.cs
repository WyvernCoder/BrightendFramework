using System;
using System.Collections;
using ProSheep;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Scenes.Loginmenu
{
    public class S_LoginMenu : MonoBehaviour
    {
        public TMP_InputField ifUsername;
        public TMP_InputField ifPassword;
        public Toggle tTerm;
        public string nextLevel;

        public void OnClick_Login()
        {
            var username = ifUsername.text;
            var password = ifPassword.text;
            Brightend.UserServiceCache.Download(username, password, (succeed, user, userdata) =>
            {
                if (succeed)
                {
                    Debug.Log($"Login successful! Welcome back {Brightend.UserServiceCache.GetNickname()}");
                    SceneManager.LoadScene(nextLevel);
                }
            });
        }

        public void OnClick_ToggleLanguage()
        {
            var curLocale = Brightend.DictServiceOnline.ToggleLocale();
            
            print($"New Locale: {curLocale.CultureInfo.EnglishName}");
        }

        private void Start()
        {
            Brightend.DictServiceOnline.UpdateLocalizationSystemFromOnline();
            
            // Brightend.UserServiceOnline.FetchUser("test_user", (isSuccessfully, data) =>
            // {
            //     if (isSuccessfully)
            //     {
            //         print($"获取到登录信息：\n{data.ToString()}");
            //     }
            // });
            // Brightend.LevelServiceOnline.FetchLevelList((isSuccessfully, data) =>
            // {
            //     if (isSuccessfully)
            //     {
            //         print($"获取到关卡信息：\n{data.ToString()}");
            //     }
            // });
            // Brightend.DictServiceOnline.FetchDictList(Brightend.DictServiceOnline.LanguageType.zh,
            //     (isSuccessfully, data) =>
            //     {
            //         if (isSuccessfully)
            //         {
            //             print($"获取到语言信息：\n{data.ToString()}");
            //         }
            //     });
        }
    }
}