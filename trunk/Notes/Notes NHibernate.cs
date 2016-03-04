/*
Object / Relational Impedance Mismatch
is the idea that interconnecting data by tables/rows/columns/pk/fk is 
incompatiable with an interconnecting schemea derived from objects.

 Major problems
 - Scope: where in OO you will have more objects than DB tables
 - Inheritance & Polymorphism: in OO classes are extending each other 
    while DB has all flat matrix with key relations.
 - Identity: a datarow may be uniquely id'ed while an object requires
    more defintion.
 - Relations: DB PK/FK are not directional while OO properties are of 
    one being a HAS-A of another.  To get bi-directional both OO classes
	must HAVE-A property to each other.
*/

/*
Unit of Work
 - idea of an in-time series or combination
   of operations which are consider a single logicical unit.
*/

/*
Lazy Loading
 - similar to the SICP 3.5 concept of delayed evaluation in which
   the instance state of an object is a semblance; meaning it outwardly
   appears to contain a state but infact only contains the ability to resolve
   a state.  SICP 3.5's concept of streams is limited to expressing serial 
   data values (e.g. Fibonacci sequence) without actually resolving the entire
   list of values.  Lazy Loading is pertinent to OO and properties thereof.
*/

/*
Query by Criteria
 - a declarative static-object pipe which reads like a classic SQL statement
   e.g. MyClass.MakeThis(arg).PerformFilterUsing(val).FormatResultLike(foo).As<MyType>();
   
 - by way of comparision Language Integrated Query (LINQ) is also a declarative statement
   but is formed by way of ordered key-words rather than an object pipe.
*/

/*
(N+1) Selects Problem
 - a formula which represents the number of selects that occur when filtering a 
   child collection.
 - there is 1 select for the top-level entity and N selects for the child collection 
   where the length of the child collection is N
 - simple LINQ example 
 
    var someSearch =
        myProduct.WorkOrders.Where(
            x => x.WorkOrderRoutings.IsReadOnly);
            
    - (1) is from the select which got 'myProduct'
    - (N) equals 'WorkOrders.Count' and the framework will have to 
          select every one to test the value of 'IsReadOnly'
 - the N+1 selects problem can be avoided using the 
  (1) HQL's 'left join fetch'
  (2) ICriteria API use of 
      .SetFetchMode("MyPropertyName", FetchMode.Eager)
      //criteria expressions here
*/

//the 'entity' class does not implement any specific kind of NHibernate interface/class
using NHibernate;
using NHibernate.Cfg;

public class MyEntity
{
	public int MyId {get; set;}
	public string MyName {get; set;}
	public List<Bar> MyBar {get; set;}
}

public class MyBasicExample
{

	//(1.) a session factory makes ISession objects
	public static ISessionFactory Factory {get; set;}

	//(2.) the ISession object is the crux of NHibernate
	public static ISession OpenNHSession()
	{
		if(Factory == null)
		{
			Configuration c = new Configuration();
			c.AddAssembly(System.Reflection.Assembly.GetCallingAssembly());
			Factory = c.BuildSessionFactory();
		}
		return Factory.OpenSession();
	}
	
	//(3.) the XML mapping forms the union-point that overcomes the Object / Relational Impedance Mismatch
	string myNhMapping = @"
	<?xml version='1.0'?>
	<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2' auto-import='true'>
	  <class name='MyFullyQualified.MyTypeName, MyAssembly' lazy='false'>
		<id name='MyId' access='field'>
			<generator class='native' />
		</id>
		<property name='MyName' access='field' column='Name' />
		<many-to-one access='field' name='MyBar' column='FooBar' cascasde='all' />
	   </class>
	</hibernate-mapping>
	";
	
	//(4.) the configuration settings
	string hibernateCfgXml = @"
	<?xml version='1.0' encoding='utf-8' ?>
	<configuration>
		<configSections>
			<section name='nhibernate' 
			type='System.Configuration.NameValueSectionHandler,
				  System, Version=1.0.3300.0,Culture=neutral,
				  PublicKeyToken=b77a5c561934e089' />
		</configSections>
		<nhibernate>
			<add key='hibernate.show_sql' value='true' />
			<add key='hibernate.connection.provider' value='NHibernate.Connection.DriverConnectionProvider' />
			<add key='hibernate.connection.driver_class' value='NHivernate.Driver.SqlClientDriver' />
			<add key='hibernate.connection.connection_string' value='Data Source=MyMachine;Database=MyMSSQL;Inegrated Security=SSPI' />
		</nhibernate>
	</configuration>
	";
	

