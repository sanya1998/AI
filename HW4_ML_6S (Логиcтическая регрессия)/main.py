from sklearn.datasets import load_digits
import numpy as np
import matplotlib.pyplot as plt


def init_b(mu0, disp0, length):
	return np.random.normal(mu0, disp0, length)


def init_W(mu0, disp0, length):
	W = np.random.normal(mu0, disp0, length)
	for j in range(X_train.shape[1] - 1):  # По столбикам
		W = np.c_[W, np.random.normal(mu0, disp0, length)]
	return W


def soft_max(a):
	# Чтобы избежать переполнения, отнимаем максимальное a
	a = np.exp(a[:] - max(a))
	a = a[:] / sum(a)
	return a


def get_Y(X, W, b):
	Y = np.zeros((len(X), len(W)))
	for i in range(len(X)):
		a = np.dot(W, X[i]) + b
		y = soft_max(a)
		Y[i] = y
	return Y


def gradient_descent_w(W_0_f, T_f, X_f, b_f, gamma_f, epsilon_f, accuracy_array_f, error_array_f, text=""):
	Y_f = get_Y(X_f, W_0_f, b_f)
	
	grad_w_E = np.dot(np.transpose(Y_f - T_f), X_f)
	W_1 = W_0_f - gamma_f * grad_w_E

	i = 0 # Для вывода на консоль
	while np.linalg.norm(W_1 - W_0_f) > epsilon_f:
		W_0_f = W_1
		
		Y_f = get_Y(X_f, W_0_f, b_f)
		grad_w_E = np.dot(np.transpose(Y_f - T_f), X_f)
		W_1 = W_0_f - gamma_f * grad_w_E
		
		acc = get_accuracy(Y_f, T_f)
		accuracy_array_f = np.append(accuracy_array_f, acc)
		err = get_e(T_f, Y_f, W_1)
		error_array_f = np.append(error_array_f, err)
		i += 1
		if i % 10 == 0:
			print(text, "acc=", acc, "err=", err)

	return np.array([W_1, accuracy_array_f, error_array_f])


def gradient_descent_b(W_f, T_f, X_f, b_0_f, gamma_f, epsilon_f, accuracy_array_f, error_array_f, text=""):
	U = np.ones(len(X_f))
	
	Y_f = get_Y(X_f, W_f, b_0_f)
	
	grad_b_E = np.dot(np.transpose(Y_f - T_f), U)
	b_1 = b_0_f - gamma_f * grad_b_E
	
	i = 0 # Для вывода на консоль
	while np.linalg.norm(b_1 - b_0_f) > epsilon_f:
		b_0_f = b_1
		
		Y_f = get_Y(X_f, W_f, b_0_f)
		grad_b_E = np.dot(np.transpose(Y_f - T_f), U)
		b_1 = b_0_f - gamma_f * grad_b_E
		
		acc = get_accuracy(Y_f, T_f)
		accuracy_array_f = np.append(accuracy_array_f, acc)
		err = get_e(T_f, Y_f, W_f)
		error_array_f = np.append(error_array_f, err)
		
		i += 1
		if i % 5 == 0:
			print(text, "acc=", acc, "err=", err)
			
	return np.array([b_1, accuracy_array_f, error_array_f])


def get_e(T_f, Y_f, W_f):
	E_f = 0.0
	for i in range(T_f.shape[0]):
		for j in range(T_f.shape[1]):
			if Y_f[i, j] > 0.0:
				slag = T[i, j] * np.log(Y_f[i, j])
			else:
				slag = 0.0
			E_f -= slag
	# Для регуляризвции
	right_slag = 0.0
	for i in range(W_f.shape[0]):
		for j in range(W_f.shape[1]):
			right_slag += W_f[i, j] * W_f[i, j]
	lambda_reg = 0.0001
	right_slag *= lambda_reg/2
	
	E_f += right_slag
	return E_f


def get_accuracy(Y_f, T_f):
	tr = vsego = 0
	for i in range(len(Y_f)):
		if T_f[i][np.argmax(Y_f[i])] == 1:
			tr += 1
		vsego += 1
	return tr / vsego


def get_confusion_matrix(Y_f, T_f):
	confusion_matrix = np.zeros((T_f.shape[1], T_f.shape[1]))
	for i in range(len(Y_f)):
		confusion_matrix[np.argmax(T_f[i]), np.argmax(Y_f[i])] += 1
	return confusion_matrix


