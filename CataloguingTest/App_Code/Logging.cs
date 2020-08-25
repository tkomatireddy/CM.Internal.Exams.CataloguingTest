//----------------------------------------------------------------------
// <copyright file="Logging.cs" company="EBsco">
//     Copyright (c) EBsco. All rights reserved.
// </copyright>
//----------------------------------------------------------------------

namespace CataloguingTest
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using log4net;
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Filter;
    using log4net.Layout;
    using log4net.Repository.Hierarchy;

    /// <summary>
    /// This Class contains Log
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// assign logger to log object
        /// </summary>
        /// <param name="loggerName">refers class name</param>
        /// <returns>returns I log type object</returns>
        public static ILog AddLogger(Type loggerName)
        {
            ILog log = LogManager.GetLogger(loggerName);
            return log;
        }

        /// <summary>
        /// set specific level to specific logger object
        /// </summary>
        /// <param name="loggerName">refers class name</param>
        /// <param name="levelName">refers level name like "error</param>
        public static void SetLevel(Type loggerName, string levelName)
        {
            ILog log = LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;

            l.Level = l.Hierarchy.LevelMap[levelName];
        }

        /// <summary>
        /// update details with use of minimum and maximum level of log
        /// </summary>
        /// <param name="minlevel">refers level name like "warn"</param>
        /// <param name="maxlevel">refers level name like "error"</param>
        public static void UpdateAppender(Level minlevel, Level maxlevel)
        {
            var hierarchy = LogManager.GetRepository() as log4net.Repository.Hierarchy.Hierarchy;
            if (hierarchy != null)
            {
                var appenders = hierarchy.GetAppenders();
                foreach (IAppender appender in appenders)
                {
                    var appenderSkeleton = appender as AppenderSkeleton;
                    if (appenderSkeleton != null)
                    {
                        IFilter filterHead = appenderSkeleton.FilterHead;
                        log4net.Filter.LevelRangeFilter filter = (log4net.Filter.LevelRangeFilter)filterHead;
                        filter.LevelMax = minlevel;
                        filter.LevelMin = maxlevel;
                        filter.ActivateOptions();
                        ((log4net.Appender.AppenderSkeleton)appender).ActivateOptions();
                    }
                }
            }
        }

        /// <summary>
        /// configure log4net details from config file
        /// </summary>
        public static void XmlConfigure()
        {
            SqlHelper sqlHelper = new SqlHelper();
            var log4netxml = sqlHelper.ExecuteScalar("select ConfigurationValue from Configuration where ConfigurationKey='Log4netXML'").ToString();
            log4netxml = log4netxml.Replace("<connectionstring>", ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString);
            MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(log4netxml));
            log4net.Config.XmlConfigurator.Configure(memoryStream);
            memoryStream.Close();
        }

        /// <summary>
        /// connectionString is the Web.config's connectionString, which I wanted to share.
        /// </summary>
        public static void SetUpDbConnection()
        {
            ////update connection string for log4net dynamically
            var hier = LogManager.GetRepository() as Hierarchy;
            if (hier != null)
            {
                var adoNetAppenders = hier.GetAppenders().OfType<AdoNetAppender>();
                foreach (var adoNetAppender in adoNetAppenders)
                {
                    adoNetAppender.ConnectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
                    adoNetAppender.ActivateOptions();
                }
            }
        }
    }
}