	//Insert Example
	public void BasicExampleInput()
	{
		MyEntity e = new MyEntity();
		e.MyId = 4;
		e.MyName = "foo bar"
	
		using (ISession mySession = OpenNHSession())
		{
			//the ITransaction object represents the Unit of Work concept
			using (ITransaction t = mySession.BeginTransaction())
			{
				mySession.Save(e);
				t.Commit();
			}//end transaction
		}//end session
	}//end example
	
	public MyEntity BasicExampleOutput()
	{
		using (ISession mySession = OpenNHSession())
		{
			return mySession.Get(typeof(MyEntity), 4); //4 being the ID
		}//end session
	}
	
	/*
	Four ways to retreive data from NHibernate
	(1.) HQL
	(2.) ICriteria API
	(3.) direct SQL
	(4.) LINQ-to-NHibernate
	*/
	//HQL Example 1.
	public void HqlBasicExample()
	{
		using (ISession mySession = OpenNHSession())
		{
			IQuery myNhQry = session.CreateQuery("from MyTable as tbl order by tbl.name desc");
			IList<MyEntity> foo = myNHQry.List<MyEntity>();
		}//end session
	}//end example

	//HQL Example 2.
	public void HqlSelectUpdateAndInsert()
	{
		using (ISession mySession = OpenNHSession())
		{
			using (ITransaction t = mySession.BeginTransaction())
			{
				IQuery q = session.CreateQuery("from MyTable where Name = 'FooBar'");
				var myType = q.List<MyType>()[0];
				myType.MyName = "BarFoo";
				
				var anotherMyType = new MyType();
				anotherMyType.MyName = "FooBar";
				
				t.Commit();
			}//end transaction
		}//end session
	}//end example
	
	//ICriteria API Example 1
	public void ICriteriaBasicExample()
	{
		using (ISession mySession = OpenNHSession())
		{
			ICriteria myCriteria = session.CreateCriteria(typeof(MyType)).Add(Expression.Like("MyName","%foobar%"));
		}
	}
	
	//Direct SQL Example 1
	public void DirectSqlExample()
	{
		using (ISession mySession = OpenNHSession())
		{
			mySession.CreateSQLQuery(
				"select {c.*} from MTypeTable {c} where MyName like '%foobar%'",
				"c",
				typeOf(MyType));
		}
	}
	
	//LINQ-to-NHibernate Example 1
	public void LinqToNhibernateExample()
	{	
		//http://sourceforge.net/projects/nhcontrib/files/NHibernate.Linq/
		using (ISession mySession = OpenNHSession())
		{
			from x in session.Linq<MyType>()
			where x.MyName.StartsWith("foobar")
			select x
		}
	}
	
	public void UsingHqlParameters()
	{
		using (ISession mySession = OpenNHSession())
		{
			var searchBy = "Toy Farm";
			var queryString = "from Item item where item.Description like :searchString and item.Date > :minDate";
			
			IList result00 = mySession.CreateQuery(queryString)
							.SetString("searchString", searchBy)
							.SetDate("minDate", DateTime.AddDays(-180))
							.List();
							
			var positionalQueryString = "from Item item where item.Description like ? and item.Date > ?";
			IList result01 = mySession.CreateQuery(queryString)
							.SetString(0, searchBy)
							.SetDate(1, DateTime.AddDays(-180))
							.List();
							
			var entityQuery = "from Item item where item.Seller=:seller and item.Description like :description";
			
			var result02 = mySession.CreateQuery(entityQuery)
							.SetParameter("seller",persistentObject,NHibernateUtil.Entity(typeof(User)))
							.SetParameter("description", searchBy, NHibernateUtil.String)
							.List();
							//NHibernateUtil.Custom(typeof(MyEntityType)) 
			
		}
	}
	
