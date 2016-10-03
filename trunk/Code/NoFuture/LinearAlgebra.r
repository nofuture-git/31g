vLen <- function(a) {
    sqrt(sum(a*a))
}

orthoProj <- function(v,w) {
	((v * w) / (vLen(v)^2)) * v
}

degrees2Radians <- function(a) {
	a * (pi / 180)
}

getRotationMatrix <- function(a) {
	matrix(c(round(cos(a)), round(sin(a)), -1*round(sin(a)), round(cos(a))),ncol=2)
}