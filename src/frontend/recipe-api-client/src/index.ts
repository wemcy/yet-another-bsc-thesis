import { Configuration, PaginationMetadataFromJSONTyped, RecipesApi, type ListRecipesRequest } from '../gen/index.js';

export * from '../gen/index.js';

export class RecipeApiClient extends RecipesApi {
    constructor(configuration: Configuration) {
        super(configuration);
    }

    async listRecipesPaginated(listRecipesRequest :ListRecipesRequest) {
        const response = await this.listRecipesRaw(listRecipesRequest);
        const paginationMetadataJSON = response.raw.headers.get('X-Pagination');
        const paginationMetadata = PaginationMetadataFromJSONTyped(paginationMetadataJSON ? JSON.parse(paginationMetadataJSON) : null, false);
        const recipes = await response.value();
        return {
            recipes,
            paginationMetadata
        };    
    }
}   