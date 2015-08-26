using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whatever
{
    public class HasList
    {
        public System.Collections.Generic.List<PropA> Props { get; set; }
    }
    public class PropA
    {
        public int ID { get; set; }
        public string Value { get; set; }
    }

    public class HasSingle
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public PropB Prop { get; set; }
    }
    public class PropB
    {
        public int ID { get; set; }
        public string Value { get; set; }
    }

    public class Copy
    {
        public int ID { get; set; }
        public PropB Prop { get; set; }
    }

    public class HasValueOfPropB
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public int PropID { get; set; }
        public string PropValue { get; set; }
    }

    public class PropC
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public bool Primary { get; set; }
    }

    public class HasListWithFlags
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public List<PropC> Props { get; set; }
    }

    public class CopyToHasListWithFlags
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public PropC Prop { get; set; }
    }

    public interface UnderlyingType
    {
        int ID { get; set; }
        string ValueOne { get; set; }
        string ValueTwo { get; set; }
    }

    public class ExtendsUnderlyingType : UnderlyingType
    {
        public int ID { get; set; }
        public string ValueOne { get; set; }
        public string ValueTwo { get; set; }
    }
    public class ExtendedTypesTarget
    {
        public int TheID { get; set; }
        public string V1 { get; set; }
        public string V2 { get; set; }
    }

    public class HasAExtendedUnderlyingType
    {
        public int TopId { get; set; }
        public ExtendsUnderlyingType TheHASA { get; set; }
    }

    public class HasAMappingTarget
    {
        public int TheTopID { get; set; }
        public ExtendedTypesTarget TheHASATarget { get; set; }
    }

    public class AutoMapperNotes
    {

        //to distinguish which is source and which is dest, use visual studio's F12
        //public static TDestination Map<TSource, TDestination>(TSource source);
        public static void Register()
        {
            /*--------------------------------------
             Copy by Value Semantics
             --------------------------------------*/
            //tell AutoMapper to make full copies of these (as in two objects on the heap)
            AutoMapper.Mapper.CreateMap<Copy, Copy>();
            AutoMapper.Mapper.CreateMap<PropB, PropB>();

            //mapping are not reciprocal, you must set a map for both ways
            AutoMapper.Mapper.CreateMap<HasSingle, Copy>();
            AutoMapper.Mapper.CreateMap<Copy, HasSingle>();

            //you don't need to add '.ForMember' here since the property-names/types are the 
            //same on both object graphs
            AutoMapper.Mapper.CreateMap<PropB, PropA>();
            AutoMapper.Mapper.CreateMap<PropA, PropB>();

            /*--------------------------------------
             Value Type -to- object HAS-A Value Type
             --------------------------------------*/
            AutoMapper.Mapper.CreateMap<HasSingle, HasValueOfPropB>()
                .ForMember(dest => dest.PropID, x => x.MapFrom(src => src.Prop.ID))
                .ForMember(dest => dest.PropValue, x => x.MapFrom(src => src.Prop.Value));

            //to deal with boxing an object, instantiate the dest's type and use the object init syntax and its fine
            AutoMapper.Mapper.CreateMap<HasValueOfPropB, HasSingle>()
                .ForMember(dest => dest.Prop, x => x.MapFrom(src => new PropB { ID = src.PropID, Value = src.PropValue }));//IS-A dest's Prop type

            /*--------------------------------------
             Generic of Type -to- just that Type
             --------------------------------------*/
            //AutoMapper know how to turn PropA to ProbB and vice versa, but not a List<PropA> into a ProbB, so 
            // have it only deal with one instance of PropA and its fine
            AutoMapper.Mapper.CreateMap<HasList, HasSingle>()
                .ForMember(dest => dest.Prop, x => x.MapFrom(src => src.Props.FirstOrDefault()));//NOT the dest's type

            //on the reverse, AutoMapper knows how to turn ProbB into PropA, but not ProbB into a List<ProbA>, so
            // box the single ProbB into a list and its fine
            AutoMapper.Mapper.CreateMap<HasSingle, HasList>()
                .ForMember(dest => dest.Props, x => x.MapFrom(src => new List<PropB> { src.Prop }));//NOT a List of dest's type

            /*--------------------------------------
             Inline branching on Mappings
             --------------------------------------*/
            AutoMapper.Mapper.CreateMap<PropC, PropB>()
                .ForMember(dest => dest.ID, x => x.MapFrom(src => (src.Primary ? src.ID : 0)))
                .ForMember(dest => dest.Value, x => x.MapFrom(src => (src.Primary ? src.Value : string.Empty)));

            AutoMapper.Mapper.CreateMap<PropC, PropA>()
                .ForMember(dest => dest.ID, x => x.MapFrom(src => (src.Primary ? src.ID : 0)))
                .ForMember(dest => dest.Value, x => x.MapFrom(src => (src.Primary ? src.Value : string.Empty)));

            AutoMapper.Mapper.CreateMap<HasListWithFlags, HasList>()
                .ForMember(dest => dest.Props, x => x.MapFrom(src => src.Props.Where(p => p.Primary)));


            /*--------------------------------------
             Complex branching logic in Mappings
             --------------------------------------*/
            //using Func allows for us to skip the delegate type declaration
            Func<HasListWithFlags, PropC> complexCommand = FunctionToBeUsedInline;
            AutoMapper.Mapper.CreateMap<HasListWithFlags, CopyToHasListWithFlags>()
                .ForMember(dest => dest.Prop, x => x.MapFrom(src => complexCommand(src)));


            /*--------------------------------------
             Automapper handling a Map for a parent 
             inteface on concrete type
             --------------------------------------*/
            AutoMapper.Mapper.CreateMap<UnderlyingType, ExtendedTypesTarget>()
                .Include<ExtendsUnderlyingType, ExtendedTypesTarget>()
                .ForMember(dest => dest.TheID, x => x.MapFrom(src => src.ID))
                .ForMember(dest => dest.V1, x => x.MapFrom(src => src.ValueOne))
                .ForMember(dest => dest.V2, x => x.MapFrom(src => src.ValueTwo))
                ;
            //see https://github.com/AutoMapper/AutoMapper/wiki/Mapping-inheritance
            AutoMapper.Mapper.CreateMap<ExtendsUnderlyingType, ExtendedTypesTarget>();


            /*--------------------------------------
             Automapper handling a Map for property
             on source being IS-A interface and not
             having a map for the type which implemented 
             the interface
             --------------------------------------*/
            //in this case, AutoMapper must have the exact types the mapping was originally constructed
            // upon and therefore, the property must be downcast to the interface for the mapping to work.
            AutoMapper.Mapper.CreateMap<HasAExtendedUnderlyingType, HasAMappingTarget>()
                .ForMember(dest => dest.TheTopID, x => x.MapFrom(src => src.TopId))
                .ForMember(dest => dest.TheHASATarget, x => x.MapFrom(src => src.TheHASA))
                ;

            /*--------------------------------------
             On the reverse, handling a Map for a 
             property on the destination being an IS-A
             interface and not having a map for the 
             type that implemented the interface
             --------------------------------------*/
            AutoMapper.Mapper.CreateMap<ExtendedTypesTarget, UnderlyingType>()
                .Include<ExtendedTypesTarget, ExtendsUnderlyingType>()
                .ForMember(dest => dest.ID, x => x.MapFrom(src => src.TheID))
                .ForMember(dest => dest.ValueOne, x => x.MapFrom(src => src.V1))
                .ForMember(dest => dest.ValueTwo, x => x.MapFrom(src => src.V2))
                ;
            AutoMapper.Mapper.CreateMap<ExtendedTypesTarget, ExtendsUnderlyingType>();

            AutoMapper.Mapper.CreateMap<HasAMappingTarget, HasAExtendedUnderlyingType>()
                .ForMember(dest => dest.TopId, x => x.MapFrom(src => src.TheTopID))
                .ForMember(dest => dest.TheHASA, x => x.MapFrom(src => src.TheHASATarget))
                ;

        }

        public static PropC FunctionToBeUsedInline(HasListWithFlags src)
        {
            //...other complex logic here...

            return new PropC { ID = byte.MaxValue, Value = "something special", Primary = true };
        }

        public static void Examples()
        {
            Register();
            var original = new HasSingle { ID = 99, Prop = new PropB { ID = 12, Value = "prop b" } };
            var copyDest = new Copy();//nothing here

            //mapping continue down the object graph to terminal valueType nodes unless set explicitly to ignore
            AutoMapper.Mapper.Map(original, copyDest);

            //this is false, 'copyDest's Prop is a new object on the heap 
            Console.WriteLine(string.Format("original equals copyDest: {0}", original.Prop.Equals(copyDest.Prop)));

            HasSingle getAnotherCopy = AutoMapper.Mapper.Map<Copy, HasSingle>(copyDest);

            //yet another independent copy on the heap with another stack-residing reference
            Console.WriteLine(string.Format("copy from copy equals original: {0}", getAnotherCopy.Equals(original)));
            Console.WriteLine(string.Format("copy of copy's properties equal original: {0}", getAnotherCopy.Prop.Equals(original.Prop)));

            PropA copiedToAnotherType = AutoMapper.Mapper.Map<PropB, PropA>(getAnotherCopy.Prop);
            Console.WriteLine(string.Format("copy across types with same property name/types {0}", (copiedToAnotherType != null)));

            //have dealt in the 'Register' the complexities of going from List<PropA> to just PropB
            //and, likewise, from PropB to List<PropA> so this works as expected and is, again, new
            //objects on the heap
            HasList fromTypeToGenericOfType = AutoMapper.Mapper.Map<HasSingle, HasList>(getAnotherCopy);
            HasSingle fromGenericOfTypeToType = AutoMapper.Mapper.Map<HasList, HasSingle>(fromTypeToGenericOfType);

            // in "Register" dealt with how to convert ProbValue, PropID from "HasSingle's" Prop property 
            // and, likewise, how to take "HasSingle's" Prop property's ID, Value and copy them over to 
            // "HasValueOfPropB's" top-level value-type properties 
            HasValueOfPropB moveValuesUpObjGraph =
                AutoMapper.Mapper.Map<HasSingle, HasValueOfPropB>(fromGenericOfTypeToType);
            HasSingle moveValuesDownObjGraph = AutoMapper.Mapper.Map<HasValueOfPropB, HasSingle>(moveValuesUpObjGraph);

            PropC primaryPropC = new PropC { ID = 14, Value = "is primary", Primary = true };
            PropC secondaryPropC = new PropC { ID = 11, Value = "is secondary", Primary = false };

            PropB copiedFromPrimary = AutoMapper.Mapper.Map<PropC, PropB>(primaryPropC);
            PropB copiedFromSecondary = AutoMapper.Mapper.Map<PropC, PropB>(secondaryPropC);
            Console.WriteLine("in line ternary operations allowed in mappings {0}", copiedFromPrimary.ID == primaryPropC.ID && copiedFromPrimary.Value == primaryPropC.Value);
            Console.WriteLine("and resolves on both of the ternary operands {0}", copiedFromSecondary.ID != secondaryPropC.ID && copiedFromSecondary.Value != secondaryPropC.Value);


            //when a mapping already exist for two types, and some object HAS-A List of one of 
            //those two, the mapping at the object level may operate on the list (using a LINQ expression)
            //which may override and ternary operations at play on the List's type
            var ListOfPropC = new List<PropC>
                                  {
                                      new PropC {ID = 1, Primary = false, Value = "1"},//both the ternary operator and 
                                      new PropC {ID = 2, Primary = true, Value = "2"},//the Linq expression use this same 'Primary' property
                                      new PropC {ID = 3, Primary = false, Value = "3"},//making them exclusive of each other
                                      new PropC {ID = 4, Primary = true, Value = "4"}
                                  };

            HasListWithFlags listWithFlags = new HasListWithFlags { ID = 99, Props = ListOfPropC, Value = "with flags" };

            HasList usesDuplexOfMappingRules = AutoMapper.Mapper.Map<HasListWithFlags, HasList>(listWithFlags);

            Console.WriteLine(string.Format("object copied with duplex of mapping rules {0}", usesDuplexOfMappingRules != null));
            Console.WriteLine(string.Format("rules on list applied over rules on type therein {0}", usesDuplexOfMappingRules.Props.Count == 2));

            //when the operation on the source is simply to complex to do inline, then define a seperate function
            //to perform this logic, declare and instantiate a function pointer to it (Func<>) and used it within 
            //the mapping on the source.
            CopyToHasListWithFlags complexLogicWithinMapping =
                AutoMapper.Mapper.Map<HasListWithFlags, CopyToHasListWithFlags>(listWithFlags);

            var functionPtrWasUsed = complexLogicWithinMapping != null;
            Console.WriteLine(string.Format("the complex mapping returned the dest type {0}", functionPtrWasUsed));
            functionPtrWasUsed = complexLogicWithinMapping.Prop != null;
            Console.WriteLine(string.Format("the complex mapping called the function pointer {0}", functionPtrWasUsed));

            var prop = complexLogicWithinMapping.Prop;
            functionPtrWasUsed = prop.ID == byte.MaxValue && prop.Value == "something special" && prop.Primary;
            Console.WriteLine(string.Format("and operated as expected {0}", functionPtrWasUsed));

            var extendedType = new ExtendsUnderlyingType {ID = 32, ValueOne = "value one", ValueTwo = "value two"};
            var testMappingExtendedTypes = AutoMapper.Mapper.Map<UnderlyingType, ExtendedTypesTarget>(extendedType);

            Console.WriteLine(string.Format("the mapper returned something when given a concrete instance for a map on an Interface '{0}'",testMappingExtendedTypes != null));
            Console.WriteLine(string.Format("The value from ID -> TheID is '{0}'",testMappingExtendedTypes.TheID));

            var hasAExtendedTypeProp = new HasAExtendedUnderlyingType();
            hasAExtendedTypeProp.TopId = 99;
            hasAExtendedTypeProp.TheHASA = extendedType;

            var testMappingHasAExtendedType =
                AutoMapper.Mapper.Map<HasAExtendedUnderlyingType, HasAMappingTarget>(hasAExtendedTypeProp);

            Console.WriteLine(string.Format("the call to Map did not return null '{0}'", testMappingHasAExtendedType != null));
            Console.WriteLine(string.Format("the top level class ID copied fine '{0}'",hasAExtendedTypeProp.TopId == testMappingHasAExtendedType.TheTopID));
            Console.WriteLine(string.Format("the child object, the point of this test, also copied '{0}'", testMappingHasAExtendedType.TheHASATarget != null));
            Console.WriteLine(string.Format("the child object's ID copied as well '{0}'",testMappingHasAExtendedType.TheHASATarget.TheID == extendedType.ID));

            //attempt it in reverse
            var testMappingHasAExtendedTypeAsDest =
                AutoMapper.Mapper.Map<HasAMappingTarget, HasAExtendedUnderlyingType>(testMappingHasAExtendedType);

            Console.WriteLine(string.Format("the mapping works in both directions when either the dest or src has its property cast '{0}'", testMappingHasAExtendedTypeAsDest.TheHASA.ID == testMappingHasAExtendedType.TheHASATarget.TheID));

        }
    }
}
