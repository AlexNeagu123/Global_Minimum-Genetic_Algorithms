#pragma once
#include<iostream>
#include<vector>
#include<iomanip>
#include<random>
#include<assert.h>

class FunctionPoint {
	std::vector<double> point;
	int dimension;
public:
	FunctionPoint(int _dimension, double domain_minimum, double domain_maximum) : dimension(_dimension) {
		// constructs a random point
		std::random_device rd;
		std::mt19937 gen(rd());
		std::uniform_real_distribution<> dis(domain_minimum, domain_maximum);
		point.resize(dimension);
		for (int i = 0; i < dimension; ++i) {
			point[i] = dis(gen);
		}
	}
	FunctionPoint(int _dimension, double domain_minimum) : dimension(_dimension) {
		point.assign(dimension, domain_minimum);
	}
	explicit FunctionPoint(int _dimension) : dimension(_dimension) {
		point.assign(dimension, 0);
	}
	FunctionPoint(const FunctionPoint& copy_obj) : dimension(copy_obj.dimension) {
		point.resize(dimension);
		for (int i = 0; i < dimension; ++i) {
			point[i] = copy_obj.point[i];
		}
	}
	double& operator [] (int index) {
		assert(index >= 0 && index < dimension);
		return point[index];
	}
	FunctionPoint& operator = (const FunctionPoint& him) {
		for (int i = 0; i < dimension; ++i) {
			point[i] = him.point[i];
		}
		return *(this);
	}
	inline int get_dimension() const {
		return dimension;
	}
	void print() const {
		std::cout << dimension << std::endl;
		std::cout << "{ ";
		for (auto it : point) {
			std::cout << std::fixed << std::setprecision(15) << it << ' ';
		}
		std::cout << "}";
		std::cout << std::endl;
	}
};