using Microsoft.Extensions.Configuration;
using Sassa.eDocs.CS;
using Sassa.eDocs.CSDocuments;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sassa.eServices.Admin.Services
{
    public class CSService
	{
        private readonly IConfiguration _config;
        private string username;
        private string password;
        private eDocs.CSDocuments.OTAuthentication ota;
        private long NodeId;

        public CSService(IConfiguration config)
        {
            _config = config;
            username = _config.GetValue<string>("ContentServer:User");
            password = _config.GetValue<string>("ContentServer:Password");
        }



        public async Task Authenticate()
        {
            AuthenticationClient authClient = new AuthenticationClient();
            try
            {
                ota = new eDocs.CSDocuments.OTAuthentication();
                ota.AuthenticationToken = await authClient.AuthenticateUserAsync(username, password);
            }
            catch
            {
                throw;
            }
            finally
            {
                await authClient.CloseAsync();
            }
        }

        public async Task UploadDoc(String filename,string filepath)
        {

            if (ota == null)
            {
                await Authenticate();
            }

            Attachment attachment = new Attachment();
            attachment.FileName = filename;
            attachment.Contents = await File.ReadAllBytesAsync(filepath);
            attachment.FileSize = attachment.Contents.Length;
            attachment.CreatedDate = DateTime.Now;

            DocumentManagementClient docClient = new DocumentManagementClient();
            docClient.Endpoint.Binding.SendTimeout = new TimeSpan(0, 3, 0);
            try
            {
                //NodeId = select DATAID from dtree where name='Health Checks - BRM'
                await docClient.CreateDocumentAsync(ota, 94643845, attachment.FileName, "Asset Status Service", false, new Metadata(), attachment);
            }
            catch //(Exception ex)
            {
                throw;
            }
            finally
            {
                await docClient.CloseAsync();
            }
        }

        public async Task<byte[]> ToByteArray(FileStream stream)
        {

            const int MaxFileSize = 4 * 1024 * 1024;
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[MaxFileSize];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = await stream.ReadAsync(readBuffer);
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
