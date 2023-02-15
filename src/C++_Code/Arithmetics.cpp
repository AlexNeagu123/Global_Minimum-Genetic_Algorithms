#include "Arithmetics.h"

double fast_exponentation(double base, int exponent) {
	double ans = 1.0;
	while (exponent) {
		if (exponent & 1) {
			ans = ans * base;
		}
		base = base * base;
		exponent /= 2;
	}
	return ans;
}

int logarithm(int number) {
	int answer = 0;
	while (number) {
		answer += 1;
		number /= 2;
	}
	return answer;
}
