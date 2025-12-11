using System;
using System.Collections;
using System.IO;
using ProSheep;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.Premainmenu
{
    public class S_PreMainmenu : MonoBehaviour
    {
        public Image imgCover;
        public string loginmenuName = "SE_LoginMenu";
        
        // 这外加载的玩意要你自己管理销毁，你不正确销毁，它就敢躺尸在你内存里
        private Texture2D t2d;
        private Sprite sp;
        

        private void Start()
        {
            var fileName = "premainmenu_cover.png";
            var fileFullpath = Path.Combine(Application.persistentDataPath, fileName);
            FreeDownloadManager.CheckValidate().DownloadFile(fileName, false, (fileName, isSuccessfully) =>
            {
                if (isSuccessfully)
                {
                    if (t2d != null) Destroy(t2d);
                    if (sp != null) Destroy(sp);
                    
                    if (BinaryFileConverter.LoadImage(fileName, ref sp, ref t2d))
                    {
                        imgCover.sprite = sp;
                        imgCover.material = null;
                    }
                }
                else
                {
                    Debug.Log($"Failed to download file {fileName} from server! ");
                }

                StartCoroutine(IE_MainLogic(isSuccessfully));
            });
        }

        private void OnDestroy()
        {
            if (t2d != null)
                Destroy(t2d);
            if(sp != null)
                Destroy(sp);
        }

        IEnumerator IE_MainLogic(bool isSucceed)
        {
            imgCover.color = Color.clear;

            var timer = 0f;
            const float transitionDuration = 2f;

            while (true)
            {
                imgCover.color = Color.Lerp(Color.clear, Color.white, timer / transitionDuration);

                timer += Time.deltaTime;

                if (timer >= transitionDuration)
                    break;

                yield return null;
            }

            imgCover.color = Color.white;

            // Don't fade out if failed to connect server. 
            if (isSucceed == false)
                yield break;

            // Load login menu scene 
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(loginmenuName, LoadSceneMode.Additive);

            timer = 0;
            yield return new WaitForSecondsRealtime(transitionDuration / 2f);

            while (true)
            {
                imgCover.color = Color.Lerp(Color.white, Color.clear, timer / transitionDuration);

                timer += Time.deltaTime;

                if (timer >= transitionDuration)
                    break;

                yield return null;
            }

            SceneManager.UnloadSceneAsync(currentScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }
    }
}