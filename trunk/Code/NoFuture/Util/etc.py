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
    """Reduces all repeating sequence of space-characters to one.

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
                dataOut += htmlEscStrings.get(dex)
            else:
                dataOut += "&#{0};".format(dex)
    elif escapeType == nfGlobals.EscapeStringType.XML:
        for dex in data:
            if dex in nfNet.xmlEscStrings:
                dataOut += xmlEscStrings.get(dex)
            else:
                dataOut += chr(dex)
    elif escapeType == nfGlobals.EscapeStringType.BLANK:
        for dex in data:
            dataOut += ' '
    else:
        dataOut = value
    return dataOut


        