import encodings
import string
import urllib.parse
from enum import Enum
from datetime import timedelta

class EscapeStringType(Enum):
    """For encoding strings"""
    DECIMAL = 1
    DECIMAL_LONG = 2
    HEXDECIMAL = 3
    UNICODE = 4
    REGEX = 5
    HTML = 6
    XML = 7
    URI = 8
    BLANK = 9

def distillString(someString):
    """Distills the continous spaces into a single space and
    replaces Cr [0x0D] and Lf [0x0A] characters with a single space.
    """
    if someString is None:
        return None
    sb = ''
    prev = ''
    for c in list(someString):
        if ((c == ' ' or c == '\r' or c == '\n') and prev == ' '):
            continue
        if (c == '\r' or c == '\n'):
            sb += ' '
            prev = ' '
            continue

        sb += c
        prev = c
    return sb

def distillTabs(someString):
    """Reduces all repeating sequence of tab-characters 
    to a single-space

    Returns:
        (str): as a modification of the arg having all 
        sequence of tabs [0x09] reduced to one single space 
        char [0x20] regardless of the length of the sequence.
    """
    if someString is None:
        return None
    while True:
        if someString.find('\t\t') == 0:
            someString = someString.replace('\t\t','\t')
            continue
        return someString.replace('\t', ' ')

def convertToCrLf(fileContent):
    """Converts line endings to CrLf"""
    if fileContent is None:
        return None
    fileContent = fileContent.replace( WIN_NEW_LINE,'\n')
    fileContent = fileContent.replace('\r','\n')
    fileContent = fileContent.replace('\n', WIN_NEW_LINE)
    return fileContent

def escapeString(value, escapeType = EscapeStringType.REGEX):
    """Returns string ``value`` as an escape sequence.
    
    Args:
        value (str): Any string which is to be escaped.
        escapeType (enum, optional): The kind of escape sequence to encode into.
    
    Examples:
        etc.escapeString("I am decimal", EscapeStringType.DECIMAL)
        # "&#73;&#32;&#97;&#109;&#32;&#100;&#101;&#99;&#105;&#109;&#97;&#108;"

        etc.escapeString("[regex]", EscapeStringType.REGEX)
        #"\x5b\x72\x65\x67\x65\x78\x5d"
    """
    if value is None:
        return None
    myCodec = encodings.codecs.lookup("ISO-8859-1")
    data, _ = myCodec.encode(value)
    dataOut = ""
    if escapeType == EscapeStringType.DECIMAL:
        for dex in data:
            dataOut += "&#{0};".format(dex)
    elif escapeType == EscapeStringType.DECIMAL_LONG:
        for dex in data:
            dataOut += "&#{0:0>7}".format(dex)
    elif escapeType == EscapeStringType.HEXDECIMAL:
        for dex in data:
            dataOut += "&#x{0}".format(dex)
    elif escapeType == EscapeStringType.REGEX:
        #switch back to df utf-8
        myCodec = encodings.codecs.lookup("UTF-8")
        data, _ = myCodec.encode(value)
        for dex in data:
            dataOut += "\\x{0:x}".format(dex)
    elif escapeType == EscapeStringType.UNICODE:
        for dex in list(value):
            dataOut += "\\u{:0>4X}".format(ord(dex))
    elif escapeType == EscapeStringType.HTML:
        for dex in data:
            if dex in htmlEscStrings:
                dataOut += htmlEscStrings.get(dex)
            else:
                dataOut += "&#{0};".format(dex)
    elif escapeType == EscapeStringType.XML:
        for dex in data:
            if dex in xmlEscStrings:
                dataOut += xmlEscStrings.get(dex)
            else:
                dataOut += chr(dex)
    elif escapeType == EscapeStringType.URI:
        return urllib.parse.quote(value)
    elif escapeType == EscapeStringType.BLANK:
        for dex in data:
            dataOut += ' '
    else:
        dataOut = value
    return dataOut

def toCamelCase(name, perserveSep = False):
    """Transforms a string of mixed case into standard 
    camel-case (e.g. userName)
    """
    if name == None or name.isspace():
        return ""

    name = name.strip()
    
    #has no letters at all
    if len([x for x in name if x.isalpha()]) == 0:
        return name

    #is all caps
    if name.isupper():
        return name.lower()

    nameformatted = ""
    markStart = False
    sepChars = list(string.punctuation)
    sepChars += " "
    nameChars = list(name)
    i = 0
    while i < len(name):
        c = nameChars[i]
        if c in sepChars:
            if perserveSep:
                nameformatted += c
                i += 1
                continue
            if i+1 < len(name):
                nameChars[i+1] = nameChars[i+1].upper()
                i += 1
                continue

        if not markStart:
            markStart = True
            nameformatted += c.lower()
            i += 1
            continue

        if i > 0 and nameChars[i-1].isupper():
            nameformatted += c.lower()
            i += 1
            continue

        nameformatted += c
        i += 1
    
    return nameformatted
    
