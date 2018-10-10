using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NoFuture.Shared.Core;
using NoFuture.Util.Core;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenType;

namespace NoFuture.Util.DotNetMeta.TokenName
{
    /// <summary>
    /// The resolved name of a single metadata token
    /// </summary>
    [Serializable]
    public class MetadataTokenName
    {
        [NonSerialized] private MetadataTokenName[] _items;
        [NonSerialized] private bool _isByRef;
        [NonSerialized] private readonly MetadataTokenNameComparer _comparer = new MetadataTokenNameComparer();
        [NonSerialized] private List<MetadataTokenName> _allByRefs;
        [NonSerialized] private List<MetadataTokenName> _byVals;
        [NonSerialized] private List<MetadataTokenName> _allDeclNames;
        [NonSerialized] private MetadataTokenName _firstByVal;
        [NonSerialized] private int? _fullDepthCount;
        [NonSerialized] private bool? _anyByRef;
        [NonSerialized] private Dictionary<MetadataTokenName, MetadataTokenName> _implentorDictionary;

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
        /// Indicates a short-hand version of some fuller copy of itself elsewhere 
        /// in the parent&apos;s items.
        /// </summary>
        public bool IsByRef
        {
            get => _isByRef;
            set => _isByRef = value;
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
        /// This is needed since the names are all partial (to save space on the transfered json).
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
            var tokenNamesOut = tokenNames.BindTree2Names(tokenIds);
            tokenNamesOut.Name = NfSettings.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture);
            tokenNamesOut.ApplyFullName(asmIndices);
            //turn all pointer'esque tokens into their full-expanded counterparts
            tokenNamesOut.ReassignAllByRefs(reportProgress);
            if (tokenTypes != null)
            {
                tokenNamesOut.ReassignAllInterfaceTokens(tokenTypes, reportProgress);
            }

            return tokenNamesOut;
        }

        protected internal MetadataTokenName[] SelectDistinct(Stack<MetadataTokenName> callStack = null)
        {
            callStack = callStack ?? new Stack<MetadataTokenName>();
            var innerItems = new List<MetadataTokenName> {this};
            if (Items == null || !Items.Any())
                return innerItems.ToArray();
            if (callStack.Any(v => _comparer.Equals(v, this)))
                return innerItems.ToArray();
            callStack.Push(this);
            foreach (var item in Items)
            {
                innerItems.AddRange(item.SelectDistinct(callStack));
            }
            callStack.Pop();

            return innerItems.Distinct(_comparer).Select(v => v.GetShallowCopy()).ToArray();
        }

