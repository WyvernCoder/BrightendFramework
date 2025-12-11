using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ProSheep;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class BundleUploaderWindow : EditorWindow
    {
        private string serverUrl = "http://localhost:8080/";
        private string serverUrl_up => serverUrl + "v1/files/upload_gameassets/dlcs/";
        private string serverUrl_de => serverUrl + "v1/files/delete_gameassets/dlcs/";
        private string buildPath = "ServerData/StandaloneWindows64";    // 默认Win打包就这地方

        private Texture2D banner;

        private void OnEnable()
        {
            banner = Resources.Load<Texture2D>("Editor/managarmr");
        }

        [MenuItem("Brightend Tools/Bundle Uploader Window")]
        public static void ShowWindow() => GetWindow<BundleUploaderWindow>("Bundle Uploader");

        private void OnGUI()
        {
            GUILayout.Label(banner, GUILayout.Height(100), GUILayout.ExpandWidth(true));

            GUILayout.Space(5);
            
            serverUrl = $"{FreeDownloadManager.ServerURL.ToString()}";
            GUILayout.Label("Bundle Uploader Settings", EditorStyles.boldLabel);
            
            GUILayout.Space(2);
            
            EditorGUILayout.LabelField($"Upload URL: {serverUrl_up}");
            EditorGUILayout.LabelField($"Delete URL: {serverUrl_de}");
            
            buildPath = EditorGUILayout.TextField("Local Bundle Build Path", buildPath);

            GUILayout.Space(20);

            if (GUILayout.Button("Upload Bundles Only"))
                _ = UploadBundlesAsync();

            if (GUILayout.Button("Delete All Bundles on Server"))
                _ = DeleteBundlesAsync();
        }

        private async Task UploadBundlesAsync()
        {
            if (!Directory.Exists(buildPath))
            {
                Debug.LogError("Build path does not exist: " + buildPath);
                return;
            }

            var files = Directory.GetFiles(buildPath);
            if (files.Length == 0)
            {
                Debug.LogWarning("No bundles found in: " + buildPath);
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Upload new bundles
                    var form = new MultipartFormDataContent();
                    foreach (var file in files)
                    {
                        var content = new ByteArrayContent(File.ReadAllBytes(file));
                        content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                        form.Add(content, "files", Path.GetFileName(file));
                    }

                    var uploadResponse = await client.PostAsync(serverUrl_up, form);
                    Debug.Log("Upload response: " + await uploadResponse.Content.ReadAsStringAsync());
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Upload failed: " + ex.Message);
                }
            }
        }
        private async Task DeleteBundlesAsync()
        {
            if (!Directory.Exists(buildPath))
            {
                Debug.LogError("Build path does not exist: " + buildPath);
                return;
            }

            var files = Directory.GetFiles(buildPath);
            if (files.Length == 0)
            {
                Debug.LogWarning("No bundles found in: " + buildPath);
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // DELETE with body via SendAsync
                    var filenamesObj = new { filenames = files.Select(Path.GetFileName).ToList() };
                    var deleteJson = JsonUtility.ToJson(filenamesObj);
                    var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, serverUrl_de)
                    {
                        Content = new StringContent(deleteJson, System.Text.Encoding.UTF8, "application/json")
                    };
                    var deleteResponse = await client.SendAsync(deleteRequest);
                    Debug.Log("Delete response: " + await deleteResponse.Content.ReadAsStringAsync());
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Delete failed: " + ex.Message);
                }
            }
        }
    }
}
