﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Tokens.DotNetMeta.TokenAsm;
using NoFuture.Tokens.DotNetMeta.TokenId;
using NoFuture.Tokens.DotNetMeta.TokenType;
using NoFuture.Util.Core;

namespace NoFuture.Tokens.DotNetMeta.TokenName
{
    /// <summary>
    /// The resolved name of a single metadata token
    /// </summary>
    [Serializable]
    public class MetadataTokenName : INfToken
    {
        #region fields
        [NonSerialized] private MetadataTokenName[] _items;
        [NonSerialized] private bool _isByRef;
        [NonSerialized] private readonly MetadataTokenNameComparer _comparer = new MetadataTokenNameComparer();
        [NonSerialized] private int? _fullDepthCount;
        [NonSerialized] private bool? _anyByRef;
        [NonSerialized] private int _idx = 0;
        #endregion

        #region properties
        /// <summary>
        /// The original metadata token id
        /// </summary>
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// The index of the <see cref="Assembly"/> which contains 
        /// the type's definition.
        /// </summary>
        public int OwnAsmIdx { get; set; }

        /// <summary>
        /// The index of the <see cref="Assembly"/>  with 
        /// whose <see cref="Assembly.ManifestModule"/>
        /// the <see cref="Id"/> was resolved.
        /// </summary>
        public int RslvAsmIdx { get; set; }

        /// <summary>
        /// A grouping label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The id which matches back to the type in which this name was declared.
        /// </summary>
        public int DeclTypeId { get; set; }

        /// <summary>
        /// For the calling assembly to construct the full mapping.
        /// </summary>
        public MetadataTokenName[] Items
        {
            get => _items;
            set => _items = value;
        }

        /// <summary>
        /// A flag to indicate this is one of many concrete implementations
        /// of some abstract member defined elsewhere or it 
        /// is itself abstract (i.e. an interface token)
        /// </summary>
        public bool IsAmbiguous { get; set; }
        #endregion

        /// <summary>
        /// Convenience method to get Items count
        /// </summary>
        public int Count()
        {
            return Items?.Length ?? 0;
        }

        /// <summary>
        /// Convenience method to add another item to the <see cref="Items"/> array with equality checks
        /// </summary>
        public void AddItem(MetadataTokenName item)
        {
            if (item == null)
                return;
            if (Items.Any(i => _comparer.Equals(item, i)))
                return;

            var buffer = new MetadataTokenName[Items.Length + 1];
            Array.Copy(Items, buffer, Items.Length);
            buffer[Items.Length] = item;
            Items = buffer;
        }

