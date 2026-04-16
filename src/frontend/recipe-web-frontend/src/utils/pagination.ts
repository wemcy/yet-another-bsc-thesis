import type { PaginationState } from '@/types/recipe/recipe'
import { isBoolean, isFiniteNumber, isJsonObject } from '@/utils/typeGuards'

interface PaginationHeaderPayload {
    pageNumber?: number
    PageNumber?: number
    pageSize?: number
    PageSize?: number
    totalCount?: number
    TotalCount?: number
    pageCount?: number
    PageCount?: number
    hasNextPage?: boolean
    HasNextPage?: boolean
    hasPreviousPage?: boolean
    HasPreviousPage?: boolean
}

export const createDefaultPaginationState = (
    pageNumber: number,
    pageSize: number,
): PaginationState => ({
    pageNumber,
    pageSize,
    totalCount: 0,
    pageCount: 0,
    hasNextPage: false,
    hasPreviousPage: pageNumber > 0,
})

const parseNumber = (value: unknown): number | undefined => {
    if (isFiniteNumber(value)) return value
    return undefined
}

const parseBoolean = (value: unknown): boolean | undefined => {
    if (isBoolean(value)) return value
    return undefined
}

const toPaginationHeaderPayload = (value: unknown): PaginationHeaderPayload | null => {
    if (!isJsonObject(value)) return null
    return value as PaginationHeaderPayload
}

export const parsePaginationState = (
    headerValue: string | null,
    fallbackPageNumber: number,
    fallbackPageSize: number,
): PaginationState => {
    if (!headerValue) {
        return createDefaultPaginationState(fallbackPageNumber, fallbackPageSize)
    }

    try {
        const parsed = toPaginationHeaderPayload(JSON.parse(headerValue))
        if (!parsed) {
            return createDefaultPaginationState(fallbackPageNumber, fallbackPageSize)
        }

        const pageNumber =
            parseNumber(parsed.pageNumber) ??
            parseNumber(parsed.PageNumber) ??
            fallbackPageNumber
        const pageSize =
            parseNumber(parsed.pageSize) ?? parseNumber(parsed.PageSize) ?? fallbackPageSize
        const totalCount = parseNumber(parsed.totalCount) ?? parseNumber(parsed.TotalCount) ?? 0
        const pageCount = parseNumber(parsed.pageCount) ?? parseNumber(parsed.PageCount) ?? 0
        const hasNextPage =
            parseBoolean(parsed.hasNextPage) ?? parseBoolean(parsed.HasNextPage) ?? false
        const hasPreviousPage =
            parseBoolean(parsed.hasPreviousPage) ??
            parseBoolean(parsed.HasPreviousPage) ??
            pageNumber > 0

        return {
            pageNumber,
            pageSize,
            totalCount,
            pageCount,
            hasNextPage,
            hasPreviousPage,
        }
    } catch {
        return createDefaultPaginationState(fallbackPageNumber, fallbackPageSize)
    }
}
