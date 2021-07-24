import matplotlib.pyplot as plt
import numpy as np
c0 = np.random.normal(190, 10, 500)
c1 = np.random.normal(210, 10, 500)
TP = TN = FP = FN = 0
c0_classif = []
c1_classif = []
porog0 = 200
for i in c0:
    if i >= porog0:
        c1_classif.append(i)
        FP += 1
    else:
        c0_classif.append(i)
        TN += 1
for i in c1:
    if i >= porog0:
        c1_classif.append(i)
        TP += 1
    else:
        c0_classif.append(i)
        FN += 1
print(TP, " ", FP, " ", TN, " ", FN)
Accuracy = (TP+TN)/(TP+TN+FP+FN)
Precision = TP/(TP+FP)
Recall = TP/(TP+FN)
F1_score = 2*(Recall*Precision)/(Recall+Precision)
alfa = FP/(TN+FP)
beta = FN/(TP+FN)
print(Accuracy, " ", Precision, " ", Recall, " ", F1_score, " ", alfa, " ", beta)
roc_FP = roc_TP = []
for porog in range(301):
    TP = TN = FP = FN = 0
    for i in c0:
        if i >= porog:
            FP += 1
        else:
            TN += 1
    for i in c1:
        if i >= porog:
            TP += 1
        else:
            FN += 1
    roc_FP.append(FP/(TN+FP))
    roc_TP.append(TP/(TP+FN))
plt.plot(roc_FP, roc_TP)
plt.show()