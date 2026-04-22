#!/usr/bin/env node

import fs from 'node:fs/promises'
import path from 'node:path'

const ALLERGEN = {
    GLUTEN: 'GLUTEN',
    CRUSTACEANS: 'CRUSTACEANS',
    EGGS: 'EGGS',
    FISH: 'FISH',
    PEANUTS: 'PEANUTS',
    SOYBEANS: 'SOYBEANS',
    MILK: 'MILK',
    NUTS: 'NUTS',
    CELERY: 'CELERY',
    MUSTARD: 'MUSTARD',
    SESAMESEEDS: 'SESAMESEEDS',
    SULPHURDIOXIDE: 'SULPHURDIOXIDE',
    LUPIN: 'LUPIN',
    MOLLUSCS: 'MOLLUSCS',
}

function entry(name, allergens = []) {
    return {
        name,
        allergens: [...new Set(allergens)],
    }
}

function addGroup(target, names, allergens = []) {
    for (const name of names) {
        target.push(entry(name, allergens))
    }
}

function normalizeName(name) {
    return name.trim().toLocaleLowerCase('hu-HU').replace(/\s+/g, ' ')
}

function parseArgs(argv) {
    const options = {
        dryRun: false,
        insecure: false,
        exportPath: null,
        limit: null,
        apiBaseUrl: process.env.API_BASE_URL ?? 'http://localhost:5173/api',
        adminEmail: process.env.ADMIN_EMAIL ?? process.env.userEmail ?? null,
        adminPassword: process.env.ADMIN_PASSWORD ?? process.env.userPassword ?? null,
        authCookie: process.env.AUTH_COOKIE ?? null,
    }

    for (let index = 0; index < argv.length; index += 1) {
        const arg = argv[index]

        if (arg === '--dry-run') {
            options.dryRun = true
            continue
        }

        if (arg === '--insecure') {
            options.insecure = true
            continue
        }

        if (arg === '--export') {
            options.exportPath = argv[index + 1] ?? null
            index += 1
            continue
        }

        if (arg === '--limit') {
            const raw = argv[index + 1] ?? ''
            const parsed = Number.parseInt(raw, 10)
            options.limit = Number.isFinite(parsed) && parsed > 0 ? parsed : null
            index += 1
            continue
        }

        if (arg === '--api-base-url') {
            options.apiBaseUrl = argv[index + 1] ?? options.apiBaseUrl
            index += 1
            continue
        }

        if (arg === '--email') {
            options.adminEmail = argv[index + 1] ?? options.adminEmail
            index += 1
            continue
        }

        if (arg === '--password') {
            options.adminPassword = argv[index + 1] ?? options.adminPassword
            index += 1
            continue
        }

        if (arg === '--cookie') {
            options.authCookie = argv[index + 1] ?? options.authCookie
            index += 1
        }
    }

    return options
}

