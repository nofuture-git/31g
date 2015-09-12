using System;
using System.IO;
using System.Xml.Linq;
using NoFuture.Shared;
using NoFuture.Util;
using Nm = NoFuture.Hbm.Globals.HbmXmlNames;

namespace NoFuture.Hbm
{
    public class XeFactory
    {
        public static XElement ConfigSection()
        {
            return new XElement(Nm.SECTION,
                                new XAttribute(Nm.NAME, Nm.HIBERNATE_CONFIGURATION),
                                new XAttribute(Nm.TYPE, Globals.HBM_CFG_CONFIG_SECTION_HANDLER));
        }

        public static XElement HibernateConfigurationNode(string connectionString, string outputNamespace)
        {
            outputNamespace = Etc.CapitalizeFirstLetterOfWholeWords(outputNamespace, Constants.DefaultTypeSeparator);
            XNamespace hbmXmlNs = Globals.HBM_XML_NS;
            var hbmConfigNode = new XElement(hbmXmlNs + Nm.HIBERNATE_CONFIGURATION);
            var sessionFactoryNode = new XElement(Nm.SESSION_FACTORY);

            var propertyNode = new XElement(Nm.PROPERTY, new XAttribute(Nm.NAME, NHibernate.Cfg.Environment.ConnectionProvider)) { Value = "NHibernate.Connection.DriverConnectionProvider" };
            sessionFactoryNode.Add(propertyNode);

            propertyNode = new XElement(Nm.PROPERTY, new XAttribute(Nm.NAME, NHibernate.Cfg.Environment.ConnectionDriver)) { Value = "NHibernate.Driver.SqlClientDriver" };
            sessionFactoryNode.Add(propertyNode);

            propertyNode = new XElement(Nm.PROPERTY, new XAttribute(Nm.NAME, NHibernate.Cfg.Environment.Dialect)) { Value = "NHibernate.Dialect.MsSql2008Dialect" };
            sessionFactoryNode.Add(propertyNode);

            propertyNode = new XElement(Nm.PROPERTY, new XAttribute(Nm.NAME, NHibernate.Cfg.Environment.ConnectionString)) { Value = connectionString };

            sessionFactoryNode.Add(propertyNode);

            propertyNode = new XElement(Nm.PROPERTY, new XAttribute(Nm.NAME, NHibernate.Cfg.Environment.Isolation)) { Value = "ReadCommitted" };

            sessionFactoryNode.Add(propertyNode);

            propertyNode = new XElement(Nm.PROPERTY, new XAttribute(Nm.NAME, NHibernate.Cfg.Environment.CommandTimeout)) { Value = "30" };

            sessionFactoryNode.Add(propertyNode);

            propertyNode = new XElement(Nm.PROPERTY, new XAttribute(Nm.NAME, NHibernate.Cfg.Environment.MaxFetchDepth)) { Value = "3" };

            sessionFactoryNode.Add(propertyNode);

            var mappingNode = new XElement(Nm.MAPPING, new XAttribute(Nm.ASSEMBLY, TypeName.DraftCscExeAsmName(outputNamespace)));
            sessionFactoryNode.Add(mappingNode);

            hbmConfigNode.Add(sessionFactoryNode);

            return hbmConfigNode;

        }

        public static XElement HibernateMappingNode()
        {
            return new XElement(XName.Get(Globals.HbmXmlNames.HIBERNATE_MAPPING));
        }

        public static XElement ClassNode(string className, string tableName, string schemaName)
        {
            if (String.IsNullOrWhiteSpace(tableName))
                return new XElement(Globals.HbmXmlNames.CLASS,
                    new XAttribute(Globals.HbmXmlNames.NAME, className));

            return new XElement(Globals.HbmXmlNames.CLASS,
                new XAttribute(Globals.HbmXmlNames.NAME, className),
                new XAttribute(Globals.HbmXmlNames.TABLE, String.Format("[{0}]", tableName)),
                new XAttribute(Globals.HbmXmlNames.SCHEMA, String.Format("[{0}]", schemaName)));
        }