def transformCaseToSeparator(camelCaseString, sep = '_'):
    """Given a string in the form of camel-case (or Pascal case) - a
    ``sep`` will be inserted between characters 
    which are lowercase followed by uppercase.
    """

    if camelCaseString == None or camelCaseString.isspace():
        return ""

    separatorName = ""
    for i, c in enumerate(camelCaseString):
        separatorName += c
        if i + 1 > len(camelCaseString)-1:
            continue
        if c.islower() and camelCaseString[(i+1)].isupper():
            separatorName += sep

    return separatorName

def toPascelCase(name, perserveSep = False):
    """Transforms ``name`` into Pascel case"""

    if name == None or name.isspace():
        return ""

    name = toCamelCase(name, perserveSep)
    rslt = name[0].upper()

    i = 1
    while i < len(name):
        rslt += name[i]
        i += 1
        
    return rslt
    
def distillToWholeWords(value):
    """Splits `value` into an array on any readable
    separator - being both camel-case words or special chars.

    Examples:
        etc.distillToWholeWords("The-VariousThings\which,AllowYou ToRead=this")
    """

    if value == None or value.isspace():
        return None

    if len(value) <= 1:
        return [value]

    value = toPascelCase(value)
    value = transformCaseToSeparator(value, defaultCharSeparator)
    #need to perserve order
    outList = list()
    for p in value.split(defaultCharSeparator):
        if not p in outList:
            outList.append(p)
    
    return outList
    
def calcLuhnCheckDigit(someValue):
    """Ref. [http://en.wikipedia.org/wiki/Luhn_algorithm]"""

    if someValue == None or someValue.isspace():
        return -1

    dblEveryOtherSum = 0
    valueChars = [x for x in someValue if x.isnumeric()]
    if len(valueChars) <= 0:
        return -1

    valueChars.reverse()
    for i, valAti in enumerate(valueChars):
        if (i+1) % 2 == 0:
            dblEveryOtherSum += int(valAti)
            continue

        dblValAti = int(valAti) * 2
        if dblValAti == 10:
            dblEveryOtherSum += 1
        elif dblValAti == 12:
            dblEveryOtherSum += 3
        elif dblValAti == 14:
            dblEveryOtherSum += 5
        elif dblValAti == 16:
            dblEveryOtherSum += 7
        elif dblValAti == 18:
            dblEveryOtherSum += 9
        else:
            dblEveryOtherSum += dblValAti

    calc = 10 - (dblEveryOtherSum % 10)
    if calc == 10:
        return 0

    return calc

def jaroWinklerDistance(a, b, mWeightThreshold = 0.7, mNumChars = 4):
    """The Jaro–Winkler distance (Winkler, 1990) is a measure of 
    similarity between two strings.

    Returns:
        (float): 1 means a perfect match, 0 means not match what-so-ever

    Links:
       [http://stackoverflow.com/questions/19123506/jaro-winkler-distance-algorithm-in-c-sharp] 
    """

    if a == None:
        a = ""
    if b == None:
        b = ""
    aLen = len(a)
    bLen = len(b)

    if aLen == 0:
        if bLen == 0:
            return 1.0
        else:
            return 0.0
        
    searchRng = max([0.0, int(max([aLen, bLen])/2) -1])

    aMatched = [False] * aLen
    bMatched = [False] * bLen

    lNumCommon = 0
    for i in range(aLen):
        lStart = max([0, i - searchRng])
        lEnd = min([i + searchRng + 1, bLen])
        for j in range(lStart, lEnd):
            if bMatched[j] == True:
                continue
            if a[i] != b[j]:
                continue
            aMatched[i] = True
            bMatched[j] = True
            lNumCommon += 1
            break

    if lNumCommon == 0:
        return 0.0

    lNumHalfTrans = 0
    k = 0
    for i in range(aLen):
        if aMatched[i] == False:
            continue
        while bMatched[k] == False:
            if a[i] != b[k]:
                lNumHalfTrans += 1
            k += 1
        k += 1
    
    lNumTransposed = lNumHalfTrans/2

    lNumCommonD = lNumCommon
    xc = lNumCommonD/aLen
    yc = lNumCommonD/bLen
    zc = (lNumCommon - lNumTransposed)/lNumCommonD
    lWeight = (xc + yc + zc)/3.0

    if lWeight <= mWeightThreshold:
        return lWeight

    lMax = min([mNumChars, min([len(a), len(b)])])
    lPos = 0
    while lPos < lMax and a[lPos] == b[lPos]:
        lPos += 1

    if lPos == 0:
        return lWeight

    return lWeight + 0.1*lPos*(1.0 -lWeight)