	public void UsingNamedQuery()
	{
		//1. a named query may be a string literal in an hbm file
		var namedQueryHbm01 = @"
			<query name='MyNamedQuery'><![CDATA[
				from Item item where item.Description like :description
			]]></query>
		";
		//these go inside an hbm.xml file but outside of a class node
		var namedQueryHbm02 = @"
			<sql-query name='MyStoredProc'><![CDATA[
				exec NameOfMyStoredProc
					:parameter1,
					:parameter2
				]]>
			  <return alias='item' class='Item' />
			 </sql-query>
		";
		
		//returning a specific type
		var namedQueryHbmWithType = @"
		<sql-query name='MyCallableQry' callable='true'>
			<return alias='MyType' class='MyType'>
				<return-property column='ID' name='Id' />
				<return-property column='PN' name='PartNumber' />
			</return><![CDATA[
				select ROW_NUMBER() OVER ( ORDER BY c.CustId, cp.PartNum) as ID,
					   cp.PartNum as [PN]
				from dbo.Customer as c
				 join dbo.Parts cp
				 on c.CustId = cp.CustId
				where c.ParentCompany = :parentId]]>
		</sql-query>
		";
		
		//being called from code as
		using (ISession mySession = OpenNHSession())
		{
			var results = mySession.GetNamedQuery("MyNamedQuery")
							.SetString("description","Toy Farm")
							.List();
		}
	}
	
	public void UsingICriteria()
	{
		using(ISession mySession = OpenNHSession())
		{
			/* results in 
			
			select I.ID, I.DESCRIPTION from ITEM I
			
			which gets every item in the table*/
			ICriteria c = mySession.CreateCriteria(typeof(Item));
			
			ICriteria myCrit = mySession.CreateCriteria(typeof(Item));
			ICriterion descriptionEquals = NHibernate.Criterion.Expression.Eq("Description", "Toy Farm");
			myCrit.Add(descriptionEquals);
			
			var myItem = (Item) myCrit.UniqueResult();
			
			//-or-
			
			myItem = (Item) mySession.CreateCriteria(typeof(Item))
							.Add(Expression.Eq("Description", "Toy Farm"))
							.UniqueResult();
							
			//other expressions are
			//.Expression.IsNotNull("MyProperty")
			//.Expression.IsNull("MyProperty")
			//.Expression.In("MyProperty",myPropetiesList)
			//.Expression.Gt("Amount",100)
			//.Expression.Between("Amount",50,100)
			//.Expression.Like("FirstName","Sta%") -or- .Expression.Like("FirstName","Sta",MatchMode.Start)
			//                                                           also have MatchMode.End, Anywhere and Exact
			//.Expression.Eq("Description","Toy Farm").IgnoreCase()
			
			//also there is 
			
			//expressions may be nested using 'AND' & 'OR' statements
			myCrit  = mySession.CreateCriteria(typeof(Item))
						.Add(Expression.Disjunction()
								.Add( Expression.Conjunction()
										.Add(Expression.Like("FirstName", "Bri%"))
										.Add(Expression.Like("LastName", "Sta%"))
									)
									.Add( Expression.In("MyProperty", myProperties))
							);
							
			myCrit = mySession.CreateCriteria(typeof(Item))
						.AddOrder(Order.Asc("LastName"))
						.AddOrder(Order.Asc("FirstName"));
							
		}
	}
	
	public static void Main(string[] args)	{}
}//end class

public class CapitalAPI
{
	#region NHibernate configuration
	Configuration cfg {get; set;} //capital object, needed to generate the factory to generate the sessions
	#endregion
	
	#region nexus of Create, Retrieve, Update and Delete
	ISession s {get; set;} //cheap and lightweight, is not thread safe
	ISessionFactory sf {get; set;} //heavy and expensive, should be a singleton
	ITransaction t {get; set;} //related to tandem action and time, generated from the Session
	IQuery q {get; set;} //noted as 'HQL' for Hibernate Query Lang., a dialect of SQL - light and contextual to Session
	ICriteria c {get; set;} //for query by object-pipe
	#endregion
	
	#region Callback interface - to react to NHibernate events
	IInterceptor spt {get; set;} //subscribe to by persistent classes that do not implement any NH API
	ILifecycle cyc {get; set;} //event a persistent object emmits and which only it may subscribe
	IValidatable vtb {get; set;} //likewise.
	#endregion
	
	#region Extension API
	IUserType uty {get; set;} //implement for extending NH's RDBMS-to-OO interchange Types
	ICompositeUserType cty {get; set;} //likewise
	#endregion
	
