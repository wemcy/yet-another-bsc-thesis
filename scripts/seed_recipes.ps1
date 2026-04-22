param(
    [string]$BaseUrl = "https://127.0.0.1:9393/api",
    [string]$Email = "recipe@example.com",
    [string]$Password = "Admin123!"
)

# Seed script: ~20 receptet hoz létre az API-n keresztül
# Használat: pwsh -File scripts/seed_recipes.ps1 [-BaseUrl <url>] [-Email <email>] [-Password <password>]

$ErrorActionPreference = "Stop"

# --- Bejelentkezés ---
Write-Host "Bejelentkezés mint $Email ..."
$loginBody = @{ email = $Email; password = $Password } | ConvertTo-Json
$session = New-Object Microsoft.PowerShell.Commands.WebRequestSession

try {
    Invoke-RestMethod -Uri "$BaseUrl/auth/login" `
        -Method POST `
        -ContentType "application/json" `
        -Body $loginBody `
        -WebSession $session `
        -SkipCertificateCheck | Out-Null
}
catch {
    Write-Error "Bejelentkezés sikertelen: $_"
    exit 1
}

Write-Host "Sikeres bejelentkezés."

# --- Recept definíciók ---
$recipes = @(
    @{
        title       = "Gulyásleves"
        description = "Klasszikus magyar gulyásleves marhahússal, burgonyával és csipetkével."
        allergens   = @("GLUTEN", "CELERY")
        steps       = @(
            "A hagymát apróra vágjuk és zsírban megpároljuk.",
            "Hozzáadjuk a kockára vágott marhahúst és megpirítjuk.",
            "Megszórjuk bőven pirospaprikával, megkeverjük.",
            "Felöntjük vízzel, sózzuk, hozzáadjuk a köménymagot.",
            "Amikor a hús félig puha, beletesszük a kockára vágott burgonyát, sárgarépát és zellert.",
            "Csipetkét készítünk tojásból és lisztből, beleszaggatjuk a levesbe.",
            "Addig főzzük, amíg a hús és a zöldségek megpuhulnak.",
            "Tálalás előtt friss erős paprikával és kenyérrel kínáljuk."
        )
        ingredients = @(
            @{ name = "Marha lapocka"; quantity = 500; unitOfMeasurement = "g" }
            @{ name = "Burgonya"; quantity = 300; unitOfMeasurement = "g" }
            @{ name = "Hagyma"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Sárgarépa"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Zeller"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Édes pirospaprika"; quantity = 2; unitOfMeasurement = "ek" }
            @{ name = "Zsír"; quantity = 3; unitOfMeasurement = "ek" }
            @{ name = "Liszt"; quantity = 100; unitOfMeasurement = "g" }
        )
    },
    @{
        title       = "Chicken Tikka Masala"
        description = "Omlós csirkemell krémes, fűszeres paradicsomos szószban."
        allergens   = @("MILK")
        steps       = @(
            "A csirkemellet joghurtban, citromlében és fűszerekben legalább 1 órát pácoljuk.",
            "Grillen vagy serpenyőben pirosra sütjük a csirkét.",
            "Külön serpenyőben vajon megdinszteljük a hagymát és fokhagymát.",
            "Hozzáadjuk a garam masalát, köményt, paprikát és chiliport; 1 percig pirítjuk.",
            "Hozzákeverjük a paradicsompürét és a tejszínt; 15 percig pároljuk.",
            "A kész csirkét a szószhoz adjuk és átmelegítjük.",
            "Basmati rizzsel vagy naan kenyérrel tálaljuk."
        )
        ingredients = @(
            @{ name = "Csirkemell"; quantity = 600; unitOfMeasurement = "g" }
            @{ name = "Natúr joghurt"; quantity = 150; unitOfMeasurement = "ml" }
            @{ name = "Tejszín"; quantity = 200; unitOfMeasurement = "ml" }
            @{ name = "Paradicsompüré"; quantity = 400; unitOfMeasurement = "g" }
            @{ name = "Hagyma"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Fokhagyma gerezd"; quantity = 3; unitOfMeasurement = "db" }
            @{ name = "Garam masala"; quantity = 2; unitOfMeasurement = "tk" }
            @{ name = "Vaj"; quantity = 30; unitOfMeasurement = "g" }
        )
    },
    @{
        title       = "Caesar saláta"
        description = "Ropogós római saláta Caesar öntettel, krutonnal és parmezánnal."
        allergens   = @("GLUTEN", "EGGS", "FISH", "MILK")
        steps       = @(
            "A római salátát megmossuk és falatnyi darabokra vágjuk.",
            "Elkeverjük a tojássárgáját, citromlevet, dijoni mustárt, fokhagymát és szardellát.",
            "Lassan hozzácsurgatjuk az olívaolajat keverés közben.",
            "A salátát az öntettel összekeverjük.",
            "Krutonnal és reszelt parmezánnal megszórjuk.",
            "Fekete borssal ízesítjük és azonnal tálaljuk."
        )
        ingredients = @(
            @{ name = "Római saláta"; quantity = 2; unitOfMeasurement = "fej" }
            @{ name = "Tojássárgája"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Citromlé"; quantity = 2; unitOfMeasurement = "ek" }
            @{ name = "Szardella filé"; quantity = 3; unitOfMeasurement = "db" }
            @{ name = "Parmezán sajt"; quantity = 50; unitOfMeasurement = "g" }
            @{ name = "Kruton"; quantity = 100; unitOfMeasurement = "g" }
            @{ name = "Olívaolaj"; quantity = 80; unitOfMeasurement = "ml" }
            @{ name = "Fokhagyma gerezd"; quantity = 1; unitOfMeasurement = "db" }
        )
    },
    @{
        title       = "Marhapörkölt galuskával"
        description = "Hagyományos magyar marhapörkölt házi nokedlivel."
        allergens   = @("GLUTEN", "EGGS")
        steps       = @(
            "A hagymát apróra vágjuk és zsírban üvegesre pároljuk.",
            "Hozzáadjuk a kockára vágott marhahúst és megpirítjuk.",
            "Megszórjuk pirospaprikával, megkeverjük.",
            "Hozzáadjuk az apróra vágott paradicsomot és paprikát.",
            "Kevés vízzel felöntjük és fedő alatt puhára pároljuk.",
            "A galuskához lisztet, tojást és sót összedolgozunk.",
            "A nokedli tésztát forrásban lévő sós vízbe szaggatjuk.",
            "A pörköltöt galuskával és tejföllel tálaljuk."
        )
        ingredients = @(
            @{ name = "Marha lapocka"; quantity = 600; unitOfMeasurement = "g" }
            @{ name = "Hagyma"; quantity = 3; unitOfMeasurement = "db" }
            @{ name = "Édes pirospaprika"; quantity = 2; unitOfMeasurement = "ek" }
            @{ name = "Paradicsom"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Zöldpaprika"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Liszt"; quantity = 300; unitOfMeasurement = "g" }
            @{ name = "Tojás"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Zsír"; quantity = 3; unitOfMeasurement = "ek" }
        )
    },
    @{
        title       = "Zöldséges wok"
        description = "Gyors és színes zöldségmix szójás-gyömbéres szószban."
        allergens   = @("SOYBEANS", "SESAMESEEDS")
        steps       = @(
            "A zöldségeket előkészítjük: felszeleteljük a paprikát, brokkolit, hónapos retket és sárgarépát.",
            "Szezámolajat hevítünk wokban erős tűzön.",
            "A fokhagymát és gyömbért 30 másodpercig pirítjuk.",
            "Hozzáadjuk a sárgarépát és brokkolit; 2 percig sütjük.",
            "Beletesszük a paprikát és a cukorborsót; további 2 percig pirítjuk.",
            "Ráöntjük a szójaszószt, rizsecetet és egy csipet cukrot.",
            "Összekeverjük és párolt rizzsel tálaljuk."
        )
        ingredients = @(
            @{ name = "Kaliforniai paprika"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Brokkoli"; quantity = 200; unitOfMeasurement = "g" }
            @{ name = "Cukorborsó"; quantity = 150; unitOfMeasurement = "g" }
            @{ name = "Sárgarépa"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Szójaszósz"; quantity = 3; unitOfMeasurement = "ek" }
            @{ name = "Szezámolaj"; quantity = 2; unitOfMeasurement = "ek" }
            @{ name = "Friss gyömbér"; quantity = 1; unitOfMeasurement = "ek" }
            @{ name = "Fokhagyma gerezd"; quantity = 2; unitOfMeasurement = "db" }
        )
    },
    @{
        title       = "Margherita pizza"
        description = "Klasszikus nápolyi pizza paradicsommal, friss mozzarellával és bazsalikommal."
        allergens   = @("GLUTEN", "MILK")
        steps       = @(
            "Elkészítjük a pizzatésztát és 1 órát kelesztjük.",
            "A sütőt 250°C-ra előmelegítjük, pizzakővel ha van.",
            "A tésztát lisztezett felületen vékony koronggá nyújtjuk.",
            "Vékony réteg paradicsomszószt kenünk a tésztára.",
            "A friss mozzarellát feltépkedjük és elosztjuk a pizzán.",
            "8-10 percig sütjük, amíg a tészta aranybarna és a sajt buborékos.",
            "Friss bazsalikomlevelekkel és egy csepp olívaolajjal tálaljuk."
        )
        ingredients = @(
            @{ name = "Pizzatészta"; quantity = 250; unitOfMeasurement = "g" }
            @{ name = "Paradicsomszósz"; quantity = 100; unitOfMeasurement = "ml" }
            @{ name = "Friss mozzarella"; quantity = 200; unitOfMeasurement = "g" }
            @{ name = "Friss bazsalikom"; quantity = 10; unitOfMeasurement = "levél" }
            @{ name = "Olívaolaj"; quantity = 1; unitOfMeasurement = "ek" }
            @{ name = "Só"; quantity = 1; unitOfMeasurement = "tk" }
        )
    },
    @{
        title       = "Thai zöld curry"
        description = "Illatos, kókusztejes curry csirkével és zöldségekkel."
        allergens   = @("FISH", "CRUSTACEANS")
        steps       = @(
            "Kókuszolajat hevítünk egy nagy lábasban közepes lángon.",
            "A zöld curry pasztát 1-2 percig pirítjuk, amíg illatozni kezd.",
            "Hozzáadjuk a szeletelt csirkemellet és rápirítjuk.",
            "Felöntjük kókusztejjel és csirkealaplével; felforralás után lassú tűzre vesszük.",
            "Beletesszük a babkukoricát, bambuszrügyet és zöldbabot.",
            "15 percig pároljuk, amíg a csirke átfő.",
            "Halszósszal, cukorral és lime lével ízesítjük.",
            "Jázmin rizzsel és thai bazsalikommal tálaljuk."
        )
        ingredients = @(
            @{ name = "Csirkemell"; quantity = 400; unitOfMeasurement = "g" }
            @{ name = "Kókusztej"; quantity = 400; unitOfMeasurement = "ml" }
            @{ name = "Zöld curry paszta"; quantity = 3; unitOfMeasurement = "ek" }
            @{ name = "Halszósz"; quantity = 2; unitOfMeasurement = "ek" }
            @{ name = "Zöldbab"; quantity = 100; unitOfMeasurement = "g" }
            @{ name = "Babkukorica"; quantity = 100; unitOfMeasurement = "g" }
            @{ name = "Bambuszrügy"; quantity = 100; unitOfMeasurement = "g" }
            @{ name = "Jázmin rizs"; quantity = 300; unitOfMeasurement = "g" }
        )
    },
    @{
        title       = "Gombás risottó"
        description = "Krémes olasz rizsétel vargánya- és csiperkegombával."
        allergens   = @("MILK", "SULPHURDIOXIDE")
        steps       = @(
            "A szárított vargányát 20 percig forró vízben áztatjuk; a levét félretesszük.",
            "A hagymát vajon üvegesre dinszteljük.",
            "Hozzáadjuk az arborio rizst és 2 percig kevergetve pirítjuk.",
            "Felöntjük fehérborral és addig keverjük, amíg felszívódik.",
            "A meleg alaplevet merőkanalanként adjuk hozzá, folyamatosan keverve.",
            "15 perc után hozzáadjuk a szeletelt csiperkét és az áztatott vargányát.",
            "Addig adagoljuk az alaplevet, amíg a rizs al dente és krémes.",
            "Parmezánt keverünk bele, sóval és borssal ízesítjük."
        )
        ingredients = @(
            @{ name = "Arborio rizs"; quantity = 300; unitOfMeasurement = "g" }
            @{ name = "Csiperkegomba"; quantity = 200; unitOfMeasurement = "g" }
            @{ name = "Szárított vargánya"; quantity = 30; unitOfMeasurement = "g" }
            @{ name = "Hagyma"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Fehérbor"; quantity = 100; unitOfMeasurement = "ml" }
            @{ name = "Zöldségleves alaplé"; quantity = 1; unitOfMeasurement = "L" }
            @{ name = "Parmezán sajt"; quantity = 60; unitOfMeasurement = "g" }
            @{ name = "Vaj"; quantity = 40; unitOfMeasurement = "g" }
        )
    },
    @{
        title       = "Hortobágyi húsos palacsinta"
        description = "Húsos töltelékkel készült palacsinta, pirospaprikás tejfölös szószban."
        allergens   = @("GLUTEN", "EGGS", "MILK")
        steps       = @(
            "Elkészítjük a palacsinta tésztát lisztből, tojásból, tejből és sóból.",
            "Vékony palacsintákat sütünk serpenyőben.",
            "A borjú- vagy csirkehúst megfőzzük és ledaráljuk.",
            "A hagymát zsírban megpároljuk, pirospaprikával fűszerezzük.",
            "A darált húst a hagymás alappal összekeverjük, tejföllel lazítjuk.",
            "A palacsintákat megtöltjük és feltekerve tepsibe rakjuk.",
            "Tejfölös-paprikás szósszal leöntjük.",
            "Sütőben 180°C-on 15 percig melegítjük, majd tálaljuk."
        )
        ingredients = @(
            @{ name = "Liszt"; quantity = 200; unitOfMeasurement = "g" }
            @{ name = "Tojás"; quantity = 3; unitOfMeasurement = "db" }
            @{ name = "Tej"; quantity = 300; unitOfMeasurement = "ml" }
            @{ name = "Borjúhús vagy csirkemell"; quantity = 400; unitOfMeasurement = "g" }
            @{ name = "Hagyma"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Tejföl"; quantity = 300; unitOfMeasurement = "ml" }
            @{ name = "Édes pirospaprika"; quantity = 1; unitOfMeasurement = "ek" }
            @{ name = "Zsír"; quantity = 2; unitOfMeasurement = "ek" }
        )
    },
    @{
        title       = "Banános palacsinta"
        description = "Puha, banános palacsinta, tökéletes hétvégi reggelihez."
        allergens   = @("GLUTEN", "EGGS", "MILK")
        steps       = @(
            "Az érett banánt villával pépesre nyomjuk.",
            "Hozzákeverjük a tojásokat, tejet és az olvasztott vajat.",
            "Külön tálba összekeverjük a lisztet, cukrot, sütőport és sót.",
            "A nedves és száraz hozzávalókat óvatosan összekeverjük (ne dolgozzuk túl).",
            "Tapadásmentes serpenyőt közepes lángra teszünk és enyhén megzsírozzuk.",
            "A tésztát a serpenyőbe öntjük és sütjük, amíg buborékok jelennek meg; megfordítjuk.",
            "Juharsziruppal és friss bogyós gyümölcsökkel tálaljuk."
        )
        ingredients = @(
            @{ name = "Érett banán"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Tojás"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Tej"; quantity = 200; unitOfMeasurement = "ml" }
            @{ name = "Liszt"; quantity = 200; unitOfMeasurement = "g" }
            @{ name = "Cukor"; quantity = 2; unitOfMeasurement = "ek" }
            @{ name = "Sütőpor"; quantity = 2; unitOfMeasurement = "tk" }
            @{ name = "Vaj"; quantity = 30; unitOfMeasurement = "g" }
            @{ name = "Juharszirup"; quantity = 3; unitOfMeasurement = "ek" }
        )
    },
    @{
        title       = "Töltött káposzta"
        description = "Savanyú káposztába tekert húsos töltelék, tejfölösen tálalva."
        allergens   = @("GLUTEN", "EGGS")
        steps       = @(
            "A savanyú káposzta leveleket szétválogatjuk és kiöblítjük.",
            "A darált sertéshúst összegyúrjuk a rizzsel, tojással, sóval, borssal és fokhagymával.",
            "Minden káposzta levélbe egy-egy adagot teszünk és szorosan feltekerünk.",
            "Egy nagy lábas aljára aprított káposztát terítünk.",
            "Ráhelyezzük a töltött káposztákat szorosan egymás mellé.",
            "Felöntjük vízzel, fedő alatt lassú tűzön 1,5-2 órát főzzük.",
            "Tejföllel és friss kenyérrel tálaljuk."
        )
        ingredients = @(
            @{ name = "Savanyú káposzta"; quantity = 1; unitOfMeasurement = "kg" }
            @{ name = "Darált sertéshús"; quantity = 500; unitOfMeasurement = "g" }
            @{ name = "Rizs"; quantity = 100; unitOfMeasurement = "g" }
            @{ name = "Tojás"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Hagyma"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Fokhagyma gerezd"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Tejföl"; quantity = 200; unitOfMeasurement = "ml" }
            @{ name = "Füstölt kolbász"; quantity = 200; unitOfMeasurement = "g" }
        )
    },
    @{
        title       = "Pad Thai"
        description = "Klasszikus thai pirított rizstészta garnélával, mogyoróval és lime-mal."
        allergens   = @("CRUSTACEANS", "PEANUTS", "EGGS", "FISH", "SOYBEANS")
        steps       = @(
            "A rizstésztát 20 percig langyos vízben áztatjuk, majd leszűrjük.",
            "Összekeverjük a tamarind pasztát, halszószt, cukrot és lime levet a szószhoz.",
            "Wokban olajat hevítünk; a garnélát pirosra sütjük, majd félretesszük.",
            "A wokban tojást rántunk és darabokra törjük.",
            "Hozzáadjuk a tésztát és a szószt; addig keverjük, amíg a tészta bevonódik.",
            "Visszatesszük a garnélát, hozzáadjuk a csírát és újhagymát.",
            "Darált mogyoróval, lime gerezdekkel és korianderrel tálaljuk."
        )
        ingredients = @(
            @{ name = "Rizstészta"; quantity = 200; unitOfMeasurement = "g" }
            @{ name = "Garnéla"; quantity = 300; unitOfMeasurement = "g" }
            @{ name = "Tojás"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Tamarind paszta"; quantity = 2; unitOfMeasurement = "ek" }
            @{ name = "Halszósz"; quantity = 3; unitOfMeasurement = "ek" }
            @{ name = "Mogyoró"; quantity = 50; unitOfMeasurement = "g" }
            @{ name = "Szójababcsíra"; quantity = 100; unitOfMeasurement = "g" }
            @{ name = "Lime"; quantity = 1; unitOfMeasurement = "db" }
        )
    },
    @{
        title       = "Francia hagymaleves"
        description = "Karamellizált hagymás leves pirított bagettel és gruyère sajttal."
        allergens   = @("GLUTEN", "MILK", "SULPHURDIOXIDE")
        steps       = @(
            "A hagymát vékony szeletekre vágjuk. Vajon lassú tűzön olvasztjuk.",
            "Hozzáadjuk a hagymát egy csipet sóval; 40-50 percig pároljuk kevergetve, amíg mélyen karamellizálódik.",
            "Hozzáadjuk a fokhagymát és kakukkfüvet; 1 percig pirítjuk.",
            "Fehérborral feloldjuk az odaragadt részt, amíg elpárolog.",
            "Marha alaplevet öntünk hozzá és 20 percig főzzük.",
            "Tűzálló tálakba meritjük a levest, pirított bagett szeleteket teszünk rá.",
            "Bőségesen megszórjuk reszelt gruyère sajttal és grillen pirosra sütjük."
        )
        ingredients = @(
            @{ name = "Hagyma"; quantity = 6; unitOfMeasurement = "db" }
            @{ name = "Vaj"; quantity = 50; unitOfMeasurement = "g" }
            @{ name = "Marha alaplé"; quantity = 1; unitOfMeasurement = "L" }
            @{ name = "Fehérbor"; quantity = 100; unitOfMeasurement = "ml" }
            @{ name = "Bagett"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Gruyère sajt"; quantity = 150; unitOfMeasurement = "g" }
            @{ name = "Fokhagyma gerezd"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Friss kakukkfű"; quantity = 3; unitOfMeasurement = "szál" }
        )
    },
    @{
        title       = "Vöröslencse krémleves"
        description = "Laktató, meleg leves vöröslencsével, köménnyel és citrommal."
        allergens   = @("CELERY")
        steps       = @(
            "Olívaolajat hevítünk lábasban. Megdinszteljük a hagymát, sárgarépát és zellert.",
            "Hozzáadjuk a fokhagymát, köményt és füstölt paprikát; 1 percig pirítjuk.",
            "Beletesszük a megmosott vöröslencsét és felöntjük zöldségalapléval.",
            "Felforralás után lassú tűzön 25 percet főzzük.",
            "Részben vagy egészben botmixerrel pürésítjük.",
            "Citromlével, sóval és borssal ízesítjük.",
            "Friss kenyérrel és egy csepp olívaolajjal tálaljuk."
        )
        ingredients = @(
            @{ name = "Vöröslencse"; quantity = 250; unitOfMeasurement = "g" }
            @{ name = "Hagyma"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Sárgarépa"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Szárzeller"; quantity = 2; unitOfMeasurement = "szál" }
            @{ name = "Zöldségleves alaplé"; quantity = 1; unitOfMeasurement = "L" }
            @{ name = "Őrölt kömény"; quantity = 1; unitOfMeasurement = "tk" }
            @{ name = "Citrom"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Olívaolaj"; quantity = 2; unitOfMeasurement = "ek" }
        )
    },
    @{
        title       = "Csokis láva torta"
        description = "Egyéni adagolású sütemény meleg, folyós csokis belsővel."
        allergens   = @("GLUTEN", "EGGS", "MILK")
        steps       = @(
            "A sütőt 220°C-ra előmelegítjük. A formákat kizsírozzuk és kakaóporral meghintjük.",
            "Az étcsokoládét és vajat vízgőz felett együtt megolvasztjuk.",
            "A tojásokat és cukrot habosra keverjük.",
            "Az olvasztott csokoládét óvatosan a tojásmasszába forgatjuk.",
            "A lisztet beleszitáljuk és óvatosan összekeverjük.",
            "A tésztát elosztjuk a formákban.",
            "10-12 percig sütjük — a széle legyen szilárd, a közepe lágy.",
            "Tányérra borítjuk és azonnal tejszínhabbal vagy fagylalttal tálaljuk."
        )
        ingredients = @(
            @{ name = "Étcsokoládé"; quantity = 150; unitOfMeasurement = "g" }
            @{ name = "Vaj"; quantity = 100; unitOfMeasurement = "g" }
            @{ name = "Tojás"; quantity = 3; unitOfMeasurement = "db" }
            @{ name = "Cukor"; quantity = 80; unitOfMeasurement = "g" }
            @{ name = "Liszt"; quantity = 30; unitOfMeasurement = "g" }
            @{ name = "Kakaópor"; quantity = 1; unitOfMeasurement = "ek" }
        )
    },
    @{
        title       = "Gazpacho"
        description = "Hideg spanyol paradicsomleves — frissítő forró nyári napokra."
        allergens   = @("GLUTEN")
        steps       = @(
            "A paradicsomot, uborkát, paprikát és hagymát durván felvágjuk.",
            "A száraz kenyeret 5 percig vízben áztatjuk; kinyomkodjuk.",
            "Az összes zöldséget kenyérrel, fokhagymával, olívaolajjal és sherryecettel turmixoljuk.",
            "Sóval és borssal ízesítjük.",
            "Ha selymes állagot szeretnénk, szűrőn áttörjük.",
            "Legalább 2 órát hűtőben pihentetjük.",
            "Hidegen tálaljuk kockára vágott uborkával, krutonnal és olívaolajjal."
        )
        ingredients = @(
            @{ name = "Érett paradicsom"; quantity = 1; unitOfMeasurement = "kg" }
            @{ name = "Uborka"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Kaliforniai paprika"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Száraz kenyér"; quantity = 100; unitOfMeasurement = "g" }
            @{ name = "Fokhagyma gerezd"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Olívaolaj"; quantity = 80; unitOfMeasurement = "ml" }
            @{ name = "Sherryecet"; quantity = 2; unitOfMeasurement = "ek" }
            @{ name = "Lilahagyma"; quantity = 0.5; unitOfMeasurement = "db" }
        )
    },
    @{
        title       = "Vajcsirke (Butter Chicken)"
        description = "Enyhe, krémes paradicsomos indiai curry omlós csirkével."
        allergens   = @("MILK", "NUTS")
        steps       = @(
            "A csirkehúst joghurtban, citromlében, chiliporban és kurkumában 1 órát pácoljuk.",
            "Grillen vagy serpenyőben pirosra sütjük.",
            "Vajon megpároljuk a gyömbér-fokhagyma pasztát.",
            "Hozzáadjuk a paradicsompürét és 10 percig főzzük.",
            "Belekeverjük a tejszínt, mézet, görögszéna leveleket és garam masalát.",
            "Hozzáadjuk a csirkét és 10 percig pároljuk.",
            "Tejszínnel és kesudió darabokkal díszítjük. Naan kenyérrel vagy rizzsel tálaljuk."
        )
        ingredients = @(
            @{ name = "Csirkecomb"; quantity = 600; unitOfMeasurement = "g" }
            @{ name = "Natúr joghurt"; quantity = 100; unitOfMeasurement = "ml" }
            @{ name = "Paradicsompüré"; quantity = 400; unitOfMeasurement = "g" }
            @{ name = "Tejszín"; quantity = 150; unitOfMeasurement = "ml" }
            @{ name = "Vaj"; quantity = 50; unitOfMeasurement = "g" }
            @{ name = "Kesudió"; quantity = 30; unitOfMeasurement = "g" }
            @{ name = "Garam masala"; quantity = 2; unitOfMeasurement = "tk" }
            @{ name = "Méz"; quantity = 1; unitOfMeasurement = "ek" }
        )
    },
    @{
        title       = "Shakshuka"
        description = "Fűszeres paradicsomos-paprikás szószban sült tojás — közel-keleti klasszikus."
        allergens   = @("EGGS")
        steps       = @(
            "Olívaolajat hevítünk mély serpenyőben. Megdinszteljük a hagymát és paprikát.",
            "Hozzáadjuk a fokhagymát, köményt, paprikát és chilit; 1 percig pirítjuk.",
            "Ráöntjük a darált paradicsomot és 10 percig pároljuk, amíg besűrűsödik.",
            "Kis mélyedéseket formálunk a szószban és beletörjük a tojásokat.",
            "Lefedve, lassú tűzön 5-8 percig főzzük, amíg a tojásfehérje megköt.",
            "Morzsolt feta sajttal és friss petrezselyemmel megszórjuk.",
            "A serpenyőből kínáljuk, ropogós kenyérrel."
        )
        ingredients = @(
            @{ name = "Tojás"; quantity = 6; unitOfMeasurement = "db" }
            @{ name = "Darált paradicsom"; quantity = 400; unitOfMeasurement = "g" }
            @{ name = "Kaliforniai paprika"; quantity = 2; unitOfMeasurement = "db" }
            @{ name = "Hagyma"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Fokhagyma gerezd"; quantity = 3; unitOfMeasurement = "db" }
            @{ name = "Kömény"; quantity = 1; unitOfMeasurement = "tk" }
            @{ name = "Pirospaprika"; quantity = 1; unitOfMeasurement = "tk" }
            @{ name = "Feta sajt"; quantity = 50; unitOfMeasurement = "g" }
        )
    },
    @{
        title       = "Japán ramen"
        description = "Gazdag sertés alapleves ramen chashuval, lágy tojással és norival."
        allergens   = @("GLUTEN", "EGGS", "SOYBEANS", "SESAMESEEDS")
        steps       = @(
            "Sertéscsontokat, fokhagymát, gyömbért és újhagymát 4+ órán át főzünk az alapléhez.",
            "A szeletelt sertésoldalast szójaszószban, mirinben és cukorban pácolva puhára pároljuk.",
            "A tojásokat lágyra főzzük (6-7 perc), meghámozzuk és szóját-mirines lében áztatjuk.",
            "A ramen tésztát a csomagolás szerint megfőzzük.",
            "Az alaplevet leszűrjük, szójaszósszal, miso pasztával és szezámolajjal ízesítjük.",
            "A tésztát tálakba osztjuk, ráöntjük a forró alaplevet.",
            "Chashu sertéssel, marinált tojással, norival, újhagymával és szezámmaggal díszítjük."
        )
        ingredients = @(
            @{ name = "Ramen tészta"; quantity = 400; unitOfMeasurement = "g" }
            @{ name = "Sertéscsont"; quantity = 1; unitOfMeasurement = "kg" }
            @{ name = "Sertés oldalas"; quantity = 300; unitOfMeasurement = "g" }
            @{ name = "Tojás"; quantity = 4; unitOfMeasurement = "db" }
            @{ name = "Szójaszósz"; quantity = 4; unitOfMeasurement = "ek" }
            @{ name = "Miso paszta"; quantity = 2; unitOfMeasurement = "ek" }
            @{ name = "Nori lap"; quantity = 4; unitOfMeasurement = "db" }
            @{ name = "Szezámmag"; quantity = 1; unitOfMeasurement = "ek" }
        )
    },
    @{
        title       = "Guacamole"
        description = "Friss avokádókrém lime-mal, korianderrel és csípős paprikával."
        allergens   = @()
        steps       = @(
            "Az érett avokádókat kettévágjuk, kimagozzuk, a húst kikanálazzuk.",
            "Villával a kívánt állagúra nyomkodjuk.",
            "A lilahagymát, paradicsomot és jalapeñót apróra vágjuk.",
            "Az avokádóba keverjük az aprított korianderrel együtt.",
            "Lime levet, sót és egy csipet köményt adunk hozzá.",
            "Megkóstoljuk és igény szerint ízesítjük.",
            "Azonnal tálaljuk tortilla chipsszel vagy köretként."
        )
        ingredients = @(
            @{ name = "Érett avokádó"; quantity = 3; unitOfMeasurement = "db" }
            @{ name = "Lime"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Lilahagyma"; quantity = 0.5; unitOfMeasurement = "db" }
            @{ name = "Paradicsom"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Jalapeño"; quantity = 1; unitOfMeasurement = "db" }
            @{ name = "Friss koriander"; quantity = 1; unitOfMeasurement = "csokor" }
            @{ name = "Só"; quantity = 0.5; unitOfMeasurement = "tk" }
        )
    }
)

# --- Receptek létrehozása ---
$created = 0
$failed = 0

foreach ($recipe in $recipes) {
    $body = $recipe | ConvertTo-Json -Depth 4
    try {
        $response = Invoke-RestMethod -Uri "$BaseUrl/recipes" `
            -Method POST `
            -ContentType "application/json" `
            -Body ([System.Text.Encoding]::UTF8.GetBytes($body)) `
            -WebSession $session `
            -SkipCertificateCheck

        $created++
        Write-Host "[OK] $($recipe.title)" -ForegroundColor Green
    }
    catch {
        $failed++
        $status = $_.Exception.Response.StatusCode
        Write-Host "[HIBA] $($recipe.title) — $status $_" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "Kész: $created létrehozva, $failed sikertelen, összesen $($recipes.Count) recept."
