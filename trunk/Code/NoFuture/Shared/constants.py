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
    BLANK = 8

WIN_NEW_LINE = '\r\n'

DBL_TROPICAL_YEAR = 365.24255

tropicalYear = timedelta(days=365,hours=5, minutes=49,seconds=16,milliseconds=320)