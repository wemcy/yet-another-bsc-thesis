import type { RouteParams } from 'vue-router'

export function getSingleRouteParam(
    params: RouteParams,
    key: string,
): string | null {
    const value = params[key]

    if (typeof value === 'string') return value
    if (Array.isArray(value) && typeof value[0] === 'string') return value[0]

    return null
}
