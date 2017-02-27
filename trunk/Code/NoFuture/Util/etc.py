import encodings
import Util.net as nfNet
import Shared.globals as nfGlobals

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

    Returns a string as a modification of the arg having all 
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
    fileContent = fileContent.replace( nfGlobals.WIN_NEW_LINE,'\n')
    fileContent = fileContent.replace('\r','\n')
    fileContent = fileContent.replace('\n', nfGlobals.WIN_NEW_LINE)
    return fileContent

def escapeString(value, escapeType = nfGlobals.EscapeStringType.REGEX):
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
    if escapeType == nfGlobals.EscapeStringType.DECIMAL:
        for dex in data:
            dataOut += "&#{0};".format(dex)
    elif escapeType == nfGlobals.EscapeStringType.DECIMAL_LONG:
        for dex in data:
            dataOut += "&#{0:0>7}".format(dex)
    elif escapeType == nfGlobals.EscapeStringType.HEXDECIMAL:
        for dex in data:
            dataOut += "&#x{0}".format(dex)
    elif escapeType == nfGlobals.EscapeStringType.REGEX:
        #switch back to df utf-8
        myCodec = encodings.codecs.lookup("UTF-8")
        data, _ = myCodec.encode(value)
        for dex in data:
            dataOut += "\\x{0:x}".format(dex)
    elif escapeType == nfGlobals.EscapeStringType.UNICODE:
        for dex in list(value):
            dataOut += "\\u{:0>4X}".format(ord(dex))
    elif escapeType == nfGlobals.EscapeStringType.HTML:
        for dex in data:
            if dex in nfNet.htmlEscStrings:
                dataOut += nfNet.htmlEscStrings.get(dex)
            else:
                dataOut += "&#{0};".format(dex)
    elif escapeType == nfGlobals.EscapeStringType.XML:
        for dex in data:
            if dex in nfNet.xmlEscStrings:
                dataOut += nfNet.xmlEscStrings.get(dex)
            else:
                dataOut += chr(dex)
    elif escapeType == nfGlobals.EscapeStringType.BLANK:
        for dex in data:
            dataOut += ' '
    else:
        dataOut = value
    return dataOut

def toCamelCase(name):
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
    
    for i, c in enumerate(name):
        if not c.isalpha():
            nameformatted += c
            continue

        if not markStart:
            markStart = True
            nameformatted += c.lower()
            continue

        if i > 0 and name[i-1].isupper():
            nameformatted += c.lower()
            continue

        nameformatted += c
    
    return nameformatted
    
def transformCamelCaseToSeparator(camelCaseString, sep = '_'):
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