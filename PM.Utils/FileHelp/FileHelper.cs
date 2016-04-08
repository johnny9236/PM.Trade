using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PM.Utils.FileHelp.CustomConfig
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public static class FileHelper
    {
        public static bool SafeCoyFile(string srcPath, string destPath)
        {
            try
            { 
                File.Copy(srcPath, destPath, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string SafeCreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return null;
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public static bool SafeDeleteDirectory(string path)
        {
            try
            {
                Directory.Delete(path, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SafeDeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string SafeGetDirectoryName(string path)
        {
            try
            {
                return Path.GetDirectoryName(path);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string SafeGetFileExtensionName(string path)
        {
            try
            {
                return Path.GetExtension(path);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string SafeGetFileName(string path)
        {
            try
            {
                return Path.GetFileName(path);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string SafeGetFileNameWithoutExtension(string path)
        {
            try
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static byte[] SafeReadBinFile(string path)
        {
            byte[] buffer2;
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    stream.Close();
                    buffer2 = buffer;
                }
            }
            catch
            {
                buffer2 = null;
            }
            return buffer2;
        }

        public static string SafeReadTextFile(string path)
        {
            return SafeReadTextFile(path, Encoding.UTF8);
        }

        public static string SafeReadTextFile(string path, Encoding encoding)
        {
            try
            {
                if (File.Exists(path))
                {
                    return File.ReadAllText(path, encoding);
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string SafeReadTextFileAutoEncoding(string path)
        {
            string str;
            try
            {
                using (StreamReader reader = new StreamReader(path, true))
                {
                    str = reader.ReadToEnd();
                }
            }
            catch
            {
                str = string.Empty;
            }
            return str;
        }

        public static bool SafeTestDirectoryExist(string path)
        {
            try
            {
                return Directory.Exists(path);
            }
            catch
            {
                return false;
            }
        }

        public static bool SafeTestFileCanRead(string filePath)
        {
            bool canRead;
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    canRead = stream.CanRead;
                }
            }
            catch
            {
                canRead = false;
            }
            return canRead;
        }

        public static bool SafeTestFileExist(string path)
        {
            try
            {
                return File.Exists(path);
            }
            catch
            {
                return false;
            }
        }

        public static bool SafeWriteTextFile(string path, string contents)
        {
            return SafeWriteTextFile(path, contents, Encoding.UTF8);
        }

        public static bool SafeWriteTextFile(string path, string contents, Encoding encoding)
        {
            try
            {
                File.WriteAllText(path, contents, encoding);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string UnSafeGetFolderNameFromDirectory(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            if (info.Parent != null)
            {
                return info.Name;
            }
            return string.Empty;
        }
    }
}
