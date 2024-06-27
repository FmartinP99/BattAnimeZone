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
  favorite INTEGER CHECK (favorite IN (0, 1)) NOT NULL DEFAULT 0,
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
  is_theme INTEGER CHECK (is_theme BETWEEN 0 AND 1) DEFAULT 0,
  FOREIGN KEY (anime_id) REFERENCES Anime(id) ON DELETE CASCADE,
  FOREIGN KEY (genre_id) REFERENCES Genre(id) ON DELETE CASCADE,
  UNIQUE (anime_id, genre_id)
);


CREATE VIEW DistinctMediaTypes AS
SELECT DISTINCT media_type
FROM Anime;


CREATE VIEW DistinctYears AS
SELECT DISTINCT anime.year
FROM Anime
WHERE anime.year IS NOT NULL;