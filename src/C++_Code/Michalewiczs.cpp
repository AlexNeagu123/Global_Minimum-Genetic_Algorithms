#include "Michalewiczs.h"

double Michalewiczs::compute_value(FunctionPoint& current_point) {
	int m = 10;
	double s = 0.0;
	for (int i = 0; i < current_point.get_dimension(); ++i) {
		s = s + sin(current_point[i]) * fast_exponentation(
			sin((i + 1) * current_point[i] * current_point[i] / my_constants::PI),
			(2 * m)
		);
	}
	return -s;
}

std::pair<double, double> Michalewiczs::get_domain() const {
	return domain;
}

FunctionPoint Michalewiczs::decode_mask(const std::vector<bool>& mask) {
	return decoder.decode_bit_mask(mask);
}

double Michalewiczs::eval_mask(const std::vector<bool>& mask) {
	FunctionPoint decoded = decode_mask(mask);
	return compute_value(decoded);
}

double Michalewiczs::eval_gray_mask(const std::vector<bool>& mask) {
	FunctionPoint decoded = decoder.decode_gray_mask(mask);
	return compute_value(decoded);
}

int Michalewiczs::get_dimension() const {
	return dimension;
}

double Michalewiczs::get_epsilon() const {
	return epsilon;
}

std::vector<bool> Michalewiczs::generate_mask() {
	return decoder.generate_random_mask();
}

int Michalewiczs::get_bit_size() {
	return decoder.get_bit_count();
}

std::vector<double> Michalewiczs::get_pm()
{
	return p_m;
}

std::vector<double> Michalewiczs::get_pc()
{
	return p_c;
}

std::string Michalewiczs::get_name() {
	return "Michalewiczs";
}