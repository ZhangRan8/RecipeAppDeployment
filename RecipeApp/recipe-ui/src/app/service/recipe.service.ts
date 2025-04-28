import { Injectable } from '@angular/core';
import { Recipe } from '../model/recipe';
import { HttpClient } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {

  private recipesData: Observable<Recipe[]> = of([]);
  private apiUrl = 'https://lorraine-recipeapp-webapp-ahfvhvgaayd2fpbt.eastus-01.azurewebsites.net/recipes';
  private apiUrlAdd = 'https://lorraine-recipeapp-webapp-ahfvhvgaayd2fpbt.eastus-01.azurewebsites.net/recipe/add';

  constructor(private http: HttpClient) { }

  getRecipesData(): Observable<Recipe[]> {
    return this.recipesData = this.http.get<Recipe[]>(this.apiUrl).pipe(
      catchError((error) => {
        // Handle errors in the service layer
        console.error('Error occurred:', error);
        return throwError('Something went wrong! Please try again later.');
      })
    );
  }

  getRecipeById(id: number): Observable<Recipe> {
    return this.http.get<Recipe>(`${this.apiUrl}/${id}`);
  }

  getLimitedRecipes(limit: number): Observable<Recipe[]> {
    const url = `${this.apiUrl}/limited?limit=${limit}`;
    return this.http.get<Recipe[]>(url).pipe(
      catchError((error) => {
        console.error('Error occurred:', error);
        return throwError('Something went wrong! Please try again later.');
      })
    );
  }

  addRecipe(recipe: Recipe): Observable<Recipe> {
    return this.http.post<Recipe>(this.apiUrlAdd, recipe).pipe(
      catchError((error) => {
        console.error('Error occurred:', error);
        return throwError('Something went wrong! Please try again later.');
      })
    );
    ;
  }

  updateRecipe(recipeId: number, recipe: Recipe): Observable<Recipe> {
    const url = `${this.apiUrl}/Edit/${recipeId}`;
    return this.http.put<Recipe>(url, recipe).pipe(
      catchError((error) => {
        console.error('Error occurred:', error);
        return throwError('Something went wrong! Please try again later.');
      })
    );
  }

  deleteRecipe(recipeId: number): Observable<Recipe> {
    const url = `${this.apiUrl}/delete/${recipeId}`;
    return this.http.delete<Recipe>(url).pipe(
      catchError((error) => {
        console.error('Error occurred:', error);
        return throwError('Something went wrong! Please try again later.');
      })
    );
  }

  searchRecipe(searchText: string): Observable<Recipe[]> {
    const lowercasedSearchText = searchText.toLowerCase();
    return this.http
      .get<Recipe[]>(`${this.apiUrl}/search?search=${lowercasedSearchText}`)
      .pipe(
        catchError((error) => {
          console.error('Error occurred:', error);
          return throwError('Something went wrong! Please try again later.');
        })
      );
  }

}
