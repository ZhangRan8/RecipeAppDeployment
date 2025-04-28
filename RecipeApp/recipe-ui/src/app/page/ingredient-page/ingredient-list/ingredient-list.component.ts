import { Component, OnInit } from '@angular/core';
//import other component
import { IngredientCardComponent } from '../ingredient-card/ingredient-card.component';
import { Ingredient } from '../../../model/ingredient';

import { IngredientService } from '../../../service/ingredient.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-ingredient-list',
  standalone: true,
  imports: [IngredientCardComponent, CommonModule, RouterLink, FormsModule, ReactiveFormsModule],
  templateUrl: './ingredient-list.component.html',
  styleUrl: './ingredient-list.component.css'
})
export class IngredientListComponent implements OnInit {
  filteredIngredients: Ingredient[] = [];

  searchControl = new FormControl('');


  constructor(private ingredientService: IngredientService, private router: Router) { }

  ngOnInit(): void {
    this.getAllIngredients();
    this.handleSearch();
  }

  getAllIngredients() {
    this.ingredientService.getIngredientData().subscribe({
      next: (data) => {
        this.filteredIngredients = data;
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  handleSearch() {
    this.searchControl.valueChanges.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      switchMap((search: string | null) => {
        if (search === '') {
          return this.ingredientService.getIngredientData();
        }
        return this.ingredientService.searchIngredient(search as string);
      })
    ).subscribe({
      next: (data) => {
        this.filteredIngredients = data;
      },
      error: (error) => {
        console.log(error);
      }
    });
  }
}
