using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace NoFuture.Tokens
{
    /// <summary>
    /// Like the assembly summary says, this is slightly better than regex since it
    /// works with a sense for 'blocks' or 'enclosures' but far less than something
    /// as advanced as ANTLR.
    /// Originally coded in ps1 but switched to here for performance sake.
    /// </summary>
    public class XDocFrame
    {
        #region PROPS AND FLDS
        private readonly Tuple<string, string> _wordTokens;
        private readonly Tuple<char, char> _charTokens;
        private readonly bool _usingWordTokens = false;

        private int _minTokenSpan = 1;

        /// <summary>
        /// Token pairs are only applied to the XDocument frame when the length of the string between them is this size or greater.
        /// </summary>
        public int MinTokenSpan
        {
            get { return _minTokenSpan; }
            set { _minTokenSpan = value; }
        }

        #endregion

        #region CTORS

        public XDocFrame() { }

        public XDocFrame(string startWordToken, string endWordToken)
        {
            _wordTokens = new Tuple<string, string>(startWordToken, endWordToken);
            _usingWordTokens = true;
        }

        public XDocFrame(char startCharToken, char endCharToken)
        {
            _charTokens = new Tuple<char, char>(startCharToken, endCharToken);
        }

        #endregion

        #region IMPRIMIS CODEX

        /// <summary>
        /// Method deals in just the <see cref="NoFuture.Tokens.Token"/> types
        /// and has no dependency on the Xml.Linq namespace.
        /// </summary>
        /// <param name="stringArg"></param>
        /// <returns></returns>
        public List<Token> FindEnclosingTokens(string stringArg)
        {
            return _usingWordTokens
                ? FindEnclosingTokens(stringArg, _wordTokens.Item1, _wordTokens.Item2)
                : FindEnclosingTokens(stringArg, _charTokens.Item1, _charTokens.Item2);
        }

        /// <summary>
        /// Creates and XDocument in which child elements map to token-pair matches.
        /// </summary>
        /// <param name="inputString">The string upon which the XDocument frame will be derived.</param>
        /// <returns></returns>
        /// <remarks>
        /// Given a string arguement (or a path to a text file) the function will 
        /// produce an XDocument where the nesting of child nodes represents 
        /// the internal context of a matching pair of tokens.  Therefore the output
        /// will contian nodes and those nodes may contain furthere child nodes and
        /// so on down to an internal size that is at least the defined minimum token
        /// span.
        /// 
        /// The string input will have all its concurrent occurances of spaces and 
        /// tabs reduced to a single space.
        /// 
        /// The result is simply a frame and does not contain any of the original string.
        /// </remarks>
        public XElement GetXDocFrame(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new Exceptions.ItsDeadJim("The input string arg is empty.");
            }

            inputString = Util.Etc.DistillString(inputString);
            inputString = Util.Etc.DistillTabs(inputString);

            var tokens = GetTokens(inputString);
            var gaps = GetGaps(tokens);
            var xDoc = FrameTokensXDoc(tokens);
            xDoc = FrameGapsXDoc(xDoc, gaps);

            return xDoc;
        }

        /// <summary>
        /// Applies the results of Get-XDocFrame to the original content.
        /// </summary>
        /// <param name="xDoc"></param>
        /// <param name="inputString"></param>
        /// <remarks>
        /// Returns the XDocument with the inter-string spans applied as text data 
        /// of each of the XDocument frame nodes.
        /// 
        /// The input is again distilled of all concurrent occurances of tab and space
        /// characters.
        /// </remarks>
        public void ApplyXDocFrame(XElement xDoc, string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new Exceptions.ItsDeadJim("The input string arg is empty.");
            }

            if (xDoc == null)
            {
                throw new Exceptions.ItsDeadJim("a XElement must be supplied, call GetXDocFrame prior to calling this member");
            }

            //crit - the indices must match the original so process like frame f(x) did
            inputString = Util.Etc.DistillString(inputString);
            inputString = Util.Etc.DistillTabs(inputString);

            XDocIter(xDoc,inputString);

        }
        #endregion

        #region XML LINQ XELEMENT FUNCTIONS

        protected internal List<Token> GetTokens(string stringArg)
        {
            //search for token-pair based on defined token types
            var allTokens = FindEnclosingTokens(stringArg);

            //join token pairs up into complex collection thereof
            allTokens = AppendHeadToken(allTokens, stringArg);
            allTokens = RemoveTokensByMinLength(allTokens, _minTokenSpan);
            allTokens = InterlaceTokens(allTokens);
            allTokens = SetTabCount(allTokens);

            return allTokens;
        }

        protected internal XElement FrameTokensXDoc(List<Token> allTokens)
        {
            //nsure this implicit presumption is always true
            allTokens = allTokens.OrderBy(t => t.Start).ToList();

            //sweep each token into an xml element
            var xTokens = allTokens.Select(t => t.ToXElement()).ToList();

            //get an independent ref to the root
            var xmlRoot = xTokens[0];

            //ok to use index since *child to* will corrospond to the index value of results
            for (var i = 0; i < allTokens.Count; i++)
            {
                //every token gets a full sweep through the collection
                for (var j = 0; j < allTokens.Count; j++)
                {
                    //add child node to parent
                    if(allTokens[j].ChildTo == i && !allTokens[i].Equals(allTokens[j]))
                        xTokens[i].Add(xTokens[j]);
                }
            }

            return xmlRoot;
        }

        protected internal XElement FindXElementById(XElement xDoc, string qryId)
        {
            XElement localNode = null;
            //difficult to follow but inherint in nature of linq's xdoc
            if (xDoc.Attribute(XName.Get("ID")) != null && xDoc.Attribute(XName.Get("ID")).Value == qryId)
                localNode = xDoc;

            if (xDoc.HasElements && localNode == null)
                localNode = FindXElementById((XElement)xDoc.FirstNode, qryId);

            if (xDoc.NextNode != null && localNode == null)
                localNode = FindXElementById((XElement) xDoc.NextNode, qryId);

            return localNode;
        }

        protected internal XElement FrameGapsXDoc(XElement xDoc, List<Token> allGaps)
        {
            //likewise, sweep these into a collection
            var xGaps = allGaps.Select(g => g.ToXElement()).ToList();

            //must intergrate these based on matching XDoc's Id to XGaps ChildOf
            foreach (var xg in xGaps)
            {
                var parentXElement = FindXElementById(xDoc, xg.Attribute(XName.Get("ChildOf")).Value);
                if (parentXElement == null) continue;

                //copy the Tab attribute value to the gap node, pass over if not present, not crit
                if (parentXElement.Attribute(XName.Get("Tab")) != null)
                {
                    var parentTabCount = parentXElement.Attribute(XName.Get("Tab")).Value;
                    var xTab = new XAttribute("Tab", parentTabCount);
                    xg.Add(xTab);
                }

                var gapStart = xg.Attribute(XName.Get("Start")).Value;

                //add the gap to the parent with respect to char order of original string
                if (parentXElement.Attribute(XName.Get("Start")).Value == gapStart)
                {
                    parentXElement.AddFirst(xg);
                }
                else
                {
                    //get a flag to determine what happened in the loop below
                    var wasAdded = false;

                    //parent's child nodes - find the correct spot to add the gap
                    foreach (var tp in parentXElement.Descendants(XName.Get("TokenPair")))
                    {
                        if (!wasAdded &&
                            tp.Attribute(XName.Get("End")).Value == (Convert.ToInt32(gapStart) - 1).ToString())
                        {
                            tp.AddAfterSelf(xg);
                            wasAdded = true;
                        }
                    }

                    //add it to parent's bottom if still remaining
                    if (!wasAdded)
                        parentXElement.Add(xg);
                }
            }
            return xDoc;
        }
        
        #endregion

        #region TOKEN INDEX FUNCTIONS

        protected internal void XDocIter(XElement xDoc, string stringArg)
        {
            ApplyStringValue(xDoc, stringArg);

            if(xDoc.HasElements)
                XDocIter((XElement)xDoc.FirstNode, stringArg);
            if(xDoc.NextNode != null)
                XDocIter((XElement)xDoc.NextNode, stringArg);
        }

        protected internal void ApplyStringValue(XElement xDoc, string stringArg)
        {
            if (xDoc.HasElements)
                return;
            var startAt = 0;
            var spanFor = 0;
            if (xDoc.Attribute(XName.Get("Start")) != null)
                startAt = Convert.ToInt32(xDoc.Attribute(XName.Get("Start")).Value);
            if (xDoc.Attribute(XName.Get("Span")) != null)
                spanFor = Convert.ToInt32(xDoc.Attribute(XName.Get("Span")).Value);
            if (startAt >= 0 && spanFor > 0 && startAt + spanFor <= stringArg.Length)
                xDoc.Value = stringArg.Substring(startAt, spanFor);
        }

        protected internal Tuple<List<int>, List<int>> FindAllCharTokens(string stringArg, char startToken,
            char endToken)
        {
            var arr = stringArg.ToCharArray();
            var sTokens = new List<int>();
            var eTokens = new List<int>();

            //get the start & end token positions
            for (var i = 0; i < arr.Length; i++)
            {
                //sweep through once...let another function delimit pairs
                if (arr[i] == startToken)
                    sTokens.Add(i);
                else if (arr[i] == endToken)
                    eTokens.Add(i);
            }

            return new Tuple<List<int>, List<int>>(sTokens,eTokens);

        }

        protected internal List<Token> FindEnclosingTokens(string stringArg, char startToken, char endToken)
        {
            var charTokenLists = FindAllCharTokens(stringArg, startToken, endToken);
            var sTokens = charTokenLists.Item1;
            var eTokens = charTokenLists.Item2;

            //deal with open/close tokens being the same char
            if (sTokens.Count <= 2 || sTokens.Count%2 != 0 || startToken != endToken)
                return CorrospondTokens(sTokens, eTokens, null);

            var simpleResults = new List<Token>();
            for (var i = 0; i < sTokens.Count; i++)
            {
                //at zero and each even number split them to a pair
                if(i % 2 == 0)
                    simpleResults.Add(new TokenPair(sTokens[i], sTokens[i+1]));
            }
            return simpleResults;
        }

        protected internal List<Token> FindEnclosingTokens(string stringArg, string startToken, string endToken)
        {
            var arr = stringArg.ToCharArray();
            var startMeasureOutTo = startToken.Length;
            var endMeasureOutTo = endToken.Length;
            var sTokens = new List<int>();
            var eTokens = new List<int>();

            //get the start & end token positions
            for (var i = 0; i < arr.Length; i++)
            {
                //determine if look-ahead is pertinent for start token at this position
                if (arr[i] == startToken.ToCharArray()[0])
                {
                    //check for look-ahead being out-of-bounds
                    if (i + startMeasureOutTo <= arr.Length)
                    {
                        //look-ahead at whole word
                        var potentStoken = new string(arr.Skip(i).Take(startMeasureOutTo).ToArray());

                        if (String.Equals(potentStoken, startToken))
                        {
                            //add this position to start-token-stack
                            sTokens.Add(i);
                        }
                    }
                }

                //determine look-ahead is pertinent for end token at this position
                if (arr[i] != endToken.ToCharArray()[0])
                    continue;

                //check for look-ahead being out-of-bounds
                if (i + endMeasureOutTo > arr.Length)
                    continue;

                //look-ahead, get whole word
                var potentEToken = new string(arr.Skip(i).Take(endMeasureOutTo).ToArray());
                if (String.Equals(potentEToken, endToken))
                {
                    //add the position of last char in word token as end
                    eTokens.Add(i+endMeasureOutTo-1);
                }
            }//end 'for each' char in string

            //pass stack in to match pairs same as char-tokens
            return CorrospondTokens(sTokens, eTokens, null);
        }

        protected internal List<Token> AppendHeadToken(List<Token> allTokens, string stringArg)
        {
            //add a document level token to the all-tokens stack - needed for comparasion
            allTokens.Add(new TokenPair(0, stringArg.Length));
            allTokens = allTokens.OrderBy(t => t.Start).ToList();
            return allTokens;
        }

        protected internal Tuple<int, int> FindTokenMatch(int ot, List<int> arr0, List<int> arr1)
        {
            //at the bottom of token-stack - must match 
            if(arr0.Count <= 1 && arr1.Count <= 1)
                return new Tuple<int, int>(ot,arr1[0]);

            //assuming $ot equals $arr[0] at the top of the call-stack
            if(arr1[0] > arr0[0] && arr1[0] < arr0[1])
                return new Tuple<int, int>(ot, arr1[0]);

            //recurse self, eliminating stack thereof
            arr0 = arr0.Skip(1).Take(arr0.Count - 1).ToList();
            arr1 = arr1.Skip(1).Take(arr1.Count - 1).ToList();
            return FindTokenMatch(ot, arr0, arr1);
        }

        protected internal List<Token> CorrospondTokens(List<int> arr0, List<int> arr1, List<Token> results)
        {
            //instantiate results variable at the top of the call-stack
            if (results == null)
                results = new List<Token>();

            //check for exhausted token-stacks
            if (arr0 == null || arr1 == null || arr0.Count == 0 || arr1.Count == 0)
                return results;

            //end of token-stacks, add last remaining as an ordered pair
            if (arr0.Count <= 1 && arr1.Count <= 1)
            {
                results.Add(new TokenPair(arr0[0], arr1[0]));
                return results;
            }
            //normal op until bottom of the token-stacks

            //add this call-stack's pair to the results
            var rc = FindTokenMatch(arr0[0], arr0, arr1);
            results.Add(new TokenPair(rc.Item1, rc.Item2));

            //splice the token-stacks, removing *this* call-stack's results 
            arr0 = arr0.Skip(1).Take(arr0.Count - 1).ToList();
            var i = 0;
            var j = 0;

            foreach (var c in arr1)
            {
                if (c == rc.Item2)
                    i = j;
                j += 1;
            }
            var lastIndex = arr1.Count;
            if (i >= arr1.Count - 1)
                arr1 = arr1.Take(lastIndex - 1).ToList();
            else if (i <= 0)
                arr1 = arr1.Skip(1).Take(lastIndex).ToList();
            else
            {
                var left = arr1.Take(i).ToList();
                var right = arr1.Skip(i+1).Take(lastIndex).ToList();
                left.AddRange(right);
                arr1 = left;
            }

            //arr's now redefined less this call-stacks results, continue thereof
            return CorrospondTokens(arr0, arr1, results);
        }

        protected internal List<Token> RemoveTokensByMinLength(List<Token> allTokens, int minimumLength)
        {
            return allTokens.Where(t => t.Span >= minimumLength).ToList();
        }

        protected internal List<Token> InterlaceTokens(List<Token> allTokens)
        {
            //brand each token pair with index value thereof
            for (var i = 0; i < allTokens.Count; i++)
            {
                allTokens[i].Register = i;
            }

            //Truth condition is End(i) -lt End(i-j) starting 'j' as 'i' and having 'j' decremented until its true 
            for (var i = 0; i < allTokens.Count; i++)
            {
                for (var j = i; j >= 0; j--)
                {
                    if (allTokens[i].End < allTokens[j].End && allTokens[i].ChildTo == 0)
                    {
                        //truth condition met
                        allTokens[i].ChildTo = allTokens[j].Register;
                    }
                }
            }

            return allTokens;
        }

        protected internal List<Token> SetTabCount(List<Token> allTokens)
        {
            //nothing special - simply refactored to its own ideninty
            for (var i = 0; i < allTokens.Count; i++)
            {
                if (allTokens[i].ChildTo <= 0)
                    allTokens[i].Tab = 0;
                else
                {
                    allTokens[i].Tab = allTokens[(allTokens[i].ChildTo)].Tab + 1;
                }
            }

            return allTokens;
        }

        protected internal List<Token> GetGaps(List<Token> allTokens)
        {
            //go through all tokens finding gaps between current index and next
            var gapTokens = new List<Token>();
            for (var i = 0; i < allTokens.Count; i++)
            {
                var gapStart = 0;
                var gapEnd = 0;
                var gapChild = 0;
                //limit gaps to only those with a next
                if (i + 1 < allTokens.Count)
                {
                    //locals for readability
                    var thisEnd = allTokens[i].End;
                    var nextStart = allTokens[i + 1].Start;
                    if (thisEnd < nextStart && nextStart - thisEnd > 1)
                    {
                        //the gap is between current index's end and the next's start
                        gapStart = allTokens[i].End + 1;
                        gapEnd = allTokens[i + 1].Start - 1;

                        //this gap is a sibling node of the current index
                        gapChild = allTokens[i].ChildTo;
                        gapTokens.Add(new Gap(gapStart,gapEnd,gapChild));
                    }
                    else if (thisEnd > nextStart && thisEnd - nextStart > 1)
                    {
                        //this gap comes after the current index's start but before next's start
                        gapStart = allTokens[i].Start; //consume parent's first char as well

                        //special condition for gap that is capture of top of string
                        gapEnd = allTokens[i + 1].Start;
                        gapChild = allTokens[i].Register;

                        //this gap is a child node of the current index
                        gapTokens.Add(new Gap(gapStart, gapEnd, gapChild));
                    }
                }
                else if (i == allTokens.Count - 1)
                {
                    //check if the last token/gap captured end-of-the-string
                    var finalTokenEnd = allTokens[(allTokens.Count - 1)].End;
                    var finalGapEnd = gapTokens.Count > 0 ? gapTokens[(gapTokens.Count - 1)].End : 0;

                    //determine highest among two
                    var finalEnd = finalTokenEnd > finalGapEnd ? finalTokenEnd : finalGapEnd;

                    if (allTokens[0].End - finalEnd > 0)
                    {
                        //another gap needs to be added to capture remainder of string
                        gapStart = finalEnd;
                        gapEnd = allTokens[0].End - 1;
                        gapChild = 0;
                        gapTokens.Add(new Gap(gapStart, gapEnd, gapChild));
                    }

                }
            }

            return gapTokens.OrderBy(g => g.Start).ToList();
        }

        #endregion
    }

    public abstract class Token
    {
        protected int _start;
        protected int _end;
        protected int _span;
        protected Token(int start, int end)
        {
            _start = start;
            _end = end;
            _span = end - start;
        }
        public int Start { get { return _start; } }
        public int End { get { return _end; } }
        public int Span { get { return _span; } }
        public int Register { get; set; }
        public int Tab { get; set; }
        public int ChildTo { get; set; }
        public abstract XElement ToXElement();
    }

    public class TokenPair : Token
    {
        public TokenPair(int start, int end) : base(start, end)
        {
            _end = end + 1;//consumes end token
            _span = end - start + 1;
        }


        public override XElement ToXElement()
        {
            var xStart = new XAttribute("Start", _start);
            var xEnd = new XAttribute("End", _end);
            var xSpan = new XAttribute("Span", _span);
            var xId = new XAttribute("ID", Register);
            var xTab = new XAttribute("Tab", Tab);
            return new XElement("TokenPair",xId,xStart,xEnd,xSpan,xTab);

        }
    }

    public class Gap : Token
    {
        public Gap(int start, int end, int childTo) : base(start, end)
        {
            ChildTo = childTo;
        }

        public override XElement ToXElement()
        {
            var xStart = new XAttribute("Start", _start);
            var xEnd = new XAttribute("End", _end);
            var xSpan = new XAttribute("Span", _span);
            var xChildOf = new XAttribute("ChildOf", ChildTo);
            return new XElement("Gap",xStart,xEnd,xSpan,xChildOf);
        }
    }
}
