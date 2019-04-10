$myScriptLocation = Split-Path $PSCommandPath -Parent

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
    Converst the UMB PDF into a text document by first pulling all the 
    line items, which are images, from the PDF then applies OCR to read
    each image into text data.
    
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

            $counter += 1
        }

        Pop-Location
        return $txtStatement
    }
}

<#
    .SYNOPSIS
    Formats the text data from Convert-UmbCheckingStatement
    
    .DESCRIPTION
    This cmdlet takes the results Convert-UmbCheckingStatement and
    filters and parses them into structured data.
    
    .PARAMETER UmbStatement
    The full file-path the Convert-UmbCheckingStatement text file.
    
    .EXAMPLE
    C:\PS> $myStatementData = Format-UmbStatement "C:\Temp\MyScripts\Checking_Statement_20180108.txt"
    C:\PS> ConvertTo-Json $myStatementData >> C:\Temp\MyData.json
    
    .OUTPUTS
    Hashtable
#>
function Format-UmbStatement
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
            $flag = $flag -or $line.StartsWith("DATE REF CHECK NO")

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