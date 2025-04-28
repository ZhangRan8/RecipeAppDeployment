import { Routes } from '@angular/router';
import { IngredientListComponent } from './page/ingredient-page/ingredient-list/ingredient-list.component';
import { IngredientFormComponent } from './page/ingredient-page/ingredient-form/ingredient-form.component';

import { RecipeFormComponent } from './page/recipe-page/recipe-form/recipe-form.component';
import { RecipeListComponent } from './page/recipe-page/recipe-list/recipe-list.component';
import { RecipeDetailComponent } from './page/recipe-page/recipe-detail/recipe-detail.component';
import { HomepageComponent } from './page/homepage/homepage.component';

export const routes: Routes = [

    {
        path: 'ingredients',
        component: IngredientListComponent,
        title: 'Ingredients Page'
    },

    {
        path: 'ingredients/add',
        component: IngredientFormComponent,
        title: 'Ingredients Add Page'
    },
    {
        path: 'ingredients/edit/:id',
        component: IngredientFormComponent,
        title: 'Ingredients Edit Page'
    },
    {
        path: 'recipes/add',
        component: RecipeFormComponent,
        title: 'Recipe Add Page'
    },
    {
        path: 'recipes',
        component: RecipeListComponent,
        title: 'Recipe Page'
    },
    {
        path: '',
        component: HomepageComponent,
        title: 'Recipe Page'
    },
    {
        path: 'recipes/edit/:id',
        component: RecipeFormComponent,
        title: 'Recipe Edit Page'
    },
    {
        path: 'recipes/:id',
        component: RecipeDetailComponent,
        title: 'Recipe Detail Page'
    },
];
