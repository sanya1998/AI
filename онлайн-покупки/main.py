import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import accuracy_score
from sklearn.metrics import confusion_matrix


df = pd.read_csv('payment_fraud.csv')
# print(df.sample(3))

# Преобразование переменной из категориальных в числовые
# (1 столбец в 3 столбца, так как только три варианта)
df = pd.get_dummies(df, columns=['paymentMethod'])

# Разделить набор данных на обучающие и тестовые наборы
X_train, X_test, y_train, y_test = train_test_split(
	df.drop('label', axis=1), df['label'],
	test_size=0.33, random_state=16
	)

# Логистическая регрессия. Инициализация.
# Аргумент добавил из инета, чтоб не вылазило исключение
clr = LogisticRegression(solver='lbfgs')
# Обучение модели классификатора
clr.fit(X_train, y_train)

# Предсказание на тестовых знаниях
y_pred = clr.predict(X_test)

print(accuracy_score(y_pred, y_test))
print(confusion_matrix(y_test,y_pred))

# Применение модели на реальной транзакции
# df_real =
# clr.predict_proba(df_real)
# print(df.items)
