CREATE DATABASE hurtownia_szkola
GO

USE hurtownia_szkola
GO

CREATE TABLE Czas(
	ID_Czasu INTEGER IDENTITY(1,1) PRIMARY KEY,
	Godzina INTEGER UNIQUE NOT NULL,
	PoraDnia varchar(20) NOT NULL,
)
GO

CREATE TABLE Data
(
    ID_Daty INTEGER IDENTITY(1,1) PRIMARY KEY,
    Data datetime unique,
	Rok varchar(4),
	Miesiac varchar(15),
	DzienTygodnia varchar(15),
	Semestr varchar(10),
)
GO

CREATE TABLE Prowadzacy(
	ID_Prowadz¹cego INTEGER IDENTITY(1,1) PRIMARY KEY,
	ID_Daty_Wstawienia datetime not null,
	ID_Daty_Wygasniecia datetime,
	ID_Przelozonego INTEGER FOREIGN KEY REFERENCES Prowadzacy,
	Aktualnosc varchar(15),
	PESEL varchar(20),
	ImieINazwisko varchar(128),
	PrzedzialWiekowy varchar(20),
	PrzedzialStazu varchar(20),
	Katedra varchar(100),
	Wydzial varchar(100),
	Tytul varchar(50),
)
GO

CREATE TABLE Studenci(
	ID_Studenta INTEGER IDENTITY(1,1) PRIMARY KEY,
	Nr_Indeksu INTEGER,
	PESEL varchar(20),
	ImieINazwisko varchar(128),
)
GO

CREATE TABLE Przedmioty(
	ID_Przedmiotu INTEGER IDENTITY(1,1) PRIMARY KEY,
	Nazwa_Przedmiotu varchar(100),
	ID_Odpowiedzialnego INTEGER FOREIGN KEY REFERENCES Prowadzacy,
	PrzedzialIlosciGodzin varchar(20),
	PrzedzialIlosciEcts varchar(20),
	Semestr INTEGER,
)
GO

CREATE TABLE SkladowePrzedmiotu(
	ID_Skladowej INTEGER IDENTITY(1,1) PRIMARY KEY,
	ID_Odpowiedzialnego INTEGER FOREIGN KEY REFERENCES Prowadzacy,
	Data_Wstawienia datetime not null,
	Data_Wygasniecia datetime,
	ID_Przedmiotu INTEGER FOREIGN KEY REFERENCES Przedmioty,
	Aktualnosc varchar(15),
	PrzedzialIlosciGodzin varchar(20),
	Nazwa_Skladowej varchar(100) not null,
)
GO

CREATE TABLE Wyniki(
	ID_Studenta INTEGER FOREIGN KEY REFERENCES Studenci,
	ID_Przedmiotu INTEGER FOREIGN KEY REFERENCES Przedmioty,
	Wynik Float,
)
GO

CREATE TABLE UzyskaneOceny(
	ID_Daty INTEGER FOREIGN KEY REFERENCES Data,
	ID_Czasu INTEGER FOREIGN KEY REFERENCES Czas,
	ID_Protokolu_z_Ocena INTEGER not null,
	ID_Prowadzacego INTEGER FOREIGN KEY REFERENCES Prowadzacy,
	ID_Studenta INTEGER FOREIGN KEY REFERENCES Studenci,
	ID_Skladowej INTEGER FOREIGN KEY REFERENCES SkladowePrzedmiotu,
	Mozliwe_Punkty_do_Uzyskania Float not null,
	Wynik_Procentowy Float not null,
	Wynik_Punktowy Float not null,
	Obecnosc INTEGER not null,
)
GO

CREATE TABLE ProwadzacySkladowych(
	ID_Prowadzacego INTEGER FOREIGN KEY REFERENCES Prowadzacy,
	ID_Skladowej INTEGER FOREIGN KEY REFERENCES SkladowePrzedmiotu,
)
GO