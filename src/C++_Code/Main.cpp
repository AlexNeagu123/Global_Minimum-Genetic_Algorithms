#include <iostream>
#include <fstream>
#include "GenericFunction.h"
#include "Rastrigin.h"
#include "Michalewiczs.h"
#include "DeJong.h"
#include "Schwefel.h"
#include "Algorithm.h"

std::ofstream out("optimal_results_el6.txt");

int main() {
	for (int dim : {30, 10, 5}) {
		GenericFunction* functions[4];
		functions[0] = new Rastrigin(std::make_pair(-5.12, 5.12), dim, 0.0000001);
		functions[1] = new Michalewiczs(std::make_pair(0, my_constants::PI), dim, 0.0000001);
		functions[2] = new DeJong(std::make_pair(-5.12, 5.12), dim, 0.0000001);
		functions[3] = new Schwefel(std::make_pair(-500, 500), dim, 0.0000001);
		for (int fun = 0; fun < 4; ++fun) {
			if (fun != 3)
				continue;
			my_constants::cross_prob = 0.87;
			my_constants::mut_prob = 0.001;
			out << "Func " << functions[fun]->get_name() << std::endl;
			out << "Dim " << dim << std::endl;
			out << "Pc " << my_constants::cross_prob << std::endl;
			out << "Pm  ?" << std::endl;
			for (int step = 0; step < 50; step++) {
				auto start = std::chrono::system_clock::now();
				double cur_ans = genetic_algorithm::compute_best_individual(functions[fun], 0, 0);
				auto end = std::chrono::system_clock::now();
				std::chrono::duration<double> elapsed_seconds = end - start;
				out << "Ans " << cur_ans << std::endl;
				out << "Elapsed time " << elapsed_seconds.count() << std::endl;
			}
		}
	}
}