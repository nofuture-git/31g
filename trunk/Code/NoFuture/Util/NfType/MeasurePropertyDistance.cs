using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NoFuture.Util.Core;

namespace NoFuture.Util.NfType
{
    /// <summary>
    /// Utility class for measuring properties on one instance to the properties on another.
    /// </summary>
    public class MeasurePropertyDistance
    {
        private const string STR_FN = "System.String";
        private const string ERROR_PREFIX = "[ERROR]";

        /// <summary>
        /// Reassignable flags for selecting properties
        /// </summary>
        public static BindingFlags DefaultFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance |
                                                  BindingFlags.Public;

        private static readonly List<string[]> assignPiLog = new List<string[]>();

        /// <summary>
        /// Gets info from any errors which occured in <see cref="TryAssignProperties"/>
        /// </summary>
        public static string[] GetAssignPropertiesErrors(string delimiter = "\t")
        {
            return
                assignPiLog.Where(x => x != null && x.Any() && x.FirstOrDefault() == ERROR_PREFIX)
                    .Select(x => string.Join(delimiter, x))
                    .ToArray();
        }

        /// <summary>
        /// Gets the mapping data from the last call to <see cref="TryAssignProperties"/>
        /// </summary>
        /// <param name="delimiter"></param>
        /// <param name="noHeaders"></param>
        /// <returns></returns>
        public static string[] GetAssignPropertiesData(string delimiter = "\t", bool noHeaders = false)
        {
            var data = new List<string>();
            if(!noHeaders)
                data.Add(string.Join(delimiter, "FromProperty", "ToProperty", "FromToPath", "Score"));
            var dataAdd =
                assignPiLog.Where(x => x != null && x.Any() && x.FirstOrDefault() != ERROR_PREFIX)
                    .Select(x => string.Join(delimiter, x))
                    .ToList();
            data.AddRange(dataAdd);
            return data.ToArray();
        }

        /// <summary>
        /// Attempts to assign all the property&apos;s values on <see cref="fromObj"/> to 
        /// some property on <see cref="toObj"/> based on the closest matching name.
        /// </summary>
        /// <param name="fromObj"></param>
        /// <param name="toObj"></param>
        /// <returns>1.0 when it was a perfect match</returns>
        /// <remarks>
        /// Use the <see cref="GetAssignPropertiesData"/> to see what it ended up choosing.
        /// </remarks>
        public static double TryAssignProperties(object fromObj, object toObj)
        {
            if (fromObj == null || toObj == null)
                return 0;
            var rScores = new List<Tuple<int, int>>();
            var prevActions = new Dictionary<string, int>();
            try
            {
                //remove any previous records.
                assignPiLog.Clear();

                //only fromObj's value-type props
                rScores.AddRange(TryAssignValueTypeProperties(fromObj, toObj, prevActions));

                //only fromObj's ref-type props
                foreach (var fromPi in fromObj.GetType().GetProperties(DefaultFlags))
                {
                    if (fromPi == null || !fromPi.CanRead)
                        continue;

                    if (NfReflect.IsValueTypeProperty(fromPi, true))
                    {
                        continue;
                    }

                    var fromPv = fromPi.GetValue(fromObj);
                    if (fromPv == null)
                    {
                        fromPv = Activator.CreateInstance(fromPi.PropertyType);
                        fromPi.SetValue(fromObj, fromPv);
                    }
                    rScores.AddRange(TryAssignValueTypeProperties(fromPv, toObj, prevActions, fromPi.Name));
                }

            }
            catch (Exception ex)
            {
                assignPiLog.Add(new[] { ERROR_PREFIX, ex.Message, ex.StackTrace });
                return 0;
            }

            return 1D - (double)rScores.Select(x => x.Item1).Sum() / rScores.Select(x => x.Item2).Sum();
        }

