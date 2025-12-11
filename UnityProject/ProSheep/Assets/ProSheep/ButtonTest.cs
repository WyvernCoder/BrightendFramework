using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProSheep
{
    public class ButtonTest : MonoBehaviour
    {
        private FreeDownloadManager _fdm => FreeDownloadManager.Singleton;
        private TMP_InputField _inputField_upload; // 被上传文件的路径
        private TMP_InputField _inputField_downloadName; // 要下载文件的名称及扩展名
        private Image _spriteRenderer_headshot;

        // 这外加载的玩意要你自己管理销毁，你不正确销毁，它就敢躺尸在你内存里
        private Texture2D t2d;
        private Sprite sp;

        private void OnEnable()
        {
            var inputFields = GetComponentsInChildren<TMP_InputField>();
            _inputField_upload = inputFields[0];
            _inputField_downloadName = inputFields[1];
            _spriteRenderer_headshot = GetComponentInChildren<Image>();

            foreach (var inputField in inputFields)
            {
                inputField.transform.parent.gameObject.SetActive(false);
            }
        }

        public void OnClick_Upload()
        {
            // 开始上传
            _fdm.UploadFile(_inputField_upload.text, (arg0, isSuccess) =>
            {
                // 上传后头像变色
                _spriteRenderer_headshot.color = Color.HSVToRGB(Time.fixedTime % 1f, 1, 1);
            });
        }

        public void OnClick_DownloadName()
        {
            _fdm.DownloadFile(_inputField_downloadName.text, true, (arg0, isSuccess) =>
            {
                if (t2d != null) Destroy(t2d);
                if (sp != null) Destroy(sp);

                if (BinaryFileConverter.LoadImage(arg0, ref sp, ref t2d))
                {
                    // 下载后恢复颜色并修改贴图
                    // BinaryFileConverter是在FDM里定义的类
                    _spriteRenderer_headshot.color = Color.white;
                    _spriteRenderer_headshot.sprite = sp;
                }
                else
                {
                    Debug.LogError($"Could not load image {arg0}");
                }
            });
        }

        private void OnDestroy()
        {
            if (t2d != null) Destroy(t2d);
            if (sp != null) Destroy(sp);
        }
    }
}