        /// <summary>
        /// Selectively gets the flatten call stack for the given type-method pair
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <returns>
        /// A single token name whose Items represent the full extent of its call stack. 
        /// </returns>
        public MetadataTokenName SelectDistinct(string typeName, string methodName)
        {
            var df = new MetadataTokenName
            {
                Items = new MetadataTokenName[] { },
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
            if (string.IsNullOrWhiteSpace(typeName))
                return df;
            methodName = methodName ?? "";
            if (methodName.Contains("("))
                methodName = methodName.Substring(0, methodName.IndexOf('('));

            //want to avoid an interface type 
            var tokenName = SelectTheseTypeNames(typeName);
            var typeNameTree = tokenName.Items.FirstOrDefault(t => t.IsTypeName());

            if (typeNameTree == null)
                return tokenName;

            var names = new List<MetadataTokenName>();
            if (string.IsNullOrWhiteSpace(methodName) || !typeNameTree.Items.Any())
            {
                names.AddRange(typeNameTree.SelectDistinct());
                df.Items = names.Distinct(_comparer).ToArray();
                return df;
            }

            //match on overloads
            var targetMethods = typeNameTree.Items.Where(item =>
                item.Name.Contains($"{typeName}{Constants.TYPE_METHOD_NAME_SPLIT_ON}{methodName}("));
            foreach (var t in targetMethods)
            {
                names.AddRange(t.SelectDistinct());
            }

            df.Items = names.Distinct(_comparer).ToArray();
            return df;
        }

        /// <summary>
        /// Another flavor of distinct where an interface with only one implementation
        /// will have its tokens replace with its concrete counterpart.  Therefore,
        /// the call-stack is fuller since is does not terminate on interface token ids.
        /// </summary>
        /// <param name="tokenTypes"></param>
        /// <returns></returns>
        public MetadataTokenName SelectDistinct(MetadataTokenType tokenTypes)
        {
            //replace any byRef terminating nodes with their fully expander counterpart
            ReassignAllByRefs();
            ReassignAllInterfaceTokens(tokenTypes);

            return new MetadataTokenName
            {
                Items = SelectDistinct(),
                Name = NfSettings.DefaultTypeSeparator.ToString()
            };
        }

        /// <summary>
        /// Locates all the interface types in <see cref="tokenTypes"/>, finds the belonging 
        /// token names thereof, with likewise concrete counterpart and reassigns each.
        /// </summary>
        /// <param name="tokenTypes">
        /// A root token type whose Items are all possible types in all assemblies.
        /// </param>
        /// <param name="reportProgress">
        /// Optional, allows caller to get feedback on progress since this takes some time to run.
        /// </param>
        /// <remarks>
        /// The idea is that having a call-stack terminating on an interface token may
        /// not be the end since the given interface only has but one implementation.
        /// The call-stack can be further expanded by swapping the interface token with
        /// its concrete implementation.
        /// </remarks>
        public void ReassignAllInterfaceTokens(MetadataTokenType tokenTypes, Action<ProgressMessage> reportProgress = null)
        {
            //replace any interface token\names with implementation when its the only one
            var reassignInterfaces = tokenTypes.GetAllInterfacesWithSingleImplementor();
            if (reassignInterfaces == null || !reassignInterfaces.Any())
                return;
            var totalLen = reassignInterfaces.Length;
            for (var i = 0; i < totalLen; i++)
            {
                var ri = reassignInterfaces[i];
                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = $"{ri.Name}",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(i, totalLen),
                    Status = "Reassigning all interfaces"
                });
                var cri = tokenTypes.FirstInterfaceImplementor(ri);
                if (cri == null)
                    continue;

                var n2n = GetImplentorDictionary(ri, cri);
                if (n2n == null || !n2n.Any())
                    continue;

                ReassignTokens(n2n);
            }

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
                RslvAsmIdx = RslvAsmIdx
            };
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
        /// Final analysis to merge the token Ids to thier names in a hierarchy.
        /// </summary>
        /// <param name="tokenIds"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal MetadataTokenName BindTree2Names(MetadataTokenId tokenIds)
        {
            if (tokenIds?.Items == null || !tokenIds.Items.Any())
                return this;
            if (Items == null || Items.Length <= 0)
                return this;

            var nameMapping = new List<MetadataTokenName>();
            foreach (var tokenId in tokenIds.Items)
                nameMapping.Add(GetNames(tokenId));
            return new MetadataTokenName { Items = nameMapping.ToArray() };
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal MetadataTokenName GetNames(MetadataTokenId tokenId)
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

            if (tokenId.Items == null || tokenId.Items.Length <= 0)
                return nm;

            var hs = new List<MetadataTokenName>();
            foreach (var tToken in tokenId.Items)
            {
                var nmTokens = GetNames(tToken);
                if (nmTokens == null)
                    continue;
                hs.Add(nmTokens);
            }

            nm.Items = hs.ToArray();

            return nm;
        }

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
        /// Gets the cound of leaf nodes at all depths
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
            return IsMethodName() ? Name.Substring(Name.IndexOf(SPLT) + SPLT.Length) : String.Empty;
        }

        /// <summary>
        /// Gets the full namespace qual. type name
        /// </summary>
        /// <returns></returns>
        public string GetTypeName()
        {
            const string SPLT = Constants.TYPE_METHOD_NAME_SPLIT_ON;
            if (String.IsNullOrWhiteSpace(Name))
                return String.Empty;
            var idxOut = Name.IndexOf(SPLT);
            if (idxOut <= 0)
                return Name;
            return Name.Substring(0, Name.IndexOf(SPLT));
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
        /// Asserts that the part of the token name which
        /// matches the assembly name is omitted.
        /// </summary>
        /// <returns></returns>
        public bool IsPartialName()
        {
            return !String.IsNullOrWhiteSpace(Name) &&
                   Name.StartsWith(NfSettings.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Finds the first matching token name which is <see cref="IsByRef"/> false.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public MetadataTokenName FirstByVal(MetadataTokenName tokenName)
        {
            if (tokenName == null)
                return null;

            if (_comparer.Equals(tokenName, this) && !IsByRef)
                return this;
            if (Items == null || !Items.Any())
                return null;

            //use prev. cache if avail
            if (_firstByVal != null)
                return _firstByVal;

            foreach (var nm in Items)
            {
                var matched = nm.FirstByVal(tokenName);
                if (matched != null)
                {
                    //cache instance level to improv. perf.
                    _firstByVal = matched;
                    return matched;
                }
            }

            return null;
        }

        /// <summary>
        /// Reassigns any member in <see cref="Items"/> to the <see cref="tokenName"/>
        /// if they having matching names.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <param name="newName">
        /// Optional, what the matching name is reassigned to, defaults to the <see cref="tokenName"/> if null.
        /// </param>
        /// <param name="callStack">
        /// Used internally to detect infinite loops
        /// </param>
        public void ReassignAnyItemsByName(MetadataTokenName tokenName, MetadataTokenName newName = null, Stack<MetadataTokenName> callStack = null)
        {
            if (tokenName == null)
                return;
            callStack = callStack ?? new Stack<MetadataTokenName>();
            newName = newName ?? tokenName;
            if (callStack.Count > 0 && callStack.Any(v => _comparer.Equals(v, this)))
            {
                return;
            }

            if (Items == null || !Items.Any())
                return;

            callStack.Push(this);

            for (var i = 0; i < Items.Length; i++)
            {
                if (_comparer.Equals(Items[i], this))
                    continue;
                if (callStack.Count > 0 && callStack.Any(v => _comparer.Equals(v, Items[i])))
                    continue;

                if (_comparer.Equals(Items[i], tokenName))
                {
                    Items[i] = newName;
                    continue;
                }

                Items[i].ReassignAnyItemsByName(tokenName, newName, callStack);
            }

            callStack.Pop();
        }

        /// <summary>
        /// Gets all by-ref token names from all depths 
        /// </summary>
        /// <param name="tokenNames"></param>
        public void GetAllByRefNames(List<MetadataTokenName> tokenNames)
        {
            tokenNames = tokenNames ?? new List<MetadataTokenName>();
            if (IsByRef && tokenNames.All(tn => !_comparer.Equals(tn, this)))
                tokenNames.Add(this);
            if (Items == null || !Items.Any())
                return;
            //use prev. cache if avail
            if (_allByRefs != null)
            {
                tokenNames.AddRange(_allByRefs);
                return;
            }
            foreach (var nm in Items)
            {
                nm.GetAllByRefNames(tokenNames);
            }
            //cache instance level to improv. perf.
            if (tokenNames.Any())
            {
                _allByRefs = new List<MetadataTokenName>();
                _allByRefs.AddRange(tokenNames);
            }
        }

        /// <summary>
        /// Get the method names attached to the given token type
        /// </summary>
        /// <param name="tokenType"></param>
        /// <param name="tokenNames"></param>
        /// <param name="callStack"></param>
        public void GetAllDeclNames(MetadataTokenType tokenType, List<MetadataTokenName> tokenNames, Stack<MetadataTokenName> callStack = null)
        {
            callStack = callStack ?? new Stack<MetadataTokenName>();
            tokenNames = tokenNames ?? new List<MetadataTokenName>();


            if (tokenType == null)
                return;
            if (tokenNames.Any(n => _comparer.Equals(n, this)))
                return;
            if(TypeNameOrDeclEquals(tokenType, this))
                tokenNames.Add(this);
            if (Items == null || !Items.Any())
                return;
            //use prev. cache if avail
            if (_allDeclNames != null)
            {
                tokenNames.AddRange(_allDeclNames);
                return;
            }
            if (callStack.Any(v => _comparer.Equals(v, this)))
                return;
            callStack.Push(this);
            foreach (var nm in Items)
            {
                nm.GetAllDeclNames(tokenType, tokenNames, callStack);
            }

            callStack.Pop();
            //cache instance level to improv. perf.
            if (tokenNames.Any())
            {
                _allDeclNames = new List<MetadataTokenName>();
                _allDeclNames.AddRange(tokenNames);
            }
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
            return Name?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
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

            return new MetadataTokenName { Items = listOut.ToArray() };
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
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            var d = rightList.Items.Distinct(_comparer).ToDictionary(hashCode);
            var e = leftList.Items.Distinct(_comparer).ToDictionary(hashCode);

            foreach (var key in e.Keys.Where(k => !d.ContainsKey(k)))
                d.Add(key, e[key]);

            return new MetadataTokenName
            {
                Items = d.Values.ToArray(),
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
        /// Selects the all token names whose type is in <see cref="typenames"/> as a
        /// shallow copy at all depths
        /// </summary>
        /// <param name="typenames"></param>
        /// <returns></returns>
        public MetadataTokenName SelectTheseTypeNames(params string[] typenames)
        {
            return SelectByFunc(n => n.GetTypeName(), (s, f) => s.Any(f.EndsWith), typenames);
        }

        /// <summary>
        /// Selects the all token names whose namespace is in <see cref="namespaceNames"/> as a
        /// shallow copy at all depths
        /// </summary>
        /// <param name="namespaceNames"></param>
        /// <returns></returns>
        public MetadataTokenName SelectTheseNamespaceNames(params string[] namespaceNames)
        {
            return SelectByFunc(n => n.GetNamespaceName(), (s, f) => s.Any(f.StartsWith), namespaceNames);
        }

        /// <summary>
        /// Selects the all token names whose type is anything but one of the <see cref="typenames"/> as a
        /// shallow copy at all depths.
        /// </summary>
        /// <param name="typenames"></param>
        /// <returns></returns>
        public MetadataTokenName SelectNotTheseTypeNames(params string[] typenames)
        {
            return SelectByFunc(n => n.GetTypeName(), (s, f) => s.All(v => !f.EndsWith(v)), typenames);
        }

        /// <summary>
        /// Selects the all token names whose namespace is anything but one of the <see cref="namespaceNames"/> as a
        /// shallow copy at all depths.
        /// </summary>
        /// <param name="namespaceNames"></param>
        /// <returns></returns>
        public MetadataTokenName SelectNotTheseNamespaceNames(params string[] namespaceNames)
        {
            return SelectByFunc(n => n.GetNamespaceName(), (s, f) => s.All(v => !f.StartsWith(v)), namespaceNames);
        }

        protected internal MetadataTokenName SelectByFunc(Func<MetadataTokenName, string> getNameFunc,
            Func<string[], string, bool> selector, params string[] searchNames)
        {
            var names = new List<MetadataTokenName>();
            if (selector(searchNames, getNameFunc(this)))
                names.Add(GetShallowCopy());

            if (Items == null || !Items.Any())
                return new MetadataTokenName
                {
                    Items = names.ToArray(),
                    Name = NfSettings.DefaultTypeSeparator.ToString()
                };
            foreach (var name in Items)
            {
                var nameMatch = name.SelectByFunc(getNameFunc, selector, searchNames);
                if (nameMatch.Items.Any())
                    names.AddRange(nameMatch.Items.Select(v => v.GetShallowCopy()));
            }

            return new MetadataTokenName
            {
                Items = names.Distinct(_comparer).ToArray(),
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

            foreach (var nm in Items)
                nm.GetAllByRefNames(byRefs);

            if (!byRefs.Any())
                return;

            //for each byref, find it byVal counterpart
            var byVals = new List<MetadataTokenName>();
            if (_byVals != null)
            {
                byVals.AddRange(_byVals);
            }
            else
            {
                foreach (var byRef in byRefs)
                {
                    MetadataTokenName byVal = null;
                    foreach (var nm in Items)
                    {
                        byVal = nm.FirstByVal(byRef);
                        if (byVal == null)
                            continue;
                        byVals.Add(byVal);
                        break;
                    }
                }
                _byVals = byVals;
            }

            if (!byVals.Any())
            {
                Console.WriteLine($"There are {byRefs.Count} ByRef names but no ByVal counterparts.");
                return;
            }

            //reassign each byRef's over to byVal
            var totalLen = byVals.Count;
            for (var i = 0; i < totalLen; i++)
            {
                var byVal = byVals[i];
                reportProgress?.Invoke(new ProgressMessage
                {
                    Activity = $"{byVal.Name}",
                    ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    ProgressCounter = Etc.CalcProgressCounter(i, totalLen),
                    Status = "Reassigning all by ref names"
                });
                foreach (var nm in Items)
                {
                    nm.ReassignAnyItemsByName(byVal);
                }

            }
        }

        /// <summary>
        /// Gets a dictionary of token names whose keys are the interface&apos;s token while
        /// the values are the concret implementation thereof.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="concreteType"></param>
        /// <returns></returns>
        public Dictionary<MetadataTokenName, MetadataTokenName> GetImplentorDictionary(MetadataTokenType interfaceType,
            MetadataTokenType concreteType)
        {
            var n2n = new Dictionary<MetadataTokenName, MetadataTokenName>();
            var interfaceNames = new List<MetadataTokenName>();
            var concreteNames = new List<MetadataTokenName>();
            if (Items == null || !Items.Any())
                return n2n;

            if (_implentorDictionary != null)
                return _implentorDictionary;
            foreach (var nm in Items)
            {
                nm.GetAllDeclNames(interfaceType, interfaceNames);
                nm.GetAllDeclNames(concreteType, concreteNames);
            }

            if (!interfaceNames.Any() || !concreteNames.Any())
                return n2n;

            //get a mapping of concrete to interface
            foreach (var concreteName in concreteNames)
            {
                if (!concreteName.IsMethodName())
                    continue;

                if (n2n.Keys.Any(v => string.Equals(v.Name, concreteName.Name)))
                    continue;

                foreach (var interfaceName in interfaceNames)
                {
                    if (MemberNameAndArgsEqual(interfaceName, concreteName))
                    {
                        n2n.Add(concreteName, interfaceName);
                        break;
                    }
                }
            }

            _implentorDictionary = n2n;
            return _implentorDictionary;
        }

        /// <summary>
        /// Locates the each token name in the dictionary keys and reassigns it, at 
        /// any depth, to the dictionary value.
        /// </summary>
        /// <param name="n2n"></param>
        public void ReassignTokens(Dictionary<MetadataTokenName, MetadataTokenName> n2n)
        {
            if (Items == null || !Items.Any())
                return;

            if (n2n == null)
                return;

            if (!n2n.Any())
                return;

            //reassign the interface token to concrete implementation token
            foreach (var concreteName in n2n.Keys)
            {
                var interfaceName = n2n[concreteName];
                if (interfaceName == null)
                    continue;
                foreach (var nm in Items)
                {
                    nm.ReassignAnyItemsByName(interfaceName, concreteName);
                }
            }
        }

        internal static bool MemberNameAndArgsEqual(MetadataTokenName a1, MetadataTokenName a2)
        {
            if (a1 == null || a2 == null)
                return false;
            var a1MemberName = a1.GetMemberName();
            var a2MemberName = a2.GetMemberName();
            if (!string.Equals(a1MemberName, a2MemberName))
                return false;
            var a1Args = string.Join(",",ParseArgsFromTokenName(a1.Name) ?? new[] {""});
            var a2Args = string.Join(",", ParseArgsFromTokenName(a2.Name) ?? new[] {""});

            var someVal =  string.Equals(a1Args, a2Args);
            return someVal;
        }

        internal static bool TypeNameOrDeclEquals(MetadataTokenType tokenType, MetadataTokenName tokenName)
        {
            if (tokenName == null || tokenType == null)
                return false;
            if (string.Equals(tokenType.Name, tokenName.GetTypeName()))
                return true;
            if (tokenName.DeclTypeId == tokenType.Id)
                return true;
            return false;
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

                var countOfProp = Items.Count(n => n.GetMemberName().Contains($"get_{propName}(")
                                                   || n.GetMemberName().Contains($"set_{propName}("));
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

        internal void RemoveClrAndEmptyNames()
        {
            RemoveClrGeneratedNames();
            RemoveEmptyNames();
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