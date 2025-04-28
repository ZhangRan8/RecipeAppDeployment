import { Injectable } from '@angular/core';
import { Ingredient } from '../model/ingredient';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class IngredientService {

  private ingredientsData: Observable<Ingredient[]> = of([]);

  private apiUrl = 'http://localhost:5263/Ingredients';

  constructor(private http: HttpClient) { }

  getIngredientData(): Observable<Ingredient[]> {
    return this.ingredientsData = this.http.get<Ingredient[]>(this.apiUrl).pipe(
      catchError((error) => {
        // Handle errors in the service layer
        console.error('Error occurred:', error);
        return throwError('Something went wrong! Please try again later.');
      })
    );
  }

  searchIngredient(searchText: string): Observable<Ingredient[]> {
    const lowercasedSearchText = searchText.toLowerCase();
    return this.http
      .get<Ingredient[]>(`${this.apiUrl}/search?search=${lowercasedSearchText}`)
      .pipe(
        catchError((error) => {
          console.error('Error occurred:', error);
          return throwError('Something went wrong! Please try again later.');
        })
      );
  }

  getIngredientById(id: number): Observable<Ingredient | undefined> {
    return this.http.get<Ingredient>(`${this.apiUrl}/${id}`);
  }

  addIngredient(name: string, imageURL: string): Observable<Ingredient> {
    const newIngredient: Ingredient = {
      ingredientName: name,
      imageUrl: imageURL
    };
    const url = `${this.apiUrl}/add`;
    return this.http.post<Ingredient>(url, newIngredient);
  }


  deleteIngredient(ingredientId: number): Observable<void> {
    const url = `${this.apiUrl}/${ingredientId}`;
    return this.http.delete<void>(url);
  }


  updateIngredient(ingredientId: number, name: string, imageURL: string): Observable<Ingredient> {
    const updatedIngredient: Ingredient = {
      ingredientId: ingredientId,
      ingredientName: name,
      imageUrl: imageURL
    };
    const url = `${this.apiUrl}/Edit/${ingredientId}`;

    return this.http.put<Ingredient>(url, updatedIngredient).pipe(
      catchError((error) => {
        console.error('Error occurred:', error);
        return throwError('Something went wrong! Please try again later.');
      })
    );

  }
}
