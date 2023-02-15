#include "DeJong.h"

double DeJong::compute_value(FunctionPoint& current_point) {
	double s = 0;
	for (int j = 0; j < current_point.get_dimension(); ++j)
		s = s + current_point[j] * current_point[j];
	return s;
}

std::pair<double, double> DeJong::get_domain() const {
	return domain;
}

FunctionPoint DeJong::decode_mask(const std::vector<bool>& mask) {
	return decoder.decode_bit_mask(mask);
}

double DeJong::eval_mask(const std::vector<bool>& mask) {
	FunctionPoint decoded = decode_mask(mask);
	return compute_value(decoded);
}

double DeJong::eval_gray_mask(const std::vector<bool>& mask) {
	FunctionPoint decoded = decoder.decode_gray_mask(mask);
	return compute_value(decoded);
}

int DeJong::get_dimension() const {
	return dimension;
}

double DeJong::get_epsilon() const {
	return epsilon;
}

std::vector<bool> DeJong::generate_mask() {
	return decoder.generate_random_mask();
}

int DeJong::get_bit_size() {
	return decoder.get_bit_count();
}

std::vector<double> DeJong::get_pc()
{
	return p_c;
}

std::vector<double> DeJong::get_pm()
{
	return p_m;
}

std::string DeJong::get_name() {
	return "DeJong";
}