from sklearn.datasets import load_digits
from random import randint
import numpy as np



class Node:
	def __init__(self, data, targets, parent = None):
		self.data = np.array(data) # Пришедшие X
		self.targets = np.array(targets) # Пришедшие targets
		self.parent = parent
		if parent is None:
			self.depth = 1
		else:
			self.depth = parent.depth + 1
		self.right = None
		self.left = None
		self.split_ind = None
		self.split_val = None
		self.entropy_val = None
	
	def calculate_t_normaliz(self):
		counts_target = np.zeros(len(target_names))
		self.t_normaliz = np.zeros(len(target_names))
		# Считаем сколько элементов каждого класса пришло
		for i in self.targets:
			counts_target[int(i)] += 1
		# Считаем нормализованный вектор t
		for i in np.arange(len(target_names)):
			self.t_normaliz[i] = counts_target[i]/len(self.targets)


class DecisionTree:
	def __init__(self, _root, _max_depth=10, _min_entropy=0.5, _min_elements=10, _max_feature=2):
		self._max_depth = _max_depth
		self._min_entropy = _min_entropy
		self._min_elements = _min_elements
		self._max_feature = _max_feature
		self.root = _root
		self.root.depth = 1


def calculate_entropy(targets):
	# Считаем сколько элементов каждого класса пришло
	counts_target = np.zeros(len(target_names))
	for i in targets:
		counts_target[int(i)] += 1
	# Считаем энтропию
	entropy = 0.0
	for i in counts_target:
		if not len(targets) == 0:
			p = i/len(targets)
			if not p == 0:
				entropy += p*np.log2(p)
	return -entropy


# Для бинарного дерева - два разделения
def calculate_I(s_i_t, s_i_0, s_i_1):
	answer = calculate_entropy(s_i_t)
	if not len(s_i_t) == 0:
		answer -= len(s_i_0)*calculate_entropy(s_i_0)/len(s_i_t)
		answer -= len(s_i_1)*calculate_entropy(s_i_1)/len(s_i_t)
	return answer


def data_separation(node):
	I_best = 10**(-10)
	first = True
	node.data = node.data[1:]
	# Прогоним Information Gain несколько раз
	for attempt in np.arange(5):
		# Берем рандомное число - номер координаты
		coord = randint(0, len(node.data[0])-1)
		# Верхнюю или нижнюю границу берем
		ind = randint(0, 1)
		tay = randint(0, np.max(node.data))
		s_i_0_t = np.array([])
		s_i_1_t = np.array([])
		s_i_0_x = np.zeros(shape=(1,len(node.data[0])))
		s_i_1_x = np.zeros(shape=(1,len(node.data[0])))
		if ind == 0:
			for i in np.arange(len(node.data)):
				if node.data[i][coord] > tay:
					s_i_0_x = np.r_[s_i_0_x, [node.data[i]]]
					s_i_0_t = np.append(s_i_0_t, node.targets[i])
				else:
					s_i_1_x = np.r_[s_i_1_x, [node.data[i]]]
					s_i_1_t = np.append(s_i_1_t, node.targets[i])
		else:
			for i in np.arange(len(node.data)):
				
				if node.data[i][coord] < tay:
					s_i_0_x = np.r_[s_i_0_x, [node.data[i]]]
					s_i_0_t = np.append(s_i_0_t, node.targets[i])
				else:
					s_i_1_x = np.r_[s_i_1_x, [node.data[i]]]
					s_i_1_t = np.append(s_i_1_t, node.targets[i])
		if first:
			s_i_0_x = s_i_0_x[1:]
			s_i_1_x = s_i_0_x[1:]
			first = False
		
		I = calculate_I(node.targets, s_i_0_t, s_i_1_t)
		if I > I_best:
			I_best = I
			node.split_ind = ind
			node.split_val = tay
			node.entropy_val = calculate_entropy(node.targets)
			node.left = Node(s_i_0_x, s_i_0_t, node)
			# node.left.calculate_t_normaliz()
			node.right = Node(s_i_1_x, s_i_1_t, node)
			# node.right.calculate_t_normaliz()
	
	# Пока что только по глубине
	# Энтропия тоже считается, но не придумал граничные условия для нее
	if node.right.depth < 3:
		node.right = data_separation(node.right)
	if node.left.depth < 3:
		node.left = data_separation(node.left)
	
	return node
	
	
digits = load_digits()
print(type(digits))
X = np.array(digits.data)
T = np.array(digits.target)
target_names = digits.target_names

# Перемешиваем массив индексов
indexes_prm = np.random.permutation(np.arange(len(X)))

X = X[indexes_prm]
T = T[indexes_prm]
# Делим выборку на обучающую и валидационную
part_train_end = part_valid_begin = np.int32(0.85 * len(indexes_prm))

train_ind = indexes_prm[:part_train_end]
valid_ind = indexes_prm[part_valid_begin:]

X_train = X[train_ind]
T_train = T[train_ind]
X_valid = X[valid_ind]
T_valid = T[valid_ind]

# Количество деревьев
amount_trees = 3
trees = np.array([])
for i in np.arange(amount_trees):
	root = Node(X_train, T_train)
	tree = DecisionTree(root)
	root = data_separation(root)
	trees = np.append(trees, tree)
	
#print(trees[0].root.right.t_normaliz)



