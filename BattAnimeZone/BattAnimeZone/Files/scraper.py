import time
import pandas as pd
import json
import requests
import shutil
import os
example_data = dict()


def main(BASE_URL, IDS):

    for ix, id in enumerate(IDS):
        if ix % 10 == 0:
            # do a backup at every 10 fetch. If for some reason it can't (i.e the file is opened or something) it makes another copy
            try:
                shutil.copy2("mal_data.csv", "mal_data_backup.csv")
            except Exception:
                try:
                    shutil.copy2("mal_data.csv", "mal_data_backup_2.csv")
                except Exception:
                    pass
        print(f"{ix+1}/{len(IDS)}")
        fetch_mal(BASE_URL, id)
        time.sleep(1)

def fetch_mal(BASE_URL, id):
    global example_data

    request_url = BASE_URL + "/" + str(id) + "/full"
    print(request_url)
    response = requests.get(request_url)
    if response.status_code != 200:
        return
    anime_data = response.text
    loaded = json.loads(anime_data)
    loaded = loaded["data"]

    if id == 1:
        example_data = loaded
    if example_data.keys() == loaded.keys():
        flattened = pd.json_normalize(loaded)
        if not os.path.isfile('mal_data.csv'):
            flattened.to_csv('mal_data.csv', index=False)
        else:  # else it exists so append without writing the header
            flattened.to_csv('mal_data.csv', mode='a', header=False, index=False)
    else:
        print(f"{id}'s request doesn't have the same keys as the id==1 data. Therefore skipping it!")




def get_anime_genres():

    request_url = "https://api.jikan.moe/v4/genres/anime"

    response = requests.get(request_url)
    if response.status_code != 200:
        return
    data = response.text
    loaded = json.loads(data)
    loaded = loaded["data"]


    mal_ids = []
    names = []

    urls = []
    counts = []
    for ix, row in enumerate(loaded):
        mal_ids.append(row["mal_id"])
        names.append(row["name"])
        urls.append(row["url"])
        counts.append(row["count"])

    df_dict = {"mal_id": mal_ids, "name":names, "url": urls, "count": counts}

    df = pd.DataFrame.from_dict(df_dict)


    df.to_csv("mal_anime_genres.csv", index_label="mal_id", index=False)

def get_producers():
    has_next_page = True
    pagination = 1
    while has_next_page:
        request_url = f"https://api.jikan.moe/v4/producers?page={pagination}"
        response = requests.get(request_url)
        if response.status_code != 200:
            return
        data = response.text
        loaded = json.loads(data)
        print()

        loaded_data = loaded["data"]
        flattened = pd.json_normalize(loaded_data)
        flattened = flattened.drop_duplicates(subset=["mal_id"])
        if not os.path.isfile('mal_producers.csv'):
            flattened.to_csv('mal_producers.csv',  index=False)
        else:  # else it exists so append without writing the header
            flattened.to_csv('mal_producers.csv', mode='a', header=False,  index=False)
        pagination += 1
        time.sleep(1)
        has_next_page = loaded["pagination"]["has_next_page"]
        print(has_next_page)



def check_and_fill_empty_cols():
    df = pd.read_csv('mal_data_filtered.csv')

    for col in df.columns:
        # Check data type of the column
        col_type = df[col].dtype
        print(f"{col} - {col_type}")
        # Fill missing values based on data type
        if col_type == 'int64' or col_type == "float64":
            print(col)
            df[col].fillna(0, inplace=True)

    df.drop(["Unnamed: 0"], axis=1, inplace=True)
    df.to_csv("mal_data_filtered_filled.csv", index=False)




if __name__ == "__main__":

    """
    anime_cache_ids_url = "https://raw.githubusercontent.com/seanbreckenridge/mal-id-cache/master/cache/anime_cache.json"
    anime_ids = requests.get(anime_cache_ids_url).text
    data = json.loads(anime_ids)
    sfw_ids = data["sfw"]

    BASE_URL = "https://api.jikan.moe/v4/anime"
    death_note_data = requests.get(BASE_URL).text

    main(BASE_URL, sfw_ids)
    """

    #df = pd.read_csv("mal_data.csv")
    """accidently I wrote the header at every append, so had to delete them."""
    #df_filtered = df.drop(df.index[1::2])
    #df_filtered.to_csv("mal_data_filtered.csv", encoding="utf-8")

    #check_and_fill_empty_cols("mal_data_filtered.csv")
    #get_anime_genres()

    #get_producers()

