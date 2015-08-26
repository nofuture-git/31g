using System;
using Castle.Windsor.Installer;
using Castle.MicroKernel;
using Castle.Windsor;

public class MyDiExample
{
	private Castel.Windsor.IWindsorContainer _container;
	
	public void BasicExample()
	{
		//make the container
		_container = new Castel.Windsor.WindsorContainer();
		
		//register something with it
		_container.Register(Castle.MicroKernel.Registration.Component.For<MyType>());
		
		//get an instance 
		MyType foo = _container.Resolve<MyType>();//no null check required, if it cannot be resolved an exception occurs
	}
	
	public void AbstractExample()
	{
		//make the container
		_container = new Castle.Windsor.WindsorContainer();
		
		//register an abstraction with it and its corrosponding concrete implementation
		_container.Register(Castle.MicroKernel.Registration.Component.For<IMyInterface>().ImplementedBy<MyType>());
		
		//get an instance of the concrete type AS-A abstract type
		IMyInterface foo = _container.Resolve<IMyInterface>();
		
		//may also perform this in a non type manner
		var bar = _container.Resolve(typeof(IMyInterface));//returns a castable object 
		
	}
	
	public void ConcreteAbstractRegMix()
	{
		//make the container
		_container = new Castle.Windsor.WindsorContainer();
		
		//register a abstract type to a concrete with an overload as 
		_container.Register(Castle.MicroKernel.Registration.Component.For<MyType, IMyInterface>());
		
		//now the concrete type may be returned directly from the call to resolve
		MyType foo = _container.Resolve<IMyInterface>();
	}
	
	public void AutoRegistrationExample()
	{
		//make the container
		_container = new Castle.Windsor.WindsorContainer();
		
		//use a variety of such calls to register all the types of some value that are present in an assembly
		_contianer.Register(Castle.MicroKernel.Registration.AllTypes
							.FromAssemblyContaining<MyType>()
							.BasedOn<IMyInterface>());

		//other examples as
		_container.Register(Castle.MicroKernel.Registration.Classes
							.FromThisAssembly()
							.Where(t => t.Name.StartsWith("IMy")));
		
	}
	
	public void RegisterFromXmlExample()
	{
		//get a container
		_container = new Castle.Windsor.WindsorContainer();
		
		//have the api parse its pertinent section of the app config and load it using this
		_container.Install(Castle.Windsor.Installer.Configuration.FromAppConfig());
		
		//the section of the app config would look something like this
		string sectionOfAppConfig = @"
		<configSections>
			<section name='castle'
					 type='Castle.Windsor.Configuration.AppDomain.CastleSectionHandler, Castle.Windsor' />
		</configSections>
		
		<castle>
			<components>
				<component id='myCastleId'
						   service='IMyInterface'
						   type='MyType' />
			</components>
		</castle>
		";
	}
	
	public void RegistrationFromIWindsorInstaller()
	{
		//get a container
		_container = new Castle.Windsor.WindsorContainer();
		
		//call the Install member passing in an instance of IWindsorInstaller
		_container.Install(new MyRegistration());
	}
	
	public void RegisterScopeResolution()
	{
		//get a container
		_container = new Castle.Windsor.WindsorContainer();
		
		//default is Singleton
		_container.Register(Castle.MicroKernel.Registration.Component.For<MyType>());
		
		//must specify if each call serves up a new instance as 
		_container.Register(Castle.MicroKernel.Registration.Component.For<MyType>().LifeStyle.Transient);
		
		MyType foo = _container.Resolve<MyType>();
		
		//mentioned in the text 
		_container.Release(foo);
	}
	
	public void MultipleRegistrationExample()
	{
		//get a container
		_container = new Castle.Windsor.WindsorContainer();
		
		//register a abstract type mapping to a concrete implementation
		_container.Register(Component.For<IMyInterface>().ImplementedBy<MyType>());
		_container.Register(Component.For<IMyInterface>().ImplementedBy<MyOtherType>());
		
		//the underlying type is 'MyType' since it was registered first
		IMyInterface foo = _container.Resolve<IMyInterface>();
		
		//using a name, resolution to a type becomes more straightfoward
		_container.Register(Component.For<IMyInterface>().ImplementedBy<MyThirdType>().Named("myThirdType"));
		
		//now we get a specific type back
		IMyInterface bar = _container.Resolve<IMyInterface>("myThirdType");
	}
}

//to make a registration more reusable, implement this
public class MyRegistration : Castle.MicroKernel.Registration.IWindsorInstaller
{
	#region IWindsorInstaller members
	public void Install(IWindsorContainer container, IConfigurationStore store)
	{
		container.Register(Castle.MicroKernel.Registration.Classes
							.FromThisAssembly()
							.Where(t => t.Name.StartsWith("IMy")));
	}
	#end region
}
