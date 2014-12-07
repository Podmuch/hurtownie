use szkola
GO
/* nale¿y zmieniæ œcie¿kê */
BULK INSERT dbo.Prowadzacy FROM 'C:/hurtownie/bulki/prowadzacy.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.Przedmioty FROM 'C:/hurtownie/bulki/przedmioty.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.Studenci FROM 'C:/hurtownie/bulki/studenci.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.Wyniki FROM 'C:/hurtownie/bulki/wyniki.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.Rodzaje_skladowych FROM 'C:/hurtownie/bulki/rodzaje_skladowych.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.Skladowe_przedmiotow FROM 'C:/hurtownie/bulki/skladowe_przedmiotow.bulk' WITH (FIELDTERMINATOR=';')
BULK INSERT dbo.Prowadzacy_skladowych_czesci FROM 'C:/hurtownie/bulki/prowadzacy_skladowych_czesci.bulk' WITH (FIELDTERMINATOR=';')