        /// <summary>
        /// Indicates if this instance is a list or tree data structure
        /// </summary>
        /// <returns></returns>
        public bool IsFlattened()
        {
            if (Items == null || !Items.Any())
                return false;

            foreach (var i in Items)
            {
                if ((i?.Count() ?? 0) > 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Indicates a short-hand version of some fuller copy of itself elsewhere 
        /// in the parent&apos;s items.
        /// </summary>
        /// <remarks>
        /// While crawling the call-of-call tokens, when the algo comes across some
        /// token its already expanded, it will mark this flag and move on in order 
        /// to avoid having to expand it again.
        /// </remarks>
        public bool IsByRef
        {
            get => _isByRef;
            set => _isByRef = value;
        }

        protected internal bool IsRoot()
        {
            return string.Equals(Name, NfSettings.DefaultTypeSeparator.ToString());
        }

        /// <summary>
        /// Determines, by naming pattern, if the current name was derived from <see cref="MethodInfo"/>
        /// </summary>
        /// <returns></returns>
        public bool IsMethodName()
        {
            return !String.IsNullOrWhiteSpace(Name)
                   && Name.Contains(Constants.TYPE_METHOD_NAME_SPLIT_ON)
                   && Name.Contains("(")
                   && Name.Contains(")");
        }

        /// <summary>
        /// Asserts if the token is derived from a type rather than
        /// a member token.
        /// </summary>
        /// <returns></returns>
        public bool IsTypeName()
        {
            return !String.IsNullOrWhiteSpace(Name)
                   && !String.IsNullOrWhiteSpace(Label)
                   && String.Equals("RuntimeType", Label);
        }

        /// <summary>
        /// Asserts if even one token name at any depth is by-ref
        /// </summary>
        /// <returns></returns>
        public bool IsAnyByRef()
        {

            if (IsByRef)
                return true;
            if (Items == null || !Items.Any())
                return false;
            //use prev. cache if avail
            if (_anyByRef != null)
                return _anyByRef.Value;
            foreach (var i in Items)
            {
                if (i.IsByRef)
                {
                    _anyByRef = true;
                    return true;
                }

                if (i.IsAnyByRef())
                {
                    _anyByRef = true;
                    return true;
                }
            }
            _anyByRef = false;
            return false;
        }

        /// <summary>
        /// Asserts that the part of the token name which
        /// matches the assembly name is omitted.
        /// </summary>
        /// <returns></returns>
        public bool IsPartialName()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   Name.StartsWith(NfSettings.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Puts the results of an Assembly Analysis into a full tree of token names
        /// </summary>
        /// <param name="tokenNames">
        /// This represents all possible names, but its a flat list, not a tree.
        /// </param>
        /// <param name="tokenIds">
        /// This represents the call-token tree, but it has no names.
        /// </param>
        /// <param name="asmIndices">
        /// This is needed since the names are all partial (to save space on the transferred json).
        /// </param>
        /// <param name="tokenTypes">
        /// Optional, passing this in will signal that all interfaces with only one implementation
        /// should have their tokens swapped out for the concrete implementation counterpart.
        /// </param>
        /// <param name="reportProgress">
        /// Optional, allows caller to get feedback on progress since this takes some time to run.
        /// </param>
        /// <returns>
        /// A single root token name where the first layer of Items is the various types, followed by
        /// those type&apos;s methods, followed by those methods call and so on.
        /// </returns>
        public static MetadataTokenName BuildMetadataTokenName(MetadataTokenName tokenNames, MetadataTokenId tokenIds,
            AsmIndexResponse asmIndices, MetadataTokenType tokenTypes = null, Action<ProgressMessage> reportProgress = null)
        {
            if (tokenNames == null)
                throw new ArgumentNullException(nameof(tokenNames));
            if (tokenIds == null)
                throw new ArgumentNullException(nameof(tokenIds));
            if (asmIndices == null)
                throw new ArgumentNullException(nameof(asmIndices));
            tokenNames.Name = NfSettings.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture);
            tokenNames.ApplyFullName(asmIndices);
            var tokenNamesOut = tokenNames.BindTokens2Names(tokenIds, reportProgress);
            //turn all pointer'esque tokens into their full-expanded counterparts
            tokenNamesOut.ReassignAllByRefs(reportProgress);
            if (tokenTypes != null && tokenTypes.Count() > 0)
            {
                //reassign terminating interface tokens to the concrete counterparts
                tokenNamesOut.ReassignAllInterfaceTokens(tokenTypes, reportProgress);
            }

            return tokenNamesOut;
        }

        /// <summary>
        /// Fills in the rest of the <see cref="Name"/> with the part which is defined by its assembly
        /// </summary>
        /// <param name="asmIndicies"></param>
        public void ApplyFullName(AsmIndexResponse asmIndicies)
        {
            if (!IsPartialName())
                return;
            if (asmIndicies?.Asms == null || !asmIndicies.Asms.Any())
                return;

            var asm = asmIndicies.Asms.FirstOrDefault(x => x.IndexId == OwnAsmIdx);
            if (asm == null)
                return;
            var asmName = new AssemblyName(asm.AssemblyName);

            //when the name has been explicitly set as the root - leave it alone
            if(!string.Equals(Name, NfSettings.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture)))
                Name = asmName.Name + Name;

            if (Items == null || !Items.Any())
                return;
            //recurse down the tree
            foreach (var myItems in Items)
            {
                myItems.ApplyFullName(asmIndicies);
            }
        }

        /// <summary>
        /// Final analysis to merge the token Ids to their names in a hierarchy.
        /// </summary>
        /// <param name="tokenIds"></param>
        /// <param name="reportProgress">
        /// Optional, allows caller to get feedback on progress since this takes some time to run.
        /// </param>
        /// <returns></returns>
        internal MetadataTokenName BindTokens2Names(MetadataTokenId tokenIds, Action<ProgressMessage> reportProgress = null)
        {
            if (tokenIds?.Items == null || !tokenIds.Items.Any())
                return this;
            if (Items == null || Items.Length <= 0)
                return this;

            var nameMapping = new List<MetadataTokenName>();
            var total = tokenIds.Items.Length;
            var counter = 0;
            foreach (var tokenId in tokenIds.Items)
            {
                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = $"{tokenId.Id}",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(counter, total),
                    Status = "Applying token name to token ids"
                });
                counter += 1;
                nameMapping.Add(BindToken2Name(tokenId));
            }
            return new MetadataTokenName
            {
                Items = nameMapping.ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture)
            };
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal MetadataTokenName BindToken2Name(MetadataTokenId tokenId)
        {
            if (tokenId == null || Items == null)
                return null;
            var nm = new MetadataTokenName { Id = tokenId.Id, RslvAsmIdx = tokenId.RslvAsmIdx };
            var frmNm = Items.FirstOrDefault(x => x.Id == tokenId.Id && x.RslvAsmIdx == tokenId.RslvAsmIdx);
            if (frmNm == null)
                return nm;

            nm.OwnAsmIdx = frmNm.OwnAsmIdx;
            nm.Label = frmNm.Label;
            nm.Name = frmNm.Name;
            nm.IsByRef = tokenId.IsByRef > 0;
            nm.DeclTypeId = frmNm.DeclTypeId;
            nm.IsAmbiguous = frmNm.IsAmbiguous;

            if (tokenId.Items == null || tokenId.Items.Length <= 0)
                return nm;

            var hs = new List<MetadataTokenName>();
            foreach (var tToken in tokenId.Items)
            {
                var nmTokens = BindToken2Name(tToken);
                if (nmTokens == null)
                    continue;
                hs.Add(nmTokens);
            }

            nm.Items = hs.ToArray();

            return nm;
        }

        /// <summary>
        /// Converts a token names, in a tree data structure, into its
        /// token id equiv.
        /// </summary>
        /// <returns></returns>
        public MetadataTokenId Convert2MetadataTokenId()
        {
            return new MetadataTokenId
            {
                Id = Id,
                Items = Items?.Select(i => i.Convert2MetadataTokenId()).ToArray(),
                RslvAsmIdx = RslvAsmIdx
            };
        }

        /// <summary>
        /// Gets an copy of this instance less its <see cref="Items"/>
        /// </summary>
        /// <returns></returns>
        public MetadataTokenName GetShallowCopy()
        {
            return new MetadataTokenName
            {
                Name = Name,
                DeclTypeId = DeclTypeId,
                Id = Id,
                IsByRef = IsByRef,
                Label = Label,
                OwnAsmIdx = OwnAsmIdx,
                RslvAsmIdx = RslvAsmIdx,
                IsAmbiguous =  IsAmbiguous,
            };
        }

        /// <summary>
        /// Gets the count of leaf nodes at all depths
        /// </summary>
        /// <returns></returns>
        public int GetFullDepthCount()
        {
            var c = 1;
            if (Items == null || !Items.Any())
                return c;
            //use prev. cache if avail
            if (_fullDepthCount != null)
                return _fullDepthCount.Value;
            foreach (var i in Items)
                c += i.GetFullDepthCount();

            //cache instance level to improv. perf.
            _fullDepthCount = c;
            return c;
        }

