import sys
import os
import xml.etree.ElementTree as ET
import re

"""Represents a central runtime hub from which all 
the other parts of the namespace are dependent.
"""

FILE_NAME = "nfConfig.cfg.xml"
"""The file name from which `nfConfig` receives all its values."""

__MY_PS_HOME_VAR_NAME = "myPsHome"
"""A kind of (root) directory from which all other paths in `FILE_NAME` are leafs """

__cfgIdName2PropertyAssignment = {
    "favicon" : lambda s: setattr(sys.modules[__name__], "favicon", s),
    "certFileNoFutureX509" : lambda s: setattr(SecurityKeys, 'noFutureX509Cert', s),
    "tempRootDir" : lambda s: setattr(TempDirectories, 'root', s),
    "tempTsvCsvDir" : lambda s: setattr(TempDirectories, 'tsvCsv', s),
    "keyAesEncryptionKey" : lambda s: setattr(SecurityKeys, 'aesEncryptionString', s),
    "keyAesIV" : lambda s: setattr(SecurityKeys, 'aesIv', s),
    "keyHMACSHA1" : lambda s: setattr(SecurityKeys, 'hmacsha1', s),
    "code-file-extensions" : lambda s: setattr(sys.modules[__name__], 'codeFileExtensions', s.split(' ')),
    "config-file-extensions" : lambda s: setattr(sys.modules[__name__], 'configFileExtensions', s.split(' ')),
    "binary-file-extensions" : lambda s: setattr(sys.modules[__name__], 'binaryFileExtensions', s.split(' ')),
    "search-directory-exclusions" : lambda s: setattr(sys.modules[__name__], 'excludeCodeDirectories', s.split(' ')),
    "default-block-size" : lambda s: setattr(sys.modules[__name__], 'defaultBlockSize', int(s)),
    "default-char-separator" : lambda s: setattr(sys.modules[__name__], 'defaultCharSeparator', s),
    "cmd-line-arg-switch" : lambda s: setattr(sys.modules[__name__], 'cmdLineArgSwitch', s),
    "cmd-line-arg-assign" : lambda s: setattr(sys.modules[__name__], 'cmdLineArgAssign', s),
    "punctuation-chars" : lambda s: setattr(sys.modules[__name__], 'punctuationChars', s.split(' ')),
}
"""dict: Is the key-value hash which links the id's in `FILE_NAME` to the properties of the `nfConfig`"""

def findNfConfigFile(pwd = None):
    """A helper function which looks for the `FILE_NAME` in the
    normal runtime directories (e.g. current working dir, user dir, etc.)     
    """
    if pwd != None:
        searchDirs.append(pwd)
    searchDirs = [os.path.join(x,"bin") for x in sys.path if x.endswith("NoFuture") ]
    searchDirs.append(os.path.join(os.path.expanduser("~"), "AppData\\Roaming\\NoFuture"))
    searchDirs.append(os.getcwd())

    nfCfg = ""
    for dir in searchDirs:
        nfCfg = os.path.join(dir, FILE_NAME)
        if os.path.exists(nfCfg):
            break
    return nfCfg

def init(nfCfg):
    """A static ctor of the NfConfig settins

    Args:
        nfCfg (str): 
            This may be either a path to `FILE_NAME` 
            or the actual full xml content thereof.
    """

    if nfCfg == None:
        nfCfg = FindNfConfigFile()
        if nfCfg == None or nfCfg.isspace():
            raise FileNotFoundError("Cannot locate a copy of " + FILE_NAME)

    if isinstance(nfCfg, ET.ElementTree):
        cfgXml = nfCfg
    elif isinstance(nfCfg, str) and nfCfg.strip().startswith("<"):
        cfgXml = ET.fromstring(nfCfg)
    else:
        cfgXml = ET.parse(nfCfg)

    idValueHash = _getIdValueHash(cfgXml)
    _resolveIdValueHash(idValueHash)

    assignedSomething = False
    assignmentHash = __cfgIdName2PropertyAssignment
    for key in assignmentHash.keys():
        if not key in idValueHash.keys():
            continue
        val = idValueHash[key]
        act = assignmentHash[key]
        act(val)
        assignedSomething = True

    return assignedSomething
        
def _resolveIdValueHash(idValueHash):
    """Resolves all the place holders found in the `FILE_NAME` to 
    their fully-expanded representation.
    """
    if not __MY_PS_HOME_VAR_NAME in idValueHash:
        idValueHash.update({__MY_PS_HOME_VAR_NAME, os.getcwd()})
    
    for key in idValueHash:
        idValueHash[key] = _expandCfgValue(idValueHash, idValueHash[key])
    

def _expandCfgValue(idValueHash, value):
    """A recursive function to turn the place-holders of `value`
    into their fully expanded form.

    See the xml commment atop of `FILE_NAME` for an explanation 
    of what a placeholder is and how it works.
    """

    xRefIsMatch = re.search("\\x24\\x28([a-zA-Z0-9_\\-\\x25\\x28\\x29]+)\\x29", value)
    if xRefIsMatch == None:
        return value
    xRefId = xRefIsMatch.group(1)

    valueLessXRefId = value.replace("$(" + xRefId + ")", "")

    if xRefId.startswith("%") and xRefId.endswith("%"):
        envVar = xRefId.replace("%","")
        xRefId = os.environ.get(envVar)
        return xRefId + valueLessXRefId

    if not xRefId in idValueHash:
        return os.getcwd() + valueLessXRefId

    if idValueHash[xRefId].find("$") > -1:
        rValue = _expandCfgValue(idValueHash, idValueHash[xRefId])
        return rValue + valueLessXRefId
    
    return idValueHash[xRefId] + valueLessXRefId

def _getIdValueHash(cfgXml):
    """Resolves the contents of `FILE_NAME` into a hashtable of
    id-to-value pairs.
    """

    idNodes = cfgXml.findall(".//*[@id]")
    
    if idNodes == None or len(idNodes) == 0:
        raise ValueError("The " + FILE_NAME + " has no 'id' "
            "attributes and is probably not the correct file")

    idValueHash = {}
    for idNode in idNodes:
        idAttrs = idNode.attrib
        id = idAttrs.get('id')
        val = idAttrs.get('value')
            
        if id == None:
            continue

        if val == None:
            #expecting CDATA values
            val = idNode.text
                
        idValueHash.update({id:val})

    return idValueHash

defaultCharSeparator = ','
"""str: The op char used to delimit the start of a command line switch."""

cmdLineArgSwitch = "-"
"""str: Typical char of '-' used to delimit the start of a command line switch. """

cmdLineArgAssign = '='
"""str: The op char used in a command line to assign a value to a swtich."""

defaultBlockSize = 256

punctuationChars = []

codeFileExtensions = []

configFileExtensions = []

binaryFileExtensions = []

excludeCodeDirectories = []

favicon = ''

class TempDirectories:
    """Paths to directories used for storing temp results of NoFuture scripts."""
    root = ''
    tsvCsv = ''

class SecurityKeys:
    """
    Various keys, those which are not assigned a value would be real keys externally defined -
    the rest is just for flippin' bits.
    """
    aesEncryptionString = 'gb0352wHVco94Gr260BpJzH+N1yrwmt5/BaVhXmPm6s='
    aesIv = 'az9HzsMj6pygMvZyTpRo6g=='
    hmacsha1 = 'eTcmPilTLmtbalRpKjFFJjpMNns='
    proxyServer = ''
    googleCodeApiKey = ''
    beaDataApiKey = ''
    censusDataApiKey = ''
    noFutureX509Cert = ''
    blsApiRegistrationKey = ''

    
