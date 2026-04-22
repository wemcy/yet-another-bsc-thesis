import type { JsonObject } from '@/types/common/json'

export function isObject(value: unknown): value is Record<string, unknown> {
    return typeof value === 'object' && value !== null
}

export function isJsonObject(value: unknown): value is JsonObject {
    return isObject(value)
}

export function isFiniteNumber(value: unknown): value is number {
    return typeof value === 'number' && Number.isFinite(value)
}

export function isBoolean(value: unknown): value is boolean {
    return typeof value === 'boolean'
}

export function isString(value: unknown): value is string {
    return typeof value === 'string'
}
