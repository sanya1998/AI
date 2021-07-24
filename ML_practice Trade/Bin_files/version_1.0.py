from datetime import datetime
from forWorkWithBinFiles import *
from OtherFunctions import *
import numpy as np

timeframes = [1, 30, 60]

for timeframe in timeframes:
	f = open('EURUSD/' + str(timeframe) + '.txt', 'r')
	bars = int(f.readline())
	all_bars = []
	for line in f:
		arr = line.split(";")
		ti = datetime.strptime(arr[0], '%Y.%m.%d %H:%M:%S')
		op = float(arr[1])
		hi = float(arr[2])
		lo = float(arr[3])
		cl = float(arr[4])
		vo = int(arr[5])
		all_bars.append([ti, op, hi, lo, cl, vo])
	f.close()
	all_bars = np.array(all_bars)
	
	write_bin_file(all_bars, 'all_bars_M' + str(timeframe) + '.pickle')
	all_bars = read_bin_file('all_bars_M' + str(timeframe) + '.pickle')
	
	data = []
	targetsHigh = []
	targetsLow = []
	targetsClose = []
	for i, b in enumerate(all_bars[:len(all_bars)-1], 1):
		data.append([int(b[0].month), int(b[0].weekday()), int(b[0].hour), b[1], b[2], b[3], b[4], int(b[5])])
		targetsHigh.append(all_bars[i][2]) # High следующего
		targetsLow.append(all_bars[i][3]) # Low следующего
		targetsClose.append(all_bars[i][4]) # Close следующего
	
	write_bin_file(np.array(data), 'data_M' + str(timeframe) + '.pickle')
	write_bin_file(np.array(targetsHigh), 'targets_M' + str(timeframe) + '_High.pickle')
	write_bin_file(np.array(targetsLow), 'targets_M' + str(timeframe) + '_Low.pickle')
	write_bin_file(np.array(targetsClose), 'targets_M' + str(timeframe) + '_Close.pickle')

# Временно
tf_ind = 2

data = read_bin_file('data_M' + str(timeframes[tf_ind]) + '.pickle')
targetsHigh = read_bin_file('targets_M' + str(timeframes[tf_ind]) + '_High.pickle')
targetsLow = read_bin_file('targets_M' + str(timeframes[tf_ind]) + '_Low.pickle')
targetsClose = read_bin_file('targets_M' + str(timeframes[tf_ind]) + '_Close.pickle')

train_ind, valid_ind, test_ind = permutation_and_split(len(data))

data_train = data[train_ind]
targetsClose_train = targetsClose[train_ind]
targetsHigh_train = targetsHigh[train_ind]
targetsLow_train = targetsLow[train_ind]

data_valid = data[valid_ind]
targetsClose_valid = targetsClose[valid_ind]
targetsHigh_valid = targetsHigh[valid_ind]
targetsLow_valid = targetsLow[valid_ind]

data_test = data[test_ind]
targetsClose_test = targetsClose[test_ind]
targetsHigh_test = targetsHigh[test_ind]
targetsLow_test = targetsLow[test_ind]

from sklearn.linear_model import LinearRegression

regressClose = LinearRegression()
regressHigh = LinearRegression()
regressLow = LinearRegression()

regressClose.fit(data_train, targetsClose_train)
regressHigh.fit(data_train, targetsHigh_train)
regressLow.fit(data_train, targetsLow_train)

write_bin_file(regressClose, 'regress_M' + str(timeframes[tf_ind]) + '_Close.pickle')
write_bin_file(regressHigh, 'regress_M' + str(timeframes[tf_ind]) + '_High.pickle')
write_bin_file(regressLow, 'regress_M' + str(timeframes[tf_ind]) + '_Low.pickle')

regressClose = read_bin_file('regress_M' + str(timeframes[tf_ind]) + '_Close.pickle')
regressHigh = read_bin_file('regress_M' + str(timeframes[tf_ind]) + '_High.pickle')
regressLow = read_bin_file('regress_M' + str(timeframes[tf_ind]) + '_Low.pickle')

predictionClose = np.round(regressClose.predict(data_test), 5)
predictionHigh = np.round(regressHigh.predict(data_test), 5)
predictionLow = np.round(regressLow.predict(data_test), 5)

sum = 0.0
plus = minus = 0
for i in range(len(test_ind)):
	"""
	print("   ", "предыдущ|", "предск", "реальный")
	print("op:", format(data_test[i][3], '.5f'), "|", format(data_test[i][6], '.5f'), format(data_test[i][6], '.5f'))
	print("hi:", format(data_test[i][4], '.5f'), "|",format(predictionHigh[i], '.5f'), format(targetsHigh_test[i], '.5f'))
	print("lo:", format(data_test[i][5], '.5f'), "|",format(predictionLow[i],'.5f'), format(targetsLow_test[i], '.5f'))
	print("cl:", format(data_test[i][6], '.5f'), "|",format(predictionClose[i], '.5f'), format(targetsClose_test[i], '.5f'))
	print()"""
	
	if predictionClose[i] > data_test[i][6] + 0.0001:
		slag = targetsClose_test[i] - data_test[i][6] - 0.0001
		sum += slag
		if slag > 0:
			plus += 1
		else:
			minus += 1
	elif predictionClose[i] + 0.0001 < data_test[i][6]:
		slag = data_test[i][6] - targetsClose_test[i] - 0.0001
		sum += slag
		if slag > 0:
			plus += 1
		else:
			minus += 1
print("ПП", int(sum * 10 ** 5), " +", plus, " -", minus, " ИЗ", len(test_ind))

"""pred = regress.predict([[8, 2, 12, 1.10971, 1.10988, 1.10935, 1.10943, 1327]])
print(pred)"""
