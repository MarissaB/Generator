/* Uncomment query to generate JSON text to copy/paste into respective file under /Data/Seeds/<tablename>.json */
/* If a table is empty, no results will be produced.*/

SELECT Name, Image, CreatureCapacity, TreasureCapacity FROM Vessel FOR JSON AUTO
SELECT Name, Image, Description, Rarity, Category, Size FROM Treasure FOR JSON AUTO
SELECT Name, Image, Description FROM Creature FOR JSON AUTO
SELECT Name, Image, TreasureCapacity, TreasureMaxSize FROM Container FOR JSON AUTO

SELECT Name, Image, ReligionCapacity, ArtisanCapacity, SpecialtyShopCapacity FROM Outpost FOR JSON AUTO
SELECT Name, Image, Description FROM ReligiousSite FOR JSON AUTO
SELECT Name, Image, Description FROM Artisan FOR JSON AUTO
SELECT Name, Image, Description FROM SpecialtyShop FOR JSON AUTO