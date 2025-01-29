# CurrencyRates - Aplikacja do wyświetlania kursów walut

## Opis projektu

CurrencyRates to aplikacja webowa stworzona w technologii ASP.NET Core (MVC), która umożliwia pobieranie i wyświetlanie kursów walut z API Narodowego Banku Polskiego (NBP). Projekt jest zorganizowany w sposób modularny, z podziałem na warstwy logiczne, co ułatwia rozwój i testowanie aplikacji.

---

## Funkcjonalności

- Pobieranie kursów walut z API NBP.
- Wyświetlanie danych w podziale na daty.
- Obsługa filtrowania kursów walut dla wybranej daty.
- Struktura wspierająca testowanie jednostkowe i BDD.
- Konteneryzacja aplikacji przy użyciu Dockera.

---

## Architektura projektu

Aplikacja składa się z następujących modułów:

1. **CurrencyRates**
   - Warstwa frontend + backend (kontrolery, modele, widoki).
   - Obsługa żądań użytkownika i prezentacja danych.

2. **CurrencyRates.Services**
   - Logika biznesowa aplikacji.
   - Wywołania do API NBP i przetwarzanie danych.

3. **CurrencyRates.Repository**
   - Warstwa zarządzania danymi (przechowywanie i pobieranie kursów walut).
   - Wykorzystuje EF Core z bazą InMemory.

4. **CurrencyRates.Tests**
   - Testy jednostkowe i testy BDD.
   - Pokrycie kluczowych funkcji serwisu i repozytorium.

---

## Technologie i narzędzia

- **Backend:** ASP.NET Core MVC 8.0
- **Baza danych:** EF Core (InMemory)
- **API:** NBP API (https://api.nbp.pl)
- **Konteneryzacja:** Docker (Linux)
- **Testy:** xUnit, Moq

---

## Wymagania systemowe

- .NET SDK 8.0
- Docker
- Visual Studio 2022 (lub nowsze)

---

## Instalacja i uruchomienie

1. **Klonowanie repozytorium:**
   ```bash
   git clone https://github.com/your-repo/currency-rates.git
   cd currency-rates

2. **Uruchomienie aplikacji z Dockera:**
   ```bash
   docker-compose up

3. Uruchomienie aplikacji lokalnie:
   - Otwórz projekt w Visual Studio
   - Ustaw CurrencyRates jako projekt startowy
   - Kliknij Run (F5)