def levenshteinDistance(s, t, asRatioOfMax = False):
    """A string metric for measuring the difference between two sequences.

    Links:
        https://en.wikipedia.org/wiki/Levenshtein_distance#Iterative_with_two_matrix_rows
    """

    if s == None:
        s = ""
    if t == None:
        t = ""

    if t == s:
        return 0
    if len(s) == 0:
        return len(t)
    if len(t) == 0:
        return len(s)

    v0 = [x for x in range(len(t)+1)]
    v1 = [0 for x in range(len(t)+1)]

    for i, si in enumerate(s):

        v1[0] = i + 1

        for j, tj in enumerate(t):

            if si == tj:
                cost = 0
            else:
                cost = 1

            j1 = v1[j] + 1
            j2 = v0[j + 1] + 1
            j3 = v0[j] + cost
            
            if j1 < j2 and j1 < j3:
                v1[j + 1] = j1
                continue
            
            if j2 < j3:
                v1[j+1] = j2
            else:
                v1[j+1] = j3
        
        v0 = v1.copy()

    if not asRatioOfMax:
        return v1[len(t)]

    return 1 - v1[len(t)]/max([len(t), len(s)])

def shortestDistance(s, candidates):
    """Of the possiable `candidates` returns the one with the 
    shortest distance from `s` using the `levenshteinDistance` algo.
    """

    if s == None or s.isspace():
        return None
    if candidates == None:
        return None
    
    dict = {}
    for c in candidates:
        dict[c] = int(levenshteinDistance(s,c))
    minValue = min(dict.values())
    return [k for (k,v) in dict.items() if v == minValue]


WIN_NEW_LINE = '\r\n'

DBL_TROPICAL_YEAR = 365.24255

tropicalYear = timedelta(days=365,hours=5, minutes=49,seconds=16,milliseconds=320)    
        
defaultCharSeparator = ','
"""str: The op char used to delimit the start of a command line switch."""

xmlEscStrings = {
    60: "&lt;",
    62 : "&gt;",
    38 : "&amp;",
    34 : "&quot;",
    39 : "&apos;"
}

htmlEscStrings = {
    162 : '&cent;', 
    161 : '&iexcl;',
    160 : '&nbsp;',
    229 : '&aring;',
    202 : '&Ecirc;',
    227 : '&atilde;',
    197 : '&Aring;',
    225 : '&aacute;',
    255 : '&yuml',
    254 : '&thorn;',
    253 : '&yacute;',
    252 : '&uuml;',
    251 : '&ucirc;',
    250 : '&uacute;',
    249 : '&ugrave;',
    248 : '&oslash;',
    247 : '&divide;',
    246 : '&ouml;',
    245 : '&otilde;',
    244 : '&ocirc;',
    243 : '&oacute;',
    242 : '&ograve;',
    241 : '&ntilde;',
    240 : '&eth;',
    239 : '&iuml;',
    238 : '&icirc;',
    237 : '&iacute;',
    236 : '&igrave;',
    235 : '&euml;',
    234 : '&ecirc;',
    233 : '&eacute;',
    232 : '&egrave;',
    231 : '&ccedil;',
    230 : '&aelig;',
    223 : '&szlig;',
    228 : '&auml;',
    226 : '&acirc;',
    224 : '&agrave;',
    222 : '&THORN;',
    221 : '&Yacute;',
    220 : '&Uuml;',
    219 : '&Ucirc;',
    218 : '&Uacute;',
    217 : '&Ugrave;',
    216 : '&Oslash;',
    215 : '&times;',
    214 : '&Ouml;',
    213 : '&Otilde;',
    212 : '&Ocirc;',
    211 : '&Oacute;',
    210 : '&Ograve;',
    209 : '&Ntilde;',
    208 : '&ETH;',
    207 : '&Iuml;',
    206 : '&Icirc;',
    205 : '&Iacute;',
    204 : '&Igrave;',
    203 : '&Euml;',
    201 : '&Eacute;',
    200 : '&Egrave;',
    199 : '&Ccedil;',
    198 : '&AElig;',
    196 : '&Auml;',
    195 : '&Atilde;',
    194 : '&Acirc;',
    193 : '&Aacute;',
    192 : '&Agrave;',
    191 : '&iquest;',
    190 : '&frac34;',
    189 : '&frac12;',
    188 : '&frac14;',
    187 : '&raquo;',
    186 : '&ordm;',
    185 : '&sup1;',
    184 : '&cedil;',
    183 : '&middot;',
    182 : '&para;',
    181 : '&micro;',
    180 : '&acute;',
    179 : '&sup3;',
    178 : '&sup2;',
    177 : '&plusmn;',
    176 : '&deg;',
    175 : '&macr;',
    174 : '&reg;',
    173 : '&shy;',
    172 : '&not;',
    171 : '&laquo;',
    170 : '&ordf;',
    169 : '&copy;',
    168 : '&uml;',
    167 : '&sect;',
    166 : '&brvbar;',
    165 : '&yen;',
    164 : '&curren;',
    163 : '&pound;',
}

htmlEscStrings.update(xmlEscStrings)