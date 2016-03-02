using System;
using System.Text;
using System.Net;
using System.IO;

namespace UtilityToolkit.WebTools
{
    public static class WebTools
    {
        #region FTP

        /// <summary>
        /// Connects to an FTP location and returns an array of filenames contained in the location
        /// </summary>
        /// <param name="FTPPath">Full path (without the protocol prefix) of the FTP location</param>
        /// <param name="uName">FTP username</param>
        /// <param name="pwd">FTP password</param>
        /// <returns>An array of file names contained in the FTP location path</returns>
        public static string[] GetFileListFromFtp(string FTPPath, string uName, string pwd)
        {
            string[] files;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            FtpWebRequest reqFTP = null;

            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + FTPPath.Trim() + "/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(uName.Trim(), pwd);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.Proxy = new WebProxy();
                reqFTP.KeepAlive = true;

                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                if (result != null && (result.ToString() != ""))
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);

            }
            catch { result = null; }

            try
            {
                reader.Close();
                response.Close();
                reqFTP.Abort();
            }
            catch { }

            if (result == null)
            {
                files = null;
                return files;
            }
            else
                return result.ToString().Split('\n');
        }


        /// <summary>
        /// Downloads a single file from an FTP location to a local path, with an option to deleted the file from the FTP when done. 
        /// </summary>
        /// <param name="FTPPath">Full path (without the protocol prefix) of the FTP location</param>
        /// <param name="uName">FTP username</param>
        /// <param name="pwd">FTP password</param>
        /// <param name="fileName">Name of file in the FTP location to download.</param>
        /// <param name="downloadPath">Path to local directory to download the file to.</param>
        /// <param name="deleteAfterDownload">True = delete file from FTP, False = will leave the file.</param>
        public static void DownloadFileFromFtp(string FTPPath, string uName, string pwd, string fileName, string downloadPath, bool deleteAfterDownload)
        {
            if (!String.IsNullOrEmpty(FTPPath) && !String.IsNullOrEmpty(uName) && !String.IsNullOrEmpty(pwd) && !String.IsNullOrEmpty(downloadPath))
            {
                FtpWebRequest reqFTP = null;
                FtpWebResponse response = null;
                FileStream writeStream = null;
                Stream responseStream = null;

                try
                {
                    string uri = "ftp://" + FTPPath.Trim() + "/" + fileName;
                    Uri serverUri = new Uri(uri);

                    if (serverUri.Scheme != Uri.UriSchemeFtp)
                    {
                        return;
                    }

                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + FTPPath.Trim() + "/" + fileName));
                    reqFTP.Credentials = new NetworkCredential(uName.Trim(), pwd.Trim());
                    reqFTP.KeepAlive = true;
                    reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                    reqFTP.UseBinary = true;
                    reqFTP.Proxy = null;

                    response = (FtpWebResponse)reqFTP.GetResponse();
                    responseStream = response.GetResponseStream();
                    writeStream = new FileStream(downloadPath.Trim() + "\\" + fileName, FileMode.Create);
                    int Length = 2048;
                    Byte[] buffer = new Byte[Length];
                    int bytesRead = responseStream.Read(buffer, 0, Length);
                    while (bytesRead > 0)
                    {
                        writeStream.Write(buffer, 0, bytesRead);
                        bytesRead = responseStream.Read(buffer, 0, Length);
                    }
                }
                catch { }
                finally
                {
                    reqFTP.Abort();
                    response.Close();
                    writeStream.Close();
                    responseStream.Close();

                    if (deleteAfterDownload)
                    {
                        //Deleting the file from the FTP
                        FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + FTPPath.Trim() + "/" + fileName));
                        requestFileDelete.Credentials = new NetworkCredential(uName.Trim(), pwd.Trim());
                        requestFileDelete.Proxy = null;

                        requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;

                        try
                        {
                            FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                        }
                        catch { }

                        requestFileDelete.Abort();
                    }
                }
            }
        }
        #endregion
    }
}
