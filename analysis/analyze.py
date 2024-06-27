import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import os
import json
from os.path import isfile, join


class GameMetrics:
    '''
    This class calculates the metrics for a game instance.
    Call GetData to get a dictionary with all the metrics or use the plantPickPositions field to get the positions the plants were picked at.
    '''
    def __init__(self, gameEvents):
        self.spearPickCount = 0
        self.triceTotemPicked = False
        self.ankyloTotemPicked = False
        self.plantPickPositions = []

        self.triceUseCount = 0
        self.ankyloUseCount = 0
        self.plantUseCount = 0
        self.spearUseCount = 0
        self.CalculateMetrics(gameEvents)
        self.data = dict()
        self.PopulateData()

    def PopulateData(self) -> None:
        self.data['spearPicks'] = self.spearPickCount
        self.data['tricePicked'] = self.triceTotemPicked
        self.data['ankyloPicked'] = self.ankyloTotemPicked
        self.data['triceUses'] = self.triceUseCount
        self.data['ankyloUses'] = self.ankyloUseCount
        self.data['plantUses'] = self.plantUseCount
        self.data['spearUses'] = self.spearUseCount

    def GetData(self) -> dict:
        return self.data

    def CalculateMetrics(self,events) -> None:
        self.CalculateItemPicks(events)
        self.CalculateItemUses(events)
        

    def CalculateItemPicks(self, events) -> None:
        getItemEvents = events[events['event_type'] == "GetItem"]
        self.spearPickCount = getItemEvents[getItemEvents['item_name'] == 'Lanza'].shape[0]
        plantEvents = getItemEvents[getItemEvents['item_name'] == 'PlantaVerde']
        self.plantPickPositions = [dict(x=float(evt['position_x']), y=float(evt['position_y'])) for idx, evt in plantEvents.iterrows()]
        self.triceTotemPicked = getItemEvents[getItemEvents['item_name'] == 'TotemTrice'].shape[0] > 0
        self.ankyloTotemPicked = getItemEvents[getItemEvents['item_name'] == 'TotemAnkylo'].shape[0] > 0

    def CalculateItemUses(self, events) -> None:
        useItemEvents = events[events['event_type'] == "UseItem"]
        self.spearUseCount = useItemEvents[useItemEvents['item_name'] == 'Lanza'].shape[0]
        self.plantUseCount = useItemEvents[useItemEvents['item_name'] == 'PlantaVerde'].shape[0] 
        self.ankyloUseCount = useItemEvents[useItemEvents['item_name'] == 'TotemAnkylo'].shape[0]
        self.triceUseCount = useItemEvents[useItemEvents['item_name'] == 'TotemTrice'].shape[0]



def get_folder_csv(path) -> list:
    '''
    Returns all the .csv files in a given path
    '''
    dir = os.listdir(path)
    files = [join(path,f) for f in dir if isfile(join(path,f)) and f.endswith(".csv")]
  
    return files

def get_games(eventsTable: pd.DataFrame) -> list:
    
    gameGroups = eventsTable.groupby("id_game")
    gamesList = [GameMetrics(g[1]) for g in gameGroups]
    return gamesList


def generate_percentages(numeric: bool, dataframe: pd.DataFrame, label: str, title: str, filename: str, graphText: list, threshold: int) -> None:
    BoolToString = lambda x: graphText[0] if x else graphText[1]

    if numeric:
        data = dataframe[label].apply(lambda x: x > threshold) 
    else:
        data = dataframe[label] 
    
    dataPercent = data.value_counts(normalize=True)*100.0
    dataPercent.index = dataPercent.index.map(BoolToString)
    fig,ax = fig,ax = plt.subplots(tight_layout=True)
    
    ax = dataPercent.plot.pie(fig = fig, ax = ax)
    ax.set_ylabel('')
    ax.set_title(title)
    fig.savefig(filename)

    return

