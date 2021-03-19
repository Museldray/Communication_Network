# Projekt 3 – Sieć komunikacyjna

Zaimplementować projekt wspomagający obsługę sieci internetowej. Projekt powinien składać się z
następujących funkcjonalności:
1. Struktura drzewiasta umożliwiająca szybkie (log n) wykonywanie operacji (20 pkt):
- wyszukiwanie urządzenia (po adresie IP4 X.X.X.X)
- dodawanie nowego urządzenia (adres IP4 X.X.X.X)
- usuwanie istniejącego urządzenia (po adresie)
- wyszukanie liczby komputerów w danej podsieci X.X.X.* (maska 24-bitowa)

Wariant A: implementacja drzewa AVL (20 pkt), lub drzewa BST (10 pkt)
Wariant B: implementacja 2-3 drzewa (20 pkt), lub drzewa BST (10 pkt)

2. Struktura grafu wraz z następującymi funkcjonalnościami (20 pkt):
- dodawanie/usuwanie połączenia pomiędzy routerami (podsieciami X.X.X.*)
- znalezienie najszybszego połączenia pomiędzy zadaną parą komputerow

> UWAGA: koszt danego połączenia liczony jest jako iloraz przepustowości maksymalnej (100 Gbs) i
jego przepustowości np. dla łącza 100 Mbs jego koszt wynosi 1000.
Algorytmy grafowe będą oceniane pod względem poprawności i szybkości działania operacji

3. Możliwość wczytania, modyfikacji danych i testowania na podstawie pliku z instrukcjami:
* DK ip4 - dodanie komputera o zadanym adresie
* UK ip4 – usunięcie komputera o zadanym adresie
* WK ip4 – wyszukanie komputera o zadanym adresie (wypisz TAK/NIE)
* LK podsiec – wypisanie liczby komputerow w danej podsieci (bez routera, który zawsze
istnieje w podsieci)
* WY – wypisanie struktury drzewa
* DP podsiec1 podsiec2 szybkosc (np. 253.25.18 253.22.17 100M) – dodanie połączenia
pomiędzy routerami (podsieciami) o danej przepustowości (100 M, może być też np. 10G).
* UP podsiec1 podsiec2 – usunięcie połączenia
* NP ip4 ip4 – obliczenie najszybszego połączenia pomiędzy zadaną parą komputerów (liczone
jako suma kosztów), przy założeniu że uwzględniamy jedynie koszt przesyłu pomiędzy routerami
(podsieciami)

> W przypadku nie istnienia w drzewie komputerów, między którymi chcemy wyszukać najszybsze
połączenie lub w przypadku nie istnienia samego połączenia, należy wypisać NIE.
