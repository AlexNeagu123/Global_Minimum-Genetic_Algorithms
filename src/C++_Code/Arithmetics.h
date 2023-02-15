#pragma once
#include <cmath>
namespace my_constants {
	double constexpr PI = 3.14159265358979323846;
	static double mut_prob = 0.0001;
	static double cross_prob = 0.70;
	int constexpr pop_size = 200;
	int constexpr generation_nr = 2000;
}
double fast_exponentation(double base, int exponent);
int logarithm(int number);