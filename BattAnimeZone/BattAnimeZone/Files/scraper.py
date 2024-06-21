import time
import pandas as pd
import json
import requests
import shutil
import os
example_data = dict()


def main(BASE_URL, IDS, outfile):

    for ix, id in enumerate(IDS):
        if ix % 10 == 0:
            # do a backup at every 10 fetch. If for some reason it can't (i.e the file is opened or something) it makes another copy
            try:
                shutil.copy2(f"{outfile}.csv", f"{outfile}_backup.csv")
            except Exception:
                try:
                    shutil.copy2(f"{outfile}.csv", f"{outfile}_backup_2.csv")
                except Exception:
                    pass
        print(f"{ix+1}/{len(IDS)}")
        fetch_mal(BASE_URL, id, outfile)
        time.sleep(1)

def fetch_mal(BASE_URL, id, outfile):
    global example_data

    request_url = BASE_URL + "/" + str(id) + "/full"
    print(request_url)
    response = requests.get(request_url)
    if response.status_code != 200:
        return
    anime_data = response.text
    loaded = json.loads(anime_data)
    loaded = loaded["data"]

    if id == ids_to_request[0]:
        example_data = loaded
    if example_data.keys() == loaded.keys():
        flattened = pd.json_normalize(loaded)
        if not os.path.isfile(f'{outfile}.csv'):
            flattened.to_csv(f'{outfile}.csv', index=False)
        else:  # else it exists so append without writing the header
            flattened.to_csv(f'{outfile}.csv', mode='a', header=False, index=False)
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
            break
        data = response.text
        loaded = json.loads(data)
        print()

        loaded_data = loaded["data"]
        flattened = pd.json_normalize(loaded_data)
        flattened = flattened.drop_duplicates(subset=["mal_id"])
        if not os.path.isfile('mal_producers.csv'):
            flattened.to_csv('mal_producers_2.csv',  index=False)
        else:  # else it exists so append without writing the header
            flattened.to_csv('mal_producers_2.csv', mode='a', header=False,  index=False)
        pagination += 1
        time.sleep(1)
        has_next_page = loaded["pagination"]["has_next_page"]
        print(has_next_page)




def get_season(month):
    if month in [12, 1, 2]:
        return 'winter'
    elif month in [3, 4, 5]:
        return 'spring'
    elif month in [6, 7, 8]:
        return 'summer'
    elif month in [9, 10, 11]:
        return 'fall'
    return ''


"""this function is for filling empty values"""
def check_and_fill_empty_cols():
    df = pd.read_csv('mal_data_full_sfw_updated_20240615.csv')

    for col in df.columns:
        # Check data type of the column
        col_type = df[col].dtype
        print(f"{col} - {col_type}")
        # Fill missing values based on data type
        if col_type == 'int64' or col_type == "float64":
            print(col)
            df[col].fillna(0, inplace=True)

    "filling missing seasons"
    df['season'] = df.apply(
        lambda row: get_season(row['aired.prop.from.month']) if pd.isna(row['season']) or row['season'] == '' else row[
            'season'],
        axis=1
    )

    df.to_csv("mal_data_full_sfw_updated_20240615_2.csv", index=False)

def make_smaller_animes_csv(csvfile):

    df = pd.read_csv(f"{csvfile}.csv")

    df = df[df["aired.prop.from.year"] > 2021]

    df.to_csv(f"{csvfile}_2021plus_subset.csv")



"""this function is for updating the csv"""
def merge_or_replace_csv():


    df1 = pd.read_csv("mal_data_filtered_filled.csv")
    df2 = pd.read_csv("2024_refreshed_sfw_animes_filtered_filled.csv")
    df1['mal_id'] = df1['mal_id'].astype(float)
    df2['mal_id'] = df2['mal_id'].astype(float)
    common_mal_ids = df1['mal_id'].isin(df2['mal_id'])
    df1.loc[common_mal_ids] = df2[df2['mal_id'].isin(df1['mal_id'])]
    new_mal_ids = ~df2['mal_id'].isin(df1['mal_id'])
    df1 = pd.concat([df1, df2[new_mal_ids]])
    df1.to_csv('mal_data_full_sfw_updated_20240615.csv', index=False)
    make_smaller_animes_csv("mal_data_full_sfw_updated_20240615")

if __name__ == "__main__":

    """
    anime_cache_ids_url = "https://raw.githubusercontent.com/seanbreckenridge/mal-id-cache/master/cache/anime_cache.json"
    anime_ids = requests.get(anime_cache_ids_url).text
    data = json.loads(anime_ids)
    sfw_ids = data["sfw"]
    sfw_ids = list(set(sfw_ids))

    BASE_URL = "https://api.jikan.moe/v4/anime"
    death_note_data = requests.get(BASE_URL).text

    df = pd.read_csv("mal_data_filtered_filled.csv")
    anime2023ids = df[df["aired.prop.from.year"] < 2024]["mal_id"].tolist()
    anime2023ids = list(set(anime2023ids))

    ids_to_request = list(filter(lambda x: x not in anime2023ids, sfw_ids))
    ids_to_request = list(set(ids_to_request))
    outfile = "2024_refreshed_sfw_animes"
    main(BASE_URL, ids_to_request, outfile)

    """
    #df = pd.read_csv("mal_data.csv")
    """accidently I wrote the header at every append, so had to delete them."""
    #df_filtered = df.drop(df.index[1::2])
    #df_filtered.to_csv("mal_data_filtered.csv", encoding="utf-8")

    #check_and_fill_empty_cols()
    #merge_or_replace_csv()
    #get_anime_genres()

    #get_producers()

    make_smaller_animes_csv("mal_data_full_sfw_updated_20240615");