        /// <summary>
        /// Takes the values of the value-type-only-properties on <see cref="fromObj"/> and finds some
        /// property, no matter how deep-down in the obj graph, on <see cref="toObj"/> to assign that value to.
        /// </summary>
        /// <param name="fromObj"></param>
        /// <param name="toObj"></param>
        /// <param name="prevActions"></param>
        /// <param name="contextName"></param>
        internal static List<Tuple<int,int>> TryAssignValueTypeProperties(object fromObj, object toObj, Dictionary<string, int> prevActions, string contextName = null)
        {
            var df = new List<Tuple<int, int>> {new Tuple<int, int>(int.MaxValue, 0)};
            if (fromObj == null || toObj == null)
                return df;
            var rScores = new List<Tuple<int, int>>();
            try
            {
                var fromObjTypeName = fromObj.GetType().FullName;
                var fromObjPropertyNames = fromObj.GetType().GetProperties(DefaultFlags).Select(p => p.Name).ToArray();

                //if still nothing found - just leave
                if (!fromObjPropertyNames.Any())
                    return df;
                
                var toPis = toObj.GetType().GetProperties(DefaultFlags);
                prevActions = prevActions ?? new Dictionary<string, int>();

                foreach (var pn in fromObjPropertyNames)
                {
                    var fromPi = fromObj.GetType().GetProperty(pn);

                    if (fromPi == null || !fromPi.CanRead || !NfReflect.IsValueTypeProperty(fromPi,true))
                        continue;
                    //this will get us those closest on name only
                    var closestMatches = GetClosestMatch(fromPi, fromObj, toPis, toObj);
                    if (closestMatches == null || !closestMatches.Any())
                        continue;

                    //have to decide how to break a tie
                    if (closestMatches.Count > 1)
                    {
                        closestMatches = closestMatches.Where(x => x.Item3.Contains(pn)).ToList();
                    }
                    foreach (var cm in closestMatches)
                    {
                        //its possiable that two different pi names are both attempting to write to the exact same target pi in toObj
                        if (prevActions.ContainsKey(cm.Item3) && cm.Item2 >= prevActions[cm.Item3])
                        {
                            //we only want the one with the shortest distance
                            continue;
                        }

                        //exec the assignment on the target
                        cm.Item1();

                        //get this distance as a ratio to the possiable max distance
                        rScores.Add(new Tuple<int, int>(cm.Item2, pn.Length));

                        //add this to the log 
                        var logPn = !string.IsNullOrWhiteSpace(contextName) ? string.Join(".", contextName, pn) : pn;
                        var logPath = !string.IsNullOrWhiteSpace(contextName) ? string.Join("`", fromObjTypeName, cm.Item4) : cm.Item4;
                        assignPiLog.Add(new[] { logPn, cm.Item3, logPath, cm.Item2.ToString()});

                        prevActions.Add(cm.Item3, cm.Item2);

                    }
                }
            }
            catch (Exception ex)
            {
                assignPiLog.Add(new[] {ERROR_PREFIX, ex.Message, ex.StackTrace});
                return df;
            }

            //return average
            return  rScores;
        }

