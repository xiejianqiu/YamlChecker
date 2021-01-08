using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
namespace YAMLCheckerWin
{
    public class AppFacade
    {
        static Dictionary<Type, BaseChecker> checkerDict = new Dictionary<Type, BaseChecker>();
        public static void StartUp(string filePath, string logSavePath, bool printlog) {
            if (File.Exists(logSavePath))
            {
                File.Delete(logSavePath);
            }
            AppFacade.Discover(filePath, printlog);
            AppFacade.Execute(logSavePath);
        }
        /// <summary>
        /// 发现类并初始化
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="printlog"></param>
        static void Discover(string filePath, bool printlog) {
            var attrList = new List<CheckerAttribute>();
            var assembly = Assembly.GetCallingAssembly();
            foreach (var type in assembly.DefinedTypes)
            {
                attrList.Clear();
                if (type.IsSubclassOf(typeof(BaseChecker)))
                {
                    attrList.AddRange(type.GetCustomAttributes<CheckerAttribute>());
                    if (null != attrList && attrList.Count > 0)
                    {
                        var customAttr = attrList[0];
                        var checker = Activator.CreateInstance(type, filePath, customAttr.val, printlog) as BaseChecker;
                        checkerDict.Add(type, checker);
                    }
                }
            }
        }
        /// <summary>
        /// 开始执行检查
        /// </summary>
        /// <param name="logSavePath"></param>
        static void Execute(string logSavePath) {
            var iter = checkerDict.GetEnumerator();
            while (iter.MoveNext())
            {
                iter.Current.Value.Execute();
            }
            iter = checkerDict.GetEnumerator();
            string allLog = "";
            while (iter.MoveNext())
            {
                allLog += iter.Current.Value.GetLog();
            }
            SaveLogToFile(logSavePath, allLog);
        }
        /// <summary>
        /// 把log保存到文件
        /// </summary>
        /// <param name="_logSavePath"></param>
        /// <param name="_allLog"></param>
        static void SaveLogToFile(string _logSavePath, string _allLog)
        {
            if (false == string.IsNullOrEmpty(_allLog))
            {
                try
                {
                    var dir_path = Path.GetDirectoryName(_logSavePath);
                    if (false == Directory.Exists(dir_path))
                    {
                        Directory.CreateDirectory(dir_path);
                    }
                    var logs = _allLog.Trim();
                    File.WriteAllLines(_logSavePath, new string[] { logs });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
                Environment.Exit(5);
            }
        }
    }
}
