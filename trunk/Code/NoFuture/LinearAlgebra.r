vLen <- function(a) {
    sqrt(sum(a*a))
}

orthoProj <- function(v,w) {
	((v * w) / (vLen(v)^2)) * v
}