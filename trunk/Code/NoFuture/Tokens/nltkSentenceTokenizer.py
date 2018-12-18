#Simple hook python script to pass some text file
#to the sentence tokenizer of NLTK package.
#To install the package:
# C:\PS> python -m pip install nltk
#
#To run the script (from PowerShell on Windows):
# C:\PS> $thisScript = "C:\Tokens\nltkSentenceTokenizer.py"
# C:\PS> $myTextFile = C:\Temp\someTextFile.txt
# C:\PS> $tokenFile = python $thisScript $myTextFile

from nltk.tokenize import sent_tokenize, word_tokenize
import sys
import os

#get the file path from the invocation 
fullFilePath = sys.argv[1]
(filePath, fileName) = os.path.split(fullFilePath)
(fileNameLessExt, fileExt) = os.path.splitext(fileName)
fullOutFile = os.path.join(filePath, fileNameLessExt + ".nltk" + fileExt)

txtFile = open(fullFilePath, 'r+')
tokenFile = open(fullOutFile, 'w')

txtLines = txtFile.readlines()
tokenLines = []
for txtLn in txtLines:
    for tokenSen in sent_tokenize(txtLn):
        tokenLines.append(tokenSen + "\n")

tokenFile.writelines(tokenLines)
tokenFile.flush()
tokenFile.close()
txtFile.close()
print(fullOutFile)