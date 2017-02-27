from enum import Enum
import encodings

class EscapeStringType(Enum):
    """For encoding strings"""
    DECIMAL = 1
    DECIMAL_LONG = 2
    HEXDECIMAL = 3
    UNICODE = 4
    REGEX = 5
    HTML = 6
    XML = 7
    BLANK = 8

WIN_NEW_LINE = '\r\n'

DOT_NET_EN_PUNCTUATION_CHARS = [
    '.', '?', "'", 
    '"', ':', '!', 
    ',', ';', '{',
    '}', '[', ']',
    '(', ')', '_', 
    '@', '%', '&',
    '*', '-', '\\'
    '/'
]

DEFAULT_TYPE_SEPARATOR = '.'