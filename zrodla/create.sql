CREATE DATABASE szkola
GO

USE szkola
GO

CREATE TABLE Prowadzacy
(
	Id_pracownika integer PRIMARY KEY,
	Pesel varchar (20) NOT NULL,
	Tytul varchar(20) NOT NULL,
	Staz integer NOT NULL,
	Wiek integer NOT NULL,
	Imie varchar(20) NOT NULL,
	Nazwisko varchar(30) NOT NULL,
	Katedra varchar(100) NOT NULL,
	Wydzial varchar(100) NOT NULL,	
);
GO

CREATE TABLE Przedmioty
(
	Nazwa varchar(100) PRIMARY KEY,
	Semestr integer NOT NULL,
	Ilosc_godzin integer NOT NULL,
	Ilosc_ects integer NOT NULL,
	Fk_gl_prowadz integer NOT NULL REFERENCES Prowadzacy(Id_pracownika),
);
GO

CREATE TABLE Studenci
(
	Nr_indeksu integer PRIMARY KEY,
	Pesel varchar (20) NOT NULL,
	Imie varchar(20) NOT NULL,
	Nazwisko varchar(30) NOT NULL,
	Rok integer NOT NULL,
	Semestr integer NOT NULL,
	Dlug_ects integer NOT NULL,
);
GO

CREATE TABLE Wyniki
(
	Fk_Student integer NOT NULL REFERENCES Studenci(Nr_indeksu),
	Fk_Przedmiot varchar(100) NOT NULL REFERENCES Przedmioty(Nazwa),
	Wynik float NOT NULL,
);
GO

CREATE TABLE Rodzaje_skladowych
(
	Nazwa varchar(100) PRIMARY KEY,
);
GO

CREATE TABLE Skladowe_przedmiotow
(
	Id_skladowej integer PRIMARY KEY,
	Fk_Przedmiot varchar(100) NOT NULL REFERENCES Przedmioty(Nazwa),
	Fk_skladowa varchar(100) NOT NULL REFERENCES Rodzaje_skladowych(Nazwa),
	Ilosc_godzin integer NOT NULL,
	Fk_osoby_odp integer NOT NULL REFERENCES Prowadzacy(Id_pracownika),
);
GO

CREATE TABLE Prowadzacy_skladowych_czesci
(
	Fk_id_pracownika integer NOT NULL REFERENCES Prowadzacy(Id_pracownika),
	Fk_id_skladowej integer NOT NULL REFERENCES Skladowe_przedmiotow(Id_skladowej),
);
GO