	#region Other extendable API
	IIdentifierGenerator idg {get; set;}  //PK gen
	Dialect dlt {get; set;} //abstract class, for dialects of SQL
	ICache chc {get; set;} //caching strategies
	ICacheProvider cpv {get; set;} //likewise
	IConnectionProvider icp {get; set;} //ado.net connection management
	ITransactionFactory trf {get; set;} //obvious
	IProxyFactory pft {get; set;} //issues with proxy settings
	#endregion
}

public class DetailedConfigurationExample
{
	enum ConfigExamples
	{
		XML_PIPE,
		OL_SKOOL,
		EMBEDDED_XML,
		CFG_EMBEDDED_XML,
		MY_CONFIG_FILE_NAME
	}
	private static ISessionFactory fct = null; //singleton
	private static bool ConfigExamples = ConfigExamples.XML_PIPE;
	public static ISessionFactory MyFactoryExample()
	{
		if (fct == null)
		{
			if(XML_PIPE)
			{
				//this text calls it 'Method Chaining'
				fct = new Configuration()
							.Configure()
							.AddXmlFile("MyType.hbm.xml")
							.BuildSessionFactory();
			}
			else if(OL_SKOOL)
			{
				var cfg = new Configuration();
				cfg.Configure();
				cfg.AddXmlFile("MyType.hbm.xml");
				//other option:  cfg.AddInputStream(HbmSerializer.Default.Serialize(typeof(MyType)));
				fct = cfg.BuildSessionFactory();
			}
			else if(EMBEDDED_XML)
			{
				fct = new Configuration()
							.Configure()
							.AddClass( typeof(Model.MyType)) //assumes the original xml mapping is an embedded resource in this assembly
							.BuildSessionFactory();
			}//end embedded type 
			else if(CFG_EMBEDDED_XML)
			{
				//connection.provider is a class implementing the IConnectionProvider interface
				//dialect, handles subtlies with SQL beyond ANSI standard, comes packaged with most common
				//connection.driver_class, a class implementing the .NET ADO Driver
				//connection.connection_string, obvious

				string cfg = @"
				<?xml version='1.0' encoding='utf-8' ?>
				<hibernate-configuration xmlns='urn:nhibernate-configuration-2.2'>
					<session-factory>
						<property name='connection.provider'>NHibernate.Connection.DriverConnectionProvider</property>
						<property name='connection.driver_class'>NHibernate.Driver.SqlClientDriver</property>
						<property name='dialect'>NHibernate.Dialect.MsSql2008Dialect</property>
						<property name='connection.connection_string_name'>Get_It_From_App_Config</property>
						<property name='show_sql'>true</property>
						<property name='current_session_context_class'>thread_static</property>
						<mapping assembly='MyAssembly.WithAllEmbedded.hbm.xml'/>
				  </session-factory>
				</hibernate-configuration>
				";
				
				fct = new Configuration()
							.Configure(); //the cfg.xml file already did this on the 'mapping' node
							
			}//end default config file name and full embedded assembly
			else if (MY_CONFIG_FILE_NAME)
			{
				//use a filename other than the default 'hibernate.cfg.xml' - you must pass the full name to the Configure method
				fct = new Configuration.Configure(@"..\My_Config.xml").BuildSessionFactory();
			}//end custom config file name
		}
		return fct;
	}//end configuration examples
	
	public void LoggingExamples()
	{
		//depends on 'log4net'
		string configFileSettings = @"
		<?xml version='1.0'?>
		<configuration>
			<configSections>
				<section name='log4net' type='log4net.Config.Log4NetConfigurationSectionHandler,log4net' />
			</configSections>
			<log4net>
				<appender name='ConsoleAppender' type='log4net.Appender.ConsoleAppender, log4net'>
					<layout type='log4net.Layout.PatternLayout, log4net'>
						<param name='ConversionPattern' value='%m' />
					</layout>
				</appender>
				<root>
					<priority value='WARN' />
					<appender-ref ref='ConsoleAppender' />
				</root>
			</log4net>
		</configuration>
		";
		
		//1. activate 'log4net' via the Config Sections element
		//2. add 'log4net' element
		//3. define one or more log4net 'Appender'(s)
		//4. add the 'log4net\root' node and set the trace level
		//5. activate any defined appender(s) within the root node
	}
}

