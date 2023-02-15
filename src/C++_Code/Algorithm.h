#pragma once
#pragma warning(disable : 4996)
#include <algorithm>
#include "FunctionPoint.h"
#include "GenericFunction.h"
#include "Decoder.h"
#include "Arithmetics.h"
#include <set>
namespace genetic_algorithm {
	std::mt19937 rng(std::chrono::steady_clock::now().time_since_epoch().count());
	std::random_device rd;
	std::mt19937 gen(rd());
	std::uniform_real_distribution<> dis(0, 1);
	void print_mask(const std::vector<bool>& mask) {
		std::cout << mask.size() << std::endl;
		for (auto bit : mask)
			std::cout << bit;
		std::cout << std::endl;
	}
	void mutate_population(GenericFunction* function, std::vector<std::vector<bool>>& population, bool adaptive) {
		for (int i = 0; i < (int)population.size(); ++i) {
			for (int j = 0; j < (int)population[i].size(); ++j) {
				double random_val = dis(gen);
				double mutation_prob = my_constants::mut_prob;
				if (adaptive == 1)
					mutation_prob = function->p_m[i];
				if (random_val < mutation_prob) {
					population[i][j] = !population[i][j];
				}
			}
		}
	}
	std::pair<std::vector<bool>, std::vector<bool>> crossover(const std::vector<bool>& parent_1, const std::vector<bool>& parent_2) {
		int cut_point = 1 + std::uniform_int_distribution<int>(0, (int)parent_1.size() - 3)(rng);
		std::vector<bool> child_1 = parent_1;
		std::vector<bool> child_2 = parent_2;
		for (int i = cut_point; i < (int)parent_1.size(); ++i) {
			child_1[i] = parent_2[i];
			child_2[i] = parent_1[i];
		}
		return std::make_pair(child_1, child_2);
	}
	void cross_population(std::vector<std::vector<bool>>& population) {
		std::vector<std::pair<double, int>> mapping;
		for (int i = 0; i < (int)population.size(); ++i) {
			mapping.push_back(std::make_pair(dis(gen), i));
		}
		std::sort(mapping.begin(), mapping.end());
		for (int i = 0; i < (int)mapping.size() - 1; i += 2) {
			if (mapping[i].first >= my_constants::cross_prob) {
				break;
			}
			int ind1 = mapping[i].second;
			int ind2 = mapping[i + 1].second;
			double random_val = dis(gen);
			if (mapping[i + 1].first < my_constants::cross_prob || random_val < 0.5) {
				std::pair<std::vector<bool>, std::vector<bool>> crossed = crossover(population[ind1], population[ind2]);
				population.push_back(crossed.first);
				population.push_back(crossed.second);
			}
		}
	}

	std::vector<std::vector<bool>> select(const std::vector<std::vector<bool>> &population, GenericFunction *function, bool gray, bool adaptive) {
		std::vector<std::pair<double,int>> values;
		for (int i = 0; i < population.size(); ++i) {
			if(!gray)
				values.push_back({function->eval_mask(population[i]), i});
			else 
				values.push_back({function->eval_gray_mask(population[i]), i});
		}
		sort(values.begin(), values.end());
		double max_value = values.back().first;
		double min_value = values[0].first;
		std::vector<double> norm_values;
		double sum_norm = 0;
		for (int i = 0; i < (int) values.size(); ++i) {
			double norm_value = (max_value - values[i].first) / (max_value - min_value + 0.00001) + 0.01;
			norm_values.push_back(norm_value);
			sum_norm += norm_value;
		}
		double max_fitness = -1, mean_fitness = 0;
		for (int i = 0; i < norm_values.size(); ++i) {
			max_fitness = std::max(max_fitness, norm_values[i]);
			mean_fitness += norm_values[i];
		}
		mean_fitness /= norm_values.size();

		std::vector<double> p;
		for (int i = 0; i < (int) norm_values.size(); ++i) {
			p.push_back(norm_values[i] / sum_norm);
		}

		std::vector<double> q;
		double accumulated_sum = 0;
		for (int i = 0; i < (int) p.size(); ++i) {
			accumulated_sum += p[i];
			q.push_back(accumulated_sum);
		}
		q.back() = 1.0;
		std::vector<std::vector<bool>> next_population;
		for (int i = 0; i < 5; ++i) {
			int index = values[i].second;
			next_population.push_back(population[index]);
			if (adaptive) {
				if (norm_values[i] < mean_fitness) {
					function->p_m[i] = 0.1;
				}
				else {
					function->p_m[i] = 0.0058 * (max_fitness - norm_values[i]) / (max_fitness - mean_fitness);
					function->p_m[i] = std::max(function->p_m[i], 0.0008);
				}
			}
		}
		for (int step = 0; next_population.size() < my_constants::pop_size; ++step) {
			double random_value = dis(gen);
			int new_index = next_population.size();
			for (int i = 0; i < (int) q.size(); ++i) {
				if (random_value < q[i]) {
					int index = values[i].second;
					next_population.push_back(population[index]);
					if (adaptive) {
						if (norm_values[i] < mean_fitness) {
							function->p_m[new_index] = 0.1;
						}
						else {
							function->p_m[new_index] = 0.0058 * (max_fitness - norm_values[i]) / (max_fitness - mean_fitness);
							function->p_m[new_index] = std::max(function->p_m[new_index], 0.0008);
						}
					}
					break;
				}
			}
		}
		return next_population;
	}
	double compute_best_individual(GenericFunction *function, bool gray, bool adaptive) {
		// initialize population
		std::vector<std::vector<bool>> population;
		for (int i = 0; i < my_constants::pop_size; ++i) {
			population.push_back(function->generate_mask());
		}
		double answer = 2e9;
		for (int step = 0; step < my_constants::generation_nr; ++step) {
			population = select(population, function, gray, adaptive);
			mutate_population(function, population, adaptive);
			cross_population(population);
		}
		population = select(population, function, gray, adaptive);
		for (int i = 0; i < my_constants::pop_size; ++i) {
			if (!gray)
				answer = std::min(answer, function->eval_mask(population[i]));
			else
				answer = std::min(answer, function->eval_gray_mask(population[i]));
		}
		return answer;
	}
}