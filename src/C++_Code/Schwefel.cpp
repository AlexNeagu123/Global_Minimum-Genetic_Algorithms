#include "Schwefel.h"

double Schwefel::compute_value(FunctionPoint& current_point) {
	double s = 0.0;
	for (int j = 0; j < current_point.get_dimension(); ++j) {
		s = s + current_point[j] * sin(sqrt(abs(current_point[j])));
	}
	return 418.9829 * current_point.get_dimension() - s;
}

std::pair<double, double> Schwefel::get_domain() const {
	return domain;
}

FunctionPoint Schwefel::decode_mask(const std::vector<bool>& mask) {
	return decoder.decode_bit_mask(mask);
}

double Schwefel::eval_mask(const std::vector<bool>& mask) {
	FunctionPoint decoded = decode_mask(mask);
	return compute_value(decoded);
}

double Schwefel::eval_gray_mask(const std::vector<bool>& mask) {
	FunctionPoint decoded = decoder.decode_gray_mask(mask);
	return compute_value(decoded);
}

int Schwefel::get_dimension() const {
	return dimension;
}

double Schwefel::get_epsilon() const {
	return epsilon;
}

std::vector<bool> Schwefel::generate_mask() {
	return decoder.generate_random_mask();
}

int Schwefel::get_bit_size() {
	return decoder.get_bit_count();
}

std::vector<double> Schwefel::get_pc()
{
	return p_c;
}

std::vector<double> Schwefel::get_pm()
{
	return p_m;
}

std::string Schwefel::get_name() {
	return "Schwefell";
}