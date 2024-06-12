select * from AnimeProductionEntity as ape JOIN ProductionEntity as pe on ape.productionEntity_id == pe.id 
JOIN ProductionEntityTitle as pet on pe.id == pet.id WHERE anime_id == 55813 GROUP BY pe.id;