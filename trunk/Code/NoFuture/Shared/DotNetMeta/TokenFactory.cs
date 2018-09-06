using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Shared
{
    /// <summary>
    /// Factory methods to reduce the amount of code needed in PowerShell
    /// </summary>
    public static class TokenFactory
    {
        public static Tuple<AsmIndicies, TokenNames> CoupleIdx(this AsmIndicies indicies, TokenNames names)
        {
            return new Tuple<AsmIndicies, TokenNames>(indicies,names);
        }

        public static Tuple<AsmIndicies, TokenNames> CoupleIdx(this AsmIndicies indicies, MetadataTokenName[] mtNames)
        {
            return new Tuple<AsmIndicies, TokenNames>(indicies, new TokenNames{Names = mtNames});
        }

        /// <summary>
        /// For the given assembly, gets the named being made from itself to itself.
        /// </summary>
        /// <param name="asmIdx"></param>
        /// <param name="tokenMap"></param>
        /// <param name="tokenNames"></param>
        /// <returns></returns>
        public static TokenNames InternallyCalled(AsmIndicies asmIdx, TokenIds tokenMap, TokenNames tokenNames)
        {
            if (tokenNames?.Names == null || tokenNames.Names.Length <= 0)
                return new TokenNames {Names = new List<MetadataTokenName>().ToArray()};
            if (tokenMap?.Tokens == null || tokenMap.Tokens.Length <= 0)
                return new TokenNames {Names = new List<MetadataTokenName>().ToArray()};
            var typeTokens = tokenMap.Tokens;

            if (typeTokens.All(x => x.Items == null || x.Items.Length <= 0))
                return new TokenNames {Names = new List<MetadataTokenName>().ToArray()};


            var memberTokens = typeTokens.SelectMany(x => x.Items).ToList();

            //these are all the token Ids one could call on this assembly
            var memberTokenIds = memberTokens.Select(x => x.Id).ToList();

            //get all the tokenIds, defined in this assembly and being called from this assembly
            var callsInMembers =
                memberTokens.SelectMany(x => x.Items)
                    .Select(x => x.Id)
                    .Where(x => memberTokenIds.Any(y => x == y))
                    .ToList();


            tokenNames.ApplyFullName(asmIdx);

            var namesArray = tokenNames.Names.Where(x => callsInMembers.Any(y => y == x.Id)).ToArray();
            return new TokenNames {Names = namesArray};
        }

        /// <summary>
        /// Set operation for comparision of two <see cref="TokenNames"/> with
        /// thier respective <see cref="NoFuture.Shared.AsmIndicies"/>
        /// </summary>
        /// <param name="leftList"></param>
        /// <param name="rightList"></param>
        /// <param name="rightListTopLvlOnly">
        /// Set to true to have <see cref="rightListTopLvlOnly"/> 
        /// only those in <see cref="rightList"/> whose <see cref="NoFuture.Shared.MetadataTokenName.OwnAsmIdx"/> is '0'.
        /// </param>
        /// <returns></returns>
        public static TokenNames RightSetDiff(Tuple<AsmIndicies, TokenNames> leftList,
            Tuple<AsmIndicies, TokenNames> rightList, bool rightListTopLvlOnly = false)
        {
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (leftList == null || rightList == null)
                return new TokenNames { Names = new List<MetadataTokenName>().ToArray() };
            if (rightList.Item2?.Names == null || rightList.Item2.Names.Length <= 0)
                return new TokenNames { Names = new List<MetadataTokenName>().ToArray() };
            if (leftList.Item2?.Names == null || leftList.Item2.Names.Length <= 0)
                return new TokenNames { Names = rightList.Item2.Names.ToArray() }; 

            //expand to full names
            leftList.Item2.ApplyFullName(leftList.Item1);
            rightList.Item2.ApplyFullName(rightList.Item1);

            var setOp = rightList.Item2.Names.Select(hashCode).Except(leftList.Item2.Names.Select(hashCode));

            var listOut = new List<MetadataTokenName>();
            foreach (var j in setOp)
            {
                var k = rightList.Item2.Names.FirstOrDefault(x => hashCode(x) == j);
                if (k == null || (rightListTopLvlOnly && k.OwnAsmIdx != 0) || !k.IsMethodName())
                    continue;
                listOut.Add(k);
            }

            return new TokenNames {Names = listOut.ToArray()};
        }

        /// <summary>
        /// Set operation for the joining of two <see cref="TokenNames"/> with
        /// thier respective <see cref="NoFuture.Shared.AsmIndicies"/>
        /// </summary>
        /// <param name="leftList"></param>
        /// <param name="rightList"></param>
        /// <returns></returns>
        public static TokenNames Union(Tuple<AsmIndicies, TokenNames> leftList,
            Tuple<AsmIndicies, TokenNames> rightList)
        {
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (leftList == null || rightList == null)
                return new TokenNames { Names = new List<MetadataTokenName>().ToArray() };
            if (rightList.Item2 == null)
                return new TokenNames {Names = leftList.Item2.Names};
            if (leftList.Item2 == null)
                return new TokenNames { Names = rightList.Item2.Names};

            //expand to full names
            leftList.Item2.ApplyFullName(leftList.Item1);
            rightList.Item2.ApplyFullName(rightList.Item1);

            var d = rightList.Item2.Names.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);
            var e = leftList.Item2.Names.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);

            foreach (var key in e.Keys.Where(k => !d.ContainsKey(k)))
                d.Add(key, e[key]);

            return new TokenNames { Names = d.Values.Where(x => x.IsMethodName()).ToArray()};
        }
    }
}