using LinqConsoleLab.PL.Data;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    /// <summary>
    /// Zadanie:
    /// Wyszukaj wszystkich studentów mieszkających w Warsaw.
    /// Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    /// FROM Studenci
    /// WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        return DaneUczelni.Studenci
            .Where(s => s.Miasto == "Warsaw")
            .Select(s => $"{s.NumerIndeksu} | {s.Imie} {s.Nazwisko} | {s.Miasto}");
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj listę adresów e-mail wszystkich studentów.
    /// Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci
            .Select(s => s.Email);
    }

    /// <summary>
    /// Zadanie:
    /// Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    /// Zwróć numer indeksu i pełne imię i nazwisko.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko
    /// FROM Studenci
    /// ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        return DaneUczelni.Studenci
            .OrderBy(s => s.Nazwisko)
            .ThenBy(s => s.Imie)
            .Select(s => $"{s.NumerIndeksu} | {s.Imie} {s.Nazwisko}");
    }

    /// <summary>
    /// Zadanie:
    /// Znajdź pierwszy przedmiot z kategorii Analytics.
    /// Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///
    /// SQL:
    /// SELECT TOP 1 Nazwa, DataStartu
    /// FROM Przedmioty
    /// WHERE Kategoria = 'Analytics';
    /// </summary>
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var przedmiot = DaneUczelni.Przedmioty.FirstOrDefault(p => p.Kategoria == "Analytics");
        if (przedmiot is null)
            return ["Brak przedmiotów z kategorii Analytics."];
        return [$"{przedmiot.Nazwa} | start: {przedmiot.DataStartu:yyyy-MM-dd}"];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    /// Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        var wynik = DaneUczelni.Zapisy.Any(z => !z.CzyAktywny);
        return [$"Czy istnieje nieaktywny zapis: {(wynik ? "Tak" : "Nie")}"];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    /// Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    /// THEN 1 ELSE 0 END
    /// FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        var wynik = DaneUczelni.Prowadzacy.All(p => !string.IsNullOrWhiteSpace(p.Katedra));
        return [$"Czy wszyscy prowadzący mają katedrę: {(wynik ? "Tak" : "Nie")}"];
    }

    /// <summary>
    /// Zadanie:
    /// Policz, ile aktywnych zapisów znajduje się w systemie.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Zapisy
    /// WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var liczba = DaneUczelni.Zapisy.Count(z => z.CzyAktywny);
        return [$"Liczba aktywnych zapisów: {liczba}"];
    }

    /// <summary>
    /// Zadanie:
    /// Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///
    /// SQL:
    /// SELECT DISTINCT Miasto
    /// FROM Studenci
    /// ORDER BY Miasto;
    /// </summary>
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        return DaneUczelni.Studenci
            .Select(s => s.Miasto)
            .Distinct()
            .OrderBy(m => m);
    }

    /// <summary>
    /// Zadanie:
    /// Zwróć trzy najnowsze zapisy na przedmioty.
    /// W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///
    /// SQL:
    /// SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    /// FROM Zapisy
    /// ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(z => $"{z.DataZapisu:yyyy-MM-dd} | student: {z.StudentId} | przedmiot: {z.PrzedmiotId}");
    }

    /// <summary>
    /// Zadanie:
    /// Zaimplementuj prostą paginację dla listy przedmiotów.
    /// Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///
    /// SQL:
    /// SELECT Nazwa, Kategoria
    /// FROM Przedmioty
    /// ORDER BY Nazwa
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        return DaneUczelni.Przedmioty
            .OrderBy(p => p.Nazwa)
            .Skip(2)
            .Take(2)
            .Select(p => $"{p.Nazwa} | {p.Kategoria}");
    }

    /// <summary>
    /// Zadanie:
    /// Połącz studentów z zapisami po StudentId.
    /// Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, z.DataZapisu
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (s, z) => $"{s.Imie} {s.Nazwisko} | {z.DataZapisu:yyyy-MM-dd}"
            );
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    /// Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, p.Nazwa
    /// FROM Zapisy z
    /// JOIN Studenci s ON s.Id = z.StudentId
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        return DaneUczelni.Studenci
            .SelectMany(
                s => DaneUczelni.Zapisy.Where(z => z.StudentId == s.Id),
                (s, z) => new { Student = s, Zapis = z }
            )
            .Join(
                DaneUczelni.Przedmioty,
                sz => sz.Zapis.PrzedmiotId,
                p => p.Id,
                (sz, p) => $"{sz.Student.Imie} {sz.Student.Nazwisko} | {p.Nazwa}"
            );
    }

    /// <summary>
    /// Zadanie:
    /// Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///
    /// SQL:
    /// SELECT p.Nazwa, COUNT(*)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        return DaneUczelni.Zapisy
            .Join(
                DaneUczelni.Przedmioty,
                z => z.PrzedmiotId,
                p => p.Id,
                (z, p) => new { Zapis = z, Przedmiot = p }
            )
            .GroupBy(x => x.Przedmiot.Nazwa)
            .Select(g => $"{g.Key} | liczba zapisów: {g.Count()}");
    }

    /// <summary>
    /// Zadanie:
    /// Oblicz średnią ocenę końcową dla każdego przedmiotu.
    /// Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///
    /// SQL:
    /// SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa.HasValue)
            .Join(
                DaneUczelni.Przedmioty,
                z => z.PrzedmiotId,
                p => p.Id,
                (z, p) => new { Zapis = z, Przedmiot = p }
            )
            .GroupBy(x => x.Przedmiot.Nazwa)
            .Select(g => $"{g.Key} | średnia: {g.Average(x => x.Zapis.OcenaKoncowa!.Value):F2}");
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    /// W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                pr => pr.Id,
                p => p.ProwadzacyId,
                (pr, przedmioty) => $"{pr.Imie} {pr.Nazwisko} | liczba przedmiotów: {przedmioty.Count()}"
            );
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    /// Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa.HasValue)
            .Join(
                DaneUczelni.Studenci,
                z => z.StudentId,
                s => s.Id,
                (z, s) => new { Zapis = z, Student = s }
            )
            .GroupBy(x => new { x.Student.Imie, x.Student.Nazwisko })
            .Select(g => $"{g.Key.Imie} {g.Key.Nazwisko} | najwyższa ocena: {g.Max(x => x.Zapis.OcenaKoncowa!.Value):F1}");
    }

    /// <summary>
    /// Wyzwanie:
    /// Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    /// Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Imie, s.Nazwisko
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.CzyAktywny)
            .Join(
                DaneUczelni.Studenci,
                z => z.StudentId,
                s => s.Id,
                (z, s) => s
            )
            .GroupBy(s => new { s.Id, s.Imie, s.Nazwisko })
            .Where(g => g.Count() > 1)
            .Select(g => $"{g.Key.Imie} {g.Key.Nazwisko} | aktywne zapisy: {g.Count()}");
    }

    /// <summary>
    /// Wyzwanie:
    /// Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///
    /// SQL:
    /// SELECT p.Nazwa
    /// FROM Przedmioty p
    /// JOIN Zapisy z ON p.Id = z.PrzedmiotId
    /// WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    /// GROUP BY p.Nazwa
    /// HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        return DaneUczelni.Przedmioty
            .Where(p => p.DataStartu.Month == 4 && p.DataStartu.Year == 2026)
            .Join(
                DaneUczelni.Zapisy,
                p => p.Id,
                z => z.PrzedmiotId,
                (p, z) => new { Przedmiot = p, Zapis = z }
            )
            .GroupBy(x => x.Przedmiot.Nazwa)
            .Where(g => g.All(x => x.Zapis.OcenaKoncowa == null))
            .Select(g => g.Key);
    }

    /// <summary>
    /// Wyzwanie:
    /// Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    /// Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        throw Niezaimplementowano(nameof(Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach));
    }

    /// <summary>
    /// Wyzwanie:
    /// Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    /// Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///
    /// SQL:
    /// SELECT s.Miasto, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Miasto
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        throw Niezaimplementowano(nameof(Wyzwanie04_MiastaILiczbaAktywnychZapisow));
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
