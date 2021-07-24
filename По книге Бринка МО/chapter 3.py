# Modeling and prediction
import numpy as np
import pandas
data = pandas.read_csv("data/titanic.csv")

# We make a 80/20% train/test split of the data
data_train = data[:int(0.8*len(data))]
data_test = data[int(0.8*len(data)):]


# The categorical-to-numerical function from chapter 2
# Changed to automatically add column names
def cat_to_num(data):
    categories = np.unique(data)
    features = {}
    for cat in categories:
        binary = (data == cat)
        features["%s=%s" % (data.name, cat)] = binary.astype("int")
    return pandas.DataFrame(features)


def prepare_data(data):
	"""Takes a dataframe of raw data and returns ML model features
	"""
	
	# Initially, we build a model only on the available numerical values
	features = data.drop(["PassengerId", "Survived", "Fare", "Name", "Sex", "Ticket", "Cabin", "Embarked"], axis=1)
	
	# Setting missing age values to -1
	features["Age"] = data["Age"].fillna(-1)
	
	# Adding the sqrt of the fare feature
	features["sqrt_Fare"] = np.sqrt(data["Fare"])
	
	# Adding gender categorical value
	features = features.join(cat_to_num(data['Sex']))
	
	# Adding Embarked categorical value
	features = features.join(cat_to_num(data['Embarked']))
	
	return features


# Лисинг 3. 1. Посроение кассификатора методом погисичекой регрессии на базе библиотеки scikit-leam (стр 106)
# Импортирует алгоритм логистической регрессии
from sklearn.linear_model import LogisticRegression as Model
# Импортирует метод опорных векторов
# from sklearn.svm import SVC as Model


cat_to_num(data['Sex'])
features = prepare_data(data_train)


# Обучает алгоритм логистической регрессии на признаках и целевых данных
def train(features, target):
	model = Model()
	model.fit(features, target)
	return model


# Даёт прогноз для нового набора признаков, используя построенную модель
def predict(model, new_features):
	preds = model.predict(new_features)
	return preds


# Данные с "Титаника" загружены в titanic_feats, titanic_target и titanic_test

# Возвращает модель, построенную алгоритмом
model = train(titanic_feats, titanic_target)

# Возвращает предсказание (0 или 1)
predictions = predict(model, titanic_test)
