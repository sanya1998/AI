import numpy as np


def permutation_and_split(length=0, border_left=0.7, border_right=0.85):
	# Создаем массив индексов
	ind = np.arange(length)
	
	# Перемешиваем массив индексов
	ind_prm = np.random.permutation(ind)
	
	part_train_right_border = part_valid_left_border = np.int32(border_left * len(ind_prm))
	part_valid_right_border = part_test_left_border = np.int32(border_right * len(ind_prm))
	
	train_ind = ind_prm[:part_train_right_border]
	valid_ind = ind_prm[part_valid_left_border:part_valid_right_border]
	test_ind = ind_prm[part_test_left_border:]
	
	return train_ind, valid_ind, test_ind


def what_class(prev_f, next_f, spread_f):
	if next_f > prev_f + 3*spread_f:
		return 3
	elif next_f > prev_f + 2*spread_f:
		return 2
	elif next_f > prev_f + spread_f:
		return 1
	elif next_f < prev_f - spread_f:
		return -1
	elif next_f < prev_f - 2*spread_f:
		return -2
	elif next_f < prev_f - 3*spread_f:
		return -3
	else:
		return 0