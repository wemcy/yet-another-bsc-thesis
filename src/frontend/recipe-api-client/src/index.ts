import { Configuration, PaginationMetadataFromJSONTyped, ProfileApi, RecipesApi, type GetRecipesByAuthorIdRequest, type ListRecipeCommentsRequest, type ListRecipesRequest } from '../gen/index.js';

export * from '../gen/index.js';

export class RecipeApiClient extends RecipesApi {
    constructor(configuration: Configuration) {
        super(configuration);
    }

    AddPaginationMetadataToResponse<T>(response: Response, responseBody: T) {
        const paginationMetadataJSON = response.headers.get('X-Pagination');
        const paginationMetadata = PaginationMetadataFromJSONTyped(paginationMetadataJSON ? JSON.parse(paginationMetadataJSON) : null, false);
        return {
            responseBody,
            paginationMetadata
        };
    }

    async listRecipesPaginated(listRecipesRequest :ListRecipesRequest) {
        const response = await this.listRecipesRaw(listRecipesRequest);
        return this.AddPaginationMetadataToResponse(response.raw, await response.value());
    }

    async listRecipeCommentsByIdPaginated(listRecipeCommentsRequest: ListRecipeCommentsRequest) {
        const response = await this.listRecipeCommentsRaw(listRecipeCommentsRequest);
        return this.AddPaginationMetadataToResponse(response.raw, await response.value());   
    }

    async getRecipesByAuthorIdRawPaginated(listRecipesByAuthorIdRequest: GetRecipesByAuthorIdRequest) {
        const response = await this.getRecipesByAuthorIdRaw(listRecipesByAuthorIdRequest);
        return this.AddPaginationMetadataToResponse(response.raw, await response.value());
    }
}   