        /// <summary>
        /// Get a list of action, string distance measurement, destination object path in dot-notation, and additional info for logging
        /// for the properties on <see cref="toObj"/> which have the shortest distanct from the <see cref="fromPi"/> by name.  
        /// The return value is a list incase of a tie in which the calling assembly must decide which ones to execute.
        /// </summary>
        /// <param name="fromPi">The property whose value is to be copied over somewhere onto <see cref="toObj"/></param>
        /// <param name="fromObj">The source object of <see cref="fromPi"/> which is able to resolve it to an actual value.</param>
        /// <param name="toPis">The list of possiable canidate properties on <see cref="toObj"/>.</param>
        /// <param name="toObj">The destination object which is receiving property assignment.</param>
        /// <param name="namePrefix">Optional, used internally with <see cref="minLen"/></param>
        /// <param name="minLen">
        /// Optional, some minimum string length in which any property name which is -le it will have its name prefixed (or suffixed if its a shorter distance)
        /// with <see cref="namePrefix"/>. The most common case being something like EntityId should match to Entity.Id.
        /// </param>
        /// <returns>
        /// The Action (Item1) on the returned results will perform the actual assignment of one property to another.
        /// </returns>
        internal static List<Tuple<Action, int, string, string>> GetClosestMatch(PropertyInfo fromPi, object fromObj, PropertyInfo[] toPis, object toObj, string namePrefix = null, int minLen = 2)
        {

            if (fromPi == null || toPis == null || !toPis.Any())
                return new List<Tuple<Action, int, string, string>>();

            //we want to map a whole assignment expression to a distance on name
            var ops2Score = new List<Tuple<Action, int, string, string>>();

            Func<PropertyInfo, string, bool, string> getMeasureableName = (gmnPi, prefix, inReverse) =>
            {
                if (gmnPi.Name.Length <= minLen)
                {
                    return inReverse ? string.Join("", gmnPi.Name, prefix) : string.Join("", prefix, gmnPi.Name);
                }
                return gmnPi.Name;
            };

            foreach (var toPi in toPis)
            {
                if (NfReflect.IsValueTypeProperty(toPi, true))
                {
                    string toFromTns;
                    Action simpleAssignment = GetSimpleAssignment(toPi, toObj, fromPi, fromObj, out toFromTns);
                    if (string.IsNullOrWhiteSpace(namePrefix))
                    {
                        //for simple value types -to- value types where dest has a very short name 
                        namePrefix = toObj.GetType().Name;
                    }
                    var fromPiName = getMeasureableName(fromPi, namePrefix, false);
                    var cpiName = getMeasureableName(toPi, namePrefix, false);
                    var fromPiReverseName = getMeasureableName(fromPi, namePrefix, true);
                    var cpiReverseName = getMeasureableName(toPi, namePrefix, true);

                    var score = (int)Etc.LevenshteinDistance(fromPiName, cpiName);

                    //with short property names, prehaps a better score is when prefix is a suffix instead
                    if (fromPiName != fromPiReverseName || cpiName != cpiReverseName)
                    {
                        var revScore = (int)Etc.LevenshteinDistance(fromPiReverseName, cpiReverseName);
                        if (revScore < score)
                            score = revScore;
                    }

                    ops2Score.Add(new Tuple<Action, int, string, string>(simpleAssignment, score, toPi.Name, toFromTns));
                }
                else
                {
                    var toInnerPi = toPi.PropertyType.GetProperties(DefaultFlags);
                    if (!toInnerPi.Any())
                        continue;
                    
                    var toInnerObj = toPi.GetValue(toObj) ?? Activator.CreateInstance(toPi.PropertyType);
                    var innerMeasurements = GetClosestMatch(fromPi, fromObj, toInnerPi, toInnerObj, toPi.Name, minLen);

                    if (!innerMeasurements.Any())
                        continue;

                    //these will by def, be the shortest distance
                    foreach (var innerM in innerMeasurements)
                    {
                        //we will chain-link these actions
                        Action complexAction = () =>
                        {
                            innerM.Item1();
                            if (toPi.GetValue(toObj) == null)
                                toPi.SetValue(toObj, toInnerObj);
                        };
                        var actionId = string.Join(".", toPi.Name, innerM.Item3);

                        ops2Score.Add(new Tuple<Action, int, string, string>(complexAction, innerM.Item2, actionId, string.Join("`", toPi.PropertyType.FullName, innerM.Item4) ));
                    }
                }
            }

            var totalMin = ops2Score.Min(x => x.Item2);
            var closest = ops2Score.Where(x => x.Item2 == totalMin).ToList();

            return closest;
        }

