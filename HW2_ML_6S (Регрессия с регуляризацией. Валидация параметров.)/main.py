import numpy as np
import matplotlib.pyplot as plt


def get_design_matrix(possible_functions, current_basic_func_ind, x_cur):
    # Создаем матрицу плана
    # 1ый столбец
    F = np.array(possible_functions[current_basic_func_ind[0]](x_cur))
    for i in range(1, len(current_basic_func_ind)):
        # Следующий столбец матрицы плана
        stolbec = possible_functions[current_basic_func_ind[i]](x_cur)

        # Справа добавляем столбец
        F = np.c_[F, stolbec]
    return F


def get_w(F, current_lambda, t_cur):
    # Чтобы три раза не транспонировать в формулах
    F_transp = F.transpose()
    chislo_strok = F_transp.shape[0]
    I = np.eye(chislo_strok)
    try:
        mnozhitel = np.linalg.inv((F_transp @ F) + (current_lambda * I))
    except BaseException:
        # Матрица сингулярна, обратную нельзя найти
        return (False, 0)
    return (True, np.array(mnozhitel @ F_transp @ t_cur))


def get_error(w, t_cur, current_lambda, y):

    E = 0.0
    for i in range(len(y)):
        E += pow(y[i] - t_cur[i], 2)

    slag = 0.0
    q = 2
    if len(w.shape) == 0:
        slag += w ** q
    else:
        for i in range(len(w)):
            slag += w[i]**q
        slag *= current_lambda
    E += slag

    return E/2

# Data generation
N_elementov = 1000
x = np.linspace(0, 1, N_elementov)

# 2 * np.pi * 3 * x == 6 * np.pi * x
z = 20 * np.sin(6*np.pi*x) + 100*np.exp(x)

error = 10 * np.random.randn(N_elementov)
t = z + error

# Split data
# Создаем массив индексов
ind = np.arange(len(t))
# Перемешиваем массив индексов
ind_prm = np.random.permutation(ind)

part_train_konec = part_valid_nachalo = np.int32(0.8*len(ind_prm))
part_valid_konec = part_test_nachalo = np.int32(0.9*len(ind_prm))

train_ind = ind_prm[:part_train_konec]
valid_ind = ind_prm[part_valid_nachalo:part_valid_konec]
test_ind = ind_prm[part_test_nachalo:]

x_train = x[train_ind]
t_train = t[train_ind]
x_valid = x[valid_ind]
t_valid = t[valid_ind]
x_test = x[test_ind]
t_test = t[test_ind]

# Validation
lambdas_array = np.array([0, 0.0001, 0.0005, 0.001, 0.005, 0.01, 0.05, 0.1, 0.5, 1, 5, 10, 50, 100, 500, 1000])

possible_functions = [
    lambda xi: np.sin(xi),
    lambda xi: np.cos(xi),
    lambda xi: np.tan(xi),
    lambda xi: np.exp(xi),
    lambda xi: np.sqrt(xi)
]
basic_func_str = [
    "sin(x)", "cos(x)", "tan(x)", "exp(x)", "sqrt(x)"
]
#  К базисным функциям добавляем степени
naib_step_polinom = 10
for i in range(naib_step_polinom + 1):
    possible_functions.append(lambda xi : xi**i)
    basic_func_str.append("x^" + str(i))

N_validation = 1000
E_min = 10**10
best_lambda = lambdas_array[0]
best_funcs_inds = possible_functions[0]
w_best = 0
y_best = 0
for i in range(N_validation):
    current_lambda = np.random.choice(lambdas_array)

    fix_func_amount = np.random.randint(1, len(possible_functions) + 1)
    current_basic_func_ind = np.random.choice(np.arange(len(possible_functions)), fix_func_amount, replace=False)

    F = get_design_matrix(possible_functions, current_basic_func_ind, x_train)

    res_w = get_w(F, current_lambda, t_train)
    if res_w[0]:
        w_cur = res_w[1]
    else:
        # Матрица сингулярна, нельзя найти обратную
        continue

    F_valid = get_design_matrix(possible_functions, current_basic_func_ind, x_valid)

    y = np.array(np.dot(w_cur, F_valid.transpose()))

    E_cur = get_error(w_cur, t_valid, current_lambda, y)
    if E_cur < E_min:
        E_min = E_cur
        best_lambda = current_lambda
        best_funcs_inds = current_basic_func_ind

#Test
F_test = get_design_matrix(possible_functions, best_funcs_inds, x_test)
w_best = get_w(F_test, best_lambda, t_test)[1]
y_best = np.array(np.dot(w_best, F_test.transpose()))
E_test = get_error(w_best, t_test,  best_lambda, y_best)

print("E = ", E_test)
print("Best lambda = ", best_lambda)
print("Best set basic func: ")

for i in range(len(best_funcs_inds)):
    print(basic_func_str[best_funcs_inds[i]])

# Сортируем пузырьком (дедйлайн поджимал, не успел найти что-то поумнее)
for i in range(len(x_test)-1):
    for j in range(len(x_test)-i-1):
        if x_test[j] > x_test[j+1]:
            x_test[j], x_test[j+1] = x_test[j+1], x_test[j]
            y_best[j], y_best[j+1] = y_best[j+1], y_best[j]


plt.figure()
plt.plot(x_test, y_best, '-k')
plt.plot(x, t, ',r')
plt.plot(x, z, '-b')
plt.show()