        public static XElement CompositeIdNode(string name, string className)
        {
            return new XElement(Globals.HbmXmlNames.COMPOSITE_ID,
                new XAttribute(Globals.HbmXmlNames.NAME, name),
                new XAttribute(Globals.HbmXmlNames.CLASS, className));
        }

        public static XElement KeyManyToOneNode(string name, string className)
        {
            return new XElement(Globals.HbmXmlNames.KEY_MANY_TO_ONE,
                new XAttribute(Globals.HbmXmlNames.NAME, name),
                new XAttribute(Globals.HbmXmlNames.CLASS, className));
        }

        public static XElement ColumnNode(string name, string comment)
        {
            var columnXe = new XElement(Globals.HbmXmlNames.COLUMN,
                new XAttribute(Globals.HbmXmlNames.NAME, String.Format("[{0}]", name)));
            if (string.IsNullOrWhiteSpace(comment))
                return columnXe;

            var commentXe = new XElement(Globals.HbmXmlNames.COMMENT);
            var commentCdata = new XCData(comment);
            commentXe.Add(commentCdata);
            columnXe.Add(commentXe);
            return columnXe;
        }

        public static XElement KeyPropertyNode(string name, string columnName, string type, string length, string comment)
        {
            var xe = new XElement(Globals.HbmXmlNames.KEY_PROPERTY,
                new XAttribute(Globals.HbmXmlNames.NAME, name),
                new XAttribute(Globals.HbmXmlNames.TYPE, type),
                new XAttribute(Globals.HbmXmlNames.LENGTH, length));
            xe.Add(ColumnNode(columnName, comment));
            return xe;
        }

        public static XElement KeyPropertyNode(string name, string columnName, string type, string comment)
        {
            var xe = new XElement(Globals.HbmXmlNames.KEY_PROPERTY,
                new XAttribute(Globals.HbmXmlNames.NAME, name),
                new XAttribute(Globals.HbmXmlNames.TYPE, type));
            xe.Add(ColumnNode(columnName, comment));
            return xe;
        }

        public static XElement IdNode(string name, string columnName, string typeName, string length, string comment)
        {
            var xe = new XElement(Globals.HbmXmlNames.ID.ToLower(),
                new XAttribute(Globals.HbmXmlNames.NAME, name),
                new XAttribute(Globals.HbmXmlNames.TYPE, typeName),
                new XAttribute(Globals.HbmXmlNames.LENGTH, length));

            xe.Add(ColumnNode(columnName, comment));
            return xe;
        }

        public static XElement IdNode(string name, string columnName, string typeName, string comment)
        {
            var xe = new XElement(Globals.HbmXmlNames.ID.ToLower(),
                new XAttribute(Globals.HbmXmlNames.NAME, name),
                new XAttribute(Globals.HbmXmlNames.TYPE, typeName));
            xe.Add(ColumnNode(columnName, comment));
            return xe;
        }

        public static XElement IdNode(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                name = String.Format("{0}__{1}", Globals.HbmXmlNames.ID, Path.GetRandomFileName().Replace(".", "_"));
            return new XElement(Globals.HbmXmlNames.ID.ToLower(),
                new XAttribute(Globals.HbmXmlNames.NAME, name),
                new XAttribute(Globals.HbmXmlNames.TYPE, "Guid"),
                new XAttribute(Globals.HbmXmlNames.GENERATOR, "guid"));
        }

        public static XElement GeneratorId(string generatorTypeName)
        {
            return new XElement(Globals.HbmXmlNames.GENERATOR,
                new XAttribute(Globals.HbmXmlNames.CLASS, generatorTypeName));
        }

        public static XElement ManyToOneNode(string name, string className)
        {
            return new XElement(Globals.HbmXmlNames.MANY_TO_ONE,
                new XAttribute(Globals.HbmXmlNames.NAME, name),
                new XAttribute(Globals.HbmXmlNames.CLASS, className));
        }

