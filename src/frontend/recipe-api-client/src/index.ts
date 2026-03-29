import { Configuration, PaginationMetadataFromJSONTyped, RecipesApi } from '../gen/index.js';

export * from '../gen/index.js';

export class RecipeApiClient extends RecipesApi {
    constructor(configuration: Configuration) {
        super(configuration);
    }

    async listRecipesPaginated() {
        const response = await this.listRecipesRaw();
        const paginationMetadataJSON = response.raw.headers.get('X-Pagination');
        const paginationMetadata = PaginationMetadataFromJSONTyped(paginationMetadataJSON ? JSON.parse(paginationMetadataJSON) : null, false);
        const recipes = await response.value();
        return {
            recipes,
            paginationMetadata
        };    
    }
}   