function buildDataset() {
    const items = []

    addGroup(
        items,
        [
            'burgonya',
            'édesburgonya',
            'vöröshagyma',
            'lilahagyma',
            'fokhagyma',
            'póréhagyma',
            'újhagyma',
            'medvehagyma',
            'metélőhagyma',
            'salottahagyma',
            'sárgarépa',
            'petrezselyemgyökér',
            'fehérrépa',
            'paszternák',
            'karalábé',
            'karfiol',
            'brokkoli',
            'fejes káposzta',
            'vöröskáposzta',
            'kelkáposzta',
            'kelbimbó',
            'kínai kel',
            'fodros kel',
            'spenót',
            'bébispenót',
            'sóska',
            'mángold',
            'fejes saláta',
            'jégsaláta',
            'rukkola',
            'madársaláta',
            'uborka',
            'csemegeuborka',
            'koktéluborka',
            'cukkini',
            'padlizsán',
            'főzőtök',
            'sütőtök',
            'patisszon',
            'paradicsom',
            'koktélparadicsom',
            'zöldparadicsom',
            'tv paprika',
            'kápia paprika',
            'kaliforniai paprika',
            'pritamin paprika',
            'csípős paprika',
            'chili paprika',
            'jalapeno',
            'hegyes erős paprika',
            'zöldborsó',
            'zöldbab',
            'vajbab',
            'kukorica',
            'csemegekukorica',
            'cékla',
            'retek',
            'fekete retek',
            'daikon retek',
            'torma',
            'csiperkegomba',
            'barna csiperke',
            'laskagomba',
            'shiitake gomba',
            'vargánya',
            'zöldspárga',
            'fehér spárga',
            'articsóka',
            'édeskömény',
            'okra',
            'savanyú káposzta',
            'bambuszrügy',
            'vízitorma',
            'rebarbara',
            'pak choi',
            'csillagtök',
            'csicsóka',
            'gomba',
            'paradicsompaprika',
            'mini uborka',
            'saláta mix',
            'zsenge répa',
            'zöldfokhagyma',
            'lila burgonya',
            'fehér hagyma',
            'gyöngyhagyma',
            'tarlórépa',
        ],
        [],
    )
    addGroup(items, ['zeller', 'zellerszár', 'zellergumó'], [ALLERGEN.CELERY])

    addGroup(items, [
        'alma',
        'körte',
        'birsalma',
        'szilva',
        'ringló',
        'mirabella szilva',
        'őszibarack',
        'nektarin',
        'sárgabarack',
        'kajszi',
        'cseresznye',
        'meggy',
        'eper',
        'szamóca',
        'málna',
        'szeder',
        'áfonya',
        'vörösáfonya',
        'ribizli',
        'feketeribizli',
        'egres',
        'szőlő',
        'vörös szőlő',
        'fehér szőlő',
        'mazsola',
        'narancs',
        'vérnarancs',
        'mandarin',
        'citrom',
        'lime',
        'grapefruit',
        'pomelo',
        'kumquat',
        'banán',
        'ananász',
        'mangó',
        'papaya',
        'kiwi',
        'gránátalma',
        'füge',
        'datolya',
        'aszalt szilva',
        'aszalt sárgabarack',
        'aszalt füge',
        'aszalt datolya',
        'aszalt vörösáfonya',
        'görögdinnye',
        'sárgadinnye',
        'kókusz',
        'avokádó',
        'passiógyümölcs',
        'licsi',
        'guava',
        'naspolya',
        'csipkebogyó',
        'bodzabogyó',
        'narancshéj',
        'citromhéj',
        'limehéj',
        'kandírozott narancshéj',
    ])

    addGroup(items, [
        'petrezselyemzöld',
        'kapor',
        'bazsalikom',
        'oregánó',
        'kakukkfű',
        'rozmaring',
        'majoránna',
        'zsálya',
        'tárkony',
        'korianderzöld',
        'menta',
        'citromfű',
        'babérlevél',
        'lestyán',
        'curry por',
        'kurkuma',
        'őrölt kömény',
        'egész kömény',
        'római kömény',
        'fekete bors',
        'fehér bors',
        'zöld bors',
        'cayenne bors',
        'fűszerpaprika',
        'csípős fűszerpaprika',
        'füstölt paprika',
        'fahéj',
        'szegfűszeg',
        'szerecsendió',
        'szerecsendió-virág',
        'kardamom',
        'csillagánizs',
        'ánizs',
        'vanília',
        'vaníliarúd',
        'vaníliás cukor',
        'sütőpor',
        'szódabikarbóna',
        'friss élesztő',
        'instant élesztő',
        'zselatin',
        'agar-agar',
        'só',
        'tengeri só',
        'füstölt só',
        'cukor',
        'barna cukor',
        'porcukor',
        'nádcukor',
        'méz',
        'juharszirup',
        'melasz',
        'étkezési keményítő',
        'burgonyakeményítő',
        'kukoricakeményítő',
        'pektin',
        'citromsav',
        'szegfűbors',
        'borókabogyó',
        'sáfrány',
        'chilipehely',
        'fokhagymapor',
        'hagymapor',
        'gyömbér',
        'őrölt gyömbér',
        'kurkumagyökér',
        'őrölt koriander',
        'vanillin',
        'narancsvirágvíz',
        'rózsavíz',
        'gombapor',
        'szumák',
        'currypaszta',
        'gyros fűszerkeverék',
        'olasz fűszerkeverék',
        'provance-i fűszerkeverék',
        'szárított paradicsompor',
    ])
    addGroup(items, ['mustármag'], [ALLERGEN.MUSTARD])
    addGroup(items, ['fehér szezámmag', 'fekete szezámmag'], [ALLERGEN.SESAMESEEDS])

    addGroup(items, [
        'tej',
        'laktózmentes tej',
        'kecsketej',
        'juhtej',
        'tejszín',
        'főzőtejszín',
        'habtejszín',
        'tejföl',
        'joghurt',
        'görög joghurt',
        'kefir',
        'túró',
        'ricotta',
        'mascarpone',
        'krémsajt',
        'vaj',
        'író',
        'sajt',
        'trappista sajt',
        'cheddar sajt',
        'mozzarella',
        'parmezán',
        'gouda sajt',
        'feta',
        'camembert',
        'tejpor',
    ], [ALLERGEN.MILK])
    addGroup(items, ['tojás', 'fürjtojás', 'tojásfehérje', 'tojássárgája'], [ALLERGEN.EGGS])

    addGroup(items, [
        'csirkemell',
        'csirkecomb',
        'csirkeszárny',
        'csirkeaprólék',
        'pulykamell',
        'pulykacomb',
        'kacsa',
        'kacsamell',
        'libacomb',
        'sertéscomb',
        'sertéslapocka',
        'sertéstarja',
        'sertésoldalas',
        'sertésszűz',
        'marhahátszín',
        'marhalábszár',
        'marhapofa',
        'borjúhús',
        'báránycomb',
        'báránylapocka',
        'darált marhahús',
        'darált sertéshús',
        'darált pulykahús',
        'darált csirkehús',
        'bacon',
        'sonka',
        'főtt sonka',
        'füstölt sonka',
        'kolbász',
        'csabai kolbász',
        'virsli',
        'szalonna',
        'pancetta',
        'császárszalonna',
        'csirkemáj',
        'libamáj',
        'szalámi',
        'chorizo',
        'nyúlhús',
        'szarvashús',
    ])

    addGroup(items, [
        'lazac',
        'füstölt lazac',
        'tonhal',
        'tonhalkonzerv',
        'tőkehal',
        'hekk',
        'harcsa',
        'ponty',
        'pisztráng',
        'makréla',
        'szardínia',
        'hering',
        'angolna',
        'fogas',
        'süllő',
        'keszeg',
        'szardella',
        'surimi',
        'ikra',
    ], [ALLERGEN.FISH])
    addGroup(items, ['kagyló', 'fekete kagyló', 'osztriga', 'tintahal', 'polip', 'kagylóhús', 'tintahal karika'], [ALLERGEN.MOLLUSCS])
    addGroup(items, ['rák', 'garnéla', 'homár', 'languszta'], [ALLERGEN.CRUSTACEANS])

    addGroup(items, [
        'búzaliszt',
        'finomliszt',
        'rétesliszt',
        'kenyérliszt',
        'teljes kiőrlésű búzaliszt',
        'rozsliszt',
        'árpaliszt',
        'tönkölyliszt',
        'zabliszt',
        'zabpehely',
        'zabkorpa',
        'búzadara',
        'bulgur',
        'kuszkusz',
        'árpagyöngy',
        'durumtészta',
        'spagetti',
        'penne',
        'fusilli',
        'makaróni',
        'lasagne lap',
        'tagliatelle',
        'gnocchi',
        'kenyér',
        'fehér kenyér',
        'teljes kiőrlésű kenyér',
        'rozskenyér',
        'zsemle',
        'kifli',
        'pita',
        'tortilla',
        'leveles tészta',
        'réteslap',
        'zsemlemorzsa',
        'keksz',
        'ostyalap',
        'palacsintaliszt',
        'zabkása alap',
        'müzli',
        'granola',
        'rozspehely',
        'búzacsíra',
        'korpás keksz',
        'abonett',
        'pirított kenyérkocka',
        'perec',
        'bagett',
        'pita chips',
    ], [ALLERGEN.GLUTEN])
    addGroup(items, ['csuszatészta', 'tarhonya', 'babapiskóta'], [ALLERGEN.GLUTEN, ALLERGEN.EGGS])
    addGroup(items, ['croissant', 'kalács', 'briós'], [ALLERGEN.GLUTEN, ALLERGEN.EGGS, ALLERGEN.MILK])
    addGroup(items, [
        'kukoricaliszt',
        'kukoricadara',
        'rizsliszt',
        'hajdinaliszt',
        'kölesliszt',
        'amaránt',
        'quinoa',
        'hajdina',
        'köles',
        'rizs',
        'jázminrizs',
        'basmati rizs',
        'barna rizs',
        'kerekszemű rizs',
        'arborio rizs',
        'tortilla chips',
    ])

    addGroup(items, [
        'vöröslencse',
        'barna lencse',
        'zöld lencse',
        'csicseriborsó',
        'fehérbab',
        'tarkabab',
        'vörösbab',
        'fekete bab',
        'mungóbab',
        'fenyőmag',
        'napraforgómag',
        'tökmag',
        'lenmag',
        'chia mag',
        'mák',
        'kendermag',
        'sárgaborsó',
        'feles borsó',
        'babcsíra',
        'csicseriborsóliszt',
        'vöröslencse liszt',
    ])
    addGroup(items, ['szójabab', 'tofu', 'tempeh', 'szójagranulátum', 'edamame', 'szójatej', 'szójakocka'], [ALLERGEN.SOYBEANS])
    addGroup(items, ['földimogyoró', 'mogyoróvaj', 'mogyoróolaj'], [ALLERGEN.PEANUTS])
    addGroup(items, [
        'mandula',
        'mandulaliszt',
        'dió',
        'darált dió',
        'mogyoró',
        'kesudió',
        'pisztácia',
        'pekándió',
        'brazil dió',
        'makadámdió',
        'mandulatej',
        'mogyorókrém',
        'dióolaj',
        'diókrém',
    ], [ALLERGEN.NUTS])
    addGroup(items, ['szezámmag', 'tahini', 'szezámolaj'], [ALLERGEN.SESAMESEEDS])
    addGroup(items, ['csillagfürtliszt', 'csillagfürt'], [ALLERGEN.LUPIN])

    addGroup(items, [
        'napraforgóolaj',
        'olívaolaj',
        'repceolaj',
        'tökmagolaj',
        'ecet',
        'almaecet',
        'balzsamecet',
        'borecet',
        'rizsecet',
        'ketchup',
        'paradicsompüré',
        'passata',
        'sűrített paradicsom',
        'kókusztej',
        'kókuszkrém',
        'baracklekvár',
        'szilvalekvár',
        'eperlekvár',
        'étcsokoládé',
        'kakaóvaj',
        'kávé',
        'instant kávé',
        'zöld tea',
        'fekete tea',
        'vaníliakivonat',
        'rumaroma',
        'sütőrum',
        'kapribogyó',
        'olajbogyó',
        'datolyaszirup',
        'almachips',
        'aszalt paradicsomolajban',
        'savanyított gyömbér',
        'sriracha',
    ])
    addGroup(items, ['szójaszósz', 'teriyaki szósz'], [ALLERGEN.SOYBEANS, ALLERGEN.GLUTEN])
    addGroup(items, ['tamari'], [ALLERGEN.SOYBEANS])
    addGroup(items, ['worcestershire-szósz', 'halszósz'], [ALLERGEN.FISH])
    addGroup(items, ['mustár', 'dijoni mustár'], [ALLERGEN.MUSTARD])
    addGroup(items, ['majonéz'], [ALLERGEN.EGGS, ALLERGEN.MUSTARD])
    addGroup(items, ['tejcsokoládé', 'fehér csokoládé'], [ALLERGEN.MILK])
    addGroup(items, ['zöldségalaplé', 'csirkealaplé', 'marhaalaplé', 'húsleveskocka', 'zöldségleveskocka'], [ALLERGEN.CELERY])
    addGroup(items, ['pesto'], [ALLERGEN.NUTS, ALLERGEN.MILK])

    const deduped = []
    const seen = new Set()

    for (const item of items) {
        const key = normalizeName(item.name)
        if (seen.has(key)) {
            throw new Error(`Duplicate ingredient name in dataset: ${item.name}`)
        }
        seen.add(key)
        deduped.push(item)
    }

    if (deduped.length !== 500) {
        throw new Error(`Expected exactly 500 ingredients, got ${deduped.length}`)
    }

    return deduped
}

