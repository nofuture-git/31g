import matrixEtc
import NonConformableException

def arithmetic(a, b, expr):
    if a is None: a = [[]]
    if b is None: b = [[]]
    if expr is None: expr = lambda d1, d2: d1 + d2
    if len(a) != len(b) or len(a[0]) != len(b[0]):
        msgFormat = 'The dimensions of a as ({0}X{1}) and b as ({2}X{3}) do not match'
        msg = msgFormat.format(len(a), len(a[0]), len(b), len(b[0]))
        raise NonConformableException(msg)

    iLength = len(a)
    jLength = len(a[0])
    c = []
    for i in range(iLength):
        ci = []
        for j in range(jLength):
            ci.append(expr(a[i][j], b[i][j]))
        c.append(ci)

    return c

