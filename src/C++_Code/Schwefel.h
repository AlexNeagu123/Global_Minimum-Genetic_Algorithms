#pragma once
#include <math.h>
#include "GenericFunction.h"
#include "Arithmetics.h"
#include "Decoder.h"


class Schwefel : public GenericFunction {
	std::pair<double, double> domain;
	const double epsilon;
	int dimension;
public:
	BitmaskDecoder decoder;
	explicit Schwefel(const std::pair<double, double>& _domain, int _dimension, double _epsilon) : domain(_domain), epsilon(_epsilon), dimension(_dimension), decoder(domain, epsilon, dimension) {
		p_c.resize(my_constants::pop_size);
		p_m.resize(my_constants::pop_size);
	};
	double compute_value(FunctionPoint& current_point) override;
	std::pair<double, double> get_domain() const override;
	int get_dimension() const override;
	double get_epsilon() const override;
	std::string get_name() override;
	FunctionPoint decode_mask(const std::vector<bool>& mask) override;
	std::vector<bool> generate_mask() override;
	double eval_mask(const std::vector<bool>& mask) override;
	double eval_gray_mask(const std::vector<bool>& mask) override;
	int get_bit_size() override;
	std::vector<double> get_pc();
	std::vector<double> get_pm();
};