def main() -> None:
    dataPath = 'data/'
    resultsPath = 'results/'

    # Get game data from csv data path
    csv = get_folder_csv(f'{dataPath}csv')
    data = [pd.read_csv(f, sep=";", decimal=',') for f in csv]
    table = pd.concat(data, axis=0)
    games = get_games(table)

    # Get game map extents from the json data file
    with open(f"{dataPath}mapData.json", 'r') as file:
        mapData = json.load(file)
    mapMinPositionX = mapData['minimumPosition']['x']
    mapMinPositionY = mapData['minimumPosition']['y']
    mapMaxPositionX = mapData['maximumPosition']['x']
    mapMaxPositionY = mapData['maximumPosition']['y']
    extent = [mapMinPositionX, mapMaxPositionX, mapMinPositionY , mapMaxPositionY]

    #Calculo el heatmap de las plantas
    plantUses = []
    for game in games:
        plantUses.extend(game.plantPickPositions)
    dfPlants = pd.DataFrame(plantUses)
    fig,ax = plt.subplots(figsize=(15,10))
    map = plt.imread(f'{dataPath}map.png')
    ax.imshow(map, aspect='equal', extent=extent, alpha=1)
    dfPlants.plot.hexbin(fig = fig, ax = ax, x = "x", y = "y", reduce_C_function=sum, gridsize=(15,10), extent=extent, alpha = 0.5, cmap = 'Reds')
    ax.set_xticks(range(mapMinPositionX, mapMaxPositionX, 10))
    ax.set_yticks(range(mapMinPositionY, mapMaxPositionY, 10))
    
    if not os.path.exists(resultsPath):
        os.mkdir(resultsPath)
    fig.savefig(f'{resultsPath}heatmap.png')

    gamesMetrics = [g.GetData() for g in games]
    gamesMetricsDataFrame = pd.DataFrame(gamesMetrics)
    
    meansDataFrame = gamesMetricsDataFrame.drop(['tricePicked', 'ankyloPicked'], axis = 1).mean()
    fig,ax = plt.subplots(tight_layout=True)
    ax.set_title('Medias de uso y recogida de items')
    meansDataFrame.plot.bar(fig = fig, ax=ax)
    ax.set_xticklabels(['Recogida Lanza', 'Uso Triceratops', 'Uso Anquilo', 'Uso plantas', 'Uso Lanza'])
    fig.savefig(f'{resultsPath}means.png')

    generate_percentages(True, gamesMetricsDataFrame, 'spearPicks', 'Porcentaje de partidas en las que se ha recogido la lanza', f'{resultsPath}spearPickedPercent', ['Cogida', 'No Cogida'], 0)
    generate_percentages(True, gamesMetricsDataFrame, 'plantUses', 'Porcentaje de partidas en las que se han usado las plantas', f'{resultsPath}plantsUsedPercent', ['Usada', 'No Usada'], 0)
    generate_percentages(True, gamesMetricsDataFrame, 'spearUses', 'Porcentaje de partidas en las que se ha usado la lanza', f'{resultsPath}spearUsedPercent', ['Usadas', 'No Usadas'], 0)
    generate_percentages(False, gamesMetricsDataFrame, 'tricePicked', 'Porcentaje de partidas en las que se ha recogido el Triceratops', f'{resultsPath}tricePercent', ['Recogido', 'No Recogido'],0)
    generate_percentages(False, gamesMetricsDataFrame, 'ankyloPicked', 'Porcentaje de partidas en las que se ha recogido el Anquilosaurio', f'{resultsPath}ankyloPercent', ['Recogido', 'No Recogido'],0)
    generate_percentages(True, gamesMetricsDataFrame, 'triceUses', 'Porcentaje de partidas en las que se ha usado el Triceratops', f'{resultsPath}triceUsedPercent', ['Usado', 'No Usado'],0)
    generate_percentages(True, gamesMetricsDataFrame, 'ankyloUses', 'Porcentaje de partidas en las que se ha usado el Anquilosaurio', f'{resultsPath}ankyloUsedPercent', ['Usado', 'No Usado'],0)
   
    



if __name__ == "__main__":
    main()