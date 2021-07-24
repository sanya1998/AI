import os
import pickle


def read_bin_file(file_name):
    """Функция считывания бинарного файла file_name из папки Bin_files"""
    path_to_file = os.path.join('Bin_files', file_name)
    if os.path.exists(path_to_file):
        f = open(path_to_file, 'rb')
        value = pickle.load(f)
        f.close()
    else:
        return None
    """Возвращается значение"""
    return value


def write_bin_file(value, file_name):
    """Функция записи value в бинарный файл file_name"""
    catal = 'Bin_files'
    try:
        os.makedirs(catal)
    except FileExistsError:
        # directory already exists
        pass
    path_to_file = os.path.join(catal, file_name)
    f = open(path_to_file, 'wb')
    pickle.dump(value, f)
    f.close()
