using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace ProSheep
{
    public class FreeDownloadManager : MonoBehaviour
    {
        public static FreeDownloadManager Singleton;
        private static UnityWebRequest _webRequestingFile;

        private List<DownloadTask> _downloadQueue = new List<DownloadTask>();
        private struct DownloadTask
        {
            public static DownloadTask Constructor(string filenameWextension, bool fromUserUploads, UnityAction<string, bool> callback)
            {
                var task = new DownloadTask
                {
                    FilenameWextension = filenameWextension,
                    FromUserUploads = fromUserUploads,
                    Callback = callback
                };
                return task;
            }
            
            public string FilenameWextension;
            public bool FromUserUploads;
            public UnityAction<string, bool> Callback;
        }

        public static readonly Uri
            ServerURL = new Uri("http://localhost:8080/"); // 结尾必须带/

        private void OnEnable()
        {
            if (Singleton != null && Singleton != this)
            {
                Destroy(this);
            }
            else
            {
                DontDestroyOnLoad(this);
                Singleton = this;
            }
        }

        public static FreeDownloadManager CheckValidate()
        {
            FreeDownloadManager result = null;

            if (Singleton == null)
            {
                result = Instantiate(new GameObject("Free Download Manager")).AddComponent<FreeDownloadManager>();
            }
            else
            {
                result = Singleton;
            }

            return result;
        }

        [ContextMenu("Test Upload")]
        public void DEBUG_TestUpload()
        {
            UploadFile(Path.Combine(LocalDesktop(), "1.png"));
        }

        [ContextMenu("Test Download")]
        public void DEBUG_TestDownload()
        {
            DownloadFile("1.png", true);
        }

        private void Start()
        {
            
        }

        /// <summary>
        /// 正规上传入口
        /// </summary>
        /// <param name="filefullpath">被上传文件的完整路径</param>
        /// <param name="callback">被上传文件的全路径，是否上传成功</param>
        public void UploadFile(string filefullpath, UnityAction<string, bool> callback = null)
        {
            if (CanDownloadOrUpload())
            {
                StartCoroutine(_uploadFile(filefullpath, callback));
            }
            else
            {
                Debug.LogError($"Operation ABORT. A file is currently in process. ");
                // _webRequestingFile.Abort();
            }
        }

        /// <summary>
        /// 正规下载入口
        /// </summary>
        /// <param name="filenameWextension">需要下载文件的文件名+扩展名</param>
        /// <param name="callback">下载文件的文件名扩展名，是否下载成功</param>
        /// <param name="fromUserUploads">是否从useruploads文件夹下载文件</param>
        public void DownloadFile(string filenameWextension, bool fromUserUploads,
            UnityAction<string, bool> callback = null)
        {
            if (CanDownloadOrUpload())
            {
                StartCoroutine(_downloadFile(filenameWextension, fromUserUploads, callback));
            }
            else
            {
                _downloadQueue.Add(DownloadTask.Constructor(filenameWextension, fromUserUploads, callback));
                Debug.Log($"New task added. \n{filenameWextension}. ");
            }
        }

        /// <summary>
        /// 虚函数，继承类只需要覆盖并赋值即可。
        /// </summary>
        /// <returns></returns>
        public virtual bool CanDownloadOrUpload()
        {
            return _webRequestingFile == null;
        }

        private IEnumerator _uploadFile(string path, UnityAction<string, bool> callback)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            var filename = System.IO.Path.GetFileName(path);

            var req = new UnityWebRequest(ServerFileUpload(), "POST");
            req.uploadHandler = new UploadHandlerRaw(bytes);
            req.downloadHandler = new DownloadHandlerBuffer();

            _webRequestingFile = req;

            req.SetRequestHeader("Content-Type", "application/octet-stream");
            req.SetRequestHeader("X_FILENAME", filename);

            var asyncRequest = req.SendWebRequest();

            while (!req.isDone)
            {
                Debug.Log($"Uploading Process: {asyncRequest.progress * 100f}%");
                yield return null;
            }

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Upload complete!");

                callback?.Invoke(path, true);
            }
            else
            {
                Debug.LogError($"Upload ERROR: {req.error}\n{req.url}\n{req.downloadHandler.text}");

                callback?.Invoke(path, false);
            }

            req.Dispose();
            _webRequestingFile = null;
        }

        private IEnumerator _downloadFile(string filename, bool fromUserUploads, UnityAction<string, bool> callback)
        {
            if (Path.GetExtension(filename) == "")
            {
                Debug.LogError($"Cannot download file {filename}, caused by EMPTY EXTENSION! ");
                _webRequestingFile = null;
                TaskFlush();
                yield break;
            }

            var url = new Uri(new Uri(ServerDownloadDictionary(fromUserUploads)), filename).ToString();

            UnityWebRequest req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET)
            {
                downloadHandler = new DownloadHandlerFile(Path.Combine(PathDownloadDictionary(), filename), false)
                {
                    removeFileOnAbort = true
                }
            };

            _webRequestingFile = req;

            var asyncRequest = req.SendWebRequest();

            while (!req.isDone)
            {
                Debug.Log($"Downloading Process: {asyncRequest.progress * 100f}%");
                yield return null;
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Download failed: {req.error}\n{req.url}");

                callback?.Invoke(filename, false);
            }
            else
            {
                Debug.Log($"Downloaded: {Path.Combine(LocalPresistent(), Path.GetFileName(url))}");

                callback?.Invoke(filename, true);
            }

            req.Dispose();
            _webRequestingFile = null;
            TaskFlush();
        }

        /// <summary>
        /// Remove task and start to download it. 
        /// </summary>
        private void TaskFlush()
        {
            // Continue to download
            if (_downloadQueue.Count > 0)
            {
                // Flush task
                var nextTask = _downloadQueue[0];
                _downloadQueue.RemoveAt(0);
                
                DownloadFile(nextTask.FilenameWextension, nextTask.FromUserUploads, nextTask.Callback);
                
                Debug.Log($"Next task started: {nextTask.FilenameWextension}");
            }
        }

        private void Update()
        {
            if (_downloadQueue.Count > 0 && _webRequestingFile == null)
            {
                var tasks = "";
                foreach (var task in _downloadQueue)
                {
                    tasks += $"{task.FilenameWextension}\n";
                }
                Debug.LogWarning($"检测到下载器已经空闲但仍有任务！\n{tasks}");
            }
        }

        protected static string ServerFileUpload()
        {
            return new Uri(ServerURL, "v1/files/upload").ToString();
        }

        // protected static string ServerUploadDictionary()
        // {
        //     // Uri操作目录必须加/，否则会被识别为文件而被替换。
        //     // 问题复现：print(new Uri(ServerURL, "123"), "456.jpg")。
        //     return new Uri(ServerURL, "useruploads/").ToString();
        // }

        protected static string ServerDownloadDictionary(bool useUserUploads)
        {
            // Uri操作目录必须加/，否则会被识别为文件而被替换。
            // 问题复现：print(new Uri(ServerURL, "123"), "456.jpg")。
            var thePath = "";
            if (useUserUploads)
            {
                thePath = "v1/files/download_useruploads/";
            }
            else
            {
                thePath = "v1/files/download_gameassets/";
            }

            return new Uri(ServerURL, thePath).ToString();
        }

        public static string ServerDLCDownloadDictionary()
        {
            // Uri操作目录必须加/，否则会被识别为文件而被替换。
            // 问题复现：print(new Uri(ServerURL, "123"), "456.jpg")。
            var thePath = "";
            {
                thePath = "v1/files/download_gameassets/dlcs/";
            }

            return new Uri(ServerURL, thePath).ToString();
        }

        protected static string LocalHome()
        {
            return System.Environment.GetEnvironmentVariable("HOMEPATH");
        }

        protected static string LocalDesktop()
        {
            return Path.Combine(LocalHome(), "Desktop");
        }

        protected static string LocalPresistent()
        {
            return Application.persistentDataPath;
        }

        protected static string PathDownloadDictionary()
        {
            return LocalPresistent();
        }
    }
}

