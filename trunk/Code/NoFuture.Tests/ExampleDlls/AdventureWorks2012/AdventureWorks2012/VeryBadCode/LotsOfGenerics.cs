using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.VeryBadCode
{

    [Serializable]
    public class Agency
    {
        public string Name { get; set; }
        public string Code;
        public string Phone;

    }

    [Serializable]
    public class Applicant
    {
        public string FirstName;
        public string LastName;
    }

    [Serializable]
    public class Order
    {
        public Agency Agency;
        public Applicant Applicant;
    }

    public enum MyEnum
    {
        One,
        Two,
        Five,
        ThreeSir,
        Three
    }

    public class BasicGenerics
    {
        public string TakesGenericArg(List<SomeSecondDll.MyFirstMiddleClass> myGenericArg)
        {
            return "nothing happened";
        }

        public int TakesThisAsmGenericArg(List<Order> myGenericArg)
        {
            return -1;
        }
    }

    public class Registry<TEntity, TData> where TEntity : new()
    {
        private readonly TEntity _entity;
        private readonly PropertyInfo[] _pis;
        private readonly FieldInfo[] _fis;
        private Dictionary<string, Func<TData, object>> _propToExprHash;

        public MyEnum? ANullableEnum { get; set; }

        public Registry()
        {
            _entity = new TEntity();
            _pis = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            _fis = typeof(TEntity).GetFields(BindingFlags.Instance | BindingFlags.Public);
            _propToExprHash = new Dictionary<string, Func<TData, object>>();
        }

        public TEntity Entity { get { return _entity; } }

        public void MyMapping(Expression<Func<TEntity, object>> target, Func<TData, object> source)
        {

            var body = target.Body;
            string memberName = null;
            if (body.NodeType == ExpressionType.MemberAccess)
            {
                var linqMemberExpr = body as MemberExpression;
                if (linqMemberExpr == null)
                    return;

                memberName = linqMemberExpr.Member.Name;
            }
            if (body.NodeType == ExpressionType.Convert)
            {
                var linqUnaryExpr = body.Reduce() as UnaryExpression;
                if (linqUnaryExpr == null)
                    return;

                var unaryOpand = linqUnaryExpr.Operand.Reduce();
                var memberExpr = unaryOpand as MemberExpression;
                if (memberExpr == null)
                    return;

                memberName = memberExpr.Member.Name;
            }

            if (string.IsNullOrEmpty(memberName) || _propToExprHash.Keys.Any(x => x == memberName))
                return;

            _propToExprHash.Add(memberName, source);

        }

        public TEntity ResolveToEntity(TData data)
        {
            foreach (var memberName in _propToExprHash.Keys)
            {
                var result = _propToExprHash[memberName](data);
                var pi = _pis.FirstOrDefault(p => p.Name == memberName);
                if (pi != null)
                {
                    pi.SetValue(_entity, result, null);
                    continue;
                }
                var fi = _fis.FirstOrDefault(f => f.Name == memberName);
                if (fi != null)
                {
                    fi.SetValue(_entity, result);
                }
            }
            return _entity;
        }
    }

    public class Program
    {

        public static void Main(string[] args)
        {
            try
            {
                var testData = @"AY01      AGENCY NAME HERE.                           8885550170            8885550270                                              ";
                var testRegistry = new Registry<Agency, string>();

                testRegistry.MyMapping(agy => agy.Name, d => d.Substring(11, 43));
                testRegistry.MyMapping(agy => agy.Phone, d => d.Substring(54, 10));

                var testResult = testRegistry.ResolveToEntity(testData);
                Console.WriteLine(testResult.Name);
                Console.WriteLine(testResult.Phone);

                var testRegContainer = new Registry<Order, string[]>();

                var testOrderData = new[]
                                    {
                                        "OR01V01OO2805900                       CASEONE                                                                                N     ",
                                        "AN01single                        vscombo             F                         55555555         FC                                 ",
                                        "AN028205 S CASS TEST                        8885550170       8885550170            United States       US   8885550170              ",
                                        "AN03                                                                                ANY TOWNNAME               IL77777              ",
                                        "AN04                                                                     OOOOOO@DOMAINNAME.COM                                      ",
                                        "CO01IOOP4830      Some long name appears here                           1555555                                                     ",
                                        "AY01      AGENCT NAME, INC.                           8885550170            8885550270                                              ",
                                        "AG01000000    UNKNOWN                  FNAME LNAMED                                                                                 ",
                                        "PI011 00000200000                                                                                    NNN        NN                  ",
                                        "PC01EXAM1     QQQQQQQ RR VVVVVVVVVVV                  Y                                                                             ",
                                        "SR01    000       TT Collection day 1                                                                                               ",
                                        "SR02    000       YY Collection day 2                                                                                               ",
                                        "SE0120110307200000                                                                                                                  "

                                    };

                testRegContainer.MyMapping(o => o.Agency, lns => testRegistry.ResolveToEntity(lns.FirstOrDefault(ln => ln.Substring(0, 2) == "AY")));

                var testOrderRslt = testRegContainer.ResolveToEntity(testOrderData);
                Console.WriteLine(testOrderRslt.Agency == null);
                Console.WriteLine(testOrderRslt.Agency.Name);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.ReadKey();
        }

    }
}
