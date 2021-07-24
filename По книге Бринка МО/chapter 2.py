# Data Processing
import numpy as np


# Листинг 2.1.П реобразование категориальных признаков в чисенные двоичные признаки (cnh 73)
# Возвращает n бинарных массивов. n - кол-во уникальных категорий. 1 - принадлежит, 0 - нет
def cat_to_num(data):
	# unique - Возвращает отсортированные уникальные элементы массива
	categories = np.unique(data)
	features = []
	for cat in categories:
		# (data == cat) возвращает список булевских значений
		binary = (data == cat)
		features.append(binary.astype("int"))
	return features


cat_data = np.array(['male', 'female', 'male', 'male', 'female', 'male', 'female', 'female'])
print(cat_to_num(cat_data))


# Лисинг 2.2. Просое иэвечение признаков иэ данных о каютах на «Титанике» (стр 79)
def cabin_features(data):
	features = []
	for cabin in data:
		cabins = cabin.split(" ")
		n_cabins = len(cabins)
		# First char in the cabin_char (Первая л итера и з обозначения каюты)
		try:
			cabin_char = cabins[0][0]
		except IndexError:
			cabin_char = "X"
			n_cabins = 0
		# Остальные символы - номер каюты
		try:
			cabin_num = int(cabins[0][1:])
		except:
			cabin_num = -1
		# Добавление 3 признака для каждого пассажира
		features.append([cabin_char, cabin_num, n_cabins])
	return features


cabin_data = np.array(["C65", "", "E36", "C54", "B57 B59 B63 B66"])
print(cabin_features(cabin_data))


# Лисинг 2.3. Нормализация признака (стр 80)
def normalize_feature(data, f_min=-1, f_max=1):
	factor = (f_max - f_min) / (max(data) - min(data))
	normalized = f_min + data*factor
	return normalized, factor


num_data = np.array([1, 10, 0.5, 43, 0.12, 8])
print(normalize_feature(num_data))