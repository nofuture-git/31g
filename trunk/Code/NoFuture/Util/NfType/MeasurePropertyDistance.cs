using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NoFuture.Util.NfType
{
    public class MeasurePropertyDistance
    {
        private const string STR_FN = "System.String";
        /// <summary>
        /// Reassignable flags for selecting properties
        /// </summary>
        public static BindingFlags DefaultFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance |
                                                  BindingFlags.Public;

        /// <summary>
        /// Attempts to assign or instantiate then assign the values of <see cref="toObj"/> with 
        /// the values from <see cref="fromObj"/> choosing the properties whose name&apos;s are the least 
        /// distance in which only those properties in <see cref="fromObjPropertyNames"/> are selected as targets.
        /// </summary>
        /// <param name="fromObj">
        /// Required, the object whose properties act as a source.
        /// </param>
        /// <param name="toObj">
        /// Required, the object whose properties are receiving assignment.
        /// </param>
        /// <param name="fromObjPropertyNames">
        /// Required, the calling assembly must provide 
        /// an explicit list of property names to target for copying.</param>
        /// <returns></returns>
        public static bool TryAssignProperties(object fromObj, object toObj, params string[] fromObjPropertyNames)
        {

            if (fromObj == null || toObj == null || fromObjPropertyNames == null || !fromObjPropertyNames.Any())
                return false;
            var flag = false;

            try
            {
                var bpis = toObj.GetType().GetProperties(DefaultFlags);
                var prevActions = new Dictionary<string, int>();

                foreach (var pn in fromObjPropertyNames)
                {
                    var api = fromObj.GetType().GetProperty(pn);

                    if (api == null || !api.CanRead)
                        continue;
                    //this will get us those closest on name only
                    var closestMatches = GetClosestMatch(api, fromObj, bpis, toObj);
                    if (closestMatches == null || !closestMatches.Any())
                        continue;
                    foreach (var cm in closestMatches)
                    {
                        //its possiable that two different pi names are both attempting to write to the exact same target pi in toObj
                        if (prevActions.ContainsKey(cm.Item3) && cm.Item2 >= prevActions[cm.Item3])
                        {
                            //we only want the one with the shortest distance
                            continue;
                        }

                        cm.Item1();
                        flag = true;
                        prevActions.Add(cm.Item3, cm.Item2);

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return false;
            }
            return flag;
        }

        /// <summary>
        /// Get a list of action, string distance measurement, and destintation object path for the properties on <see cref="toObj"/>
        /// which have the shortest distanct from the <see cref="fromPi"/> by name.  The return value is a list incase of a tie in which
        /// the calling assembly must decide which ones to execute.
        /// </summary>
        /// <param name="fromPi">The property whose value is to be copied over somewhere onto <see cref="toObj"/></param>
        /// <param name="fromObj">The source object of <see cref="fromPi"/> which is able to resolve it to an actual value.</param>
        /// <param name="toPi">The list of possiable canidate properties on <see cref="toObj"/>.</param>
        /// <param name="toObj">The destination object which is receiving property assignment.</param>
        /// <param name="namePrefix">Optional, used internally with <see cref="minLen"/></param>
        /// <param name="minLen">
        /// Optional, some minimum string length in which any property name which is -le it will have its name prefixed (or suffixed if its a shorter distance)
        /// with <see cref="namePrefix"/>. The most common case being something like EntityId should match to Entity.Id.
        /// </param>
        /// <returns>
        /// The Action (Item1) on the returned results will perform the actual assignment of one property to another.
        /// </returns>
        public static List<Tuple<Action, int, string>> GetClosestMatch(PropertyInfo fromPi, object fromObj, PropertyInfo[] toPi, object toObj, string namePrefix = null, int minLen = 2)
        {

            if (fromPi == null || toPi == null || !toPi.Any())
                return new List<Tuple<Action, int, string>>();

            //we want to map a whole assignment expression to a distance on name
            var ops2Score = new List<Tuple<Action, int, string>>();

            Func<PropertyInfo, bool, string> getMeasureableName = (gmnPi, inReverse) =>
            {
                if (gmnPi.Name.Length <= minLen)
                {
                    return inReverse ? string.Join("", gmnPi.Name, namePrefix) : string.Join("", namePrefix, gmnPi.Name);
                }
                return gmnPi.Name;
            };

            foreach (var cpi in toPi)
            {
                if (NfTypeName.IsValueTypeProperty(cpi, true))
                {
                    Action simpleAssignment = GetSimpleAssignment(cpi, toObj, fromPi, fromObj);
                    if (string.IsNullOrWhiteSpace(namePrefix))
                    {
                        //for simple value types -to- value types where dest has a very short name 
                        namePrefix = toObj.GetType().Name;
                    }
                    var fromPiName = getMeasureableName(fromPi, false);
                    var cpiName = getMeasureableName(cpi,false);
                    var fromPiReverseName = getMeasureableName(fromPi, true);
                    var cpiReverseName = getMeasureableName(cpi, true);

                    var score = (int)Etc.LevenshteinDistance(fromPiName, cpiName);

                    //with short property names, prehaps a better score is when prefix is a suffix instead
                    if (fromPiName != fromPiReverseName || cpiName != cpiReverseName)
                    {
                        var revScore = (int)Etc.LevenshteinDistance(fromPiReverseName, cpiReverseName);
                        if (revScore < score)
                            score = revScore;
                    }

                    ops2Score.Add(new Tuple<Action, int, string>(simpleAssignment, score, cpi.Name));
                }
                else
                {
                    var toInnerPi = cpi.PropertyType.GetProperties(DefaultFlags);
                    if (!toInnerPi.Any())
                        continue;
                    
                    var toInnerObj = cpi.GetValue(toObj) ?? Activator.CreateInstance(cpi.PropertyType);
                    var innerMeasurements = GetClosestMatch(fromPi, fromObj, toInnerPi, toInnerObj, cpi.Name, minLen);

                    if (!innerMeasurements.Any())
                        continue;

                    //these will by def, be the shortest distance
                    foreach (var innerM in innerMeasurements)
                    {
                        //we will chain-link these actions
                        Action complexAction = () =>
                        {
                            innerM.Item1();
                            if (cpi.GetValue(toObj) == null)
                                cpi.SetValue(toObj, toInnerObj);
                        };
                        var actionId = string.Join(".", cpi.Name, innerM.Item3);

                        ops2Score.Add(new Tuple<Action, int, string>(complexAction, innerM.Item2, actionId));
                    }
                }
            }

            var totalMin = ops2Score.Min(x => x.Item2);
            var closest = ops2Score.Where(x => x.Item2 == totalMin).ToList();

            return closest;
        }

        /// <summary>
        /// Performs the assignment of the <see cref="cpi"/> on the instance of <see cref="toObj"/> using 
        /// the value of <see cref="fromPi"/> on the instance of <see cref="fromObj"/> - attempting to 
        /// perform the needed casting of one kind of value type to another.
        /// </summary>
        /// <param name="cpi"></param>
        /// <param name="toObj"></param>
        /// <param name="fromPi"></param>
        /// <param name="fromObj"></param>
        /// <returns></returns>
        public static Action GetSimpleAssignment(PropertyInfo cpi, object toObj, PropertyInfo fromPi, object fromObj)
        {
            Action noop = () => { };

            if (cpi == null || toObj == null || fromPi == null || fromObj == null)
                return noop;

            //only deal in value types to value types
            if (!NfTypeName.IsValueTypeProperty(cpi, true) || !NfTypeName.IsValueTypeProperty(fromPi, true))
                return noop;

            //enums require alot of special handling especially when wrapped in Nullable`1[
            var cpiIsEnum = NfTypeName.IsPropertyEnum(cpi);
            var fromPiIsEnum = NfTypeName.IsPropertyEnum(fromPi);

            //get each pi's type
            var cpiType = cpiIsEnum ? NfTypeName.GetEnumType(cpi) : NfTypeName.GetPropertyValueType(cpi);
            var fromPiType = fromPiIsEnum ? NfTypeName.GetEnumType(fromPi) : NfTypeName.GetPropertyValueType(fromPi);

            //get each pi's type's full name
            var cpiTypeFullname = cpiType.FullName;
            var fromPiTypeFullname = fromPiType.FullName;

            //easy assignment for same types
            if (cpiTypeFullname == fromPiTypeFullname)
            {
                return () => cpi.SetValue(toObj, fromPi.GetValue(fromObj));
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
                        cpi.SetValue(toObj, enumName);
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
                        cpi.SetValue(toObj, enumVal);
                    }
                };
            }

            //going from enum to enum
            if (fromPiIsEnum && cpiIsEnum)
            {
                //will this require any cast?
                return () =>
                {
                    cpi.SetValue(toObj, fromPi.GetValue(fromObj));
                };
            }

            //going from some value-type to a string
            if (cpiTypeFullname == STR_FN)
            {
                return () => cpi.SetValue(toObj, fromPi.GetValue(fromObj).ToString());
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
                            cpi.SetValue(toObj, bout);
                    };
                case "System.Int16":
                    return () =>
                    {
                        short vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (short.TryParse(vStr, out vout))
                            cpi.SetValue(toObj, vout);
                    };
                case "System.Int32":
                    return () =>
                    {
                        int vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (int.TryParse(vStr, out vout))
                        {
                            cpi.SetValue(toObj, vout);
                        }
                    };
                case "System.DateTime":
                    return () =>
                    {
                        DateTime vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (DateTime.TryParse(vStr, out vout))
                            cpi.SetValue(toObj, vout);
                    };
                case "System.Decimal":
                    return () =>
                    {
                        decimal vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (decimal.TryParse(vStr, out vout))
                            cpi.SetValue(toObj, vout);
                    };
                case "System.Single":
                    return () =>
                    {
                        float vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (float.TryParse(vStr, out vout))
                            cpi.SetValue(toObj, vout);
                    };
                case "System.Double":
                    return () =>
                    {
                        double vout;
                        var piv = fromPi.GetValue(fromObj);
                        var vStr = piv == null ? "0" : piv.ToString();
                        if (double.TryParse(vStr, out vout))
                            cpi.SetValue(toObj, vout);
                    };
            }

            //default out to no operation
            return noop;
        }

    }
}
