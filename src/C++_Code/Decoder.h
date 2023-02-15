#pragma once
#include <bitset>
#include "FunctionPoint.h"
#include "Arithmetics.h"
#include <random>
#include <chrono>

class BitmaskDecoder {
	std::pair<double, double> domain;
	double epsilon;
	int dimension;
	int one_dim_length;
	int bit_count;
public:
	BitmaskDecoder(const std::pair<double, double>& _domain, double _epsilon, int _dimension) : domain(_domain), epsilon(_epsilon), dimension(_dimension) {
		double dim_length = (domain.first - domain.second) / epsilon;
		one_dim_length = logarithm((int)dim_length);
		bit_count = one_dim_length * dimension;
	};
	std::vector<bool> generate_random_mask() const {
		std::vector<bool> mask;
		for (int i = 0; i < bit_count; ++i) {
			std::mt19937 rng(std::chrono::steady_clock::now().time_since_epoch().count());
			mask.push_back(std::uniform_int_distribution<int>(0, 1)(rng));
		}
		return mask;
	}
	std::vector<bool> gray_2_bin(const std::vector<bool>& gray) {
		std::vector<bool> ans;
		ans.push_back(gray[0]);
		for (int i = 1; i < (int)gray.size(); ++i) {
			int x = ans.back(), y = gray[i];
			ans.push_back((x + y) % 2);
		}
		return ans;
	}
	FunctionPoint decode_bit_mask(const std::vector<bool> &mask) const {
		FunctionPoint decoding_point(dimension);
		for (int i = 0; i < dimension; ++i) {
			long long coordinate_int = 0;
			for (int j = 0; j < one_dim_length; ++j) {
				coordinate_int <<= 1;
				coordinate_int += mask[i * one_dim_length + j];
			}
			double coordinate = coordinate_int;
			coordinate = coordinate / (fast_exponentation(2LL, one_dim_length) - 1) * (domain.second - domain.first) + domain.first;
			decoding_point[i] = coordinate;
		}
		return decoding_point;
	}
	FunctionPoint decode_gray_mask(const std::vector<bool>& mask) {
		FunctionPoint decoding_point(dimension);
		for (int i = 0; i < dimension; ++i) {
			long long coordinate_int = 0;
			std::vector<bool> gray_submask;
			for (int j = 0; j < one_dim_length; ++j) {
				gray_submask.push_back(mask[i * one_dim_length + j]);
			}
			std::vector<bool> bin_submask = gray_2_bin(gray_submask);
			for (auto it : bin_submask) {
				coordinate_int <<= 1;
				coordinate_int += it;
			}
			double coordinate = coordinate_int;
			coordinate = coordinate / (fast_exponentation(2LL, one_dim_length) - 1) * (domain.second - domain.first) + domain.first;
			decoding_point[i] = coordinate;
		}
		return decoding_point;
	}
	inline int get_bit_count() const {
		return bit_count;
	}
};