namespace MappingRelationExample
{
/*
Database Axioms:
1.) a key exist as a definition within the context of one table.
2.) a key must reference at least one column of its table.
3.) a key may reference more than one column of its table.
4.) a foreign-key, in addition to its table, may reference only one seperate table.
5.) a foreign-key may be a self-reference.
6.) a primary-key is independent of a foreign-key.
7.) a table may have one and only one primary-key.
8.) a table may have many foreign-keys.

NHibernate Assumptions:
1.) one table is represented by one type.
2.) complexity is introduced by axiom 6 which means foreign keys and primary keys
    may share columns.  This affects NHibernate mapping becasue composites of columns
	must be defined as a type.


NHibernate Axioms:
1.) an instance which is mapped to an hbm is either persistent; meaning it is associated
    to a live Session, or is transient; meaning the instance was constructed by the 
    applicantion-code.
	
2.) a single foreign key is represented by a single type.
	- this means that if the FK includes many columns of the resident table
	  to many columns of its reference - it exist on the type as one property
	  of the given table type
	  
3.) a primary key whose composition is more than one column requires a seperate
    type to represent it.
	- this means the primary key of the type is, itself, a type whose definition
	  is the various columns used to define the primary key.
	  
4.) having a primary key of multiple columns of which are, in turn, part of a foreign key, 
    then the overlap is represented as the foreign key's type as properties within the
	additional primary key type.
	- this means that when a table has a primary key of say, c1, c2, c3 and, likewise,
	  has a foreign key which has say, c1 and c2.  In addition the table has another
	  foreign key of say c3 then this table's type would have a primary key which is 
	  a seperate type, and, additionally, this seperate type would itself contain 
	  two properties of two types.  The first type representing the c1, c2 columns
	  and the second representing the c3 column.
	  
5.) a type may contain properties to whom its foreign keys are referencing and may
    contain properties from whom the foreign keys of other tables to which it plays 
	the part of reference.
    - this means that when defining a type one must transverse the entire database 
      to find all the tables which have a foreign-key referencing back to this given
	  table.
*/

public class RepresentingDatabasePrimaryKeys
{
	//when a table has a PK composed of a single incrementing column
	string HbmXmlIncrementingID = @"
	<id name='PropertyName' column='ColumnName' type='Int64'>
		<generator class='native' /> <!-- or 'identity' -->
	</id>";
	
	//when a table has a PK composed of a single assigned string column
	string HbmXmlAssignedStringID = @"
	<id name='PropertyName' column='ColumnName' type='AnsiString' length='10'>
		<generator class='assigned' />
	</id>";
	
	//when a table has a PK composed of more than one column
	string HbmXmlCompositeID = @"
	<composite-id name='PropertyNameOfTablesType' class='ClassNameOfSeperateTypeUsedToDefinePK'>
		<!-- these will be properties on  ClassNameOfSeperateTypeUsedToDefinePK -->
		<key-property name='PropertyName' column='ColumnName' type='AnsiString' length='12' />
		<key-property name='AnotherProperty' column='SecondColumn' type='Int64' />
	</composite-id>";
	
	//when a table has a PK composed of more than one column and such columns are shared by a FK
	string HbmXmlCompositeIDWithTypes = @"
	<composite-id name='PropertyNameOfTablesType' class='ClassNameOfSeperateTypeUsedToDefinePK'>
		<!-- since FK's reference one table and one table = one class, classes are needed instead -->
		<key-many-to-one name='PropertyName' class='ClassNameForFK1' column='ColumnWhichIsPartOfPkAndFk' />
		<key-many-to-one name='AnotherProperty' class='ClassNameForFK2'>
			<column name='OtherColumnWhichIsPartOfPkAndFk1' />
			<column name='OtherColumnWhichIsPartOfPkAndFk2' />
		</key-many-to-one>
	</composite-id>";
}

public class RepresentingDatabaseForeignKeys
{
	//a FK defined on this table
	
	<many-to-one name='PropertyName' class='FkType' column='FkIdentifier' />";
	
	//a FK whose type you want to cast as a child-type - w/o the lazy=false a proxy is generated by NHib runtime
	string HbmXmlFkWithChildTypes = @"
	<many-to-one name='PropertyName' class='FkType' column='FkIdentifier' lazy='false'/>";