public class BinaryFileConverter : MonoBehaviour
{
    /// <summary>
    /// 转换本地图片文件为Texture2D资源
    /// </summary>
    /// <param name="filename">文件名及扩展名，位于Presistent目录下</param>
    /// <param name="outputTexture"></param>
    /// <param name="generateMipmap">是否生成Mipmap贴图</param>
    /// <returns>是否成功</returns>
    public static bool LoadImage(string filename, ref Texture2D outputTexture, bool generateMipmap = false)
    {
        var basePath = Application.persistentDataPath;
        var fileFullPath = Path.Combine(basePath, filename);
        if (File.Exists(fileFullPath) == false)
            return false;

        try
        {
            byte[] bytes = File.ReadAllBytes(fileFullPath);
            outputTexture = new Texture2D(2, 2, TextureFormat.RGBA32, generateMipmap);
            outputTexture.LoadImage(bytes);
        }
        catch (Exception e)
        {
            throw new Exception($"LoadImage failed! \n{filename}\n{e.Message}");
            //return false;
        }

        return true;
    }

    /// <summary>
    /// 转换本地图片文件为Sprite资源
    /// </summary>
    /// <param name="filename">文件名及扩展名，位于Presistent目录下</param>
    /// <param name="outputSprite"></param>
    /// <param name="outputTexture"></param>
    /// <returns></returns>
    public static bool LoadImage(string filename, ref Sprite outputSprite, ref Texture2D outputTexture)
    {
        try
        {
            if (LoadImage(filename, ref outputTexture, false))
            {
                var tex = outputTexture;
                outputSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                throw new Exception($"LoadImage failed! \n{filename}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Can't load image! \nFilename: {filename}\n{e}");
            return false;
        }

        return true;
    }
}