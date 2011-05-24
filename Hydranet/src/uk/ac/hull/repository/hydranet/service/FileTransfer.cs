using System;
using System.Net;
using System.IO;
using uk.ac.hull.repository.hydranet.serviceref.fedoramanagement;
using uk.ac.hull.repository.hydranet.fedora;

namespace uk.ac.hull.repository.hydranet.service
{
    public class FileTransfer
    {

        private string ftpServerIP;
        private string ftpServerPort;
        private string ftpUsername;
        private string ftpPassword;
        private string baseFilePath;
        private string httpAccessURL;

        public FileTransfer(string ftpServerIP, int ftpServerPort, string ftpUsername, string ftpPassword, string baseFilePath, string httpAccessURL )
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpServerPort = ftpServerPort.ToString();
            this.ftpUsername = ftpUsername;
            this.ftpPassword = ftpPassword;
            this.baseFilePath = baseFilePath;
            this.httpAccessURL = httpAccessURL;

        }

        public string Upload(string file, string remoteFilePath, string remoteFileName, out long fileSize)
        {
            FileInfo fileInfo = new FileInfo(file);
            fileSize = fileInfo.Length;
            string uri = "ftp://" + ftpServerIP + ":" + ftpServerPort + "/" + baseFilePath + remoteFilePath + "/" + remoteFileName;

            //Make sure the directory exists
            MakeDirectory(remoteFilePath);

            //Create FtpWebRequest object from the Uri provided
            FtpWebRequest reqFtp;

            reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

            //Provide the WebPermission Credentials
            reqFtp.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
            //Specify the command to be executed
            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
    
            reqFtp.Proxy = null;
            reqFtp.UseBinary = true;  //Specify the data transfer type.
            reqFtp.UsePassive = true;
            reqFtp.KeepAlive = false; //By default KeepAlive is true, where the control connection is not closed after a command is executed.

            //Notify the server about the size of the uploaded file
            reqFtp.ContentLength = fileInfo.Length;

            //The buffer size is set to 2kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLength;

            //Opens a file stream (System.IO.Filestream) to read the file to be uploaded
            FileStream fs = fileInfo.OpenRead();

            try {
                //Stream to which the file to be upload is written
                Stream stream = reqFtp.GetRequestStream();

                //Read from the file stream 2kb at a time
                contentLength = fs.Read(buff, 0, buffLength);

                //Till Stream content ends
                while (contentLength != 0)
                {
                    //Write Content from the file stream to the FTP Upload Stream
                    stream.Write(buff, 0, contentLength);
                    contentLength = fs.Read(buff ,0, buffLength);
                }

                //Close the file stream and the request stream
                stream.Close();
                fs.Close();

                string httpUrl = "http://" + ftpServerIP + "/" + baseFilePath + remoteFilePath + "/" + remoteFileName;

                return httpUrl;

            }
            catch (Exception ex) {
                Console.Write("Upload Error" + ex.Message);
                return "error";
            }
        }
      
        public void Download(string remoteFilePath, string remoteFileName, string localFilePath, string localFileName)
        {
            FtpWebRequest reqFTP;

            try
            {
                FileStream outputStream = new FileStream(localFilePath + "/" + localFileName, FileMode.Create);

                string uri = "ftp://" + ftpServerIP + ":" + ftpServerPort + "/" + remoteFilePath + "/" + remoteFileName;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                reqFTP.Proxy = null;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = true;
                reqFTP.KeepAlive = false;

                //reqFTP.UsePassive = true;
          
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
              
                Stream ftpStream = response.GetResponseStream();

                long cl = response.ContentLength;
                int bufferSize = 2048;

                int readCount;

                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);

                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();


            }
            catch (Exception ex)
            {
                Console.Write("Download Error" + ex.Message);
            }
        }

        public void Delete(string remoteFilePath, string remoteFileName)
        {
            FtpWebRequest reqFTP;

            try
            {
                string uri = "ftp://" + ftpServerIP + ":" + ftpServerPort + "/" + remoteFilePath + "/" + remoteFileName;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                string statusCode = response.StatusCode.ToString();
                            
                response.Close();


            }
            catch (Exception ex)
            {
                Console.Write("Delete Error" + ex.Message);
            }
        }

        public Boolean MakeDirectory(string remoteFilePath)
        {
            FtpWebRequest reqFTP = null;
            FtpWebResponse response = null;

            try
            {
                string uri = "ftp://" + ftpServerIP + ":" + ftpServerPort + "/" + baseFilePath + remoteFilePath;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
      
                reqFTP.Proxy = null;
                reqFTP.UseBinary = false;
                reqFTP.UsePassive = true;
                reqFTP.KeepAlive = false;


                response = (FtpWebResponse)reqFTP.GetResponse();

                string statusCode = response.StatusCode.ToString();

            }
            catch (WebException e)
            {
                //If status code indicates that the folder already exists, we return true anyway...
                if (FtpStatusCode.ActionNotTakenFileUnavailable.Equals(((FtpWebResponse)e.Response).StatusCode)) 
                {
                    return true;

                }
            }

            return true;
        }
    }
}
