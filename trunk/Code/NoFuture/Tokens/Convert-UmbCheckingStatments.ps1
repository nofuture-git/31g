<#
This script is very specific in purpose; namely, it is to parse monthly banking statements 
from UMB (United Missouri Bank).  I suspect the bank's backend is still some kind of 
mainframe tech from the 20th century.  The statements are pdf's in extension only - the 
actually line data is embedded images where each line is one image.  See the annotations
of the cmdlets below for more information.

The expectation is that this script can stand all on its own and will attempt to resolve
every dependency it has likewise.
#>

$myScriptLocation = $(pwd).Path

$Script:loadedDependencies = $false

function Add-AdHocDependencies(){
    
    #this is alot of work so only do it if really needed
    $Script:loadedDependencies = $true
    
    <#-----------
    ITEXTSHARP 
    -------------#>
    $iTextSharpAsm = [AppDomain]::CurrentDomain.GetAssemblies() | % {$_.GetName().FullName} | ? {$_ -like "iTextSharp*"} | Select-Object -First 1
    if($iTextSharpVersion -eq $null){
        $localCopyITextSharp = (Join-Path $myScriptLocation "itextsharp\lib\itextsharp.dll")
        if(Test-Path $localCopyITextSharp){

            #when itextsharp.dll is here, then just load it
            $itextSharpAsm = [System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($localCopyITextSharp))
        }
        else{

            #manually download iTextSharp
            $iTextSharp = "https://www.nuget.org/api/v2/package/iTextSharp/"
            $localNuPkg = Join-Path $myScriptLocation "itextsharp.nupkg"
            Invoke-WebRequest -Uri $iTextSharp -OutFile $localNuPkg
            if(-not (Test-Path $localNuPkg)){
                throw "Manually download the iTextSharp NuGet Package at '$iTextSharp'" + `
                      " and place it in $myScriptLocation"
                break;
            }

            #now unpack it 
            [System.Reflection.Assembly]::LoadWithPartialName("System.IO.Compression.ZipFile")
            $localNuUnpkg = Join-Path $myScriptLocation "itextsharp"
            [System.IO.Compression.ZipFile]::ExtractToDirectory($localNuPkg, $localNuUnpkg)
            $localCopyITextSharp = (Join-Path $myScriptLocation "itextsharp\lib\itextsharp.dll")
            if(-not (Test-Path $localCopyITextSharp)){
                throw "manually download and unpack the itextsharp.dll to $myScriptLocation"
                break;
            }
            $itextSharpAsm = [System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($localCopyITextSharp))
        }
    }


    <#-----------
    CUSTOM CODE TO PULL IMAGES OUT OF PDF 
    -------------#>
    $sysDrawingAsm = [System.Reflection.Assembly]::LoadWithPartialName("System.Drawing")

    $dependentCode = @"
namespace NoFuture.Tokens
{
    public static class AdHoc
    {
        public static System.Drawing.Bitmap PadBitmap(string imageFile, int padLen = 4)
        {
            //read all images into memory
            System.Drawing.Bitmap finalImage = null;
            padLen = padLen <= 4 ? 4 : padLen;
            try
            {
                using (var bitmap = new System.Drawing.Bitmap(imageFile))
                {
                    //create a bitmap to hold the combined image
                    finalImage = new System.Drawing.Bitmap(bitmap.Width + 2 * padLen, bitmap.Height + 2 * padLen);

                    //get a graphics object from the image so we can draw on it
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                    {
                        //set background color
                        g.Clear(System.Drawing.Color.White);

                        g.DrawImage(bitmap,
                            new System.Drawing.Rectangle(padLen, padLen, bitmap.Width, bitmap.Height));
                    }
                    bitmap.Dispose();
                    return finalImage;
                }

            }
            catch (System.Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
        }

        /// <summary>
        /// Helper method to quickly extract all images out of a PDF file
        /// </summary>
        public static void GetPdfImages(string pdfFileFullName, string outputDirectory = null)
        {
            var newFolderName = CheckPaths(pdfFileFullName, outputDirectory);
            if (string.IsNullOrWhiteSpace(newFolderName) || !System.IO.Directory.Exists(newFolderName))
                return;
            var images = new System.Collections.Generic.List<System.Drawing.Image>();
            using (var pdf = new iTextSharp.text.pdf.PdfReader(pdfFileFullName))
            {
                for (var pageNumber = 1; pageNumber <= pdf.NumberOfPages; pageNumber++)
                {
                    var pg = pdf.GetPageN(pageNumber);
                    images.AddRange(GetImagesFromPdfDict(pg, pdf));
                }
            }

            for (var i = 0; i < images.Count; i++)
            {
                var img = images[i];
                var imgPath = System.IO.Path.Combine(newFolderName, string.Format("Image-{0:0000}.bmp",i));
                img.Save(imgPath);
            }
        }

        internal static string CheckPaths(string pdfFileFullName, string outputDirectory = null)
        {
            if (System.String.IsNullOrEmpty(pdfFileFullName) || !System.IO.File.Exists(pdfFileFullName))
                return null;
            outputDirectory =
                outputDirectory ?? System.IO.Path.GetDirectoryName(pdfFileFullName);
            if (!System.IO.Directory.Exists(outputDirectory))
                return null;

            var newFolderName = System.IO.Path.GetFileNameWithoutExtension(pdfFileFullName);
            newFolderName = System.IO.Path.Combine(outputDirectory, newFolderName);
            if (!System.IO.Directory.Exists(newFolderName))
                System.IO.Directory.CreateDirectory(newFolderName);

            return newFolderName;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/802269/extract-images-using-itextsharp
        /// </summary>
        internal static System.Collections.Generic.IList<System.Drawing.Image> GetImagesFromPdfDict(iTextSharp.text.pdf.PdfDictionary dict, iTextSharp.text.pdf.PdfReader doc)
        {
            var images = new System.Collections.Generic.List<System.Drawing.Image>();
            var res = (iTextSharp.text.pdf.PdfDictionary)(iTextSharp.text.pdf.PdfReader.GetPdfObject(dict.Get(iTextSharp.text.pdf.PdfName.RESOURCES)));
            var xobj = (iTextSharp.text.pdf.PdfDictionary)(iTextSharp.text.pdf.PdfReader.GetPdfObject(res.Get(iTextSharp.text.pdf.PdfName.XOBJECT)));

            if (xobj == null)
                return images;
            foreach (var name in xobj.Keys)
            {
                var obj = xobj.Get(name);
                if (!obj.IsIndirect())
                    continue;
                var tg = (iTextSharp.text.pdf.PdfDictionary)(iTextSharp.text.pdf.PdfReader.GetPdfObject(obj));
                var subtype = (iTextSharp.text.pdf.PdfName)(iTextSharp.text.pdf.PdfReader.GetPdfObject(tg.Get(iTextSharp.text.pdf.PdfName.SUBTYPE)));
                if (iTextSharp.text.pdf.PdfName.IMAGE.Equals(subtype))
                {
                    var xrefIdx = ((iTextSharp.text.pdf.PRIndirectReference)obj).Number;
                    var pdfObj = doc.GetPdfObject(xrefIdx);
                    var str = (iTextSharp.text.pdf.PdfStream)(pdfObj);

                    var pdfImage =
                        new iTextSharp.text.pdf.parser.PdfImageObject((iTextSharp.text.pdf.PRStream)str);
                    var img = pdfImage.GetDrawingImage();

                    images.Add(img);
                }
                else if (iTextSharp.text.pdf.PdfName.FORM.Equals(subtype) || iTextSharp.text.pdf.PdfName.GROUP.Equals(subtype))
                {
                    images.AddRange(GetImagesFromPdfDict(tg, doc));
                }
            }

            return images;
        }
        public static int CalcProgressCounter(int counter, int total)
        {
            if (total <= 0)
                return 0;
            var valout = (int)System.Math.Ceiling((counter / (double)(total)) * 100);
            return valout > 100 ? 100 : valout;
        }
    }
}

"@

    Add-Type -TypeDefinition $dependentCode -ReferencedAssemblies @($sysDrawingAsm.Location, $localCopyITextSharp)

    <#-----------
    TESSERACT OCR 
    -------------#>
    #now need to test the tesseract is installed
    try{ $tesseractVer = Invoke-Expression "tesseract --version" } catch [System.Exception] {    }

    #we have to download the installer and run it...
    if([string]::IsNullOrWhiteSpace($tesseractVer)){

        #deal with chipset 
        $ocrVer = "w64"
        if(-not (Test-Path C:\Windows\SysWOW64)){
            $ocrVer = "w32"
        }

        #download the installer
        $tesseractUri = "https://digi.bib.uni-mannheim.de/tesseract/tesseract-ocr-$ocrVer-setup-v4.1.0.20190314.exe"
        $tesseractSetupFile =  Join-Path $myScriptLocation "tesseract-ocr-$ocrVer-setup.exe"
        Invoke-WebRequest -Uri $tesseractUri -OutFile $tesseractSetupFile

        if(-not (Test-Path $tesseractSetupFile)){
            throw "download and run the latest Tesseract OCR installer from https://digi.bib.uni-mannheim.de/tesseract/"
            break;
        }

        #run the installer
        Invoke-Expression -Command "$tesseractSetupFile"

        #set tesseract environment variables
        if(Test-Path "C:/Program Files/Tesseract-OCR/tessdata"){
            [System.Environment]::SetEnvironmentVariable("TESSDATA_PREFIX","C:/Program Files/Tesseract-OCR/tessdata","Machine")
            $envPaths = [System.Environment]::GetEnvironmentVariable("Path", "Machine").Split(";")
            $envPaths += "C:\Program Files\Tesseract-OCR\"
            [System.Environment]::SetEnvironmentVariable("Path", ([string]::Join(";",$envPaths)),"Machine")
        }
    }
}

<#
    .SYNOPSIS
    Converts a UMB Checking Account PDF statement into a text document
    
    .DESCRIPTION
    In order to get this data out into some structured form (e.g. csv, xml, json, etc.), 
    multiple problems needed to be dealt with.  First was getting the images out of the pdf
    as their own files on the drive. To do this the script has a dependency on the itextsharp 
    NuGet package.  Next, the images needed to be padded with whitespace on all four-sides.
    This is required because the OCR engine couldn't read them.  This is accomplished using
    .NET's System.Drawing assembly. Next was the OCR engine, the script uses Tesseract and 
    will attempt to download and install (with user's help) if its not detected on this 
    machine's PATH variable.  Last, after having read the image data into text data - all 
    the lines needed to be put back together in their original order - a trival task in 
    PowerShell. The final result being a text file whose meaningful lines match the 
    "lines" in the original PDF.
    
    .PARAMETER UmbStatement
    The full file-path to a UMB PDF file.
    
    .EXAMPLE
    C:\PS> $myTxtStatement = Convert-UmbCheckingStatement "C:\Temp\MyScripts\Checking_Statement_20180108.pdf"
    
    .OUTPUTS
    String
#>
function Convert-UmbCheckingStatement
{
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$true,position=0)]
        [String] $UmbStatement
    )
    Process
    {

        if(-not (Test-Path $UmbStatement)){
            throw "no statement found at '$UmbStatement'"
            break;
        }

        if([System.IO.Path]::GetExtension($UmbStatement).ToLower() -ne ".pdf"){
            throw "statement expected as a PDF '$UmbStatement'"
            break;
        }

        #run the cmdlet to load all the dependencies
        if($Script:loadedDependencies -eq $false){
            Add-AdHocDependencies
        }

        #get a local copy in this working directory
        $statementFileName = [System.IO.Path]::GetFileName($UmbStatement)

        $localStatement = Join-Path $myScriptLocation $statementFileName

        if($UmbStatement -ne $localStatement){
            Copy-Item -Path $UmbStatement -Destination $myScriptLocation
            $UmbStatement = $localStatement
        }
        
        #get all the images out of the pdf
        [NoFuture.Tokens.AdHoc]::GetPdfImages($UmbStatement)

        $umbImagesDir = Join-Path $myScriptLocation ([System.IO.Path]::GetFileNameWithoutExtension($UmbStatement))
        $txtStatement = $umbImagesDir + ".txt"

        $images = ls -Path $umbImagesDir | ? {$_.Extension -eq ".bmp"} | % {$_.FullName} | Sort-Object

        Push-Location $umbImagesDir

        $counter = 0
        :nextImage foreach($img in $images){
            $imgFile = ([System.IO.Path]::GetFileName($img))
            $txtFile = ([System.IO.Path]::GetFileNameWithoutExtension($img))

            $percentComplete = [NoFuture.Tokens.AdHoc]::CalcProgressCounter($counter, $images.Count)
            Write-Progress -Activity "$img" -Status "Reading images..." -PercentComplete $percentComplete
            $counter += 1

            $newImage = [NoFuture.Tokens.AdHoc]::PadBitmap($img)

            [System.Threading.Thread]::Sleep(50)
            $newImage.Save($img)
            [System.Threading.Thread]::Sleep(50)

            #read image into text data
            try{ Invoke-Expression -Command "tesseract $imgFile $txtFile" 2>&1 | out-null} catch [System.Exception] {    }

            $txtFileFullName = Join-Path $umbImagesDir ("$txtFile.txt")
            if(-not (Test-Path $txtFileFullName)){
                continue nextImage;
            }

            $txtFileContent = [System.IO.File]::ReadAllText($txtFileFullName)
            $ffStr = ([char]0xC).ToString()
            $lfStr = ([char]0xA).ToString()
            $txtFileContent = $txtFileContent.Replace($ffStr, "").Replace($lfStr, "")
            if([string]::IsNullOrWhiteSpace($txtFileContent)){
                continue nextImage;
            }
            $txtFileContent = $txtFileContent + [System.Environment]::NewLine

            [System.IO.File]::AppendAllText($txtStatement, $txtFileContent)
        }

        Pop-Location
        return $txtStatement
    }
}

<#
    .SYNOPSIS
    Formats the text data from Convert-UmbCheckingStatement
    
    .DESCRIPTION
    This is a subsequent step which follows after a call to 
    Convert-UmbCheckingStatement.  Taking the text file generated 
    by that cmdlet, this cmdlet will get each transaction line and 
    parse it into its three components (i.e. Date, Amount and Description).
    UMB's item-dates are without a year, so this cmdlet also figures
    that out based on the input file's name.  Lastly the format will
    attempt to case the amount into a System.Double and will print 
    a warning message on any line it fails.
    
    .PARAMETER UmbStatement
    The full file-path the Convert-UmbCheckingStatement text file.
    
    .EXAMPLE
    C:\PS> $myStatementData = Format-UmbStatementTransactions "C:\Temp\MyScripts\Checking_Statement_20180108.txt"
    C:\PS> ConvertTo-Json $myStatementData >> C:\Temp\MyData.json
    
    .OUTPUTS
    Hashtable
#>
function Format-UmbStatementTransactions
{
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$true,position=0)]
        [String] $UmbStatement
    )
    Process
    {
        if(-not (Test-Path $UmbStatement)){
            throw "no statement found at '$UmbStatement'"
            break;
        }

        #need to deal with the dates being just MM-DD 
        $fileName = [System.IO.Path]::GetFileNameWithoutExtension($UmbStatement)
        $year = (Get-Date).Year

        $isYearCrossover = $false

        if($fileName -match "20[0-9][0-9]([0-1][0-9])[0-3][0-9]"){
            $fileDate = $Matches[0]
            $isYearCrossover = $Matches[1] -eq "01"
            $dnd = [System.Int32]::TryParse($fileDate.Substring(0,4), [ref] $year)
        }

        $lines = [System.IO.File]::ReadAllLines($UmbStatement)

        $linesFiltered = @()
        $flag = $false
        :nextLine foreach($line in $lines){
            if([string]::IsNullOrWhiteSpace($line)){
                continue nextLine;
            }
            
            #a marker for the end of account transaction section
            $flag = $flag `
                    -or $line.StartsWith("DATE REF CHECK NO") `
                    -or $line.StartsWith("DATE BALANCE DATE BALANCE")

            if($flag){
                continue nextLine;
            }

            #first four chars are expected MM-DD
            if($line -notmatch "^[0-1][0-9]\-[0-3][0-9]\s[0-9].*"){
                continue nextLine;
            }

            #where to split transaction description from amount
            if($line -notmatch "\.[0-9][0-9][\+\-]\s"){
                continue nextLine;
            }

            $transactionDelimiter = $Matches[0]
            $transactionStartsAt = $line.IndexOf($transactionDelimiter) + $transactionDelimiter.Length
            $description = $line.Substring($transactionStartsAt)

            $lineDate = $line.Substring(0,5)
            if($isYearCrossover -and $line.Substring(0,2) -eq "12"){
                $lineDate = ("$lineDate-{0}" -f ($year-1))
            }
            else{
                $lineDate = "$lineDate-$year"
            }

            #amount being whatever is between date and description
            $amount = $line.Substring(5).Replace($description, "").Trim().Replace(" ","")

            #the integer-sign is on the wrong side...
            $intSign = $amount.Substring($amount.Length-1)

            $amount = "{0}{1}" -f $intSign, $amount.Substring(0,$amount.Length-1)
            
            $dblAmount = 0.0
            if(-not ([double]::TryParse($amount, [ref] $dblAmount))){
                Write-Host "[WARNING] Could not parse line '$line'" -ForegroundColor Yellow
                continue nextLine;
            }

            $linesFiltered += @{
                Date = $lineDate;
                Amount = $dblAmount;
                Description = $description;
            }
        }

        return $linesFiltered
    }
}

<#
    .SYNOPSIS
    Converts the transaction data embedded in a UMB pdf statement file into JSON
    
    .DESCRIPTION
    Helper method to compose the other functionality into one place 
    so callers can avoid boilerplate code.
    
    .PARAMETER UmbStatement
    The full file-path to the UMB PDF file.
    
    .EXAMPLE
    C:\PS> ConvertTo-UmbStatementJson "C:\Temp\MyScripts\Checking_Statement_20180108.pdf"
    
    .OUTPUTS
    Hashtable
#>
function ConvertTo-UmbStatementJson
{
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$true,position=0)]
        [String] $UmbStatement
    )
    Process
    {
        $textFile = Convert-UmbCheckingStatement $UmbStatement
        if([string]::IsNullOrWhiteSpace($textFile) -or -not (Test-Path $textFile)){
            throw "Convert-UmbCheckingStatement failed"
            break;
        }

        $tableData = Format-UmbStatementTransactions -UmbStatement $textFile

        if($tableData -eq $null){
            throw "Format-UmbStatementTransactions failed"
            break;
        }

        $statementDir = [System.IO.Path]::GetDirectoryName($UmbStatement);

        $statementName = [System.IO.Path]::GetFileNameWithoutExtension($UmbStatement)

        $jsonOutput = Join-Path $statementDir "$statementName.json"

        ConvertTo-Json $tableData >> $jsonOutput
    }
}