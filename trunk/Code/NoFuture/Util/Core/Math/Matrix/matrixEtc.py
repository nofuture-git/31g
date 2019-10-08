import random

def countOfRows(multidimList):
    if multidimList is None:
        return 0
    return len(multidimList)

def countOfColumns(multidimList):
    if multidimList is None:
        return 0;
    return len(multidimList[0])

def randomMatrix(numOfRows, numOfColumns, expr = None):
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