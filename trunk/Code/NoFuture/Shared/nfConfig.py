import sys
import os
import xml.etree.ElementTree as ET

FILE_NAME = "nfConfig.cfg.xml"
__MY_PS_HOME_VAR_NAME = "myPsHome"

__cfgIdName2PropertyAssignment = {
    "certFileNoFutureX509" : lambda s: setattr(NfConfig.SecurityKeys, 'noFutureX509Cert', s),
    "tempRootDir" : lambda s: setattr(NfConfig.TempDirectories, 'root', s),
    "tempTsvCsvDir" : lambda s: setattr(NfConfig.TempDirectories, 'tsvCsv', s),
    "keyAesEncryptionKey" : lambda s: setattr(NfConfig.SecurityKeys, 'aesEncryptionString', s),
    "keyAesIV" : lambda s: setattr(NfConfig.SecurityKeys, 'aesIv', s),
    "keyHMACSHA1" : lambda s: setattr(NfConfig.SecurityKeys, 'hmacsha1', s),
    "code-file-extensions" : lambda s: setattr(self, 'codeFileExtensions', s.split(' ')),
    "config-file-extensions" : lambda s: setattr(self, 'configFileExtensions', s.split(' ')),
    "binary-file-extensions" : lambda s: setattr(self, 'binaryFileExtensions', s.split(' ')),
    "search-directory-exclusions" : lambda s: setattr(self, 'excludeCodeDirectories', s.split(' ')),
    "default-block-size" : lambda s: setattr(self, 'defaultBlockSize', int(s)),
    "default-char-separator" : lambda s: setattr(self, 'defaultCharSeparator', s),
    "cmd-line-arg-switch" : lambda s: setattr(self, 'cmdLineArgSwitch', s),
    "cmd-line-arg-assign" : lambda s: setattr(self, 'cmdLineArgAssign', s),
    "punctuation-chars" : lambda s: setattr(self, 'punctuationChars', s.split(' '))
}

def findNfConfigFile():
    """A helper function which looks for the `FILE_NAME` in the
    normal runtime directories (e.g. current working dir, user dir, etc.)     
    """
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
    if nfCfg == None or nfCfg.isspace():
        nfCfg = FindNfConfigFile()
        if nfCfg == None or nfCfg.isspace():
            raise FileNotFoundError("Cannot locate a copy of " + FILE_NAME)

    if nfCfg.strip().startswith("<"):
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
    pass

def _expandCfgValue(idValueHash, value):
    pass

def _getIdValueHash(cfgXml):
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
cmdLineArgSwitch = "-"
cmdLineArgAssign = '='
defaultBlockSize = 256
punctuationChars = []
codeFileExtensions = []
configFileExtensions = []
binaryFileExtensions = []
excludeCodeDirectories = []

class TempDirectories:
    root = ''
    tsvCsv = ''

class SecurityKeys:
    aesEncryptionString = 'gb0352wHVco94Gr260BpJzH+N1yrwmt5/BaVhXmPm6s='
    aesIv = ''
    hmacsha1 = ''
    proxyServer = ''
    googleCodeApiKey = ''
    beaDataApiKey = ''
    censusDataApiKey = ''
    noFutureX509Cert = ''
    blsApiRegistrationKey = ''

    