async function maybeExportDataset(dataset, exportPath) {
    if (!exportPath) return

    const resolvedPath = path.resolve(process.cwd(), exportPath)
    await fs.mkdir(path.dirname(resolvedPath), { recursive: true })
    await fs.writeFile(resolvedPath, JSON.stringify(dataset, null, 2), 'utf8')
    console.log(`Dataset exported to ${resolvedPath}`)
}

async function loginAndGetCookie(apiBaseUrl, email, password) {
    const response = await fetch(`${apiBaseUrl}/auth/login`, {
        method: 'POST',
        headers: {
            'content-type': 'application/json',
        },
        body: JSON.stringify({
            email,
            password,
        }),
    })

    if (!response.ok) {
        const body = await response.text()
        throw new Error(`Login failed with ${response.status}: ${body}`)
    }

    const cookieHeaders =
        typeof response.headers.getSetCookie === 'function'
            ? response.headers.getSetCookie()
            : [response.headers.get('set-cookie')].filter(Boolean)

    const sessionCookie = cookieHeaders.map((value) => value.split(';')[0]).join('; ')

    if (!sessionCookie) {
        throw new Error('Login succeeded, but no session cookie was returned.')
    }

    return sessionCookie
}

async function searchIngredient(apiBaseUrl, name) {
    const url = new URL(`${apiBaseUrl}/ingredients/`)
    url.searchParams.set('name', name)

    const response = await fetch(url)
    if (!response.ok) {
        const body = await response.text()
        throw new Error(`Search failed for "${name}" with ${response.status}: ${body}`)
    }

    return response.json()
}

