#!/usr/bin/env node

import fs from 'node:fs/promises'
import { performance } from 'node:perf_hooks'

const ALLERGENS = [
    'GLUTEN',
    'CRUSTACEANS',
    'EGGS',
    'FISH',
    'PEANUTS',
    'SOYBEANS',
    'MILK',
    'NUTS',
    'CELERY',
    'MUSTARD',
    'SESAMESEEDS',
    'SULPHURDIOXIDE',
    'LUPIN',
    'MOLLUSCS',
]

const INGREDIENTS = [
    'flour',
    'shrimp',
    'egg',
    'cod',
    'peanut',
    'soy sauce',
    'milk',
    'almond',
    'celery',
    'mustard',
    'sesame',
    'white wine',
    'lupin flour',
    'mussel',
]

function parseArgs(argv) {
    const options = {
        apiBaseUrl: process.env.API_BASE_URL ?? 'https://localhost:9393/api',
        adminEmail: process.env.ADMIN_EMAIL ?? 'recipe@example.com',
        adminPassword: process.env.ADMIN_PASSWORD ?? 'Admin123!',
        authCookie: process.env.AUTH_COOKIE ?? null,
        concurrency: 12,
        counts: [100, 10000],
        dryRun: false,
        forceSeed: false,
        insecure: true,
        iterations: 5,
        output: null,
        skipSeed: false,
        tag: 'allergen',
        warmup: 1,
    }

    for (let index = 0; index < argv.length; index += 1) {
        const arg = argv[index]

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
            continue
        }

        if (arg === '--counts') {
            options.counts = parseCounts(argv[index + 1])
            index += 1
            continue
        }

        if (arg === '--count') {
            options.counts = parseCounts(argv[index + 1])
            index += 1
            continue
        }

        if (arg === '--concurrency') {
            options.concurrency = parsePositiveInteger(argv[index + 1], options.concurrency)
            index += 1
            continue
        }

        if (arg === '--iterations') {
            options.iterations = parsePositiveInteger(argv[index + 1], options.iterations)
            index += 1
            continue
        }

        if (arg === '--warmup') {
            options.warmup = parsePositiveInteger(argv[index + 1], options.warmup)
            index += 1
            continue
        }

        if (arg === '--tag') {
            options.tag = sanitizeTag(argv[index + 1] ?? options.tag)
            index += 1
            continue
        }

        if (arg === '--output') {
            options.output = argv[index + 1] ?? options.output
            index += 1
            continue
        }

        if (arg === '--dry-run') {
            options.dryRun = true
            continue
        }

        if (arg === '--force-seed') {
            options.forceSeed = true
            continue
        }

        if (arg === '--skip-seed') {
            options.skipSeed = true
            continue
        }

        if (arg === '--insecure') {
            options.insecure = true
        }
    }

    return options
}

function parseCounts(raw) {
    const counts = String(raw ?? '')
        .split(',')
        .map((value) => Number.parseInt(value.trim(), 10))
        .filter((value) => Number.isFinite(value) && value > 0)

    if (counts.length === 0) {
        throw new Error(
            'Expected --counts to contain at least one positive number, for example: --counts 100,10000',
        )
    }

    return [...new Set(counts)]
}

function parsePositiveInteger(raw, fallback) {
    const parsed = Number.parseInt(raw ?? '', 10)
    return Number.isFinite(parsed) && parsed > 0 ? parsed : fallback
}

function sanitizeTag(tag) {
    return String(tag || 'allergen').replace(/[^a-zA-Z0-9_-]/g, '-')
}

function datasetPrefix(tag, count) {
    return `Stress ${tag} c${count}x`
}