	//a composite FK defined on this table (having more than one column)
	string HbmXmlCompositeFk = @"
	<many-to-one name='ProertyName' class='FkType'>
		<column name='FkColumn1' />
		<column name='FkColumn2' />
	</many-to-one>";
	
	//an subsequent table whose FK is a reference back to here, but the class wants to own these
	//as child records...
	string HbmXmlSubsequentFk = @"
	<bag name='PropertyName' cascade='all-delete-orphan' inverse='true' lazy='true' batch-size='512'>
		<key column='ColumnNameOnOtherTable' foreign-key='ColumnNameOnThisTable' />
		<one-to-many class='ClassNameOfAntecendentTable' />
	</bag>";
	
	//the actual IList is a protected virtual instance variable having the givnen naming pattern in the POCO
	//this is a so in the POCO one can add custome AddSuchAndSuch(...) because in NHib. to est. a releationship on a bag 
	//  one must have reciprical parentInstance.Child = childInstance & childInstane.Parent = parentInstance
	string HbmXmlSubsequentFkProtected = @"
	<bag name='PropertyName' access='field.camelcase-underscore' cascade='all-delete-orphan' inverse='true' lazy='true' batch-size='512'>
		<key column='ColumnNameOnOtherTable' />
		<one-to-many class='ClassNameOfAntecendentTable' />
	</bag>";
}

public class RepresentingSimpleProperties
{
	string HbmXmlProperty = @"
	<property name='FirstName' column='FirstName' type='AnsiString' length='50' />";
}

public class Cookbook
{
	//having some table with an actual PK that is auto-increment but 
	//want to use some unique varchar column instead
	string TableFromWhichADifferentPKIsDesired = @"
	<class name='WantDifferentPk' class='WantDifferentPk'>
		<!--this is not the actual PK, but it is unique -->
		<id name='UniqueCode' column='UniqueCode' type='AnsiString' length='6'>
			<generator class='assigned' />
		</id>
		<!--this is the real PK for the table-->
		<property name='Id' column='ID' unique='true' />
	</class>";
	
	//having some table with has a FK mapping to the former table
	string ATableWhichDependedOnTheActualPkForItsPk = @"
	<class name='SomeChildTable' class='SomeChildTable' >
		<id name='Id' column='ID' type='Int32'>
			<generator class='identity' />
		</id>
		<many-to-one name='ParentTable' class='WantDifferentPk' property-ref='Id' column='UsesActualFK' />
	</class>";
	
	//want a single x-ref'ed value using given some value in the exiting hbm
	string AnXrefedValueUsingFormula = @"
	<class name='MyEntity' class='MyEnityClass'>
		<id name='Id' column='ID' type='Int32'>
			<generator class='identity' />
		</id>
		<property name='MyXrefSource' column='SomeStringType' type='AnsiString' length='16' />
		<property name='XrefedValue' formula='(select SomeStringValue from [dbo].[SomeTypeDefTable] as stdt where stdt.SomeStringType = MyXrefSource)' type='AnsiString' length='10' /> 
	</class>
	";
	
	//some stored proc returns an integer but has no Output parameters
	public int GetStoredProcReturnValue()
	{
		using (NHibernate.ISession session = _paramedConnection.Session)
		{
			using(NHibernate.ITransaction transaction = session.BeginTransaction())
			{
				System.Data.IDbCommand command = new SqlCommand();
				command.Connection = session.Connection;

				transaction.Enlist(command);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.sp_AutoNumber";

				var tableName = new SqlParameter("@Table", SqlDbType.VarChar);
				tableName.Value = "myTable";
				command.Parameters.Add(tableName);

				var returnValue = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
				returnValue.Direction = ParameterDirection.ReturnValue;
				command.Parameters.Add(returnValue);

				command.ExecuteNonQuery();

				return returnValue.Value;
			}
		}
	}
}
}
namespace DealingWithBinary
{
//1. create a wrapper for a byte[]
public class Sid
{
	//having this a readonly means later the type "Immutable"
	private readonly byte[] _data;
	public Sid(byte[] binarySid)
	{
		_data = binarySid;
	}

	public byte[] Data { get {return _data;} }

