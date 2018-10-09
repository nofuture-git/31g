[NoFuture.Timeline.Constants+GraphStrings]::ShaftLenAfterArrowHead = 10
$oc = "operating context(1)" 
$ni = "new instance(2)" 
$re = "remote exe(3)" 
$ta = "target assembly(4)"
$myRuler = New-Object NoFuture.Timeline.Rule -Property @{StartValue = 1; EndValue = 60; RuleLineSpacing = 5}
$myTokensTl = New-Object NoFuture.Timeline.FastPlate("Using AssemblyAnalysis", $myRuler, $oc, $ni, $re, $ta)

$myTokensTl.Notes.Add("(1) assume PowerShell")
$myTokensTl.Notes.Add("(2) new instance of NoFuture.Util.Gia.AssemblyAnalysis")
$myTokensTl.Notes.Add("(3) new process NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe")
$myTokensTl.Notes.Add("(4) the assembly whose tokens we want")


$myTokensTl.FFrom($oc).FTo($ni).FText("new instance of")
$myTokensTl.FFrom($ni).FTo($ni).FText("start new process")
$myTokensTl.FFrom($ni).FTo($re).FText("provide assembly name")
$myTokensTl.FFrom($re).FTo($re).FText("launch sockets")
$myTokensTl.FFrom($re).FTo($ta).FText("get asm names on manifest")
$myTokensTl.FFrom($re).FTo($ni).FText("send AsmIndicies on socket")
$myTokensTl.FFrom($ni).FTo($ni).FText("receive AsmIndices")
$myTokensTl.FFrom($ni).FTo($ni).FText("save to disk")
$myTokensTl.FFrom($ni).FTo($oc).FText("assign to prop")
$myTokensTl.FFrom($oc).FTo($ni).FText("invoke GetTokenIds with regex")
$myTokensTl.FFrom($ni).FTo($re).FText("send GetTokenIdsCriteria on socket")
$myTokensTl.FFrom($re).FTo($ta).FText("get types as tokens")
$myTokensTl.FFrom($re).FTo($ta).FText("get members as tokens")
$myTokensTl.FFrom($re).FTo($ta).FText("get callvirts as tokens")
$myTokensTl.FFrom($re).FTo($ta).FText("get tokens-of-tokens(a)")
$myTokensTl.FFrom($re).FTo($ni).FText("send TokenIds on socket(b)")

$myTokensTl.FFrom($ni).FTo($ni).FText("get TokenIds")
$myTokensTl.FFrom($ni).FTo($ni).FText("save to disk")
$myTokensTl.FFrom($ni).FTo($oc).FText("return TokenIds")

$myTokensTl.FFrom($oc).FTo($oc).FText("flatten the TokenIds")
$myTokensTl.FFrom($oc).FTo($ni).FText("invoke GetTokenNames")
$myTokensTl.FFrom($ni).FTo($re).FText("send MetadataTokenId[] on socket")
$myTokensTl.FFrom($re).FTo($ta).FText("resolve each to a runtime type")
$myTokensTl.FFrom($re).FTo($ni).FText("send TokenNames on socket")

$myTokensTl.FFrom($ni).FTo($ni).FText("receive TokenNames")
$myTokensTl.FFrom($ni).FTo($ni).FText("save to disk")
$myTokensTl.FFrom($ni).FTo($oc).FText("return TokenNames")


$myTokensTl.Notes.Add("")
$myTokensTl.Notes.Add("(a)  The top-level types already had all thier members resolved to tokens and every virtcall found within the body of those members.")
$myTokensTl.Notes.Add("     This is now starting it all over again as if the types found on the callvirts in the body of these members were themselves the top-level types.")
$myTokensTl.Notes.Add("     This will continue until we end up on a type which is contained in an assembly whose name doesn't match our regex pattern.")
$myTokensTl.Notes.Add("(b)  This is a tree data-struct. Each token has itself a collection of tokens and so on.")




