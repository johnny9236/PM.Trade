using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PM.Utils.FileHelp
{
    /// <summary>
    /// 自定义配置文件
    /// </summary>
    public class CustomConfigFile  // 本类是将ini文件的配置信息导入到一静态类
    {
        private List<string> configName = new List<string>(); //名称集合

        private List<string> configValue = new List<string>(); //数值集合

        private string conFilePath;   // 配置文件的目录

        public string ConFilePath    //访问器和设置器
        {
            set { this.conFilePath = value; }
            get { return this.conFilePath; }
        }

        public CustomConfigFile(string conFilePath)       //构造函数
        {
            this.conFilePath = conFilePath;
            ReadConfig();

        }

        public enum ConfigFile { newFile, appendFile } // 写入文件属性

        public bool ReadConfig()    //读取配置文件的属性值
        {
            //检查配置文件是否存在
            if (!File.Exists(this.conFilePath))
            {
                return false;
            }

            StreamReader sr = new StreamReader(this.conFilePath, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                string cName, cValue;
                string[] cLine = line.Split('=');
                if (cLine.Length == 2)
                {
                    cName = cLine[0].ToLower();
                    cValue = cLine[1].ToLower();
                    configName.Add(cName);
                    configValue.Add(cValue);
                }

            }
            sr.Close();
            return true;

        }

        public string GetStringValue(string cName)  //返回字符串值
        {

            for (int i = 0; i < configName.Count; i++)
            {
                if (configName[i].Equals(cName.ToLower()))
                {
                    return configValue[i];

                }
            }

            return null;
        }


        public int GetIntValue(string cName)  //返回ini值
        {

            for (int i = 0; i < configName.Count; i++)
            {
                if (configName[i].Equals(cName.ToLower()))
                {
                    int result;
                    if (int.TryParse(configValue[i], out result))
                    {
                        return result;
                    }

                }
            }

            return 0;
        }


        public float GetFloatValue(string cName) //返回float值
        {

            for (int i = 0; i < configName.Count; i++)
            {
                if (configName[i].Equals(cName.ToLower()))
                {
                    float result;
                    if (float.TryParse(configValue[i], out result))
                    {
                        return result;
                    }

                }
            }

            return 0;
        }


        public void SetConfigValue(string cName, string cValue) //设置值
        {
            bool ishere = false;

            //检查是否已经存在.
            if (configName.Count != 0)
            {
                for (int i = 0; i < configName.Count; i++)
                {
                    if (configName[i].Equals(cName.ToLower()))
                    {
                        configValue[i] = cValue;
                        ishere = true;
                    }
                }

            }
            if (!ishere)
            {
                configName.Add(cName);
                configValue.Add(cValue);
            }

        }

        public bool WriteConfigToFile(ConfigFile cf) //保存内存数据到文件
        {
            StreamWriter sw;

            switch (cf)
            {
                case ConfigFile.newFile:
                    {

                        sw = new StreamWriter(this.conFilePath, false);

                        break;
                    };
                case ConfigFile.appendFile:
                    {
                        sw = new StreamWriter(this.conFilePath, true);
                        break;
                    }
                default:
                    {
                        sw = new StreamWriter(this.conFilePath);
                        break;
                    }
            }
            try
            {
                for (int i = 0; i < configName.Count; i++)
                {
                    sw.WriteLine("{0}={1}", configName[i].ToLower(), configValue[i]);
                }
            }

            catch
            {
                return false;

            }
            finally
            {
                sw.Close();
            }
            return true;

        }

    }
}
