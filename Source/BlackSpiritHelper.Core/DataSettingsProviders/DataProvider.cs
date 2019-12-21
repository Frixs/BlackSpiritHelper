using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Data provider allows you to manage application (predefined) data that are needed to fully run the application.
    /// </summary>
    public class DataProvider
    {
        #region Singleton

        private static DataProvider mInstance = null;

        public static DataProvider Instance
        {
            get
            {
                if (mInstance == null)
                    return new DataProvider();
                return mInstance;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        private DataProvider()
        {
        }

        #endregion

        public bool DownloadData(string url, ProgressDialogViewModel dataViewModel = null)
        {
            bool isDownloadOk = true;

            // Create web client interface.
            WebClient wc = new WebClient();
            
            try
            {
                // Start download.
                if (!DownloadDataProcess(wc, url, dataViewModel))
                    isDownloadOk = false;
            }
            catch (Exception ex)
            {
                // Github API access limit reached:
                // System.Net.WebException: The remote server returned an error: (403) Forbidden.
                // No internet connection:
                // System.Net.WebException: The remote name could not be resolved: 'api.github.com'.

                isDownloadOk = false;
                IoC.Logger.Log($"Unexpected error occurred during downloading/updating files: ({ex.GetType().ToString()}) {ex.Message}", LogLevel.Error);
            }
            finally
            {
                // Dispose web client interface.
                if (wc != null)
                    ((IDisposable)wc).Dispose();
            }

            return isDownloadOk;
        }

        /// <summary>
        /// Download a new data.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dataViewModel"></param>
        /// <returns></returns>
        private bool DownloadDataProcess(WebClient webClient, string url, ProgressDialogViewModel dataViewModel = null)
        {
            bool isDownloadOk = true;

            // Add necessary headers to access our release location (github).
            // Note: Headers are applied only for upcoming request, not for all requests.
            webClient.Headers.Add("User-Agent", "Nothing");

            // Download string that represents json.
            var content = webClient.DownloadString(url);

            // Deserialize json.
            var jobj = (JArray)JsonConvert.DeserializeObject(content);
            for (int i = 0; i < jobj.Count; i++)
            {
                // Get necessary json items.
                var isDir = jobj[i]["type"].ToString() == "dir" ? true : false;
                var name = jobj[i]["name"].ToString();
                var path = jobj[i]["path"].ToString();
                var downloadUrl = jobj[i]["download_url"].ToString();

                // If the item is DIR, go into recurse.
                if (isDir)
                {
                    if (!DownloadDataProcess(webClient, Path.Combine(url, name), dataViewModel))
                    {
                        isDownloadOk = false;
                        break;
                    }
                }
                // Otherwise, download the file.
                else
                {
                    // Get local path where to place a new file.
                    var localPath = Path.Combine(
                        SettingsConfiguration.ApplicationDataDirPath,
                        path.Length > SettingsConfiguration.RemoteDataDirRelPath.Length ? path.Substring(SettingsConfiguration.RemoteDataDirRelPath.Length) : path
                        );

                    // Create local directory.
                    if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                    }

                    // Update data.
                    if (dataViewModel != null) dataViewModel.WorkOn = "Downloading... " + name;

                    // Download file.
                    webClient.DownloadFile(downloadUrl, localPath);
                }

            }

            return isDownloadOk;
        }

    }
}