digits = load_digits()
X = np.array(digits.data)
target = np.array(digits.target)
target_names = digits.target_names

# Стандартизация
mu_array = X.sum(axis=0)[:] / len(X)  # По столбикам
sigma_array = np.array([])            # По столбикам
for j in range(X.shape[1]):
	sigma_array = np.append(sigma_array, np.std(X[:, j]))

chislitel_array = X[:] - mu_array
# Если знаменатель = 0, то ответ = числителю
X[:] = np.divide(chislitel_array, sigma_array, out=np.zeros_like(chislitel_array), where=sigma_array != 0)

# Перемешиваем массив индексов
indexes_prm = np.random.permutation(np.arange(len(X)))

X = X[indexes_prm]
target = target[indexes_prm]

# One-hot encoding
T = np.zeros(shape=(len(X), len(target_names)))
for i in range(len(T)):
	T[i][target[i]] = 1
	
# Делим выборку на обучающую и валидационную
part_train_end = part_valid_begin = np.int32(0.8 * len(indexes_prm))

train_ind = indexes_prm[:part_train_end]
valid_ind = indexes_prm[part_valid_begin:]

X_train = X[train_ind]
T_train = T[train_ind]
X_valid = X[valid_ind]
T_valid = T[valid_ind]

# Инициализация
mu = 0
disp = 0.1
b_0 = init_b(mu, disp, len(target_names))
W_0 = init_W(mu, disp, len(target_names))

# Градиентный спуск для обучающей выборки
accuracy_array_tr = np.array([])
error_array_tr = np.array([])

gamma = 0.1
epsilon = 1
answer = gradient_descent_w(W_0, T_train, X_train, b_0, gamma, epsilon, accuracy_array_tr, error_array_tr, "Train Set:")

W_1 = answer[0]
accuracy_array_tr = answer[1]
error_array_tr = answer[2]

gamma = 0.01
epsilon = 0.01
answer = gradient_descent_b(W_1, T_train, X_train, b_0, gamma, epsilon, accuracy_array_tr, error_array_tr, "Train Set:")

b_1 = answer[0]
accuracy_array_tr = answer[1]
error_array_tr = answer[2]

valid_accuracy_for_W_b_from_train = get_accuracy(get_Y(X_valid, W_1, b_1), T_valid)

# Проведем градиентный спуск для валидационной выборки
# (W и b теперь не будем инициализировать, а возьмем лучшие с train)
accuracy_array_va = np.array([])
error_array_va = np.array([])

gamma = 0.01
epsilon = 0.005
answer = gradient_descent_w(W_1, T_valid, X_valid, b_1, gamma, epsilon, accuracy_array_va, error_array_va, "Validation Set:")

W_1 = answer[0]
accuracy_array_va = answer[1]
error_array_va = answer[2]

gamma = 0.001
epsilon = 0.0005
answer = gradient_descent_b(W_1, T_valid, X_valid, b_1, gamma, epsilon, accuracy_array_va, error_array_va, "Validation Set:")

b_1 = answer[0]
accuracy_array_va = answer[1]
error_array_va = answer[2]

print()
print("Для W и b, которые получились на Train Set:")
print("Validation accuracy", valid_accuracy_for_W_b_from_train)
print()
print("Для W и b, которые получились на Validation Set:")
print("Validation accuracy", get_accuracy(get_Y(X_valid, W_1, b_1), T_valid))

print("confusion_matrix до обучения: (acc = ", get_accuracy(get_Y(X_valid, W_0, b_0), T_valid),")")
print(get_confusion_matrix(get_Y(X_valid, W_0, b_0), T_valid))
print("confusion_matrix после обучения:(acc = ", get_accuracy(get_Y(X_valid, W_1, b_1), T_valid), ")")
print(get_confusion_matrix(get_Y(X_valid, W_1, b_1), T_valid))

#Строим графики
plt.subplot(2, 2, 1)
plt.title("Accuracy_train")
plt.plot(range(len(accuracy_array_tr)), accuracy_array_tr)

plt.subplot(2, 2, 2)
plt.title("Error_train")
plt.plot(range(len(error_array_tr)), error_array_tr)

plt.subplot(2, 2, 3)
plt.title("Accuracy_validation")
plt.plot(range(len(accuracy_array_va)), accuracy_array_va)

plt.subplot(2, 2, 4)
plt.title("Error_validation")
plt.plot(range(len(error_array_va)), error_array_va)

plt.show()