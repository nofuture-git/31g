using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using Environment = NHibernate.Cfg.Environment;

namespace NoFuture.Hbm
{
    public class Globals
    {
        /// <summary>
        /// Various string literals as constants
        /// </summary>
        public static class HbmXmlNames
        {
            public const string ID = "Id";

            internal const string ANSI_STRING = "AnsiString";
            internal const string IDENTITY = "identity";
            internal const string ASSIGNED = "assigned";
            internal const string PROPERTY = "property";
            internal const string ALL_DELETE_ORPHAN = "all-delete-orphan";
            internal const string KEY_PROPERTY = "key-property";

            internal const string HIBERNATE_MAPPING = "hibernate-mapping";
            internal const string HIBERNATE_CONFIGURATION = "hibernate-configuration";
            internal const string SESSION_FACTORY = "session-factory";
            internal const string CLASS = "class";
            internal const string NAME = "name";
            internal const string TABLE = "table";
            internal const string SCHEMA = "schema";
            internal const string COMPOSITE_ID = "composite-id";
            internal const string KEY_MANY_TO_ONE = "key-many-to-one";
            internal const string COLUMN = "column";
            internal const string TYPE = "type";
            internal const string LENGTH = "length";
            internal const string NOT_NULL = "not-null";
            internal const string COMMENT = "comment";
            internal const string GENERATOR = "generator";
            internal const string MANY_TO_ONE = "many-to-one";
            internal const string BAG = "bag";
            internal const string CASCADE = "cascade";
            internal const string INVERSE = "inverse";
            internal const string LAZY = "lazy";
            internal const string BATCH_SIZE = "batch-size";
            internal const string KEY = "key";
            internal const string FOREIGN_KEY = "foreign-key";
            internal const string ONE_TO_MANY = "one-to-many";
            internal const string SQL_QUERY = "sql-query";
            internal const string CALLABLE = "callable";
            internal const string RETURN = "return";
            internal const string ALIAS = "alias";
            internal const string RETURN_PROPERTY = "return-property";
            internal const string SECTION = "section";
            internal const string MAPPING = "mapping";
            internal const string ASSEMBLY = "";
        }

        public const string COMP_KEY_ID_SUFFIX = "CompositeId";
        public const string HBM_XML_NS = "urn:nhibernate-mapping-2.2";

        internal const string HBM_CFG_CONFIG_SECTION_HANDLER = "NHibernate.Cfg.ConfigurationSectionHandler, NHibernate";
        internal const string LOG_NAME = "__NoFuture.Hbm.log";
        internal const string REPRESENT_512 = "512";

        /// <summary>
        /// This is the name of the folder joined off <see cref="Settings.HbmDirectory"/> which acts 
        /// as the working folder of the creation of stored prox .bin, .xsd, .hbm.xml and .cs
        /// files.
        /// </summary>
        public const string STORED_PROX_FOLDER_NAME = "Prox";

        public const string DEFAULT_SCHEMA_NAME = "Dbo";

        public const int MSSQL_MAX_VARCHAR = 8000;

        public const string DF_DELIMITER_START = "--Start-Nf.Hbm.Delimiter";
        public const string DF_DELIMITER_END = "--End-Nf.Hbm.Delimiter";

        /// <summary>
        /// The <see cref="NHibernate.Cfg.Configuration"/> will blow outs its stack if you 
        /// just call 'AddXmlFile' one at a time so the PowerShell cmdlets will compile the 
        /// hbm.xml files into binary assemblies to be loaded instead.
        /// </summary>
        public const int COMPILE_HBM_XML_DLL_OF_KB_SIZE = 64;

        /// <summary>
        /// A string used in the middle part of the dll's produced while embedding
        /// hbm.xml files.
        /// </summary>
        public const string HBM_XML_DLL_MIDDLE_NAME = "HbmXml";

        /// <summary>
        /// Naming conventions of tables and prox is much looser than classes and types.
        /// Simply removing the spaces may cause name collisions so this is a 
        /// worked around to replace the space with its hex rep.
        /// </summary>
        public const string REPLACE_SPACE_WITH_SEQUENCE = "x20";

        /// <summary>
        /// The EF 6.x Fluent mappings require the precision to be defined.
        /// </summary>
        public static List<string> MssqlTypesWithPrecision = new List<string>
        {
            "money",
            "real",
            "decimal",
            "double",
            "float"
        };

        public static Configuration HbmCfg { get; set; }

        public static ISessionFactory HbmSessionFactory { get; set; }

        public static Dictionary<string, string> PrintCurrentHbmCfgProps
        {
            get
            {
                if (HbmCfg == null)
                    return null;

                var allHbmEnvrionProps = new String[]
                {
                    Environment.BatchSize, Environment.BatchStrategy, Environment.BatchVersionedData,
                    Environment.CacheDefaultExpiration, Environment.CacheProvider, Environment.CacheRegionPrefix,
                    Environment.CollectionTypeFactoryClass, Environment.CommandTimeout, Environment.ConnectionDriver,
                    Environment.ConnectionProvider, Environment.ConnectionStringName, Environment.ConnectionString,
                    Environment.CurrentSessionContextClass, Environment.DefaultBatchFetchSize,
                    Environment.DefaultCatalog,
                    Environment.DefaultEntityMode, Environment.DefaultSchema, Environment.Dialect, Environment.FormatSql,
                    Environment.GenerateStatistics, Environment.Hbm2ddlAuto, Environment.Hbm2ddlKeyWords,
                    Environment.Isolation,
                    Environment.LinqToHqlGeneratorsRegistry, Environment.MaxFetchDepth, Environment.OrderInserts,
                    Environment.OutputStylesheet, Environment.PreferPooledValuesLo, Environment.PrepareSql,
                    Environment.PropertyBytecodeProvider, Environment.PropertyUseReflectionOptimizer,
                    Environment.ProxyFactoryFactoryClass,
                    Environment.QueryCacheFactory, Environment.QueryImports, Environment.QueryStartupChecking,
                    Environment.QuerySubstitutions, Environment.QueryTranslator, Environment.ReleaseConnections,
                    Environment.SessionFactoryName, Environment.ShowSql, Environment.SqlExceptionConverter,
                    Environment.StatementFetchSize, Environment.TransactionManagerStrategy,
                    Environment.TransactionStrategy,
                    Environment.UseGetGeneratedKeys, Environment.UseIdentifierRollBack, Environment.UseMinimalPuts,
                    Environment.UseProxyValidator, Environment.UseQueryCache, Environment.UseSecondLevelCache,
                    Environment.UseSqlComments, Environment.WrapResultSets
                };

                return allHbmEnvrionProps.ToDictionary(configString => configString, configString => HbmCfg.GetProperty(configString));
            }
        }

        public static void UnloadHbmDb(params ISession[] hbmSessions)
        {
            if (hbmSessions != null && hbmSessions.Length > 0)
            {
                foreach (var session in hbmSessions.Where(session => session != null))
                {
                    if (session.IsOpen)
                        session.Close();
                    session.Dispose();
                }
            }
            if (HbmSessionFactory != null)
            {
                if (!HbmSessionFactory.IsClosed)
                    HbmSessionFactory.Close();
                HbmSessionFactory.Dispose();
            }

            HbmCfg = null;
            HbmSessionFactory = null;

        }
    }
}
