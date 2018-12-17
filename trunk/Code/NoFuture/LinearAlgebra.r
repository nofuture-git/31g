 library(ggplot2)
 # used throughtout these notes
 # placed together here to allow for cut\paste in CLI
 
 getMy.Convert.Radians2Degrees <- function(a) a * 180/pi
 getMy.Convert.Degrees2Radians <- function(a) (a / 1) * (pi / 180)
 
 getMy.Draw.CoordGraph <- function(min, max){
     myX <- seq(min,max)
     myY <- seq(min,max)
     mydataframe <- data.frame(myX, myY)
     
     #setting shape makes it a blank coord-plane (will print a warning msg, just ignore)
     myplot <- ggplot(mydataframe, aes(x = myX, y = myY)) + geom_point(shape=NA)
     
     #add the "0" to the lower-left 
     myplot <- myplot + annotate("text", x=-0.20, y=-0.5, label="0")
     
     #draw the x and y axis 
     myplot <- myplot + geom_vline(xintercept = 0)
     myplot <- myplot + geom_hline(yintercept = 0)
     return(myplot)
 }
 
 getMy.Draw.Point <- function(coordPlot, p, ptColor = "black", pointLabel = ""){
    xCoord <- p[1]
    yCoord <- p[2]
    
    d <- data.frame(x = xCoord, y = yCoord, ptColor = ptColor)
    coordPlot <- coordPlot + geom_point(aes(x,y), data = d, colour=ptColor)
    if(pointLabel == "") pointLabel = sprintf("(%s,%s)",xCoord, yCoord)
    coordPlot <- coordPlot + annotate("text", x=xCoord-0.43, y=yCoord-0.43, label=pointLabel)
    return(coordPlot)
 }
 
 getMy.Draw.Vector.NonOrigin <- function(coordPlot, myOrign, myVector, lnColor = "black", vectorLabel = "") {
     xCoord <- myVector[1]
     yCoord <- myVector[2]
     xOrigin <- myOrign[1]
     yOrigin <- myOrign[2]
     d <- data.frame(x1 = xOrigin, y1 = yOrigin, x2 = xCoord, y2 = yCoord, lnColor = lnColor)
     myStdArrow <- arrow(length = unit(0.03, "npc"))
     coordPlot <- coordPlot + geom_segment(aes(x = x1, y = y1, xend = x2, yend = y2), 
         data = d, arrow = myStdArrow, size = 1, colour=lnColor)
     if(vectorLabel == "") vectorLabel = sprintf("(%s,%s)",xCoord, yCoord)
     coordPlot <- coordPlot + annotate("text", x=xCoord-0.43, y=yCoord-0.43, label=vectorLabel)
     return(coordPlot)
 }
 
 getMy.Draw.Vector <- function(coordPlot, myVector, lnColor = "black", vectorLabel = "") {
     coordPlot <- getMy.Draw.Vector.NonOrigin(coordPlot, c(0,0), myVector, lnColor, vectorLabel)
     return(coordPlot)
 }
 
 getMy.Draw.Segment <- function(coordPlot, startVector, endVector, lnColor="plum"){
     xStart = startVector[1]
     yStart = startVector[2]
     xEnd = endVector[1]
     yEnd = endVector[2]
     d <- data.frame(x1 = xStart, y1 = yStart, x2 = xEnd, y2 = yEnd, lnColor = lnColor)
     coordPlot <- coordPlot + geom_segment(aes(x = x1, y = y1, xend = x2, yend = y2), 
         data = d, size = 1, colour=lnColor, linetype="dashed")
     return(coordPlot)
 }
 
 getMy.Draw.Angle <- function(coordPlot, a1, a2, lnColor="#3A75FF"){
     a1Percent <- a1 * 0.2
     a2Percent <- a2 * 0.2
     x1 <- a1Percent[1]
     y1 <- a1Percent[2]
     x2 <- a2Percent[1]
     y2 <- a2Percent[2]
     d <- data.frame(x1, y1, x2, y2, lnColor = lnColor)
     coordPlot <- coordPlot + geom_curve(aes(x = x1, y = y1, xend = x2, yend = y2), 
         data = d, colour=lnColor)
     a1PlusA2 = ((a1+a2) * 0.5) * 0.25
     angle = getMy.Vector.Angle(a1,a2)
     coordPlot <- coordPlot + annotate("text", x=a1PlusA2[1], y=a1PlusA2[2], label=sprintf("%s°",round(angle,1)))
     return(coordPlot)
 }
 
 getMy.Draw.Line <- function(coordPlot, xPoints, yPoints, lnColor="black"){
     d <- data.frame(x = xPoints, y = yPoints)
     coordPlot <- coordPlot + geom_path(mapping = aes(x, y), data = d, 
        colour=lnColor, arrow = arrow(ends="both"))
     return(coordPlot)
 }
 
 getMy.Draw.Triangle <- function(mycoord, a1, a2, a3, fillColor = "lightblue"){
   d <- data.frame(x=c(a1[1],a2[1],a3[1]), y=c(a1[2],a2[2],a3[2]), t=c("a","a","a"))
   mycoord <- mycoord + geom_polygon(data = d, mapping = aes(x = x, y = y, group = t), fill = fillColor)
   return(mycoord)
 }
 
 getMy.Vector.EuclideanNorm <- function(v){
     sqrt(v[1]^2 + v[2]^2)
 }
  
 getMy.Vector.Normalized <- function(v){
     v / getMy.Vector.EuclideanNorm(v)
 }
 getMy.Vector.Distance <- function(q,p){
     getMy.Vector.EuclideanNorm(q-p)
 }
 
 getMy.Vector.CosTheta <- function(a,b){
     (a %*% b) / (getMy.Vector.EuclideanNorm(a) %*% getMy.Vector.EuclideanNorm(b))
 }
 
 getMy.Vector.Angle <- function(u, v){
     myCosTheta <- getMy.Vector.CosTheta(u,v)
     myAcosRadians <- acos(myCosTheta)
     #R is in radians
     myDegrees <- myAcosRadians * (180 / pi)
     return(myDegrees)
 }
 
 getMy.Vector.OrthoProj <- function(v,w){
     ((v %*% w)/getMy.Vector.EuclideanNorm(v)^2) * v
 }
 
 getMy.Line.Implicit.Coeffs.FromVectors <- function(p,q){
     v <- q - p
     ab <- t(c(-1*v[2], v[1]))
     a <- ab[1,1]
     b <- ab[1,2]
     c <- -1 * a * p[1] - b * p[2]
     return(data.frame(a,b,c))
 }
  
 getMy.Line.Explicit.Coeffs.FromVectors <- function(p,q){
     myImpVars <- getMy.Line.Implicit.Coeffs.FromVectors(p,q)
     a <- myImpVars$a
     b <- myImpVars$b
     c <- myImpVars$c
     myCoeffs <- getMy.Line.Explicit.Coeffs.FromImplicit(a,b,c)
     return(myCoeffs)
 }
  
 getMy.Line.Explicit.Coeffs.FromImplicit <- function(a,b,c){
     aTilde <- (-1 * a)/b
     bTilde <- (-1 * c)/b
     return(data.frame(slope=aTilde, intercept=bTilde))
 }
  
 getMy.Line.Reciprocal.Explicit.Coeffs.FromImplicit <- function(a,b,c,r){
     myCoeffs = getMy.Line.Explicit.Coeffs.FromImplicit(a,b,c)
     slope <- myCoeffs$slope
     intercept <-myCoeffs$intercept
     recipCoeffs <- getMy.Line.Reciprocal.Explicit.Coeffs.FromExplicit(slope, intercept,r)
     return(recipCoeffs) 
 }
 
 getMy.Line.Reciprocal.Explicit.Coeffs.FromVectors <- function(p,q,r){
     myCoeffs = getMy.Line.Explicit.Coeffs.FromVectors(p,q)
     slope <- myCoeffs$slope
     intercept <-myCoeffs$intercept
    
     #get the perpendicular line 
     recipCoeffs <- getMy.Line.Reciprocal.Explicit.Coeffs.FromExplicit(slope, intercept, r)
     return(recipCoeffs)
 } 
 
 getMy.Line.Reciprocal.Explicit.Coeffs.FromExplicit <- function(slope, intercept, r){
     recipSlope <- -1*(1/slope)
     recipIntercept <- r[2]-recipSlope*r[1]
     return(data.frame(slope=recipSlope, intercept=recipIntercept))
 }
  
 getMy.Line.Explicit.Fx.FromImplicit <- function(a,b,c){
     myCoeffs <- getMy.Line.Explicit.Coeffs.FromImplicit(a,b,c)
     myExpLn <- function(x) myCoeffs$slope*x + myCoeffs$intercept
     return(myExpLn)
 }
  
 getMy.Line.Explicit.Fx.FromVectors <- function(p,q){
     myImpVars <- getMy.Line.Implicit.Coeffs.FromVectors(p,q)
     a <- myImpVars$a
     b <- myImpVars$b
     c <- myImpVars$c
     myExpLn <- getMy.Line.Explicit.Fx.FromImplicit(a,b,c)
     return(myExpLn)
 }

 getMy.Line.Distance.Point.FromVectors <- function(p,q,r){
     #get implicit vars
     myImpVars <- getMy.Line.Implicit.Coeffs.FromVectors(p, q)
     a <- myImpVars$a
     b <- myImpVars$b
     c <- myImpVars$c
     d <- getMy.Line.Distance.Point.FromImplicit(a,b,c,r)
     return(d)
 }
 
 getMy.Line.Distance.Point.FromImplicit <- function(a,b,c,r){
     d <- abs(a*r[1] + b*r[2] + c) / sqrt(a^2 + b^2)
     return(d)
 }
 
 getMy.Line.Closest.Point.FromVectors <- function(p, v, r){
     #want to express as explicit fx 
     myCoeffs = getMy.Line.Explicit.Coeffs.FromVectors(p,v)
    
     #get the line coeffs
     slope <- myCoeffs$slope
     intercept <-myCoeffs$intercept
     closetPt <- getMy.Line.Closest.Point.FromExplict(slope, intercept, r)
     return(closetPt)
 }

 #https://www.youtube.com/watch?v=YbHOzJIHS1k
 getMy.Line.Closest.Point.FromExplict <- function(slope, intercept, r){
     recipCoeffs <- getMy.Line.Reciprocal.Explicit.Coeffs.FromExplicit(slope, intercept, r)
     recipSlope <- recipCoeffs$slope
     recipIntercept <- recipCoeffs$intercept
     intersectPt <- getMy.Line.Intersects.FromExplicit(slope, intercept, recipSlope, recipIntercept)
     return(intersectPt)
 }
 
 getMy.Line.Intersects.FromVectors <- function(p,v, pTick, vTick){
    oneCoeffs = getMy.Line.Explicit.Coeffs.FromVectors(p,v)
    twoCoeffs = getMy.Line.Explicit.Coeffs.FromVectors(pTick,vTick)
    slope = oneCoeffs$slope
    intercept = oneCoeffs$intercept
    slopeTick = twoCoeffs$slope
    interceptTick = twoCoeffs$intercept
    intersectPt <- getMy.Line.Intersects.FromExplicit(slope, intercept, slopeTick, interceptTick)
    return(intersectPt)
 }

 getMy.Line.Intersects.FromExplicit <- function(slope, intercept, slopeTick, interceptTick){
     #solve for the point where they cross
     #mx + b = m`x + b`
     #mx = m`x + b`- b
     #mx - m`x = b`- b
     #x(m - m`) = b`- b
     #x = (b`- b)/(m - m`)
     interceptX <- (interceptTick - intercept)/(slope - slopeTick)
    
     #plug that point back into either
     interceptY <- slope*interceptX + intercept
     return(c(interceptX, interceptY))
 }
 
 getMy.Convert.Degrees2Rotation <- function(a){
     aRad <- getMy.Convert.Degrees2Radians(a)
     myRotation <- matrix(c(cos(aRad),sin(aRad),-1*sin(aRad),cos(aRad)),ncol=2)
     return(myRotation)
 }
 
 getMy.Vector.Rotation <- function(v, angle){
     myR <- getMy.Convert.Degrees2Rotation(angle)
     vTick <- myR %*% v 
     return(vTick)
 }
 
 getMy.Vector.Shear <- function(v){
     vMatrix <- matrix(c(1,(-1*v[2]/v[1]),0,1), ncol=2)
     vTick = vMatrix %*% v
     return(vTick)
 }
 
 getMy.Vector.Projection.FromDegrees <- function(v,angle){
     ui <- getMy.Convert.Degrees2Radians(angle)
     u <- matrix(c(ui, ui),ncol=1)
     #defines projection matrix
     A <- u %*% t(u)
     vTick <- A %*% v
     return(vTick)
 }
 
 getMy.Vector.LinearArea <- function(a1, a2){
   T1 = 1/2*a1[1]*a2[1]
   T2 = 1/2*(a1[1] - a1[2])*(a2[2]-a2[1])
   T3 = 1/2*a1[2]*a2[2]
   
   T0 = a1[1] * a2[2] - T1 - T2 - T3 
   myMap <- data.frame(T1= abs(T1), T2= abs(T2), T3= abs(T3), T0 = abs(T0))
   return(myMap)
 }
 
 getMy.Vector.LinearAreaBox<- function(u,v, origin = c(0,0)){
   topRight <- c(max(c(u[1],v[1],origin[1])), max(c(u[2],v[2],origin[2])))
   bottomLeft <- c(min(c(u[1],v[1],origin[1])), min(c(u[2],v[2],origin[2])))
   topLeft <- c(bottomLeft[1],topRight[2])
   bottomRight <- c(topRight[1], bottomLeft[2])
   
   d <- data.frame(bottomLeft,bottomRight,topLeft,topRight)
   return(d)
 }
 
 #Sketch 6.1, Practical Linear Algebra, 3rd Edition
 getMy.Vector.AffineMap <- function(x,a1,a2,p = c(0,0),origin = c(0,0)){
    A <- matrix(c(a1,a2),ncol=2)
    xTick <- p + A %*% (x - origin)
 }
 
 #https://medium.com/@14prakash/back-propagation-is-very-simple-who-made-it-complicated-97b794c97e5c
 getMy.relu <- function(x) max(c(0,x))
 getMy.sigmoid <- function(x) 1 / (1 + exp(-1*x))
 getMy.softmax <- function(e) 
 {
    xExp <- sapply(e, exp)
    smDeno <- sum(xExp)
    mout <- sapply(xExp, `/`, smDeno)
    return(mout)
 }
 getMy.crossentropy <- function(yActual,yCalc){
    fd001 <- function(y, o) y * log10(o) + ((1 - y) * log10(1 - o))
    ff <- 0
    for(i in c(1:n))
    {
        ff <- ff + fd001(yActual[i], yCalc[i])
    }
    mout <- -1 * ff
    return(mout)
 }
 getMy.sigmoidTick <- function(x) sigmoid(x)* (1-sigmoid(x))
 getMy.reluTick <- function(x) if(x > 0) 1 else 0
 getMy.softmaxTick <- function(e) {
    xExp <- sapply(e, exp)
    fd002 <- function(ii) 
    {
        skj <- 0
        for(j in c(1:length(e)))
        {
            jv <- if(ii == j) 0 else exp(e[j])
            skj <- skj + jv
        }
        (e[ii] * skj)/xExp
    }
    
    mout <- sapply(e, fd002)
    return(mout)
 }