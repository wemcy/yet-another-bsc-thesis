# Thesis setup a projekthez

Ez a mappa mar nem csak az ELTE sablon nyers masolata, hanem a receptkezelo alkalmazashoz elokeszitett dolgozatvaz. A fo belepesi pont az [elteikthesis_hu.tex](elteikthesis_hu.tex), amely mar projekt-specifikus fejezetekre bontva hivatkozik a szovegallomanyokra.

## Mappaszerkezet

- `elteikthesis_hu.tex`: a fo dokumentum, itt kell atirni a cimet, a sajat nevet, a temavezeto nevet es a vedes evet.
- `chapters/`: a fo fejezetek kulon `.tex` allomanyokban.
- `appendices/`: fuggelekek es kiegeszito anyagok.
- `images/`: abrak, diagramok, exportalt wireframe-ek.
- `elteikthesis.bib`: irodalomjegyzek.
- `build.ps1`: helyi fordítás PowerShellbol.

## Mire erdemes eloszor figyelni

1. Töltsd ki az `elteikthesis_hu.tex` metaadatait.
2. A `chapters/introduction.tex`, `chapters/requirements.tex` es `chapters/architecture.tex` jo kiindulasi pontok a korai irashoz.
3. Ha kepeket akarsz beszurni, tedd azokat az `images/` konyvtarba.
4. A repositoryban levo `docs/usecases` es `docs/wireframes` anyagokat hasznald forraskent a szoveghez.

## Lokalis fordítás

Jelenleg ehhez a gephez nincs telepitve LaTeX toolchain, ezért a fordítashoz elobb telepiteni kell egy teljes disztribuciot.

Szukseges eszkozok:

- `pdflatex`
- `biber`
- ajanlottan TeX Live vagy MiKTeX

Forditas PowerShellbol:

```powershell
./build.ps1
```

Takaritas:

```powershell
./build.ps1 -Clean
```

A script a kimenetet a `build/` konyvtarba helyezi. A kesz PDF varhato helye: `build/elteikthesis_hu.pdf`.

## Kezi fordítás

Ha nem a PowerShell scriptet hasznalod, ugyanaz a folyamat kezzel is lefuttathato:

```bash
pdflatex -interaction=nonstopmode -file-line-error -output-directory=build elteikthesis_hu.tex
biber --input-directory build --output-directory build elteikthesis_hu
pdflatex -interaction=nonstopmode -file-line-error -output-directory=build elteikthesis_hu.tex
pdflatex -interaction=nonstopmode -file-line-error -output-directory=build elteikthesis_hu.tex
```

## VS Code

A workspace szintu feladatok koze bekerult egy thesis build task, valamint ajanlott kiegeszitokent a LaTeX Workshop. Ha ezt az extensiont hasznalod, a szerkesztes es PDF eloallitas kenyelmesebben menedzselheto lesz.

## Megjegyzes a sablonrol

Az alapul szolgalo `elteikthesis.cls` tovabbra is az ELTE IK dolgozatsablonra epul. A sablon tamogatja a magyar es angol nyelvu dokumentumokat, valamint a szokasos kep-, tablazat- es irodalomjegyzek kezelesi lehetosegeket is.
