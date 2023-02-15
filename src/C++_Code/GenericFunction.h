#pragma once
#include <cmath>
#include <utility>
#include <fstream>
#include "FunctionPoint.h"
#include "Arithmetics.h"

class GenericFunction {
public:
	std::vector<double> p_c;
	std::vector<double> p_m;
	virtual int get_dimension() const = 0;
	virtual double get_epsilon() const = 0;
	virtual std::pair<double, double> get_domain() const = 0;
	virtual double compute_value(FunctionPoint& current_point) = 0;
	virtual double eval_mask(const std::vector<bool>&mask) = 0;
	virtual double eval_gray_mask(const std::vector<bool>& mask) = 0;
	virtual std::string get_name() = 0;
	virtual FunctionPoint decode_mask(const std::vector<bool>&mask) = 0;
	virtual std::vector<bool> generate_mask() = 0;
	virtual int get_bit_size() = 0;
	virtual std::vector<double> get_pc() = 0;
	virtual std::vector<double> get_pm() = 0;
};