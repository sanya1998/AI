import numpy as np
import matplotlib.pyplot as plt


def calculate_tp_tn_fp_fn_threshold(t_f, c0_f, c1_f):
	tp_tn_fp_fn = {'FP': 0, 'TN': 0, 'TP': 0, 'FN': 0}
	for i in c0_f:
		if i > t_f:
			tp_tn_fp_fn['FP'] += 1
		else:
			tp_tn_fp_fn['TN'] += 1
	for i in c1_f:
		if i > t_f:
			tp_tn_fp_fn['TP'] += 1
		else:
			tp_tn_fp_fn['FN'] += 1
	return tp_tn_fp_fn


def calculate_tp_tn_fp_fn_coin(honesty_f, len_c0, len_c1):
	tp_tn_fp_fn = {'FP': 0, 'TN': 0, 'TP': 0, 'FN': 0}
	# Массив монет: если 1, значит это баскетболист
	
	# Здесь мы рассматриваем футболистов
	array_coins = np.random.binomial(1, honesty_f, len_c0)
	for i in array_coins:
		if i == 1:
			tp_tn_fp_fn['FP'] += 1
		else:
			tp_tn_fp_fn['TN'] += 1
	
	# Здесь мы рассматриваем баскетболистов
	array_coins = np.random.binomial(1, honesty_f, len_c1)
	for i in array_coins:
		if i == 1:
			tp_tn_fp_fn['TP'] += 1
		else:
			tp_tn_fp_fn['FN'] += 1
	return tp_tn_fp_fn


def calculate_accuracy(metrics):
	try:
		return (metrics['TP'] + metrics['TN']) / (metrics['TP'] + metrics['TN'] + metrics['FP'] + metrics['FN'])
	except ZeroDivisionError:
		return 0

	
def calculate_precision(tp, fp):
	try:
		return tp / (tp + fp)
	except ZeroDivisionError:
		return 0


def calculate_recall(tp, fn):
	try:
		return tp / (tp + fn)
	except ZeroDivisionError:
		return 0

		
def calculate_alpha(fp, tn):
	try:
		return fp / (tn + fp)
	except ZeroDivisionError:
		return 0
	

def calculate_beta(fn, tp):
	try:
		return fn / (tp + fn)
	except ZeroDivisionError:
		return 0


def calculate_f1_score(recall, precision):
	try:
		return 2 * (recall * precision) / (recall + precision)
	except ZeroDivisionError:
		return 0


def calculate_square(x, y):
	# Площадь под кривой, составленных из точек x[..] и y[..]
	# Функция станет универсальной, если перед подсчетом площади она будет сортировать значения по горизонтальной оси
	
	# Получаем индексы отсортированного x
	x_ind_sort = np.argsort(x)
	x = x[x_ind_sort]
	y = y[x_ind_sort]
	s = 0.0
	for i in range(1, len(x)):
		h = x[i] - x[i - 1]
		s += h * (y[i] + y[i - 1]) / 2
	return s


# Поехали:
N_football_players = 1000
N_basketball_players = 1000

scale_football_players = 15
scale_basketball_players = 10

mean_football_players = 170
mean_basketball_players = 190

c0 = np.random.randn(N_basketball_players) * scale_football_players + mean_football_players
c1 = np.random.randn(N_basketball_players) * scale_basketball_players + mean_basketball_players
"""
c0 = np.random.normal(mean_football_players, scale_football_players, N_football_players)
c1 = np.random.normal(mean_basketball_players, scale_basketball_players, N_basketball_players)
"""

#
# КЛАССИФИКАТОР НА ОСНОВЕ ПОРОГА
#
best_Accuracy = 0
best_t = 0
roc_FPrate = roc_TPrate = np.array([])
prc_Precis = prc_Recall = np.array([])

t_min = 0
t_max = 250
# Такой range(..), чтобы упростить сортировку по горизонтальным осям перед построением графиков
for t in range(t_max, t_min-1, -1):
	TP_TN_FP_FN = calculate_tp_tn_fp_fn_threshold(t, c0, c1)

	Accuracy = calculate_accuracy(TP_TN_FP_FN)
	if best_Accuracy < Accuracy:
		best_Accuracy = Accuracy
		best_t = t
	
	alpha = calculate_alpha(TP_TN_FP_FN['FP'], TP_TN_FP_FN['TN'])
	Recall = calculate_recall(TP_TN_FP_FN['TP'], TP_TN_FP_FN['FN'])
	
	roc_FPrate = np.append(roc_FPrate, alpha)
	roc_TPrate = np.append(roc_TPrate, Recall)
	
	Precision = calculate_precision(TP_TN_FP_FN['TP'], TP_TN_FP_FN['FP'])
	
	prc_Recall = np.append(prc_Recall, Recall)
	prc_Precis = np.append(prc_Precis, Precision)
	