function createRecipe(index, count, tag) {
    const primaryIndex = index % ALLERGENS.length
    const secondaryIndex = Math.floor(index / ALLERGENS.length) % ALLERGENS.length
    const tertiaryIndex = (index * 7) % ALLERGENS.length
    const allergens = [
        ...new Set([ALLERGENS[primaryIndex], ALLERGENS[secondaryIndex], ALLERGENS[tertiaryIndex]]),
    ]
    const paddedIndex = String(index + 1).padStart(String(count).length, '0')

    return {
        title: `${datasetPrefix(tag, count)} Recipe ${paddedIndex} Searchable Meal`,
        description: `Generated stress recipe ${index + 1} of ${count} with ${allergens.join(', ')} allergens.`,
        steps: [
            'Prepare the measured ingredients.',
            'Cook the generated base until done.',
            'Serve the repeatable benchmark recipe.',
        ],
        ingredients: allergens.map((allergen, allergenIndex) => ({
            name: `${INGREDIENTS[ALLERGENS.indexOf(allergen)]} ${paddedIndex}-${allergenIndex + 1}`,
            quantity: 100 + allergenIndex * 25,
            unitOfMeasurement: 'g',
            allergens: [allergen],
        })),
    }
}

function createDataset(count, tag) {
    return Array.from({ length: count }, (_, index) => createRecipe(index, count, tag))
}

