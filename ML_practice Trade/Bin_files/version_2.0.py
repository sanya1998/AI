from datetime import datetime
from forWorkWithBinFiles import *
from OtherFunctions import *
import numpy as np

# NEW: Classificater
timeframes = [1, 30, 60]
spread = 0.0001

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
	for i, b in enumerate(all_bars[:len(all_bars) - 1], 1):
		data.append([int(b[0].month), int(b[0].weekday()), int(b[0].hour), b[1], b[2], b[3], b[4], int(b[5])])
		
		# Специально от цены закрытия
		targetsHigh.append(what_class(b[4], all_bars[i][2], spread))  # High следующего
		targetsLow.append(what_class(b[4], all_bars[i][3], spread))  # Low следующего
		targetsClose.append(what_class(b[4], all_bars[i][4], spread))  # Close следующего
	
	write_bin_file(np.array(data), 'data_M' + str(timeframe) + '.pickle')
	write_bin_file(np.array(targetsHigh), 'targets_M' + str(timeframe) + '_High.pickle')
	write_bin_file(np.array(targetsLow), 'targets_M' + str(timeframe) + '_Low.pickle')
	write_bin_file(np.array(targetsClose), 'targets_M' + str(timeframe) + '_Close.pickle')

# Временно
tf_ind = 1

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

from sklearn.ensemble import RandomForestClassifier

classifClose = RandomForestClassifier()
classifHigh = RandomForestClassifier()
classifLow = RandomForestClassifier()

classifClose.fit(data_train, targetsClose_train)
classifHigh.fit(data_train, targetsHigh_train)
classifLow.fit(data_train, targetsLow_train)

write_bin_file(classifClose, 'regress_M' + str(timeframes[tf_ind]) + '_Close.pickle')
write_bin_file(classifHigh, 'regress_M' + str(timeframes[tf_ind]) + '_High.pickle')
write_bin_file(classifLow, 'regress_M' + str(timeframes[tf_ind]) + '_Low.pickle')

classifClose = read_bin_file('regress_M' + str(timeframes[tf_ind]) + '_Close.pickle')
classifHigh = read_bin_file('regress_M' + str(timeframes[tf_ind]) + '_High.pickle')
classifLow = read_bin_file('regress_M' + str(timeframes[tf_ind]) + '_Low.pickle')

predictionClose = np.round(classifClose.predict(data_test), 5)
predictionHigh = np.round(classifHigh.predict(data_test), 5)
predictionLow = np.round(classifLow.predict(data_test), 5)


for i in range(len(test_ind)):
	
	print("   ", "предыдущ|", "предск", "реальный")
	print("op:", format(data_test[i][3], '.5f'), "|", format(data_test[i][6], '.5f'), format(data_test[i][6], '.5f'))
	print("hi:", format(data_test[i][4], '.5f'), "|",format(predictionHigh[i], '.5f'), format(targetsHigh_test[i], '.5f'))
	print("lo:", format(data_test[i][5], '.5f'), "|",format(predictionLow[i],'.5f'), format(targetsLow_test[i], '.5f'))
	print("cl:", format(data_test[i][6], '.5f'), "|",format(predictionClose[i], '.5f'), format(targetsClose_test[i], '.5f'))
	print()

#2019.08.26 19:30:00;1.11133;1.11152;1.11076;1.11093;2023
#2019.08.26 19:00:00;1.11099;1.11146;1.11087;1.11132;2267
pred1 = classifClose.predict([[8, 0, 19, 1.11099, 1.11146, 1.11087, 1.11132, 2267]])
pred2 = classifHigh.predict([[8, 0, 19, 1.11099, 1.11146, 1.11087, 1.11132, 2267]])
pred3 = classifLow.predict([[8, 0, 19, 1.11099, 1.11146, 1.11087, 1.11132, 2267]])
print(pred1, pred2, pred3)