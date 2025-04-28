import { RecipeIngredient } from "./recipeIngredient";

export interface Recipe {
    recipeId?: number;
    recipeName: string;
    recipeImageUrl: string;
    instructions: string;
    ingredients: RecipeIngredient[];
}