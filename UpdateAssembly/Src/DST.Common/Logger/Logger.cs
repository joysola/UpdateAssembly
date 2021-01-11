using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Text;
using System.Web;

namespace DST.Common.Logger
{
    public static class Logger
    {
        private static ILog log
        {
            get;
            set;
        }

        public static bool IsLoggerEnabled = true;

        static Logger()
        {
            FileInfo fileInfo = null;
            fileInfo = ((System.Web.HttpContext.Current == null) ? new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")) : new FileInfo(Path.Combine(HttpContext.Current.Server.MapPath("~/"), "log4net.config")));
            if (!fileInfo.Exists)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<log4net>").AppendLine();
                stringBuilder.Append("  <!--For each day only the last 10 files of 1MB will be kept.-->").AppendLine();
                stringBuilder.Append("  <appender name=\"RollingLogFileAppender\" type=\"log4net.Appender.RollingFileAppender\">").AppendLine();
                stringBuilder.Append("    <param name=\"File\" value=\"logs\\\" />").AppendLine();
                stringBuilder.Append("    <param name=\"AppendToFile\" value=\"true\" />").AppendLine();
                stringBuilder.Append("    <param name=\"MaxSizeRollBackups\" value=\"10\" />").AppendLine();
                stringBuilder.Append("    <param name=\"StaticLogFileName\" value=\"false\" />").AppendLine();
                stringBuilder.Append("    <param name=\"DatePattern\" value=\"yyyy-MM-dd&quot;.txt&quot;\" />").AppendLine();
                stringBuilder.Append("    <param name=\"RollingStyle\" value=\"Date\" />").AppendLine();
                stringBuilder.Append("    <layout type=\"log4net.Layout.PatternLayout\">").AppendLine();
                stringBuilder.Append("      <param name=\"Header\" value=\"========[Header]========&#13;&#10;\"/>").AppendLine();
                stringBuilder.Append("      <param name=\"ConversionPattern\" value=\"%n%d   [%t]   %-5p - %m%n\" />").AppendLine();
                stringBuilder.Append("    </layout>").AppendLine();
                stringBuilder.Append("  </appender>").AppendLine();
                stringBuilder.Append("  <root>").AppendLine();
                stringBuilder.Append("    <level value=\"ALL\" />").AppendLine();
                stringBuilder.Append("    <appender-ref ref=\"RollingLogFileAppender\" />").AppendLine();
                stringBuilder.Append("  </root>").AppendLine();
                stringBuilder.Append("</log4net>").AppendLine();
                byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
                FileStream fileStream = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Flush();
                fileStream.Close();
                stringBuilder.Clear();
            }
            XmlConfigurator.ConfigureAndWatch(fileInfo);
            log = LogManager.GetLogger("root");
        }

        public static void Debug(this string message)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Debug((object)message);
            }
        }

        public static void Debug(this string message, Exception exception)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Debug((object)message, exception);
            }
        }

        public static void DebugFormat(this string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.DebugFormat(format, args);
            }
        }

        public static void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.DebugFormat(formatProvider, format, args);
            }
        }

        public static void Info(this string message)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Info((object)message);
            }
        }

        public static void Info(this string message, Exception exception)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Info((object)message, exception);
            }
        }

        public static void InfoFormat(this string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.InfoFormat(format, args);
            }
        }

        public static void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.InfoFormat(formatProvider, format, args);
            }
        }

        public static void Warn(this string message)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Warn((object)message);
            }
        }

        public static void Warn(this string message, Exception exception)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Warn((object)message, exception);
            }
        }

        public static void WarnFormat(this string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.WarnFormat(format, args);
            }
        }

        public static void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.WarnFormat(formatProvider, format, args);
            }
        }

        public static void Error(this string message)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Error((object)message);
            }
        }

        public static void Error(this string message, Exception exception)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.Error((object)message, exception);
            }
        }

        public static void ErrorFormat(this string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.WarnFormat(format, args);
            }
        }

        public static void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsLoggerEnabled && log != null)
            {
                log.WarnFormat(formatProvider, format, args);
            }
        }

        public static void Fatal(this string message)
        {
            if (log != null)
            {
                log.Fatal((object)message);
            }
        }

        public static void Fatal(this string message, Exception exception)
        {
            if (log != null)
            {
                log.Fatal((object)message, exception);
            }
        }

        public static void FatalFormat(this string format, params object[] args)
        {
            if (log != null)
            {
                log.FatalFormat(format, args);
            }
        }

        public static void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (log != null)
            {
                log.FatalFormat(formatProvider, format, args);
            }
        }
    }
}