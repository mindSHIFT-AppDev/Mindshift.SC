using log4net.spi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using Sitecore.Configuration;
using log4net.helpers;

using SharpCompress.Common;
using SharpCompress.Writers;

// Note: a work in progress. Trying to find a way to write the logs directly to a zip.


namespace log4net.Appender
{
  /// <summary>SitecoreLogFileAppender</summary>
  public class ZipLogFileAppender : FileAppender
  {
    private DateTime m_currentDate;
    private string m_originalFileName;

    /// ---------------------------------------------------------------
    ///              <summary></summary>
    /// 
    ///             ---------------------------------------------------------------
    public override string File
    {
      get
      {
        return base.File;
      }
      set
      {
        if (this.m_originalFileName == null)
        {
          string str = value;
          Dictionary<string, string> variables = ConfigReader.GetVariables();
          foreach (string key in variables.Keys)
          {
            string oldValue = "$(" + key + ")";
            str = str.Replace(oldValue, variables[key]);
          }
          this.m_originalFileName = this.MapPath(str.Trim());
        }
        base.File = this.m_originalFileName;
      }
    }

    /// <summary></summary>
    public ZipLogFileAppender()
    {
      this.m_currentDate = DateTime.Now;
    }

    /// <summary></summary>
    protected override void Append(LoggingEvent loggingEvent)
    {
      DateTime now = DateTime.Now;
      if ((this.m_currentDate.Day != now.Day || this.m_currentDate.Month != now.Month ? 1 : (this.m_currentDate.Year != now.Year ? 1 : 0)) != 0)
      {
        lock (this)
        {
          this.CloseFile();
          this.m_currentDate = DateTime.Now;
          this.OpenFile(string.Empty, false);
        }
      }
      base.Append(loggingEvent);
    }

    /// <summary>
    /// Sets and <i>opens</i> the file where the log output will
    /// go. The specified file must be writable.
    /// </summary>
    /// <param name="fileName">The path to the log file</param>
    /// <param name="append">If true will append to fileName. Otherwise will truncate fileName</param>
    /// <remarks>
    /// 	<para>If there was already an opened file, then the previous file
    /// is closed first.</para>
    /// 	<para>This method will ensure that the directory structure
    /// for the <paramref name="fileName" /> specified exists.</para>
    /// </remarks>
    protected override void OpenFile(string fileName, bool append)
    {
      fileName = this.m_originalFileName;
      fileName = fileName.Replace("{date}", this.m_currentDate.ToString("yyyyMMdd"));
      fileName = fileName.Replace("{time}", this.m_currentDate.ToString("HHmmss"));
      fileName = fileName.Replace("{processid}", ZipLogFileAppender.GetCurrentProcessId().ToString());
			//fileName = fileName.Replace(".txt", ".zip");
      if (System.IO.File.Exists(fileName))
        fileName = this.GetTimedFileName(fileName);


      lock (this)
      {
        this.Reset();
        LogLog.Debug("FileAppender: Opening file for writing [" + fileName + "] append [" + append.ToString() + "]");
        Directory.CreateDirectory(new FileInfo(fileName).DirectoryName);

				// TODO: one zip per day?

				using (var zip = System.IO.File.OpenWrite(fileName)) {
					using (var zipWriter = WriterFactory.Open(zip, ArchiveType.Zip, CompressionType.Deflate)) {
						zipWriter.Write(Path.GetFileName(fileName.Replace(".txt", ".zip")), fileName);
					}
					zip.Dispose();
				}


        //this.SetQWForFiles((TextWriter) new StreamWriter(fileName, append, this.m_encoding));
        this.WriteHeader();
      }

    }

    /// <summary>Gets the name of the sequenced file.</summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns></returns>
    private string GetTimedFileName(string fileName)
    {
      int num = fileName.LastIndexOf('.');
      if (num < 0)
        return fileName;
      return fileName.Substring(0, num) + "." + this.m_currentDate.ToString("HHmmss") + fileName.Substring(num);
    }

    /// <summary>Determines whether the specified file name is locked.</summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>
    /// 	<c>true</c> if the specified file name is locked; otherwise, <c>false</c>.
    /// </returns>
    private bool IsLocked(string fileName)
    {
      if (!System.IO.File.Exists(fileName))
        return false;
      try
      {
        FileStream fileStream = System.IO.File.OpenWrite(fileName);
        if (fileStream == null)
          return true;
        fileStream.Close();
        return false;
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        return true;
      }
    }

    /// ---------------------------------------------------------------
    ///              <summary></summary>
    /// 
    ///             ---------------------------------------------------------------
    public static string MakePath(string part1, string part2, char separator)
    {
      if (part1 == null || part1.Length == 0)
      {
        if (part2 == null)
          return string.Empty;
        return part2;
      }
      if (part2 == null || part2.Length == 0)
      {
        if (part1 == null)
          return string.Empty;
        return part1;
      }
      if ((int) part1[part1.Length - 1] == (int) separator)
        part1 = part1.Substring(0, part1.Length - 1);
      if ((int) part2[0] == (int) separator)
        part2 = part2.Substring(1);
      return part1 + separator.ToString() + part2;
    }

    /// ---------------------------------------------------------------
    ///              <summary></summary>
    /// 
    ///             ---------------------------------------------------------------
    private string MapPath(string fileName)
    {
      if (fileName == "" || fileName.IndexOf(":/") >= 0 || fileName.IndexOf("://") >= 0)
        return fileName;
      int index = fileName.IndexOfAny(new char[2]
      {
        '\\',
        '/'
      });
      if (index >= 0 && (int) fileName[index] == 92)
        return fileName.Replace('/', '\\');
      fileName = fileName.Replace('\\', '/');
      if (HttpContext.Current != null)
        return HttpContext.Current.Server.MapPath(fileName);
      if ((int) fileName[0] == 47)
        return ZipLogFileAppender.MakePath(HttpRuntime.AppDomainAppPath, fileName.Replace('/', '\\'), '\\');
      return fileName;
    }

    /// ---------------------------------------------------------------
    ///              <summary></summary>
    /// 
    ///             ---------------------------------------------------------------
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern int GetCurrentProcessId();
  }
}
