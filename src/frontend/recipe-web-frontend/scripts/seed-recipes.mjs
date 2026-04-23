#!/usr/bin/env node

import fs from 'node:fs/promises'
import fsSync from 'node:fs'
import path from 'node:path'

function parseArgs(argv) {
    const options = {
        dryRun: false,
        insecure: true,
        exportPath: null,
        limit: null,
        apiBaseUrl: process.env.API_BASE_URL ?? 'http://localhost:5173/api',
        file: 'scripts/seed-data/recipes.json',
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

        if (arg === '--file') {
            options.file = argv[index + 1] ?? options.file
            index += 1
            continue
        }
    }

    return options
}

async function loadDataset(filePath) {
    try {
        const raw = await fs.readFile(filePath, 'utf8')
        const parsed = JSON.parse(raw)
        if (!Array.isArray(parsed)) throw new Error('recipes file must be a JSON array')
        return parsed
    } catch (err) {
        if (err.code === 'ENOENT') {
            // fallback builtin sample dataset
            return getBuiltinRecipes()
        }
        throw err
    }
}

function getBuiltinRecipes() {
    return [
        {
            title: 'Gazpacho',
            description: 'Hideg spanyol zöldségleves',
            steps: [
                'A száraz kenyeret 5 percig vízben áztatjuk; kinyomkodjuk.',
                'Az összes zöldséget kenyérrel, fokhagymával, olívaolajjal és sherryecettel turmixoljuk.',
                'Sóval és borssal ízesítjük.',
            ],
            ingredients: [
                { name: 'Érett paradicsom', quantity: 1, unitOfMeasurement: 'kg', allergens: [] },
                { name: 'Uborka', quantity: 1, unitOfMeasurement: 'db', allergens: [] },
                { name: 'Olívaolaj', quantity: 80, unitOfMeasurement: 'ml', allergens: [] },
            ],
        },
        {
            title: 'Vajcsirke (Butter Chicken)',
            description: 'Enyhe, krémes paradicsomos indiai curry omlós csirkével.',
            steps: [
                'A csirkehúst joghurtban, citromlében, chiliporban és kurkumában 1 órát pácoljuk.',
                'Grillen vagy serpenyőben pirosra sütjük.',
            ],
            ingredients: [
                { name: 'Csirkecomb', quantity: 600, unitOfMeasurement: 'g', allergens: [] },
                { name: 'Tejszín', quantity: 150, unitOfMeasurement: 'ml', allergens: ['MILK'] },
            ],
        },
        {
            title: 'Shakshuka',
            description: 'Fűszeres paradicsomos-paprikás szószban sült tojás.',
            steps: [
                'Olívaolajat hevítünk, megdinszteljük a hagymát és paprikát.',
                'Ráöntjük a darált paradicsomot, beletörjük a tojásokat, készre sütjük.',
            ],
            ingredients: [
                { name: 'Tojás', quantity: 6, unitOfMeasurement: 'db', allergens: ['EGGS'] },
                { name: 'Darált paradicsom', quantity: 400, unitOfMeasurement: 'g', allergens: [] },
            ],
        },
    ]
}

async function maybeExportDataset(dataset, exportPath) {
    if (!exportPath) return
    const resolvedPath = path.resolve(process.cwd(), exportPath)
    await fs.mkdir(path.dirname(resolvedPath), { recursive: true })
    await fs.writeFile(resolvedPath, JSON.stringify(dataset, null, 2), 'utf8')
    console.log(`Dataset exported to ${resolvedPath}`)
}

async function searchRecipe(apiBaseUrl, title) {
    const url = new URL(`${apiBaseUrl}/recipes`)
    url.searchParams.set('title', title)
    const response = await fetch(url)
    if (!response.ok) return []
    const body = await response.json()
    if (Array.isArray(body)) return body
    if (body && Array.isArray(body.items)) return body.items
    return []
}

async function createRecipe(apiBaseUrl, recipe) {
    const response = await fetch(`${apiBaseUrl}/recipes/`, {
        method: 'POST',
        headers: {
            'content-type': 'application/json',
        },
        body: JSON.stringify(recipe),
    })
    if (!response.ok) {
        const body = await response.text()
        throw new Error(`Create failed with ${response.status}: ${body}`)
    }
    return response.json()
}

async function uploadImage(apiBaseUrl, recipeId, imagePath) {
    const form = new FormData()
    const stream = fsSync.createReadStream(imagePath)
    form.append('image', stream, path.basename(imagePath))

    const response = await fetch(`${apiBaseUrl}/recipes/${recipeId}/image`, {
        method: 'PUT',
        body: form,
    })

    if (!response.ok) {
        const body = await response.text()
        throw new Error(`Image upload failed with ${response.status}: ${body}`)
    }
}

async function seedRecipes(dataset, options) {
    let created = 0
    let skipped = 0

    for (const [index, recipe] of dataset.entries()) {
        try {
            const existing = await searchRecipe(options.apiBaseUrl, recipe.title)
            const exactMatch = Array.isArray(existing) ? existing.some((r) => (r.title || r.title) === recipe.title) : false
            if (exactMatch) {
                skipped += 1
                console.log(`[${index + 1}/${dataset.length}] skip  ${recipe.title}`)
                continue
            }

            const createdRecipe = await createRecipe(options.apiBaseUrl, recipe)
            created += 1
            console.log(`[${index + 1}/${dataset.length}] create ${recipe.title} -> ${createdRecipe.id}`)

            if (recipe.image && typeof recipe.image === 'string') {
                const imagePath = path.resolve(process.cwd(), recipe.image)
                if (fsSync.existsSync(imagePath)) {
                    await uploadImage(options.apiBaseUrl, createdRecipe.id, imagePath)
                    console.log(`  uploaded image ${recipe.image}`)
                } else {
                    console.log(`  image not found ${imagePath}, skipping upload`)
                }
            }
        } catch (err) {
            console.error(`[ERROR] ${recipe.title} — ${err instanceof Error ? err.message : err}`)
        }
    }

    console.log('')
    console.log(`Finished. Created: ${created}, skipped existing: ${skipped}, total: ${dataset.length}`)
}

async function main() {
    const options = parseArgs(process.argv.slice(2))

    if (options.insecure) process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'

    const dataset = await loadDataset(options.file)
    const limitedDataset = options.limit ? dataset.slice(0, options.limit) : dataset

    await maybeExportDataset(dataset, options.exportPath)

    if (options.dryRun) {
        console.log(`Dry run only. Dataset size: ${dataset.length}`)
        console.log(`Target API base URL: ${options.apiBaseUrl}`)
        console.log('Sample:')
        console.log(JSON.stringify(limitedDataset.slice(0, 3), null, 2))
        return
    }

    await seedRecipes(limitedDataset, options)
}

main().catch((error) => {
    console.error(error instanceof Error ? error.message : error)
    process.exitCode = 1
})
