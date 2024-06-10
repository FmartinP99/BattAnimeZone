CREATE TABLE Anime (
  id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  title text DEFAULT "",
  title_english text title_english text DEFAULT "",
  title_japanese text title_japanese text DEFAULT "",
  title_synonyms text DEFAULT "",
  media_type text DEFAULT "",
  "source" text DEFAULT "",
  episodes integer DEFAULT 0,
  duration text DEFAULT "",
  rating text DEFAULT "",
  score real DEFAULT -1,
  scored_by integer DEFAULT -1,
  "rank" integer DEFAULT -1,
  popularity integer DEFAULT -1,
  members integer DEFAULT -1,
  favorites integer DEFAULT -1,
  synopsis text DEFAULT "",
  background text DEFAULT "",
  season text DEFAULT "",
  "year" integer DEFAULT -1,
  image_jpg_url text DEFAULT "",
  image_small_jpg_url text DEFAULT "",
  image_large_jpg_url text DEFAULT "",
  image_webp_url text DEFAULT "",
  image_small_webp_url text DEFAULT "",
  image_large_webp_url text DEFAULT "",
  trailer_url text DEFAULT "",
  trailer_embed_url text DEFAULT "",
  trailer_image_url text DEFAULT "",
  trailer_image_small_url text DEFAULT "",
  trailer_image_medium_url text DEFAULT "",
  trailer_image_large_url text DEFAULT "",
  trailer_image_maximum_url text DEFAULT "",
  aired_from_day integer DEFAULT -1,
  aired_from_month integer DEFAULT -1,
  aired_from_year integer DEFAULT -1,
  aired_to_day integer DEFAULT -1,
  aired_to_month integer DEFAULT -1,
  aired_to_year integer DEFAULT -1,
  aired_string text DEFAULT ""
);

CREATE TABLE Relation (
  id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  parent_id NOT NULL,
  child_id NOT NULL,
  relationType TEXT not null,
  
  FOREIGN KEY(parent_id) REFERENCES Anime(id),
  FOREIGN KEY(child_id) REFERENCES Anime(id),
  UNIQUE(parent_id, child_id, relationType)
);


CREATE TABLE External (
  id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  "name" text NOT NULL,
   url  text NOT NULL,
   anime_id integer NOT NULL,
  FOREIGN KEY(anime_id) REFERENCES Anime(id)
);

CREATE TABLE Streaming (
   id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  "name" text NOT NULL,
   url  text NOT NULL
);



CREATE TABLE AnimeStreaming (
   id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
   anime_id NOT NULL,
   streaming_id NOT NULL,
   FOREIGN KEY(anime_id) REFERENCES Anime(id),
  FOREIGN KEY(streaming_id) REFERENCES Streaming(id),
UNIQUE(anime_id, streaming_id)
);



CREATE TABLE ProductionEntity(
id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
favorites integer NOT NULL DEFAULT -1,
established TEXT DEFAULT "",
about TEXT DEFAULT "",
count integer NOT NULL DEFAULT -1,
image_url text DEFAULT ""
);


CREATE TABLE ProductionEntityTitle(
id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
"type" TEXT NOT NULL DEFAULT "",
title TEXT NOT NULL DEFAULT "",
parent_id integer NOT NULL,
FOREIGN KEY(parent_id) REFERENCES ProductionEntity(id)
);


CREATE TABLE AnimeProductionEntity (
  id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  anime_id NOT NULL,
  productionEntity_id NOT NULL,
  "type" TEXT CHECK( "type" IN ('S','L','P') )   NOT NULL,
  
  FOREIGN KEY(anime_id) REFERENCES Anime(id),
  FOREIGN KEY(productionEntity_id) REFERENCES ProductionEntity(id),
  UNIQUE(anime_id, productionEntity_id, "type")
);



CREATE Table UserAccount(
 id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
 username  TEXT NOT NULL UNIQUE,
 "password" TEXT not null,
  email  TEXT UNIQUE,
  CHECK (
    email LIKE '%_@_%._%' AND
    LENGTH(email) - LENGTH(REPLACE(email, '@', '')) = 1 AND
    SUBSTR(LOWER(email), 1, INSTR(email, '.') - 1) NOT GLOB '*[^@0-9a-z]*' AND
    SUBSTR(LOWER(email), INSTR(email, '.') + 1) NOT GLOB '*[^a-z]*'
  )
);



CREATE TABLE AnimeUser (
  id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  anime_id NOT NULL,
  user_id NOT NULL,
  favorite integer CHECK( favorite IN (0,1)) NOT NULL DEFAULT 0,
  "status" text CHECK ("status" IN ('watching', 'completed', 'on hold', 'dropped', 'planned')),
  FOREIGN KEY(anime_id) REFERENCES Anime(id),
  FOREIGN KEY(user_id) REFERENCES UserAccount(id),
  UNIQUE(anime_id, user_id)
);