async function createIngredient(apiBaseUrl, ingredient, cookie) {
    const response = await fetch(`${apiBaseUrl}/ingredients/`, {
        method: 'POST',
        headers: {
            'content-type': 'application/json',
            cookie,
        },
        body: JSON.stringify(ingredient),
    })

    if (!response.ok) {
        const body = await response.text()
        throw new Error(`Create failed for "${ingredient.name}" with ${response.status}: ${body}`)
    }
}

async function seedIngredients(dataset, options) {
    let authCookie = options.authCookie

    if (!authCookie) {
        if (!options.adminEmail || !options.adminPassword) {
            throw new Error(
                'Missing admin authentication. Provide AUTH_COOKIE or ADMIN_EMAIL and ADMIN_PASSWORD.',
            )
        }

        authCookie = await loginAndGetCookie(
            options.apiBaseUrl,
            options.adminEmail,
            options.adminPassword,
        )
    }

    let created = 0
    let skipped = 0

    for (const [index, ingredient] of dataset.entries()) {
        const existing = await searchIngredient(options.apiBaseUrl, ingredient.name)
        const exactMatch = Array.isArray(existing)
            ? existing.some((item) => normalizeName(item.name) === normalizeName(ingredient.name))
            : false

        if (exactMatch) {
            skipped += 1
            console.log(`[${index + 1}/${dataset.length}] skip  ${ingredient.name}`)
            continue
        }

        await createIngredient(options.apiBaseUrl, ingredient, authCookie)
        created += 1
        console.log(`[${index + 1}/${dataset.length}] create ${ingredient.name}`)
    }

    console.log('')
    console.log(`Finished. Created: ${created}, skipped existing: ${skipped}, total: ${dataset.length}`)
}

async function main() {
    const options = parseArgs(process.argv.slice(2))

    if (options.insecure) {
        process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'
    }

    const dataset = buildDataset()
    const limitedDataset = options.limit ? dataset.slice(0, options.limit) : dataset

    await maybeExportDataset(dataset, options.exportPath)

    if (options.dryRun) {
        console.log(`Dry run only. Dataset size: ${dataset.length}`)
        console.log(`Target API base URL: ${options.apiBaseUrl}`)
        console.log(`TLS verification: ${options.insecure ? 'disabled' : 'enabled'}`)
        console.log('Sample:')
        console.log(JSON.stringify(limitedDataset.slice(0, 5), null, 2))
        return
    }

    await seedIngredients(limitedDataset, options)
}

main().catch((error) => {
    console.error(error instanceof Error ? error.message : error)
    process.exitCode = 1
})
