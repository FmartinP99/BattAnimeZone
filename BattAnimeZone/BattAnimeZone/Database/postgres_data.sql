DROP TABLE Anime;


CREATE TABLE Anime (
  id SERIAL PRIMARY KEY,
  title TEXT DEFAULT '',
  title_english TEXT DEFAULT '',
  title_japanese TEXT DEFAULT '',
  title_synonyms TEXT DEFAULT '',
  media_type TEXT DEFAULT '',
  source TEXT DEFAULT '',
  episodes INTEGER DEFAULT 0,
  status TEXT DEFAULT '',
  duration TEXT DEFAULT '',
  rating TEXT DEFAULT '',
  score REAL DEFAULT -1,
  scored_by INTEGER DEFAULT -1,
  rank INTEGER DEFAULT -1,
  popularity INTEGER DEFAULT -1,
  members INTEGER DEFAULT -1,
  favorites INTEGER DEFAULT -1,
  synopsis TEXT DEFAULT '',
  background TEXT DEFAULT '',
  season TEXT DEFAULT '',
  year INTEGER DEFAULT -1,
  image_jpg_url TEXT DEFAULT '',
  image_small_jpg_url TEXT DEFAULT '',
  image_large_jpg_url TEXT DEFAULT '',
  image_webp_url TEXT DEFAULT '',
  image_small_webp_url TEXT DEFAULT '',
  image_large_webp_url TEXT DEFAULT '',
  trailer_url TEXT DEFAULT '',
  trailer_embed_url TEXT DEFAULT '',
  trailer_image_url TEXT DEFAULT '',
  trailer_image_small_url TEXT DEFAULT '',
  trailer_image_medium_url TEXT DEFAULT '',
  trailer_image_large_url TEXT DEFAULT '',
  trailer_image_maximum_url TEXT DEFAULT '',
  aired_from_day INTEGER DEFAULT -1,
  aired_from_month INTEGER DEFAULT -1,
  aired_from_year INTEGER DEFAULT -1,
  aired_to_day INTEGER DEFAULT -1,
  aired_to_month INTEGER DEFAULT -1,
  aired_to_year INTEGER DEFAULT -1,
  aired_string TEXT DEFAULT ''
);

CREATE TABLE Relation (
  id SERIAL PRIMARY KEY,
  parent_id INTEGER NOT NULL,
  child_id INTEGER NOT NULL,
  relationType TEXT NOT NULL,
  
  FOREIGN KEY(parent_id) REFERENCES Anime(id) ON DELETE CASCADE,
  FOREIGN KEY(child_id) REFERENCES Anime(id) ON DELETE CASCADE,
  CONSTRAINT unique_parent_child_relation UNIQUE (parent_id, child_id, relationType)
);

CREATE TABLE External (
  id SERIAL PRIMARY KEY,
  name TEXT NOT NULL,
  url TEXT NOT NULL,
  anime_id INTEGER NOT NULL,
  FOREIGN KEY (anime_id) REFERENCES Anime(id) ON DELETE CASCADE
);

CREATE TABLE Streaming (
  id SERIAL PRIMARY KEY,
  name TEXT NOT NULL,
  url TEXT NOT NULL,
  CONSTRAINT unique_name_url UNIQUE (name, url)
);

CREATE TABLE AnimeStreaming (
  id SERIAL PRIMARY KEY,
  anime_id INTEGER NOT NULL,
  streaming_id INTEGER NOT NULL,
  FOREIGN KEY (anime_id) REFERENCES Anime(id) ON DELETE CASCADE,
  FOREIGN KEY (streaming_id) REFERENCES Streaming(id) ON DELETE CASCADE,
  UNIQUE (anime_id, streaming_id)
);

CREATE TABLE ProductionEntity (
  id SERIAL PRIMARY KEY,
  url TEXT DEFAULT '',
  favorites INTEGER NOT NULL DEFAULT -1,
  established TEXT DEFAULT '',
  about TEXT DEFAULT '',
  count INTEGER NOT NULL DEFAULT -1,
  image_url TEXT DEFAULT ''
);