        /// <summary>
        /// Performs the assignment of the <see cref="toPi"/> on the instance of <see cref="toObj"/> using 
        /// the value of <see cref="fromPi"/> on the instance of <see cref="fromObj"/> - attempting to 
        /// perform the needed casting of one kind of value type to another.
        /// </summary>
        /// <param name="toPi"></param>
        /// <param name="toObj"></param>
        /// <param name="fromPi"></param>
        /// <param name="fromObj"></param>
        /// <param name="logInfo"></param>
        /// <returns></returns>
        public static Action GetSimpleAssignment(PropertyInfo toPi, object toObj, PropertyInfo fromPi, object fromObj, out string logInfo)
        {
            Action noop = () => { };
            logInfo = null;
            if (toPi == null || toObj == null || fromPi == null || fromObj == null)
                return noop;

            //only deal in value types to value types
            if (!NfReflect.IsValueTypeProperty(toPi, true) || !NfReflect.IsValueTypeProperty(fromPi, true))
                return noop;

            //enums require alot of special handling especially when wrapped in Nullable`1[
            var cpiIsEnum = NfReflect.IsPropertyEnum(toPi);
            var fromPiIsEnum = NfReflect.IsPropertyEnum(fromPi);

            //get each pi's type
            var cpiType = cpiIsEnum ? NfReflect.GetEnumType(toPi) : NfReflect.GetPropertyValueType(toPi);
            var fromPiType = fromPiIsEnum ? NfReflect.GetEnumType(fromPi) : NfReflect.GetPropertyValueType(fromPi);

            //get each pi's type's full name
            var cpiTypeFullname = cpiType.FullName;
            var fromPiTypeFullname = fromPiType.FullName;

            logInfo = string.Join("->", fromPiTypeFullname, cpiTypeFullname);

            //easy assignment for same types
            if (cpiTypeFullname == fromPiTypeFullname)
            {
                return () => toPi.SetValue(toObj, fromPi.GetValue(fromObj));
            }

            //going from Enum to a string
            if (fromPiIsEnum && cpiTypeFullname == STR_FN)
            {
                //take enum value and get its name
                return () =>
                {
                    var enumName = Enum.GetName(fromPiType, fromPi.GetValue(fromObj));
                    if (enumName != null)
                    {
                        toPi.SetValue(toObj, enumName);
                    }
                };
            }

            //going from a string to an enum
            if (cpiIsEnum && fromPiTypeFullname == STR_FN)
            {
                return () =>
                {
                    var val = fromPi.GetValue(fromObj);
                    if (val != null && !string.IsNullOrWhiteSpace(val.ToString()) &&
                        Enum.GetNames(cpiType)
                            .Any(x => string.Equals(x, val.ToString(), StringComparison.OrdinalIgnoreCase)))
                    {
                        var enumVal = Enum.Parse(cpiType, val.ToString(), true);
                        toPi.SetValue(toObj, enumVal);
                    }
                };
            }

            //going from enum to enum
            if (fromPiIsEnum && cpiIsEnum)
            {
                //will this require any cast?
                return () =>
                {
                    toPi.SetValue(toObj, fromPi.GetValue(fromObj));
                };
            }

            //going from some value-type to a string
            if (cpiTypeFullname == STR_FN)
            {
                return () => toPi.SetValue(toObj, fromPi.GetValue(fromObj).ToString());
            }

            //going from something to value-type
            switch (cpiTypeFullname)
            {
                case "System.Byte":
                    return () =>
                    {
                        byte bout;
                        var bpiv = fromPi.GetValue(fromObj);
                        var byteString = bpiv == null ? "0" : bpiv.ToString();
                        if (byte.TryParse(byteString, out bout))
                            toPi.SetValue(toObj, bout);
                    };
                case "System.Int16":
                    return () =>
                    {
                        short vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (short.TryParse(vStr, out vout))
                            toPi.SetValue(toObj, vout);
                    };
                case "System.Int32":
                    return () =>
                    {
                        int vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (int.TryParse(vStr, out vout))
                        {
                            toPi.SetValue(toObj, vout);
                        }
                    };
                case "System.DateTime":
                    return () =>
                    {
                        DateTime vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (DateTime.TryParse(vStr, out vout))
                            toPi.SetValue(toObj, vout);
                    };
                case "System.Decimal":
                    return () =>
                    {
                        decimal vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (decimal.TryParse(vStr, out vout))
                            toPi.SetValue(toObj, vout);
                    };
                case "System.Single":
                    return () =>
                    {
                        float vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (float.TryParse(vStr, out vout))
                            toPi.SetValue(toObj, vout);
                    };
                case "System.Double":
                    return () =>
                    {
                        double vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (double.TryParse(vStr, out vout))
                            toPi.SetValue(toObj, vout);
                    };
            }

            //default out to no operation
            return noop;
        }

    }
}
