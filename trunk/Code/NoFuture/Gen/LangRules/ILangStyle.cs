using System;
using System.Collections.Generic;
using System.Text;

namespace NoFuture.Gen.LangRules
{
    public interface ILangStyle
    {
        string GetEnclosureOpenToken(CgMember cgMem);
        string GetEnclosureCloseToken(CgMember cgMem);

        /// <summary>
        /// The <see cref="ILangStyle"/> keyword used when declaring a compile-time constant.
        /// </summary>
        string DeclareConstant { get; }

        /// <summary>
        /// A dictionary of .NET primitive types to thier lang-specific alias.
        /// (e.g. 'System.Int32' -to- 'int' in C#)
        /// </summary>
        Dictionary<string, string> ValueTypeToLangAlias { get; }

        /// <summary>
        /// Renders this instance as a lang style method signature declaration.
        /// </summary>
        /// <param name="cgMem"></param>
        /// <param name="includesAccessModifier"></param>
        /// <returns></returns>
        string ToDecl(CgMember cgMem, bool includesAccessModifier = false);

        /// <summary>
        /// Renders the <see cref="cgType"/> as an instance level declaration having a
        /// no-arg ctor.  This is intended as a reference to a new type being refactored
        /// out from another.
        /// </summary>
        /// <param name="cgType"></param>
        /// <param name="variableName"></param>
        /// <param name="accessMod"></param>
        /// <returns></returns>
        string ToDecl(CgType cgType, string variableName, CgAccessModifier accessMod);

        /// <summary>
        /// Renders this instance as a <see cref="ILangStyle"/> method invocation statement.
        /// </summary>
        /// <param name="cgNamespace"></param>
        /// <param name="cgInvokeOnName"></param>
        /// <param name="cgMem"></param>
        /// <returns></returns>
        string ToStmt(CgMember cgMem, string cgNamespace, string cgInvokeOnName);

        /// <summary>
        /// Renders the <see cref="cgType"/> as a full class.
        /// </summary>
        /// <param name="cgType"></param>
        /// <param name="cgClsAccess"></param>
        /// <param name="typeModifier"></param>
        /// <returns></returns>
        string ToClass(CgType cgType, CgAccessModifier cgClsAccess, CgClassModifier typeModifier);

        /// <summary>
        /// Transforms the <see cref="cgMem"/> into a useable parameter by another <see cref="CgMember"/>
        /// </summary>
        /// <param name="cgMem"></param>
        /// <param name="asFunctionPtr"></param>
        /// <returns></returns>
        CgArg ToParam(CgMember cgMem, bool asFunctionPtr);

        /// <summary>
        /// Renders the invocation of <see cref="cgMem"/> as a regex pattern.
        /// </summary>
        /// <param name="cgMem"></param>
        /// <param name="varNames"></param>
        /// <returns></returns>
        string ToInvokeRegex(CgMember cgMem, params string[] varNames);

        /// <summary>
        /// The default string to use as the body of a code generated method.
        /// </summary>
        string NoImplementationDefault { get; }

        /// <summary>
        /// Removes Pre-processor commands for the <see cref="ILangStyle"/>
        /// </summary>
        /// <param name="fileMembers"></param>
        /// <returns></returns>
        string[] RemovePreprocessorCmds(string[] fileMembers);

        /// <summary>
        /// Removes the content of comment blocks - leaves the line-count as is.
        /// </summary>
        /// <param name="fileMembers"></param>
        /// <param name="openningBlockChars"></param>
        /// <param name="closingBlockChars"></param>
        /// <returns></returns>
        string[] RemoveBlockComments(string[] fileMembers, Tuple<char, char> openningBlockChars,
            Tuple<char, char> closingBlockChars);

        /// <summary>
        /// Removes the content of C-style comment blocks - leaves the line-count as is.
        /// </summary>
        /// <param name="fileMembers"></param>
        /// <returns></returns>
        string[] RemoveBlockComments(string[] fileMembers);

        /// <summary>
        /// Removes all content appearing after the 
        /// first occurance of <see cref="lineCommentSequence"/>
        /// the remainder is left as is even when said remainder is simply 
        /// empty
        /// </summary>
        /// <param name="fileMembers"></param>
        /// <param name="lineCommentSequence"></param>
        /// <returns></returns>
        string[] RemoveLineComments(string[] fileMembers, string lineCommentSequence);

        /// <summary>
        /// Encodes all string literals to a unicode format
        /// as "u0000"(per char) - everything else remains as is.
        /// </summary>
        /// <param name="lineIn"></param>
        /// <returns></returns>
        string EncodeAllStringLiterals(string lineIn);

        /// <summary>
        /// The dictionary key is the index of the double-quote while its
        /// value does not include either the opening nor closing double-quote
        /// </summary>
        /// <param name="lineIn"></param>
        /// <returns></returns>
        Dictionary<int, StringBuilder> ExtractAllStringLiterals(string lineIn);

        /// <summary>
        /// Presumes the text present in <see cref="codeFileLines"/> is an array
        /// from a source code file; as such, the function returns 
        /// the namespace import statements off the top.
        /// </summary>
        /// <param name="codeFileLines"></param>
        /// <returns></returns>        
        string[] ExtractNamespaceImportStatements(string[] codeFileLines);

        /// <summary>
        /// Condenses a source code file down to a continous char array
        /// containing only those chars needed by a compiler.
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        List<char> FlattenCodeToCharStream(string[] fileContent);