# Готовим графики
plt.subplot(3, 2, 1)
plt.title("(Порог) ROC")
plt.xlabel("FPrate")
plt.ylabel("TPrate")
plt.plot(roc_FPrate, roc_TPrate, '-b')

plt.subplot(3, 2, 2)
plt.title("(Порог) PRC")
plt.xlabel("Recall")
plt.ylabel("Precision")
plt.plot(prc_Recall, prc_Precis, '-b')

S_roc_threshold = calculate_square(roc_FPrate, roc_TPrate)
S_prc_threshold = calculate_square(prc_Recall, prc_Precis)

print("КЛАССИФИКАТОР НА ОСНОВЕ ПОРОГА")
print("S под ROC = " + str(S_roc_threshold))
print("S под PRC = " + str(S_prc_threshold))
print("Best Accuracy = " + str(best_Accuracy), "при threshold = " + str(best_t))

#
# МОНЕТЫ
#
best_Accuracy_coin = 0
best_honesty_coin = 0
roc_FPrate_coin = roc_TPrate_coin = np.array([])
prc_Precis_coin = prc_Recall_coin = np.array([])

# Шаг честности монеты = 0.01
honesty_min_coin = 0
honesty_max_coin = 1
for honesty in np.linspace(honesty_min_coin, honesty_max_coin, 250):
	TP_TN_FP_FN = calculate_tp_tn_fp_fn_coin(honesty, N_football_players, N_basketball_players)
	
	Accuracy = calculate_accuracy(TP_TN_FP_FN)
	if best_Accuracy_coin < Accuracy:
		best_Accuracy_coin = Accuracy
		best_honesty_coin = honesty
	
	alpha = calculate_alpha(TP_TN_FP_FN['FP'], TP_TN_FP_FN['TN'])
	Recall = calculate_recall(TP_TN_FP_FN['TP'], TP_TN_FP_FN['FN'])
	
	roc_FPrate_coin = np.append(roc_FPrate_coin, alpha)
	roc_TPrate_coin = np.append(roc_TPrate_coin, Recall)
	
	Precision = calculate_precision(TP_TN_FP_FN['TP'], TP_TN_FP_FN['FP'])
	
	prc_Recall_coin = np.append(prc_Recall_coin, Recall)
	prc_Precis_coin = np.append(prc_Precis_coin, Precision)

# В случае с монетой данные для графиков имеет смысл отсортировать по горизонтальной оси
ind_sort_roc = np.argsort(roc_FPrate_coin)
roc_TPrate_coin = roc_TPrate_coin[ind_sort_roc]
roc_FPrate_coin = roc_FPrate_coin[ind_sort_roc]
ind_sort_prc = np.argsort(prc_Recall_coin)
prc_Precis_coin = prc_Precis_coin[ind_sort_prc]
prc_Recall_coin = prc_Recall_coin[ind_sort_prc]

# Готовим графики
plt.subplot(3, 2, 5)
plt.title("(Монета) ROC")
plt.xlabel("FPrate")
plt.ylabel("TPrate")
plt.plot(roc_FPrate_coin, roc_TPrate_coin, '-r')

plt.subplot(3, 2, 6)
plt.title("(Монета) PRC")
plt.xlabel("Recall")
plt.ylabel("Precision")
plt.plot(prc_Recall_coin, prc_Precis_coin, '-r')

S_roc_threshold_coin = calculate_square(roc_FPrate_coin, roc_TPrate_coin)
S_prc_threshold_coin = calculate_square(prc_Recall_coin, prc_Precis_coin)

print("МОНЕТА")
print("S под ROC = " + str(S_roc_threshold_coin))
print("S под PRC = " + str(S_prc_threshold_coin))
print("Best Accuracy = " + str(best_Accuracy_coin), "при honesty = " + str(round(best_honesty_coin, 2)))

plt.show()