        /// <summary>
        /// Gets just the member name with its args
        /// </summary>
        /// <returns></returns>
        public string GetMemberName()
        {
            const string SPLT = Constants.TYPE_METHOD_NAME_SPLIT_ON;
            if (!IsMethodName())
                return string.Empty;

            var idx = Name.IndexOf(SPLT);

            if (idx <= 0)
            {
                return string.Empty;
            }

            return Name.Substring(idx + SPLT.Length);
        }

        /// <summary>
        /// Gets just the member name without its args
        /// </summary>
        /// <returns></returns>
        public string GetMemberNameWithoutArgs()
        {

            if (!IsMethodName())
                return string.Empty;
            var memberName = GetMemberName();
            if (memberName.Contains("("))
            {
                memberName = memberName.Split('(')[0];
            }

            return memberName;
        }

        /// <summary>
        /// Gets the full namespace qual. type name without any generic parameters
        /// </summary>
        /// <returns></returns>
        public string GetTypeName()
        {
            return AssemblyAnalysis.GetTypeName(Name);
        }

        /// <summary>
        /// Gets just the namespace part of the type name
        /// </summary>
        /// <returns></returns>
        public string GetNamespaceName()
        {
            return NfReflect.GetNamespaceWithoutTypeName(GetTypeName()) ?? String.Empty;
        }

        /// <summary>
        /// Finds the first matching token name which is <see cref="IsByRef"/> false.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        internal MetadataTokenName GetFirstByVal(MetadataTokenName tokenName)
        {
            if (tokenName == null)
                return null;
            Func<MetadataTokenName, bool> searchFor = (v) => _comparer.Equals(tokenName, v) && !v.IsByRef;
            MetadataTokenName locatedName = new MetadataTokenName();
            Func<MetadataTokenName, MetadataTokenName> getIt = (v) => locatedName = v;

            IterateTree(searchFor, getIt);
            return locatedName;
        }

        /// <summary>
        /// Gets all by-ref token names from all depths 
        /// </summary>
        /// <param name="tokenNames"></param>
        internal void GetAllByRefNames(List<MetadataTokenName> tokenNames = null)
        {
            Func<MetadataTokenName, bool> searchFor = (v) => v.IsByRef;
            tokenNames = tokenNames ?? new List<MetadataTokenName>();

            Func<MetadataTokenName, MetadataTokenName> addIt = (v) =>
            {
                if (v == null)
                    return null;
                tokenNames.Add(v);
                return v;
            };

            IterateTree(searchFor, addIt);
        }

        /// <summary>
        /// Get the method names attached to the given token type
        /// </summary>
        /// <param name="tokenType"></param>
        /// <param name="tokenNames"></param>
        public void GetAllDeclNames(MetadataTokenType tokenType, List<MetadataTokenName> tokenNames = null)
        {
            tokenNames = tokenNames ?? new List<MetadataTokenName>();
            if (tokenType == null)
                return;
            Func<MetadataTokenName, bool> searchFor = (v) => TypeNameEquals(tokenType, v);
            Func<MetadataTokenName, MetadataTokenName> addIt = (v) =>
            {
                if (v == null)
                    return null;
                tokenNames.Add(v);
                return v;
            };

            IterateTree(searchFor, addIt);
        }

