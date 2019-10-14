import random
import numpy as np
import numpy.linalg
import math

def convFromNumpyArray(numpyNdArray):
    """Converts a, typically a numpy array, into a simple python list"""
    if numpyNdArray is None:
        return [[]]
    isvector = False
    if type(numpyNdArray) is np.ndarray:
        if len(numpyNdArray.shape) == 2:
            numOfRows, numOfColumns = numpyNdArray.shape
        else:
            #this is a vector
            numOfRows = len(numpyNdArray)
            numOfColumns = 1
            isvector = True
    else:
        return numpyNdArray
    
    a = initMatrix(numOfRows, numOfColumns)
    for i in range(numOfRows):
        if isvector:
            a[i][0] = numpyNdArray[i]
            continue
        for j in range(numOfColumns):
            a[i][j] = numpyNdArray[i][j]
    return a

def initMatrix(numOfRows, numOfColumns, useOneInsteadOfZero = False):
    """Allow for matrix init to match the pattern found in .NET counterparts"""
    mi = []
    if numOfRows <= 0:
        numOfRows = 1
    if numOfColumns <= 0:
        numOfColumns = 1
    dfValue = 1 if useOneInsteadOfZero else 0
    for i in range(numOfRows):
        mj = []
        for j in range(numOfColumns):
            mj.append(dfValue)
        mi.append(mj)

    return mi

def randomMatrix(numOfRows, numOfColumns, expr = None):
    """Creates a matrix of [numOfRows X numOfColumns] where each value 
       is a random float. """
    random.seed()
    numOfRows = random.randint(4,8) if numOfRows <= 0 else numOfRows
    numOfColumns = random.randInt(4,8) if numOfColumns <= 0 else numOfColumns
    mi = []

    for i in range(numOfRows):
        mj = []

        for j in range(numOfColumns):
            mjRandValue = random.random()
            if expr is not None:
                mjRandValue = expr(mjRandValue)
            mj.append(mjRandValue)

        mi.append(mj)

    return mi

def dotProduct(a, b):
    a = convFromNumpyArray(a)
    b = convFromNumpyArray(b)
    m = len(a)
    n = len(a[0])
    p = len(b)
    q = len(b[0])

    #'a' is a vertical vector and would fit if we transposed it
    if n != p and m == p and n == 1:
        a = transpose(a)
        m = len(a)
        n = len(a[0])

    if n != p:
        msg = "The number of columns in matrix 'a' must match the number of rows in matrix 'b' " + \
              "in order to solve for the dot-product."
        raise ArithmeticError(msg)

    product = initMatrix(m,q)
    for productRows in range(m):
        for productColumns in range(q):
            for i in range(n):
                product[productRows][productColumns] += a[productRows][i] * b[i][productColumns]

    return product
                
def dotScalar(a, scalar):
    a = convFromNumpyArray(a)
    if scalar is None: scalar = 0
    iLength = len(a)
    jLength =len(a[0])
    vout = initMatrix(iLength, jLength)
    for i in range(iLength):
        for j in range(jLength):
            vout[i][j] = a[i][j] * scalar

    return vout

def transpose(a):
    a = convFromNumpyArray(a)
    tpose = initMatrix(len(a[0]), len(a))
    for i in range(len(a)):
        for j in range(len(a[0])):
            tpose[j][i] = a[i][j]
    return tpose

def crossProduct(a):
    a = convFromNumpyArray(a)
    return dotProduct(transpose(a), a)

def innerProduct(a):
    a = convFromNumpyArray(a)
    return dotProduct(a, transpose(a))

def inverse(a):
    if a is None: a = [[]]
    numpyInvA = numpy.linalg.inv(numpy.array(a))
    return convFromNumpyArray(numpyInvA)
    
def determinant(a):
    if a is None: a = [[]]
    return numpy.linalg.det(numpy.array(a))

def selectMinor(a, rowIndex, columnIndex):
    a = convFromNumpyArray(a)
    rows = len(a)
    columns = len(a[0])
    if(rowIndex > rows -1 or columnIndex > columns - 1):
        return a
    
    vout = initMatrix(rows - 1, columns - 1)
    for i in range(rows):
        if i == rowIndex:
            continue
        vi = i - 1 if i > rowIndex else i
        for j in range(columns):
            if j == columnIndex:
                continue
            vj = j - 1 if j > columnIndex else j
            vout[vi][vj] = a[i][j]

    return vout

def projectionMatrix(a):
    a = convFromNumpyArray(a)
    ata = crossProduct(a)
    ataInverse = inverse(ata)
    p0 = dotProduct(a, ataInverse)
    return dotProduct(p0, inverse(a))

def applyToEach(a, expr):
    a = convFromNumpyArray(a)
    if expr is None:
        return a
    for i in range(len(a)):
        for j in range(len(a[0])):
            a[i][j] = expr(a[i][j])
    return a

def getSoftmax(a):
    a = convFromNumpyArray(a)
    mout = initMatrix(len(a), len(a[0]))
    for i in range(len(a)):
        sumZExp = 0.0
        for j in range(len(a[0])):
            sumZExp += math.exp(a[i][j])
        
        for j in range(len(a[0])):
            mout[i][j] = math.exp(a[i][j]) / sumZExp

    return mout

def arithmetic(a, b, expr):
    a = convFromNumpyArray(a)
    b = convFromNumpyArray(b)
    if expr is None: expr = lambda v1, v2: v1 + v2
    
    arthResult = initMatrix(len(a), len(a[0]))
    for i in range(len(a)):
        for j in range(len(a[0])):
            arthResult[i][j] = expr(a[i][j], b[i][j])
        
    return arthResult