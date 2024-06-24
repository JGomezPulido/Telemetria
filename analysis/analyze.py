import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import os
from os.path import isfile, join

def get_folder_csv(path) -> list:
    '''
    Returns all the .csv files in a given path
    '''
    dir = os.listdir(path)
    files = [join(path,f) for f in dir if isfile(join(path,f)) and f.endswith(".csv")]
  
    return files

def get_games(eventsTable: pd.DataFrame) -> list:
    
    gameGroups = eventsTable.groupby("id_game")
    gamesList = [g for g in gameGroups]
    print(gamesList)
    return
def main() -> None:
    
    csv = get_folder_csv("data/")
    data = [pd.read_csv(f, sep=";") for f in csv]
    table = pd.concat(data, axis=0)
    print(table)

if __name__ == "__main__":
    main()