CREATE TABLE ProductionEntityTitle (
  id SERIAL PRIMARY KEY,
  type TEXT NOT NULL DEFAULT '',
  title TEXT NOT NULL DEFAULT '',
  parent_id INTEGER NOT NULL,
  FOREIGN KEY (parent_id) REFERENCES ProductionEntity(id) ON DELETE CASCADE
);

CREATE TABLE AnimeProductionEntity (
  id SERIAL PRIMARY KEY,
  anime_id INTEGER NOT NULL,
  productionEntity_id INTEGER NOT NULL,
  type TEXT CHECK (type IN ('S', 'L', 'P')) NOT NULL,
  FOREIGN KEY (anime_id) REFERENCES Anime(id) ON DELETE CASCADE,
  FOREIGN KEY (productionEntity_id) REFERENCES ProductionEntity(id) ON DELETE CASCADE,
  UNIQUE (anime_id, productionEntity_id, type)
);


CREATE TABLE UserAccount (
  id SERIAL PRIMARY KEY,
  username TEXT NOT NULL UNIQUE,
  password TEXT NOT NULL,
  registered_at TEXT NOT NULL,
  email TEXT NOT NULL UNIQUE,
  CHECK (
    email LIKE '%_@_%._%' AND
    LENGTH(email) - LENGTH(REPLACE(email, '@', '')) = 1 AND
    POSITION('@' IN email) > 1 AND
    POSITION('.' IN email) > POSITION('@' IN email) + 1 AND
    NOT (POSITION('.' IN email) = LENGTH(email)) AND
    NOT (POSITION('@' IN email) = 1)
  ),
  role TEXT CHECK (role IN ('Admin', 'User')) NOT NULL,
  token TEXT DEFAULT NULL,
  refreshToken TEXT DEFAULT NULL,
  refreshTokenExpiryTime TEXT DEFAULT NULL
);

CREATE TABLE AnimeUser (
  id SERIAL PRIMARY KEY,
  anime_id INTEGER NOT NULL,
  user_id INTEGER NOT NULL,
  rating INTEGER CHECK (rating BETWEEN 0 AND 10) DEFAULT 0,
  favorite boolean  NOT NULL DEFAULT False,
  date TEXT NOT NULL,
  status TEXT CHECK (status IN ('watching', 'completed', 'on hold', 'dropped', 'planned')) NOT NULL,
  FOREIGN KEY (anime_id) REFERENCES Anime(id) ON DELETE CASCADE,
  FOREIGN KEY (user_id) REFERENCES UserAccount(id) ON DELETE CASCADE,
  UNIQUE (anime_id, user_id)
);


CREATE TABLE Genre (
  id SERIAL PRIMARY KEY,
  name TEXT NOT NULL DEFAULT '',
  url TEXT NOT NULL DEFAULT ''
);


CREATE TABLE AnimeGenre (
  id SERIAL PRIMARY KEY,
  anime_id INTEGER NOT NULL,
  genre_id INTEGER NOT NULL,
  is_theme boolean  DEFAULT False,
  FOREIGN KEY (anime_id) REFERENCES Anime(id) ON DELETE CASCADE,
  FOREIGN KEY (genre_id) REFERENCES Genre(id) ON DELETE CASCADE,
  UNIQUE (anime_id, genre_id)
);


CREATE TABLE DistinctMediaTypes AS
SELECT DISTINCT media_type
FROM Anime;


CREATE TABLE DistinctYears AS
SELECT DISTINCT anime.year
FROM Anime
WHERE anime.year IS NOT NULL;