async function loginAndGetCookie(apiBaseUrl, email, password) {
    const response = await fetch(`${apiBaseUrl}/auth/login`, {
        method: 'POST',
        headers: { 'content-type': 'application/json' },
        body: JSON.stringify({ email, password }),
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
    if (!sessionCookie) throw new Error('Login succeeded, but no session cookie was returned.')

    return sessionCookie
}

async function fetchJson(url, options = {}) {
    const startedAt = performance.now()
    const response = await fetch(url, options)
    const elapsedMs = performance.now() - startedAt
    const bodyText = await response.text()

    if (!response.ok) {
        throw new Error(
            `${options.method ?? 'GET'} ${url} failed with ${response.status}: ${bodyText}`,
        )
    }

    return {
        body: bodyText ? JSON.parse(bodyText) : null,
        elapsedMs,
        pagination: parsePaginationHeader(response.headers.get('x-pagination')),
    }
}

function parsePaginationHeader(header) {
    if (!header) return null

    try {
        return JSON.parse(header)
    } catch {
        return null
    }
}

async function recipeExists(apiBaseUrl, title) {
    const url = new URL(`${apiBaseUrl}/search`)
    url.searchParams.set('title', title)

    const { body } = await fetchJson(url)
    return Array.isArray(body) && body.some((recipe) => recipe.title === title)
}

async function createRecipeRequest(apiBaseUrl, recipe, cookie) {
    await fetchJson(`${apiBaseUrl}/recipes/`, {
        method: 'POST',
        headers: {
            'content-type': 'application/json',
            cookie,
        },
        body: JSON.stringify(recipe),
    })
}

async function runWithConcurrency(items, concurrency, worker) {
    let cursor = 0
    let completed = 0
    const workerCount = Math.min(concurrency, items.length)

    await Promise.all(
        Array.from({ length: workerCount }, async () => {
            while (cursor < items.length) {
                const index = cursor
                cursor += 1
                await worker(items[index], index)
                completed += 1
                if (completed % 250 === 0 || completed === items.length) {
                    process.stdout.write(`\rSeeded ${completed}/${items.length}`)
                }
            }
        }),
    )

    process.stdout.write('\n')
}

async function seedDataset(dataset, options, cookie) {
    const first = dataset[0]
    const last = dataset.at(-1)

    if (
        !options.forceSeed &&
        (await recipeExists(options.apiBaseUrl, first.title)) &&
        (await recipeExists(options.apiBaseUrl, last.title))
    ) {
        console.log(
            `Seed skipped: ${dataset.length} recipes with prefix "${datasetPrefix(options.tag, dataset.length)}" already exist.`,
        )
        return
    }

    console.log(
        `Seeding ${dataset.length} recipes with prefix "${datasetPrefix(options.tag, dataset.length)}"...`,
    )
    await runWithConcurrency(dataset, options.concurrency, async (recipe) => {
        await createRecipeRequest(options.apiBaseUrl, recipe, cookie)
    })
}

function benchmarkCases(apiBaseUrl, tag, count) {
    const prefix = datasetPrefix(tag, count)

    return [
        {
            name: 'list include MILK',
            url: createRecipeUrl(apiBaseUrl, '/recipes', {
                title: prefix,
                includeAllergens: ['MILK'],
                pageSize: '100',
            }),
        },
        {
            name: 'list exclude GLUTEN',
            url: createRecipeUrl(apiBaseUrl, '/recipes', {
                title: prefix,
                excludeAllergens: ['GLUTEN'],
                pageSize: '100',
            }),
        },
        {
            name: 'list include MILK exclude GLUTEN',
            url: createRecipeUrl(apiBaseUrl, '/recipes', {
                title: prefix,
                includeAllergens: ['MILK'],
                excludeAllergens: ['GLUTEN'],
                pageSize: '100',
            }),
        },
        {
            name: 'search include EGGS',
            url: createRecipeUrl(apiBaseUrl, '/search', {
                title: prefix,
                includeAllergens: ['EGGS'],
            }),
        },
        {
            name: 'search title only',
            url: createRecipeUrl(apiBaseUrl, '/search', {
                title: prefix,
            }),
        },
        {
            name: 'search exclude PEANUTS',
            url: createRecipeUrl(apiBaseUrl, '/search', {
                title: prefix,
                excludeAllergens: ['PEANUTS'],
            }),
        },
    ]
}

function createRecipeUrl(apiBaseUrl, path, params) {
    const url = new URL(`${apiBaseUrl}${path}`)

    for (const [key, value] of Object.entries(params)) {
        const values = Array.isArray(value) ? value : [value]
        for (const item of values) url.searchParams.append(key, item)
    }

    return url
}

async function benchmarkCount(options, count) {
    const rows = []

    for (const testCase of benchmarkCases(options.apiBaseUrl, options.tag, count)) {
        for (let index = 0; index < options.warmup; index += 1) {
            await fetchJson(testCase.url)
        }

        const timings = []
        let resultCount = null
        let totalCount = null

        for (let index = 0; index < options.iterations; index += 1) {
            const result = await fetchJson(testCase.url)
            timings.push(result.elapsedMs)
            resultCount = Array.isArray(result.body) ? result.body.length : null
            totalCount =
                result.pagination?.TotalCount ?? result.pagination?.totalCount ?? totalCount
        }

        rows.push({
            count,
            test: testCase.name,
            returned: resultCount,
            total: totalCount,
            averageMs: average(timings),
            minMs: Math.min(...timings),
            p95Ms: percentile(timings, 0.95),
            maxMs: Math.max(...timings),
        })
    }

    return rows
}

function average(values) {
    return values.reduce((sum, value) => sum + value, 0) / values.length
}

function percentile(values, percentileValue) {
    const sorted = [...values].sort((a, b) => a - b)
    const index = Math.ceil(sorted.length * percentileValue) - 1
    return sorted[Math.max(0, Math.min(index, sorted.length - 1))]
}

function printRows(rows) {
    console.log('')
    console.log('Stress test results')
    console.table(
        rows.map((row) => ({
            recipes: row.count,
            test: row.test,
            returned: row.returned,
            total: row.total ?? '',
            avg_ms: row.averageMs.toFixed(1),
            min_ms: row.minMs.toFixed(1),
            p95_ms: row.p95Ms.toFixed(1),
            max_ms: row.maxMs.toFixed(1),
        })),
    )
}

async function main() {
    const options = parseArgs(process.argv.slice(2))
    if (options.insecure) process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'

    const datasets = options.counts.map((count) => createDataset(count, options.tag))

    if (options.dryRun) {
        console.log(`Generated datasets: ${options.counts.join(', ')}`)
        console.log(JSON.stringify(datasets[0].slice(0, 3), null, 2))
        return
    }

    let cookie = options.authCookie
    if (!options.skipSeed && !cookie) {
        cookie = await loginAndGetCookie(
            options.apiBaseUrl,
            options.adminEmail,
            options.adminPassword,
        )
    }

    if (!options.skipSeed) {
        for (const dataset of datasets) {
            await seedDataset(dataset, options, cookie)
        }
    }

    const rows = []
    for (const count of options.counts) {
        rows.push(...(await benchmarkCount(options, count)))
    }

    printRows(rows)

    if (options.output) {
        await fs.writeFile(
            options.output,
            JSON.stringify({ createdAt: new Date().toISOString(), rows }, null, 2),
            'utf8',
        )
        console.log(`Wrote benchmark results to ${options.output}`)
    }
}

main().catch((error) => {
    console.error(error instanceof Error ? error.message : error)
    process.exitCode = 1
})
