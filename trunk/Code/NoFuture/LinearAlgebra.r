vLen <- function(a) {
    sqrt(sum(a*a))
}

orthoProj <- function(v,w) {
	((v * w) / (vLen(v)^2)) * v
}

degrees2Radians <- function(a) {
	a * (pi / 180)
}