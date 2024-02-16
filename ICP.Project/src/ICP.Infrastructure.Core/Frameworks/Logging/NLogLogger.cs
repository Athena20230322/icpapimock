using ICP.Infrastructure.Abstractions.Logging;
using NLog;
using NLog.Config;
using System;
using System.Linq;

namespace ICP.Infrastructure.Core.Frameworks.Logging
{
    public class NLogLogger : Abstractions.Logging.ILogger
    {
        private readonly NLog.ILogger _logger = null;

        #region 建構子

        public NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public NLogLogger(Type type)
        {
            _logger = LogManager.GetLogger(type.FullName);
        }

        public NLogLogger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        #endregion

        #region 設定檔

        /// <summary>
        /// 指定 Config 路徑
        /// </summary>
        /// <param name="configPath"></param>
        public static void SetConfig(string configPath)
        {
            LogManager.Configuration = new XmlLoggingConfiguration(configPath, true);

            //            var dbTarget = new DatabaseTarget("NLog-SqlServer")
            //            {
            //                ConnectionStringName = DatabaseName.TestDatabase,
            //                CommandText = @"
            //EXEC
            //    [dbo].[AddWriteDatabaseLog]
            //        @Logger = @Logger,
            //        @Type = @Type,
            //        @Message = @Message,
            //        @CreateDT = @CreateDT,
            //        @RequestId = @RequestId"
            //            };

            //            dbTarget.Parameters.Add(new DatabaseParameterInfo());

            //            LogManager.Configuration.AddTarget(dbTarget);
        }

        #endregion

        #region 新增Log變數
        /// <summary>
        /// 新增Log變數
        /// </summary>
        /// <param name="variablesName"></param>
        /// <param name="variablesValue"></param>
        public void SetCustomVariables(string variablesName, string variablesValue)
        {
            //### 判斷LogManager.Configuration物件是否為NULL
            if (LogManager.Configuration != null)
            {
                LogManager.Configuration.Variables[variablesName] = variablesValue;
            }
        }
        #endregion

        #region 移除NLog新增的變數
        /// <summary>
        /// 移除NLog新增的變數
        /// </summary>
        /// <param name="variableName"></param>
        public void RemoveCustomVariables(string variableName)
        {
            //### 判斷LogManager.Configuration物件是否為NULL以及物件是否包含該名稱的變數
            if (LogManager.Configuration != null && LogManager.Configuration.Variables.ContainsKey(variableName))
            {
                LogManager.Configuration.Variables.Remove(variableName);
            }
        }
        #endregion

        #region Public

        public void Error(string msg, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(msg) && msg.IndexOf("SqlException") != -1)
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Error(msg);
            }
            else
            {
                _logger.Error(msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        public void Error(Exception ex, string msg, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(msg) && msg.IndexOf("SqlException") != -1)
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Error(ex, msg);
            }
            else
            {
                _logger.Error(ex, msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        public void Info(string msg, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(msg) && msg.IndexOf("SqlException") != -1)
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Info(msg);
            }
            else
            {
                _logger.Info(msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        public void Trace(string msg, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(msg) && msg.IndexOf("SqlException") != -1)
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Trace(msg);
            }
            else
            {
                _logger.Trace(msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        public void Warning(string msg, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(msg) && msg.IndexOf("SqlException") != -1)
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Warn(msg);
            }
            else
            {
                _logger.Warn(msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        public void Fatal(string msg, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(msg) && msg.IndexOf("SqlException") != -1)
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Fatal(msg);
            }
            else
            {
                _logger.Fatal(msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        public void Fatal(Exception ex, string msg, params object[] args)
        {
            string finalMsg = "";
            if (!string.IsNullOrWhiteSpace(msg))
            {
                finalMsg = args.Count() > 0 ? string.Format(msg, args) : msg;
            }

            if ((finalMsg.IndexOf("SqlException") != -1) || (ex!= null && ex.ToString().IndexOf("SqlException") != -1))
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Fatal(ex, msg);
            }
            else
            {
                _logger.Fatal(ex, msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        public void Debug(string msg, params object[] args)
        {
            string finalMsg = "";
            if (!string.IsNullOrWhiteSpace(msg))
            {
                finalMsg = args.Count() > 0 ? string.Format(msg, args) : msg;
            }

            if (finalMsg.IndexOf("SqlException") != -1)
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Debug(msg);
            }
            else
            {
                _logger.Debug(msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        public void Info(Exception ex, string msg, params object[] args)
        {
            string finalMsg = "";
            if (!string.IsNullOrWhiteSpace(msg))
            {
                finalMsg = args.Count() > 0 ? string.Format(msg, args) : msg;
            }

            if ((finalMsg.IndexOf("SqlException") != -1) || (ex != null && ex.ToString().IndexOf("SqlException") != -1))
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Info(ex, msg);
            }
            else
            {
                _logger.Info(ex, msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        public void Warning(Exception ex, string msg, params object[] args)
        {
            string finalMsg = "";
            if (!string.IsNullOrWhiteSpace(msg))
            {
                finalMsg = args.Count() > 0 ? string.Format(msg, args) : msg;
            }

            if ((finalMsg.IndexOf("SqlException") != -1) || (ex != null && ex.ToString().IndexOf("SqlException") != -1))
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Warn(ex, msg);
            }
            else
            {
                _logger.Warn(ex, msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        public void Trace(Exception ex, string msg, params object[] args)
        {
            string finalMsg = "";
            if (!string.IsNullOrWhiteSpace(msg))
            {
                finalMsg = args.Count() > 0 ? string.Format(msg, args) : msg;
            }

            if ((finalMsg.IndexOf("SqlException") != -1) || (ex != null && ex.ToString().IndexOf("SqlException") != -1))
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Trace(ex, msg);
            }
            else
            {
                _logger.Trace(ex, msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        public void Debug(Exception ex, string msg, params object[] args)
        {
            string finalMsg = "";
            if (!string.IsNullOrWhiteSpace(msg))
            {
                finalMsg = args.Count() > 0 ? string.Format(msg, args) : msg;
            }

            if ((finalMsg.IndexOf("SqlException") != -1) || (ex != null && ex.ToString().IndexOf("SqlException") != -1))
            {
                SetCustomVariables("errorType", "1");
            }

            if (args == null || !args.Any())
            {
                _logger.Debug(ex, msg);
            }
            else
            {
                _logger.Debug(ex, msg, args);
            }

            RemoveCustomVariables("errorType");
        }

        #endregion
    }

    public class NLogLogger<T> : NLogLogger, ILogger<T>
    {
        public NLogLogger() : base(typeof(T))
        {
        }
    }
}
