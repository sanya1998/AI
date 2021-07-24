import numpy as np
import matplotlib.pyplot as plt

N = 1000
x = np.linspace(0, 1, N)

# 2 * np.pi * 3 * x == 6 * np.pi * x
z = 20 * np.sin(6*np.pi*x) + 100*np.exp(x)

error = 10 * np.random.randn(N)
t = z + error

max_M = 100

# от 1 до max_M         (max_M элементов)
M_array = range(1, max_M+1)

# от 0 до (max_M - 1)   (max_M элементов)
E = np.zeros(max_M)

plt.figure()

# Матрица F будет доделывается при каждом увеличении M
F = np.ones(shape=(N, 1))
# F_transp = np.ones(shape=(1, N))

for M in M_array:
    stolbec = np.zeros(shape=(N, 1))
    for i in range(N):
        stolbec[i][0] = pow(x[i], M)

    # Присоединяем столбец справа
    F = np.c_[F, stolbec]

    # Чтобы три раза не транспонировать в формулах
    F_transp = F.transpose()

    # Еще хотел так: не транспонировать большую матрицу плана каждый раз,
    # а с увеличением M добавлять к F_transp строчку снизу с помощью:
    # F_transp = np.r_[F_transp, stolbec.transpose()]
    # Но начаиная с M = 11 (F.transpose() @ F) почему-то
    # отличается от (np.r_[F_transp, stolbec.transpose()] @ F)

    w = np.array(np.linalg.inv(F_transp @ F) @ F_transp @ t)
    # В любом случае получается строка (то есть здесь w.transpose() == w)
    y = np.array(w @ F_transp)

    E[M-1] = 0.0
    for i in range(N):
        E[M-1] += pow(y[i] - t[i], 2)
    E[M-1] /= 2

    if M == 1:
        plt.subplot(2, 2, 1)
        plt.title("M == 1")
        plt.plot(x, y, '-k')
        plt.plot(x, t, ',r')
        plt.plot(x, z, '-b')
    elif M == 8:
        plt.subplot(2, 2, 2)
        plt.title("M == 8")
        plt.plot(x, y, '-k')
        plt.plot(x, t, ',r')
        plt.plot(x, z, '-b')
    elif M == 100:
        plt.subplot(2, 2, 3)
        plt.title("M == 100")
        plt.plot(x, y, '-k')
        plt.plot(x, t, ',r')
        plt.plot(x, z, '-b')

plt.subplot(2, 2, 4)
plt.title("E(w) от M")
plt.plot(M_array, E, '-b')

plt.show()