        /// <summary>
        /// Gets the names which are exclusive to <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <param name="rightListTopLvlOnly"></param>
        /// <returns></returns>
        public MetadataTokenName GetRightSetDiff(MetadataTokenName otherNames, bool rightListTopLvlOnly = false)
        {
            if (otherNames?.Items == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (rightList.Items == null || rightList.Items.Length <= 0)
                return this;
            if (leftList.Items == null || leftList.Items.Length <= 0)
                return rightList;

            var setOp = rightList.Items.Select(hashCode).Except(leftList.Items.Select(hashCode));

            var listOut = new List<MetadataTokenName>();
            foreach (var j in setOp)
            {
                var k = rightList.Items.FirstOrDefault(x => hashCode(x) == j);
                if (k == null || rightListTopLvlOnly && k.OwnAsmIdx != 0)
                    continue;
                listOut.Add(k);
            }

            return new MetadataTokenName
            {
                Items = listOut.ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Joins the distinct names of this instance to the names of <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <returns></returns>
        public MetadataTokenName GetUnion(MetadataTokenName otherNames)
        {
            if (otherNames?.Items == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<INfToken, int> hashCode = x => x.GetNameHashCode();

            var d = rightList.Items.Distinct(_comparer).ToDictionary(hashCode);
            var e = leftList.Items.Distinct(_comparer).ToDictionary(hashCode);

            foreach (var key in e.Keys.Where(k => !d.ContainsKey(k)))
                d.Add(key, e[key]);

            return new MetadataTokenName
            {
                Items = d.Values.Cast<MetadataTokenName>().ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Gets the names which are shared between this instance and <see cref="otherNames"/>
        /// </summary>
        /// <param name="otherNames"></param>
        /// <returns></returns>
        public MetadataTokenName GetIntersect(MetadataTokenName otherNames)
        {
            if (otherNames?.Items == null)
                return this;
            var leftList = this;
            var rightList = otherNames;
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (rightList.Items == null || rightList.Items.Length <= 0)
                return this;
            if (leftList.Items == null || leftList.Items.Length <= 0)
                return rightList;

            var setOp = rightList.Items.Select(hashCode).Intersect(leftList.Items.Select(hashCode));

            var listOut = new List<MetadataTokenName>();
            foreach (var j in setOp)
            {
                //should be in either list
                var k = leftList.Items.FirstOrDefault(x => hashCode(x) == j);
                if (k == null)
                    continue;
                listOut.Add(k);
            }

            return new MetadataTokenName
            {
                Items = listOut.ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Gets all unique type names from all depths
        /// </summary>
        /// <returns></returns>
        public string[] GetUniqueTypeNames()
        {
            return Items.Select(n => n.GetTypeName()).Distinct().ToArray();
        }

        /// <summary>
        /// Locates all the interface types in <see cref="tokenTypes"/>, finds the belonging 
        /// token names thereof, with likewise concrete counterpart.
        /// </summary>
        /// <param name="tokenTypes">
        /// A root token type whose Items are all possible types in all assemblies.
        /// </param>
        /// <param name="reportProgress">
        /// Optional, allows caller to get feedback on progress since this takes some time to run.
        /// </param>
        /// <param name="foreignAssembly">See annotation on this methods overload</param>
        /// <returns>
        /// Dictionary where the keys are the interface token names, values are the concrete implementation
        /// </returns>
        public Dictionary<MetadataTokenName, MetadataTokenName> GetImplementorDictionary(MetadataTokenType tokenTypes,
            Action<ProgressMessage> reportProgress = null, MetadataTokenName foreignAssembly = null)
        {
            var n2n = new Dictionary<MetadataTokenName, MetadataTokenName>();
            if (tokenTypes?.Items == null)
                return n2n;
            var reassignInterfaces = tokenTypes.GetAllInterfacesWithSingleImplementor(reportProgress);
            if (reassignInterfaces == null || !reassignInterfaces.Any())
                return n2n;
            var totalLen = reassignInterfaces.Length;

            for (var i = 0; i < totalLen; i++)
            {
                var ri = reassignInterfaces[i];
                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = $"{ri.Name}",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(i, totalLen),
                    Status = "Getting interface-to-implementation"
                });
                var cri = tokenTypes.GetFirstInterfaceImplementor(ri);
                if (cri == null)
                    continue;

                var temp = GetImplementorDictionary(ri, cri, foreignAssembly);

                if (temp == null || temp.Count <= 0)
                    continue;
                foreach (var k in temp.Keys)
                {
                    if (n2n.ContainsKey(k))
                        n2n[k] = temp[k];
                    else
                        n2n.Add(k, temp[k]);
                }
            }
            return n2n;
        }

        /// <summary>
        /// Gets a dictionary of token names whose keys are the interface&apos;s token while
        /// the values are the concrete implementation thereof.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="concreteType"></param>
        /// <param name="foreignAssembly">
        /// The typical use is that the interface and its implementation are both defined 
        /// within the scope of the analysis.  However, it may be the case that this assembly
        /// only knows the concrete type and the use of the interface is found in some foreign 
        /// assembly (or vice versa). Therefore, in order to resolve the dictionary, in this case,
        /// the caller must pass in the token name analysis of the foreign assembly.
        /// </param>
        /// <returns></returns>
        protected internal Dictionary<MetadataTokenName, MetadataTokenName> GetImplementorDictionary(MetadataTokenType interfaceType,
            MetadataTokenType concreteType, MetadataTokenName foreignAssembly = null)
        {
            var n2n = new Dictionary<MetadataTokenName, MetadataTokenName>();
            if (interfaceType == null || concreteType == null)
                return n2n;
            if (Items == null || !Items.Any())
                return n2n;
            var interfaceNames = interfaceType.AbstractMemberNames?.ToList() ?? new List<MetadataTokenName>();
            var concreteNames = new List<MetadataTokenName>();

            GetAllDeclNames(concreteType, concreteNames);
            if (foreignAssembly?.Items != null)
            {
                foreignAssembly.GetAllDeclNames(concreteType, concreteNames);
            }

            if (!interfaceNames.Any() || !concreteNames.Any())
                return n2n;

            concreteNames = concreteNames.Distinct(_comparer).Cast<MetadataTokenName>().ToList();

            //get a mapping of concrete to interface
            foreach (var concreteName in concreteNames)
            {
                if (!concreteName.IsMethodName())
                    continue;

                foreach (var interfaceName in interfaceNames)
                {
                    if (!MemberNameAndArgsEqual(interfaceName, concreteName))
                        continue;

                    if (n2n.ContainsKey(interfaceName))
                    {
                        n2n[interfaceName] = concreteName;
                    }
                    else
                    {
                        n2n.Add(interfaceName, concreteName);
                    }

                    break;
                }
            }

            return n2n;
        }

        /// <summary>
        /// Sets all token name&apos;s <see cref="IsAmbiguous"/> flag for 
        /// all members attached to types defined 
        /// by the <see cref="MetadataTokenType.GetAmbiguousTypes"/>. 
        /// </summary>
        /// <param name="tokenTypes"></param>
        /// <param name="reportProgress"></param>
        /// <param name="foreignAssembly"></param>
        public void SetAllIsAmbiguousFlags(MetadataTokenType tokenTypes,
            Action<ProgressMessage> reportProgress = null, MetadataTokenName foreignAssembly = null)
        {
            if (tokenTypes?.Items == null)
                return;
            var ambiguousTypes = tokenTypes.GetAmbiguousTypes(reportProgress);
            if (ambiguousTypes == null || !ambiguousTypes.Any())
                return;

            var allAmbiguousMembers = GetAllTypesMembers(ambiguousTypes, reportProgress, foreignAssembly);

            var totalLen = allAmbiguousMembers.Length;
            for (var i = 0; i < totalLen; i++)
            {
                var ambiguousMember = allAmbiguousMembers[i];
                if (ambiguousMember == null)
                    continue;

                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = $"{ambiguousMember.Name}",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(i, totalLen),
                    Status = $"Assigning {nameof(IsAmbiguous)} flags"
                });

                AssignIsAmbiguousFlag(ambiguousMember);
            }
        }

        /// <summary>
        /// Sets any token name matching <see cref="tokenName"/>&apos;s <see cref="IsAmbiguous"/> flag as true
        /// </summary>
        /// <param name="tokenName"></param>
        /// <param name="toValue"></param>
        /// <param name="recursive">
        /// Once a method is ambiguous, all the methods it calls are considered likewise
        /// </param>
        public void AssignIsAmbiguousFlag(MetadataTokenName tokenName, bool toValue = true, bool recursive = true)
        {
            Func<MetadataTokenName, bool> selectOn = (v) => _comparer.Equals(tokenName, v);
            Func<MetadataTokenName, MetadataTokenName> assignFlag = (v) =>
            {
                if (v == null)
                    return null;
                if(recursive)
                    v.AssignIsAmbiguousFlag(toValue);
                else
                    v.IsAmbiguous = true;
                return v;
            };
            IterateTree(selectOn, assignFlag);
        }

        /// <summary>
        /// Set this and all child items at all depths to have a <see cref="IsAmbiguous"/> value of <see cref="toValue"/>
        /// </summary>
        /// <param name="toValue"></param>
        public void AssignIsAmbiguousFlag(bool toValue = true)
        {
            IsAmbiguous = toValue;
            if (Items == null || !Items.Any())
                return;
            Func<MetadataTokenName, bool> selectOn = (v) => true;
            Func<MetadataTokenName, MetadataTokenName> assignFlag = (v) =>
            {
                if (v == null)
                    return null;
                v.IsAmbiguous = toValue;
                return v;
            };

            IterateTree(selectOn, assignFlag);
        }

        /// <summary>
        /// Gets all the token names whose type matches any name in <see cref="tokenTypes"/>
        /// </summary>
        /// <param name="tokenTypes"></param>
        /// <param name="reportProgress"></param>
        /// <param name="foreignAssembly"></param>
        /// <returns></returns>
        public MetadataTokenName[] GetAllTypesMembers(MetadataTokenType[] tokenTypes,
            Action<ProgressMessage> reportProgress = null, MetadataTokenName foreignAssembly = null)
        {
            var totalLen = tokenTypes.Length;
            var allAmbiguousMembers = new List<MetadataTokenName>();
            for (var i = 0; i < totalLen; i++)
            {
                var ambiguousType = tokenTypes[i];

                if (ambiguousType == null)
                    continue;

                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = $"{ambiguousType.Name}",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(i, totalLen),
                    Status = "Getting all ambiguous members"
                });
                var ambiguousMembers = new List<MetadataTokenName>();

                GetAllDeclNames(ambiguousType, ambiguousMembers);
                foreignAssembly?.GetAllDeclNames(ambiguousType, ambiguousMembers);
                if (!ambiguousMembers.Any())
                    continue;
                allAmbiguousMembers.AddRange(ambiguousMembers);
            }

            return allAmbiguousMembers.Distinct(_comparer).Cast<MetadataTokenName>().ToArray();
        }

        /// <summary>
        /// Helper method to get a sense of a &apos;path&apos; from this 
        /// token name to some child token (at whatever depth) whose name
        /// matches the pattern.
        /// </summary>
        /// <param name="regexPattern"></param>
        /// <param name="keepMatchChildItems">
        /// Will preserve the the child Items array of the token name which matches 
        /// the pattern
        /// </param>
        /// <returns></returns>
        public MetadataTokenName GetTrace(string regexPattern, bool keepMatchChildItems = false)
        {
            var callQueue = new Queue<MetadataTokenName>();
            
            var result = GetTrace(regexPattern, callQueue, keepMatchChildItems);

            if(!result)
                return new MetadataTokenName();
            var trace = callQueue.Dequeue();

            while(callQueue.Count > 0)
            {
                var childItem = trace;
                trace = callQueue.Dequeue();
                if (trace == null)
                    continue;
                var temp = trace.GetShallowCopy();
                temp.Items = new[] {childItem};
                trace = temp;
            }

            return trace;
        }

        protected internal bool GetTrace(string regexPattern, Queue<MetadataTokenName> callQueue, bool keepMatchChildItems = false)
        {
            callQueue = callQueue ?? new Queue<MetadataTokenName>();
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(regexPattern))
                return false;
            if (Regex.IsMatch(Name, regexPattern))
            {
                callQueue.Enqueue(keepMatchChildItems ? this : GetShallowCopy());
                return true;
            }

            if (Items == null || !Items.Any())
                return false;

            foreach (var ni in Items)
            {
                if (string.IsNullOrWhiteSpace(ni?.Name))
                    continue;

                if (ni.GetTrace(regexPattern, callQueue, keepMatchChildItems))
                {
                    callQueue.Enqueue(this);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a flat, distinct, shallow root-level token names as a Set.
        /// </summary>
        /// <returns></returns>
        public MetadataTokenName SelectDistinct()
        {
            var allItems = new List<MetadataTokenName>();
            Func<MetadataTokenName, bool> selector = (v) => true;
            Func<MetadataTokenName, MetadataTokenName> addIt = (v) =>
            {
                if (v == null)
                    return null;
                allItems.Add(v.GetShallowCopy());
                return v;
            };

            IterateTree(selector, addIt);
            return new MetadataTokenName
            {
                Items = allItems.Distinct(_comparer).Cast<MetadataTokenName>().ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Selectively gets the flatten call stack for the given type-member pair as a Set
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="memberName"></param>
        /// <param name="useMemberArgs">
        /// when true will match <see cref="memberName"/> to <see cref="GetMemberName"/> and
        /// when false will match <see cref="memberName"/> to <see cref="GetMemberNameWithoutArgs"/> and
        /// </param>
        /// <returns>
        /// A single token name whose Items represent the full extent of its call stack. 
        /// </returns>
        public MetadataTokenName SelectByTypeAndMemberName(string typeName, string memberName, bool useMemberArgs)
        {
            var allItems = new List<MetadataTokenName>();
            Func<MetadataTokenName, bool> selector = null;
            if (useMemberArgs)
            {
                selector = (v) =>
                {
                    var matchedTypeName = string.Equals(v.GetTypeName(), typeName);
                    var matchedMethodName = string.Equals(v.GetMemberName(), memberName);
                    return matchedTypeName && matchedMethodName;
                };
            }
            else
            {
                selector = (v) =>
                {
                    var matchedTypeName = string.Equals(v.GetTypeName(), typeName);
                    var matchedMethodName = string.Equals(v.GetMemberNameWithoutArgs(), memberName);
                    return matchedTypeName && matchedMethodName;
                };
            }

            Func<MetadataTokenName, MetadataTokenName> addIt = (v) =>
            {
                if (v == null)
                    return null;
                allItems.Add(v.GetShallowCopy());
                return v;
            };

            IterateTree(selector, addIt);
            return new MetadataTokenName
            {
                Items = allItems.Distinct(_comparer).Cast<MetadataTokenName>().ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Helper method to get just the distinct, flatten array of names in the whole of 
        /// the tree.
        /// </summary>
        /// <returns></returns>
        public string[] SelectNames()
        {
            var t = this.SelectDistinct();
            return t.Count() <= 0 ? new string[0] : t.Items.Select(v => v.Name).ToArray();
        }

        /// <summary>
        /// Selects the all token names whose type is in <see cref="typenames"/> as a
        /// shallow copy at all depths
        /// </summary>
        /// <param name="typenames"></param>
        /// <returns></returns>
        public MetadataTokenName SelectTypeNamesThatEndWith(params string[] typenames)
        {
            return SelectByFunc(n => n.GetTypeName(), (s, f) => s.Any(f.EndsWith), true, typenames);
        }

        /// <summary>
        /// Selects the all token names whose namespace is in <see cref="namespaceNames"/> as a
        /// shallow copy at all depths
        /// </summary>
        /// <param name="namespaceNames"></param>
        /// <returns></returns>
        public MetadataTokenName SelectNamespaceNamesStartWith(params string[] namespaceNames)
        {
            return SelectByFunc(n => n.GetNamespaceName(), (s, f) => s.Any(f.StartsWith), true, namespaceNames);
        }

        /// <summary>
        /// Select all token names which match the regex pattern
        /// </summary>
        /// <param name="regexPattern"></param>
        /// <param name="getDistinct"></param>
        /// <returns></returns>
        public MetadataTokenName SelectByRegex(string regexPattern, bool getDistinct = true)
        {
            return SelectByFunc(n => n.Name, 
                               (s, f) => s.Any(v => !string.IsNullOrWhiteSpace(v) 
                                                    && !string.IsNullOrWhiteSpace(f)
                                                    && Regex.IsMatch(f, v)), getDistinct, regexPattern);
        }

        protected internal MetadataTokenName SelectByFunc(Func<MetadataTokenName, string> getNameFunc,
            Func<string[], string, bool> selector, bool getDistinct, params string[] searchNames)
        {
            var names = new List<MetadataTokenName>();
            Func<MetadataTokenName, bool> partialResolvedSelector = (v) => selector(searchNames, getNameFunc(v));
            Func<MetadataTokenName, MetadataTokenName> addIt = (v) =>
            {
                if (v == null)
                    return null;
                names.Add(v);
                return v;
            };

            IterateTree(partialResolvedSelector, addIt);

            return new MetadataTokenName
            {
                Items = getDistinct
                    ? names.Distinct(_comparer).Cast<MetadataTokenName>().ToArray()
                    : names.ToArray(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };

        }

        /// <summary>
        /// Locates any name, at whatever depth, which is ByRef and reassigns it
        /// to its full by-value counterpart which is also found at some depth herein.
        /// </summary>
        public void ReassignAllByRefs(Action<ProgressMessage> reportProgress = null)
        {
            //find all the byRefs throughout
            var byRefs = new List<MetadataTokenName>();
            if (Items == null || !Items.Any())
                return;

            GetAllByRefNames(byRefs);

            if (!byRefs.Any())
                return;

            //for each byref, find it byVal counterpart
            var byVals = new List<MetadataTokenName>();
            var totalLen = byRefs.Count;
            for (var i = 0; i < totalLen; i++)
            {
                var byRef = byRefs[i];
                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = $"{byRef.Name}",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(i, totalLen),
                    Status = "Finding the expanded counterpart of all ByRef tokens"
                });
                var byVal = GetFirstByVal(byRef);
                if (byVal == null)
                    continue;
                byVals.Add(byVal);
            }

            if (!byVals.Any())
            {
                Console.WriteLine($"There are {byRefs.Count} ByRef names but no ByVal counterparts.");
                return;
            }

            //reassign each byRef's over to byVal
            totalLen = byVals.Count;
            for (var i = 0; i < totalLen; i++)
            {
                var byVal = byVals[i];
                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = $"{byVal.Name}",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(i, totalLen),
                    Status = "Reassigning ByRef tokens"
                });
                ReassignAnyItemsByName(byVal);
            }
        }

        /// <summary>
        /// Locates the each token name in the dictionary keys, at 
        /// any depth, and reassigns it to the dictionary value.
        /// </summary>
        /// <param name="n2n"></param>
        /// <param name="reportProgress">
        /// Optional, allows caller to get feedback on progress since this takes some time to run.
        /// </param>
        public void ReassignTokens(Dictionary<MetadataTokenName, MetadataTokenName> n2n,
            Action<ProgressMessage> reportProgress = null)
        {
            if (Items == null || !Items.Any())
                return;

            if (n2n == null)
                return;

            if (!n2n.Any())
                return;

            //reassign the interface token to concrete implementation token
            var total = n2n.Count;
            var counter = 0;
            foreach (var interfaceName in n2n.Keys)
            {
                if (interfaceName == null)
                    continue;
                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = $"{interfaceName}",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(counter, total),
                    Status = "Reassigning tokens"
                });
                var concreteName = n2n[interfaceName];
                counter += 1;
                if (concreteName == null)
                    continue;
                //look it up if the items are missing
                var concreteNameLi = concreteName.Items == null || !concreteName.Items.Any()
                    ? (GetFirstByVal(concreteName) ?? concreteName)
                    : concreteName;
                ReassignAnyItemsByName(interfaceName, concreteNameLi);
            }
        }

        /// <summary>
        /// Reassigns any member in <see cref="Items"/> to the <see cref="tokenName"/>
        /// if they having matching names.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <param name="newName">
        /// Optional, what the matching name is reassigned to, defaults to the <see cref="tokenName"/> if null.
        /// </param>
        public void ReassignAnyItemsByName(MetadataTokenName tokenName, MetadataTokenName newName = null)
        {
            Func<MetadataTokenName, bool> nameOrRefEq = (v) =>
                (ReferenceEquals(v, tokenName) || _comparer.Equals(v, tokenName));
            newName = newName ?? tokenName;
            Func<MetadataTokenName, MetadataTokenName> reassignIt = (v) => newName;
            IterateTree(nameOrRefEq, reassignIt);
        }

        /// <summary>
        /// Reassigns every interface token name with its concrete counterpart when applicable.
        /// </summary>
        /// <param name="tokenTypes">
        /// A root token type whose Items are all possible types in all assemblies.
        /// </param>
        /// <param name="reportProgress">
        /// Optional, allows caller to get feedback on progress since this takes some time to run.
        /// </param>
        /// <param name="foreignAssembly">See annotations on GetImplementorDictionary</param>
        /// <remarks>
        /// The idea is that having a call-stack terminating on an interface token may
        /// not be the end since the given interface only has but one implementation.
        /// The call-stack can be further expanded by swapping the interface token with
        /// its concrete implementation.
        /// </remarks>
        public void ReassignAllInterfaceTokens(MetadataTokenType tokenTypes,
            Action<ProgressMessage> reportProgress = null, MetadataTokenName foreignAssembly = null)
        {
            var dict = GetImplementorDictionary(tokenTypes, reportProgress, foreignAssembly);
            if (foreignAssembly != null && (dict == null || !dict.Any()))
            {
                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = "secondAttempt",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(0, 100),
                    Status = "Making second attempt in reverse order"
                });
                dict = foreignAssembly.GetImplementorDictionary(tokenTypes, reportProgress, this);
            }

            ReassignTokens(dict, reportProgress);
            SetAllIsAmbiguousFlags(tokenTypes, reportProgress, foreignAssembly);
        }

        /// <summary>
        /// Removes items from <see cref="Items"/> which do not have 
        /// both &apos;get_[...]&apos; and &apos;set_[...]&apos; method for the 
        /// same property (by name)
        /// </summary>
        /// <returns></returns>
        public void RemovePropertiesWithoutBothGetAndSet()
        {
            var names = new List<MetadataTokenName>();
            if (Items == null || !Items.Any())
                return;
            foreach (var name in Items)
            {
                if (!NfReflect.IsClrMethodForProperty(name.GetMemberName(), out var propName))
                {
                    names.Add(name);
                    continue;
                }
                const string SPLT = Constants.TYPE_METHOD_NAME_SPLIT_ON;
                var typeName = name.GetTypeName();
                //need to escape these incase they are generics of some kind
                var escGetName = $@"{typeName}{SPLT}get_{propName}(".EscapeString();
                var escSetName = $@"{typeName}{SPLT}set_{propName}(".EscapeString();
                var countOfProp = Items.Where(n => !string.IsNullOrWhiteSpace(n?.Name)).Count(n =>
                    Regex.IsMatch(n.Name, escGetName) || Regex.IsMatch(n.Name, escSetName));
                if (countOfProp < 2)
                    continue;
                names.Add(name);
            }

            Items = names.ToArray();
        }

        /// <summary>
        /// Removes any names from <see cref="Items"/> which were obviously created via the .NET compiler
        /// </summary>
        public void RemoveClrGeneratedNames()
        {
            var names = new List<MetadataTokenName>();
            if (Items == null || !Items.Any())
                return;

            foreach (var name in Items)
            {
                if (NfReflect.IsClrGeneratedType(name.Name))
                    continue;
                name.RemoveClrGeneratedNames();
                names.Add(name);
            }

            Items = names.ToArray();
        }

        /// <summary>
        /// Removes all items whose <see cref="IsMethodName"/> returns false.
        /// </summary>
        public void RemoveAllNonMethodNames()
        {
            var names = new List<MetadataTokenName>();
            if (Items == null || !Items.Any())
                return;
            foreach (var name in Items)
            {
                if(name != null && name.IsMethodName())
                    names.Add(name);
            }

            Items = names.ToArray();
        }

        /// <summary>
        /// Removes all items with <see cref="IsAmbiguous"/> flag as true.
        /// </summary>
        public void RemoveAmbiguousItems()
        {
            var names = new List<MetadataTokenName>();
            if (Items == null || !Items.Any())
                return;

            foreach (var name in Items)
            {
                if (name.IsAmbiguous)
                    continue;
                name.RemoveAmbiguousItems();
                names.Add(name);
            }

            Items = names.ToArray();
        }

        /// <summary>
        /// Removes any <see cref="Items"/> which have no name.
        /// </summary>
        public void RemoveEmptyNames()
        {
            var names = new List<MetadataTokenName>();
            if (Items == null || !Items.Any())
                return;
            foreach (var name in Items)
            {
                if (string.IsNullOrWhiteSpace(name.Name))
                    continue;
                name.RemoveEmptyNames();
                names.Add(name);
            }

            Items = names.ToArray();
        }

        /// <summary>
        /// Performs a seek and replace operation recursively where the caller must decide what they are looking 
        /// for and what to do when they find it.
        /// </summary>
        /// <param name="searchFunc"> A search function </param>
        /// <param name="doSomething">
        /// A reassignment function which is executed whenever the <see cref="searchFunc"/> returns true
        /// </param>
        /// <remarks> Actual method is iterative and not recursive. </remarks>
        public void IterateTree(Func<MetadataTokenName, bool> searchFunc, Func<MetadataTokenName, MetadataTokenName> doSomething)
        {
            var callStack = new Stack<MetadataTokenName>();
            //start at top
            var ivItem = this;
            while (ivItem != null)
            {
                var nextItem = ivItem.NextItem();
                if (nextItem != null)
                {
                    //detect if some parent item is also a child item
                    if (ReferenceEquals(nextItem, ivItem) || callStack.Any(v => object.ReferenceEquals(v, nextItem)))
                    {
                        nextItem = null;
                    }
                    else
                    {
                        callStack.Push(ivItem);
                    }
                }

                ivItem = nextItem;

                if (ivItem == null)
                {
                    if (callStack.Count <= 0)
                        break;
                    ivItem = callStack.Pop();
                    continue;
                }
                //search for whatever by such-and-such
                if (!searchFunc(ivItem))
                    continue;

                //having a match then do such-and-such
                var parent = callStack.Peek();
                var ivItemIdx = (parent?.GetCurrentIdx() ?? 0) - 1;
                if (parent == null || ivItemIdx < 0 || ivItemIdx >= parent.Count())
                {
                    ivItem = doSomething(ivItem);
                    continue;
                }
                parent.Items[ivItemIdx] = doSomething(ivItem);
            } 
        }

        /// <summary>
        /// Gets the current counter index from the cumulative calls to <see cref="NextItem"/> and <see cref="PrevItem"/>
        /// </summary>
        /// <returns></returns>
        public virtual int GetCurrentIdx()
        {
            return _idx;
        }

        /// <summary>
        /// An iterative method to get the next item from <see cref="Items"/>
        /// </summary>
        /// <returns></returns>
        public virtual MetadataTokenName NextItem()
        {
            if (_idx < Count())
            {
                var v = _items[_idx];
                _idx += 1;
                return v;
            }
            _idx = 0;
            return null;
        }

        /// <summary>
        /// An iterative method to get the previous item from <see cref="Items"/>
        /// </summary>
        /// <returns></returns>
        public virtual MetadataTokenName PrevItem()
        {
            _idx -= 1;
            if (_idx >= 0)
            {
                var v = _items[_idx];
                return v;
            }
            _idx = 0;
            return null;
        }

        public override bool Equals(object obj)
        {
            var mtnObj = obj as MetadataTokenName;
            if (mtnObj == null)
                return false;

            return GetHashCode() == mtnObj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + GetNameHashCode() + OwnAsmIdx.GetHashCode();
        }

        /// <summary>
        /// Gets the hashcode of just the <see cref="Name"/>
        /// </summary>
        /// <returns></returns>
        public int GetNameHashCode()
        {
            return Name?.GetHashCode() ?? (Id.GetHashCode() + OwnAsmIdx.GetHashCode());
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Same as ToString, only printing the JSON with formatting.
        /// </summary>
        /// <returns></returns>
        public string ToFormattedString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        internal static bool MemberNameAndArgsEqual(MetadataTokenName a1, MetadataTokenName a2)
        {
            if (a1 == null || a2 == null)
                return false;
            var a1MemberName = a1.GetMemberName();
            var a2MemberName = a2.GetMemberName();
            if (!string.Equals(a1MemberName, a2MemberName))
                return false;
            var a1Args = string.Join(",", ParseArgsFromTokenName(a1.Name) ?? new[] { "" });
            var a2Args = string.Join(",", ParseArgsFromTokenName(a2.Name) ?? new[] { "" });

            var someVal = string.Equals(a1Args, a2Args);
            return someVal;
        }

        internal static bool TypeNameEquals(MetadataTokenType tokenType, MetadataTokenName tokenName)
        {
            if (tokenName == null || tokenType == null)
                return false;
            if (string.Equals(tokenType.Name, tokenName.GetTypeName()))
                return true;
            return false;
        }

        /// <summary>
        /// Gets the method parameter type-names from the token name.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public static string[] ParseArgsFromTokenName(string tokenName)
        {
            if (String.IsNullOrWhiteSpace(tokenName))
                return null;

            if (!tokenName.Contains("(") || !tokenName.Contains(")"))
                return null;

            tokenName = tokenName.Trim();
            var idxSplt = tokenName.IndexOf('(');
            if (idxSplt < 0)
                return null;

            var argNames = tokenName.Substring(tokenName.IndexOf('('));
            argNames = argNames.Replace(")", String.Empty);

            return argNames.Length <= 0
                ? null
                : argNames.Split(',').Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
        }

        /// <summary>
        /// Gets the full name (namespace plus type-name) part of the <see cref="MetadataTokenName.Name"/>
        /// </summary>
        /// <param name="tokenName"></param>
        /// <param name="owningAsmName"></param>
        /// <returns></returns>
        public static string CtorTypeNameFromTokenName(string tokenName, string owningAsmName)
        {
            if (String.IsNullOrWhiteSpace(tokenName))
                return null;

            var sep = NfSettings.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture);

            //assembly name and namespace being equal will have equal portion removed, add it back
            if (!String.IsNullOrWhiteSpace(owningAsmName) && tokenName.StartsWith(sep))
            {
                tokenName = $"{owningAsmName}{tokenName}";
            }

            var ns = NfReflect.GetNamespaceWithoutTypeName(tokenName);
            var tn = NfReflect.GetTypeNameWithoutNamespace(tokenName);

            return $"{ns}{sep}{tn}";
        }

        /// <summary>
        /// Gets just the method name of the <see cref="MetadataTokenName.Name"/>
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public static string ParseMethodNameFromTokenName(string tokenName)
        {
            if (String.IsNullOrWhiteSpace(tokenName))
                return null;

            var idxSplt = tokenName.IndexOf(Constants.TYPE_METHOD_NAME_SPLIT_ON, StringComparison.Ordinal);

            idxSplt = idxSplt + (Constants.TYPE_METHOD_NAME_SPLIT_ON).Length;
            if (idxSplt > tokenName.Length || idxSplt < 0)
                return null;

            var methodName = tokenName.Substring(idxSplt, tokenName.Length - idxSplt);

            idxSplt = methodName.IndexOf('(');
            if (idxSplt < 0)
                return methodName;

            methodName = methodName.Substring(0, methodName.IndexOf('(')).Trim();
            return methodName;
        }
    }
}