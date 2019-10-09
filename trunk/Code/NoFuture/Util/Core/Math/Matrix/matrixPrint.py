import string
import Util.Core.Math.Matrix.matrixEtc

def printCodeStyle(multidimList, style = "js"):
    if style is None:
        style = "js"
    open = "["
    close = "]"
    nfStr = ""
    isRstyle = style.lower() == "r"
    if(len([v for v in ["cs", "c#"] if v == style.lower()]) > 0):
        open = "{"
        close = "}"
        nfStr += "new[,]"
    if(len([v for v in ["ps", "powershell"] if v == style.lower()]) > 0):
        open = "@("
        close = ")"
    if(style.lower() == "java"):
        open = "{"
        close = "}"
        nfStr += "new Double[][]"
    if(isRstyle):
        open = ""
        close = ""
        nfStr += "matrix(c("

    nfStr += f"{open}\n" 
    lines = []
    rowCount = len(multidimList)
    columnCount = len(multidimList[0])
    for i in range(rowCount):
        strLn = open
        vals = []
        for j in range(columnCount):
            vals.append(multidimList[i][j])
        strLn += ",".join([str(v) for v in vals])
        strLn += close
        lines.append(strLn)
    
    nfStr += f",\n".join(lines)
    nfStr += "\n"
    if not isRstyle:
        nfStr += close
    else:
        nfStr += f"), ncol={columnCount}, byrow=TRUE)"

    return nfStr

def printRstyle(multidimList):
    nfStr = ""
    roundTo = 6
    maxLen = 0
    anyNeg = False
    rowCount = len(multidimList)
    columnCount = len(multidimList[0])
    for i in range(rowCount):
        for j in range(columnCount):
            aij = multidimList[i][j]
            aijString = str(round(aij,roundTo))
            if len(aijString) > maxLen:
                maxLen = len(aijString)
            else:
                anyNeg = True

    maxLen += 2
    
    for i in range(columnCount):
        headerStr = f"[,{i}]"
        padding = maxLen
        
        if i == 0:
            hdrFill = 2 if maxLen <= 2 else 4
            #this is weird... 
            nfStr += "".ljust(len(str(rowCount)) + hdrFill)
        
        nfStr += ("{: <" + str(padding) + "}").format(headerStr)

    nfStr += "\n"
    for i in range(rowCount):
        nfStr += f"[{i},]"
        for j in range(columnCount):
            aij = multidimList[i][j]
            aijString = str(round(aij,roundTo))
            nfFormat = (" {: <" + str(maxLen -1) + "}") if aij >= 0 and anyNeg else ("{: <" + str(maxLen) + "}")
            nfStr += nfFormat.format(aijString)
            
        nfStr += "\n"

    print(nfStr)