        public static XElement BagNode(string name, string cascadeName, string isInverse, string isLazy, string batchSize)
        {
            return new XElement(Globals.HbmXmlNames.BAG,
                new XAttribute(Globals.HbmXmlNames.NAME, name),
                new XAttribute(Globals.HbmXmlNames.CASCADE, cascadeName),
                new XAttribute(Globals.HbmXmlNames.INVERSE, isInverse),
                new XAttribute(Globals.HbmXmlNames.LAZY, isLazy),
                new XAttribute(Globals.HbmXmlNames.BATCH_SIZE, batchSize));
        }

        public static XElement KeyNodeClassName(string className)
        {
            return new XElement(Globals.HbmXmlNames.KEY,
                new XAttribute(Globals.HbmXmlNames.FOREIGN_KEY, className));
        }

        public static XElement KeyNodeColumnName(string columnName, string comment)
        {
            var xe = new XElement(Globals.HbmXmlNames.KEY);
            xe.Add(ColumnNode(columnName, comment));
            return xe;
        }

        public static XElement OneToManyNode(string className)
        {
            return new XElement(Globals.HbmXmlNames.ONE_TO_MANY,
                new XAttribute(Globals.HbmXmlNames.CLASS, className));
        }

        public static XElement PropertyNode(string nodeName, string name, string columnName, string typeName, string length, bool isnullable, string comment)
        {
            var notNullXe = new XAttribute(Globals.HbmXmlNames.NOT_NULL,
                isnullable ? Boolean.FalseString.ToLower() : Boolean.TrueString.ToLower());

            if (String.IsNullOrWhiteSpace(columnName) && String.IsNullOrWhiteSpace(length))
                return new XElement(nodeName, new XAttribute(Globals.HbmXmlNames.NAME, name),
                    new XAttribute(Globals.HbmXmlNames.TYPE, typeName), notNullXe);

            if (String.IsNullOrWhiteSpace(columnName))
                return new XElement(nodeName, new XAttribute(Globals.HbmXmlNames.NAME, name),
                    new XAttribute(Globals.HbmXmlNames.TYPE, typeName),
                    new XAttribute(Globals.HbmXmlNames.LENGTH, length),
                    notNullXe);

            if (String.IsNullOrWhiteSpace(length))
            {
                var xxe = new XElement(nodeName,
                    new XAttribute(Globals.HbmXmlNames.NAME, name),
                    new XAttribute(Globals.HbmXmlNames.TYPE, typeName),
                    notNullXe);
                xxe.Add(ColumnNode(columnName, comment));
                return xxe;
            }
            {
                var xe = new XElement(nodeName,
                    new XAttribute(Globals.HbmXmlNames.NAME, name),
                    new XAttribute(Globals.HbmXmlNames.TYPE, typeName),
                    new XAttribute(Globals.HbmXmlNames.LENGTH, length),
                    notNullXe);
                xe.Add(ColumnNode(columnName, comment));
                return xe;
            }
        }

        public static XElement SqlQueryNode(string storedProcName, bool isCallable = true)
        {
            var callableStr = isCallable.ToString().ToLower();
            return new XElement(Globals.HbmXmlNames.SQL_QUERY,
                new XAttribute(Globals.HbmXmlNames.NAME, storedProcName),
                new XAttribute(Globals.HbmXmlNames.CALLABLE, callableStr));
        }

        public static XElement ReturnNode(string alias, string typename)
        {
            return new XElement(Globals.HbmXmlNames.RETURN, new XAttribute(Globals.HbmXmlNames.ALIAS, alias),
                new XAttribute(Globals.HbmXmlNames.CLASS, typename));
        }

        public static XElement ReturnPropertyNode(string columnName, string propertyName)
        {
            return new XElement(Globals.HbmXmlNames.RETURN_PROPERTY,
                new XAttribute(Globals.HbmXmlNames.COLUMN, String.Format("[{0}]", columnName)),
                new XAttribute(Globals.HbmXmlNames.NAME, propertyName));
        }
    }
}