        /// <summary>
        /// Attempts to derive a type name using the lines of the source code file.
        /// </summary>
        /// <param name="codeFileLines"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        bool TryDeriveTypeNameFromFile(string[] codeFileLines, out string typeName);

        /// <summary>
        /// This is intended to be applied to some code block parse using the line numbers from a pdb file.
        /// It will remove pre-proc directives, all line comments and tear off the very block enclosure char(s)
        /// (if its not part of a pair)
        /// </summary>
        /// <param name="codeBlockLines"></param>
        /// <returns></returns>
        string[] CleanupPdbLinesCodeBlock(string[] codeBlockLines);

        /// <summary>
        /// Transforms the type's name, as a string, into a string which 
        /// is useable by the <see cref="ILangStyle"/>
        /// </summary>
        /// <param name="typeToString"></param>
        /// <returns></returns>
        /// <remarks>
        /// In IL inner classes are delimited by the '+' and 
        /// generics appear as '1[
        /// </remarks>
        string TransformClrTypeSyntax(string typeToString);

        /// <summary>
        /// Transforms the type's name, as a string, into a string which 
        /// is useable by the <see cref="ILangStyle"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks>
        /// In IL inner classes are delimited by the '+' and 
        /// generics appear as '1[
        /// </remarks>
        string TransformClrTypeSyntax(Type type);

        /// <summary>
        /// Asserts that the <see cref="someTypeName"/> is a string equivalent 
        /// to one of either the keys or values in <see cref="ValueTypeToLangAlias"/>
        /// </summary>
        /// <param name="someTypeName"></param>
        /// <returns></returns>
        /// <remarks>
        /// NOTE: <see cref="System.String"/> should be considered a heap-residing type.
        /// </remarks>
        bool IsPrimitiveTypeName(string someTypeName);

        /// <summary>
        /// Transposes the <see cref="am"/> into its <see cref="ILangStyle"/> keyword
        /// </summary>
        /// <param name="am"></param>
        /// <returns></returns>
        string TransposeCgAccessModToString(CgAccessModifier am);

        /// <summary>
        /// Transposes the <see cref="cm"/> to its <see cref="ILangStyle"/> keyword.
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        string TransposeCgClassModToString(CgClassModifier cm);

        /// <summary>
        /// This will generate the common lang-style's code test for the <see cref="someTypeName"/>.
        /// </summary>
        /// <param name="someTypeName">This may be the lang specific name or the .NET name (e.g. int, System.Int32)</param>
        /// <param name="variableName"></param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[
        ///     //note, the returned items are strings whose contents are C# code!
        /// 
        ///     GenUseIsDefaultValueTest("System.String","myVariable"); 
        ///     //returns the string "!string.IsNullOrWhiteSpace(myVariable)"
        /// 
        ///     GenUseIsDefaultValueTest("string","myVariable"); 
        ///     //returns the string "!string.IsNullOrWhiteSpace(myVariable)"
        /// 
        ///     GenUseIsDefaultValueTest("System.Nullable`1[System.DateTime]", "myDtVariable")
        ///     //returns the string "myDtVariable != null"
        /// 
        ///     GenUseIsDefaultValueTest("int", "myNumVariable")
        ///     //returns the string "myNumVariable != 0;"
        /// 
        /// ]]>
        /// </example>
        string GenUseIsNotDefaultValueTest(string someTypeName, string variableName);

        /// <summary>
        /// Finds the first line within the class file's content.
        /// </summary>
        /// <param name="typename">
        /// This should be only a Full Name or Class name and NOT a full assembly qualified type name
        /// </param>
        /// <param name="srcFile"></param>
        /// <param name="firstLine"></param>
        /// <returns></returns>
        bool TryFindFirstLineInClass(string typename, string[] srcFile, out int firstLine);

        /// <summary>
        /// Finds the first line within the class file's content.
        /// </summary>
        /// <param name="typename">
        /// This should be only a Full Name or Class name and NOT a full assembly qualified type name
        /// </param>
        /// <param name="srcFile"></param>
        /// <param name="lastLine">
        /// The line at which additions should begin - this line itself should not be replaced.
        /// </param>
        /// <returns></returns>
        bool TryFindLastLineInClass(string typename, string[] srcFile, out int lastLine);

        /// <summary>
        /// Intended to be added to code blocks whose enclosure is out of balance at some unknown level.
        /// </summary>
        string BlockMarker { get; }

        /// <summary>
        /// Asserts the source code, represented by <see cref="codeBlockLines"/> is
        /// out-of-balance on either the openning or closing tokens.
        /// </summary>
        /// <param name="codeBlockLines"></param>
        /// <returns></returns>
        bool IsOddNumberEnclosureChars(string[] codeBlockLines);

        /// <summary>
        /// Returns a integer whose value represents the  balance of enclosures
        /// within the <see cref="codeBlockLines"/> where '0' means its balanced a negative value
        /// indicates there are too many closing tokens while a positive indicates the opposite.
        /// </summary>
        /// <param name="codeBlockLines"></param>
        /// <returns>The balance of open to close enclosure tokens.</returns>
        /// <remarks>
        /// The balance is calculated on a flattened stream of chars.
        /// Appearances behind block comments, line comments, preprocessors and string literals
        /// are not included.
        /// </remarks>
        int EnclosureCharsCount(string[] codeBlockLines);
    }
}