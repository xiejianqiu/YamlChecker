using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YAMLCheckerWin
{
    public abstract class BaseChecker : IChecker {
        /// <summary>
        /// 保存log
        /// </summary>
        private List<string> logOut = new List<string>();
        protected string mRoot;
        protected bool mPrintLog;
        protected string mPattern = "";
        /// <summary>
        /// 需要检查的文件
        /// </summary>
        protected string[] files;
        public BaseChecker(string _root, string _pattern, bool _printLog)
        {
            this.mRoot = _root;
            this.mPrintLog = _printLog;
            mPattern = _pattern;
            this.Init(mPattern);
        }
        /// <summary>
        /// 缓存log
        /// </summary>
        /// <param name="_log"></param>
        protected void ProcessLog(string _log)
        {
            logOut.Add(_log);
            if (mPrintLog)
            {
                Console.WriteLine(_log);
            }
        }
        /// <summary>
        /// 获取log
        /// </summary>
        /// <returns></returns>
        public string GetLog() {
            if (logOut.Count > 0) {
                StringBuilder sb = new StringBuilder();
                foreach (var log in logOut) {
                    sb.AppendLine(log);
                }
                sb.Replace($"{mRoot}\\", "");
                sb.AppendLine();
                return sb.ToString();
            }
            else {
                return "";
            }
        }
        
        public abstract void Execute();
        protected virtual BaseChecker Init(string pattern) {
            files = Directory.GetFiles(this.mRoot, pattern, SearchOption.AllDirectories);
            return this;
        }
        
    }
}