	public override bool Equals(object obj)
	{
		if((obj as Sid) == null)
		{
			return false;
		}

		var compareTo = obj as Sid;

		if(compareTo.Data == null ^ this.Data == null)
		{
			return false;
		}

		if(this.Data != null)
		{
			if(this.Data.Length != compareTo.Data.Length)
			{
				return false;
			}

			for(var i =0; i<Data.Length;i++)
			{
				if(compareTo.Data[i] != this.Data[i])
				{
					return false;
				}
			}
		}

		return true;
	}

	public override int GetHashCode()
	{
		var hashCode = 1;
		if(Data != null)
		{
			hashCode += Data.GetHashCode();
		}
		return hashCode;
	}

	public override string ToString()
	{
		if(Data == null)
		{
			return base.ToString();
		}
		var hexData = new System.Text.StringBuilder();
		for(var i = 0; i<Data.Length;i++)
		{
			hexData.Append(Data[i].ToString("X2"));
		}
		return hexData.ToString();
	}
}

//2. implement the NHibernate.UserTypes.IUserType
public class SidUserType : IUserType
{
	public static readonly NHibernate.SqlTypes.SqlType[] SQL_TYPES = {NHibernateUtil.Binary.SqlType};

	public new bool Equals(object x, object y)
	{
		if (object.ReferenceEquals(x, y))
		{
			return true;
		}
		if (x == null || y == null)
		{
			return false;
		}
		return x.Equals(y);
	}

	public int GetHashCode(object x)
	{
		return x.GetHashCode();
	}

	public object NullSafeGet(IDataReader rs, string[] names, object owner)
	{
		var obj = NHibernateUtil.Binary.NullSafeGet(rs, names[0]);
		if (obj == null)
		{
			return null;
		}
		var sidData = (byte[]) obj;
		return new Sid(sidData);
	}

	public void NullSafeSet(IDbCommand cmd, object value, int index)
	{
		if(value == null)
		{
			((IDbDataParameter) cmd.Parameters[index]).Value = DBNull.Value;
		}
		else
		{
			var sid = (Sid) value;
			((IDbDataParameter) cmd.Parameters[index]).DbType = System.Data.DbType.Binary;
			((IDbDataParameter) cmd.Parameters[index]).Value = (byte[]) sid.Data;
		}
	}

	public object DeepCopy(object value)
	{
		return value;
	}

	public object Replace(object original, object target, object owner)
	{
		return original;
	}

	public object Assemble(object cached, object owner)
	{
		return cached;
	}

	public object Disassemble(object value)
	{
		return value;
	}

	public SqlType[] SqlTypes { get { return SQL_TYPES; } }
	public Type ReturnedType { get { return typeof (Sid); } }
	public bool IsMutable { get { return false; } }
}

//3. create the entity which has the binary id
public class MyBinaryIDEntity
{
	#region Id
	public virtual Sid ID { get; set; }
	#endregion

	#region ValueTypes
	public virtual bool Active { get; set; }
	public virtual string Name { get; set; }
	public virtual string Note { get; set; }
	#endregion

	public virtual bool Equals(MyBinaryIDEntity other)
	{
		if (System.Object.ReferenceEquals(null, other)) return false;
		return System.Object.ReferenceEquals(this, other) || ID.Equals(other.ID);
	}

	public override bool Equals(object obj)
	{
		if (System.Object.ReferenceEquals(null, obj)) return false;
		if (System.Object.ReferenceEquals(this, obj)) return true;
		return obj.GetType() == typeof(MyBinaryIDEntity) && Equals((MyBinaryIDEntity)obj);
	}

	public override int GetHashCode()
	{
		var newRandom = new System.Random();
		return ID == null ? newRandom.Next() : ID.GetHashCode();
	}


}//end MyBinaryIDEntity

//4. set the hbm.xml mapping to the IUserType implementation
public class ExampleOfHbmForBinaryId
{
	//rare instance where the type in the hbm.xml is not the same as the type
	// in the .cs entity file...
	public const string TheHbmFile = @"
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2' assembly='MyAssembly'>
  <class name='CarrierBySid' table='Carrier' schema='[master]'>
    <id name='ID' column='ID' type='SidUserType, MyAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'>
      <generator class='identity' />
    </id>
    <property name='Name' column='Name' type='AnsiString' length='255' />
    <property name='Active' column='Active' type='Boolean' />
    <property name='RecordNote' column='Note' type='AnsiString' length='500' />
  </class>
</hibernate-mapping>	
";
}
}