CREATE FUNCTION get_anime_count_by_genre(input_genre_id INT)
RETURNS TABLE (
    genre_id INT,
    anime_count INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT ag.genre_id, COUNT(*) AS anime_count
    FROM anime_genres ag
    WHERE ag.genre_id = input_genre_id
    GROUP BY ag.genre_id;
END;
$$ LANGUAGE plpgsql;



create
or replace function get_anime_relations_by_parent_id (_parent_id int) returns table (
  Mal_id int,
  TitleEnglish text,
  TitleJapanese text,
  Relations JSONB
) as $$
BEGIN
    RETURN QUERY
    SELECT p.id AS Mal_id,
           p.title_english AS TitleEnglish,
           p.title_japanese AS TitleJapanese,
           jsonb_agg(jsonb_build_object(
               'Mal_id', c.id,
               'TitleEnglish', c.title_english,
               'TitleJapanese', c.title_japanese,
               'MediaType', c.media_type,
               'Episodes', c.episodes,
               'Score', c.score,
               'Popularity', c.popularity,
               'Year', c.year,
               'ImageLargeWebpUrl', c.image_large_webp_url,
               'RelationType', r.relationtype
           )) AS Relations
    FROM relation r
    JOIN anime p ON r.parent_id = p.id
    JOIN anime c ON r.child_id = c.id
    WHERE r.parent_id = _parent_id
    GROUP BY p.id, p.title_english, p.title_japanese;
END;
$$ language plpgsql;




CREATE OR REPLACE FUNCTION get_filtered_animes(
    genres INT[],
    media_types TEXT[],
    year_lower INT,
    year_upper INT
) RETURNS TABLE (
    Mal_id INT,
    Title TEXT,
    TitleEnglish TEXT,
    TitleJapanese TEXT,
    MediaType TEXT,
    Episodes INT,
    Status TEXT,
    Rating TEXT,
    Score real,
    Popularity INT,
    Year INT,
    ImageLargeWebpUrl TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        a.id,
        a.title,
        a.title_english,
        a.title_japanese,
        a.media_type,
        a.episodes,
        a.status,
        a.rating,
        a.score,
        a.popularity,
        a.year,
        a.image_large_webp_url
    FROM
        Anime a
    LEFT JOIN
        AnimeGenre ag ON a.id = ag.anime_id
    WHERE
        (genres IS NULL OR ag.genre_id = ANY(genres))
         AND (media_types IS NULL OR a.media_type = ANY(media_types))
        AND (year_lower IS NULL OR a.year >= year_lower)
        AND (year_upper IS NULL OR a.year <= year_upper)
    GROUP BY
        a.id, a.title, a.title_english, a.title_japanese, a.media_type, a.episodes, a.status,
        a.rating, a.score, a.popularity, a.year, a.image_large_webp_url
    HAVING
        (genres IS NULL OR array_length(genres, 1) = 0 OR count(DISTINCT ag.genre_id) = array_length(genres, 1));
END;
$$ LANGUAGE plpgsql;





DROP FUNCTION get_genre_animes(integer);
CREATE OR REPLACE FUNCTION get_genre_animes(_genre_id INT) 
RETURNS TABLE (
    GenreName TEXT,
    Animes JSONB
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        g.name AS GenreName,
        jsonb_agg(jsonb_build_object(
            'Mal_id', a.id,
            'Title', a.title,
            'TitleEnglish', a.title_english,
            'TitleJapanese', a.title_japanese,
            'MediaType', a.media_type,
            'Episodes', a.episodes,
            'Score', a.score,
            'Popularity', a.popularity,
            'Year', a.year,
            'ImageLargeWebpUrl', a.image_large_webp_url
        )) AS Animes
    FROM
        animegenre ag
    JOIN
        genre g ON ag.genre_id = g.id
    JOIN
        anime a ON ag.anime_id = a.id
    WHERE
        ag.genre_id = _genre_id
    GROUP BY
        g.name;
END;
$$ LANGUAGE plpgsql;






CREATE OR REPLACE FUNCTION get_production_entities()
RETURNS TABLE (
    Mal_id INT,
    Favorites INT,
    Count INT,
    Image_url TEXT,
    Titles JSONB
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        pe.id,
        pe.favorites,
        pe.count,
        pe.image_url,
        jsonb_agg(jsonb_build_object(
            'Type', pet.type,
            'Title', pet.title
        )) AS titles
    FROM 
        productionentity pe
    LEFT JOIN 
        productionentitytitle pet ON pe.id = pet.parent_id
    GROUP BY 
        pe.id, pe.favorites, pe.count, pe.image_url;
END;
$$ LANGUAGE plpgsql;




CREATE OR REPLACE FUNCTION get_animes_for_prod_ent(_mal_id INT)
RETURNS JSONB AS $$
BEGIN
    RETURN (
        SELECT jsonb_build_object(
            'ProdEnt', jsonb_build_object(
                'Mal_id', pe.id,
                'Url', pe.url,
                'Favorites', pe.favorites,
                'Established', pe.established,
                'About', pe.about,
                'Count', pe.count,
                'Image_url', pe.image_url,
                'Titles', jsonb_agg(jsonb_build_object(
                    'Type', pet.type,
                    'Title', pet.title
                ))
            ),
            'ProducedAnimes', (SELECT jsonb_agg(jsonb_build_object(
                'Mal_id', a.id,
                'TitleEnglish', a.title_english,
                'TitleJapanese', a.title_japanese,
                'ImageLargeWebpUrl', a.image_large_webp_url
            )) FROM animeproductionentity ape
            JOIN anime a ON ape.anime_id = a.id
            WHERE ape.productionentity_id = _mal_id AND ape.type = 'P'),
            'LicensedAnimes', (SELECT jsonb_agg(jsonb_build_object(
                'Mal_id', a.id,
                'TitleEnglish', a.title_english,
                'TitleJapanese', a.title_japanese,
                'ImageLargeWebpUrl', a.image_large_webp_url
            )) FROM animeproductionentity ape
            JOIN anime a ON ape.anime_id = a.id
            WHERE ape.productionentity_id = _mal_id AND ape.type = 'L'),
            'StudioAnimes', (SELECT jsonb_agg(jsonb_build_object(
                'Mal_id', a.id,
                'TitleEnglish', a.title_english,
                'TitleJapanese', a.title_japanese,
                'ImageLargeWebpUrl', a.image_large_webp_url
            )) FROM animeproductionentity ape
            JOIN anime a ON ape.anime_id = a.id
            WHERE ape.productionentity_id = _mal_id AND ape.type = 'S')
        )
        FROM productionentity pe
        JOIN productionentitytitle pet ON pe.id = pet.parent_id
        WHERE pe.id = _mal_id
        GROUP BY pe.id
    );
END;
$$ LANGUAGE plpgsql;











CREATE OR REPLACE FUNCTION get_similar_animes(similar_anime_ids INT[])
RETURNS TABLE (
    Mal_id INT,
    Title TEXT,
    TitleEnglish TEXT,
    TitleJapanese TEXT,
    MediaType TEXT,
    Episodes INT,
    Status TEXT,
    Score REAL,
    Season TEXT,
    Year INT,
    ImageLargeWebpUrl TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        a.id,
        a.title,
        a.title_english,
        a.title_japanese,
        a.media_type,
        a.episodes,
        a.status,
        a.score,
        a.season,
        a.year,
        a.image_large_webp_url
    FROM
        anime a
    WHERE
        a.id = ANY(similar_anime_ids);
END;
$$ LANGUAGE plpgsql;






CREATE OR REPLACE FUNCTION get_anime_page_details(_mal_id INT)
RETURNS JSONB AS $$
DECLARE
    q1 RECORD;
    q2 RECORD;
    q3 RECORD;
    result JSONB;
BEGIN
    -- Query 1
    SELECT INTO q1
        a.id,
        a.title,
        a.title_english,
        a.title_japanese,
        a.media_type,
        a.episodes,
        a.status,
        a.duration,
        a.score,
        a.rank,
        a.popularity,
        a.synopsis,
        a.background,
        a.season,
        a.year,
        a.image_large_webp_url,
        a.aired_string,
        jsonb_agg(
            CASE
                WHEN ag.is_theme = FALSE THEN jsonb_build_object('Mal_id', ag.genre_id, 'Name', g.name)
            END
        ) FILTER (WHERE ag.is_theme = FALSE) AS Genres,
        jsonb_agg(
            CASE
                WHEN ag.is_theme = TRUE THEN jsonb_build_object('Mal_id', ag.genre_id, 'Name', g.name)
            END
        ) FILTER (WHERE ag.is_theme = TRUE) AS Themes
    FROM
        animegenre ag
    JOIN
        anime a ON ag.anime_id = a.id
    JOIN
        genre g ON ag.genre_id = g.id
    WHERE
        a.id = _mal_id
    GROUP BY
        a.id, a.title, a.title_english, a.title_japanese, a.media_type, a.episodes, a.status,
        a.duration, a.score, a.rank, a.popularity, a.synopsis, a.background, a.season, a.year,
        a.image_large_webp_url, a.aired_string;
    

      -- Query 2
    SELECT INTO q2
        jsonb_agg(
            CASE
                WHEN ap.type = 'S' AND pt.type = 'Default' THEN jsonb_build_object('Mal_id', ap.productionentity_id, 'Name', pt.title)
            END
        ) FILTER (WHERE ap.type = 'S' AND pt.type = 'Default') AS Studios,
        jsonb_agg(
            CASE
                WHEN ap.type = 'L' AND pt.type = 'Default' THEN jsonb_build_object('Mal_id', ap.productionentity_id, 'Name', pt.Title)
            END
        ) FILTER (WHERE ap.type = 'L' AND pt.type = 'Default') AS Licensors,
        jsonb_agg(
            CASE
                WHEN ap.type = 'P' AND pt.type = 'Default' THEN jsonb_build_object('Mal_id', ap.productionentity_id, 'Name', pt.title)
            END
        ) FILTER (WHERE ap.type = 'P' AND pt.type = 'Default') AS Producers
    FROM
        animeproductionentity ap
    JOIN
        anime a ON ap.anime_id = a.id
    JOIN
        productionentity p ON ap.productionentity_id = p.id
    JOIN
        productionentitytitle pt ON p.id = pt.parent_id
    WHERE
        a.id = _mal_id
    GROUP BY
        a.id;

    -- Query 3
    SELECT INTO q3
        jsonb_agg(
            jsonb_build_object('Name', s.name, 'Url', s.url)
        ) AS Streamings
    FROM
        animestreaming astr
    JOIN
        streaming s ON astr.streaming_id = s.id
    WHERE
        astr.anime_id = _mal_id
    GROUP BY
        astr.anime_id;
    
     -- Combine results
    result := jsonb_build_object(
        'Mal_id', q1.id,
        'Title', q1.title,
        'TitleEnglish', q1.title_english,
        'TitleJapanese', q1.title_japanese,
        'MediaType', q1.media_type,
        'Episodes', q1.episodes,
        'Status', q1.status,
        'Duration', q1.duration,
        'Score', q1.score,
        'Rank', q1.rank,
        'Popularity', q1.popularity,
        'Synopsis', q1.synopsis,
        'Background', q1.background,
        'Season', q1.season,
        'Year', q1.year,
        'ImageLargeWebpUrl', q1.image_large_webp_url,
        'AiredString', q1.aired_string,
        'Genres', q1.Genres,
        'Themes', q1.Themes,
        'Studios', q2.Studios,
        'Licensors', q2.Licensors,
        'Producers', q2.Producers,
        'Streamings', q3.Streamings
    );

    RETURN result;
END;
$$ LANGUAGE plpgsql;


