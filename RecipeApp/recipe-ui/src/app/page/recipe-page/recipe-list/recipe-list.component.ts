import { Component, OnInit } from '@angular/core';
import { Recipe } from '../../../model/recipe';
import { RecipeService } from '../../../service/recipe.service';
import { RecipeCardComponent } from '../recipe-card/recipe-card.component';
import { CommonModule } from '@angular/common';
import { Router, NavigationEnd, RouterModule } from '@angular/router';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Subscription, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';

@Component({
  selector: 'app-recipe-list',
  standalone: true,
  imports: [RecipeCardComponent, CommonModule, RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './recipe-list.component.html',
  styleUrl: './recipe-list.component.css'
})
export class RecipeListComponent implements OnInit {
  filteredRecipes: Recipe[] = [];
  routerSubscription!: Subscription;
  searchControl = new FormControl('');

  constructor(private recipeService: RecipeService, private router: Router) { }

  ngOnInit(): void {
    this.getAllRecipes();
    this.handleSearch();
    this.routerSubscription = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.getAllRecipes();
      }
    });
  }

  getAllRecipes() {
    this.recipeService.getRecipesData().subscribe({
      next: (data) => {
        this.filteredRecipes = data;
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
          return this.recipeService.getRecipesData();
        }
        return this.recipeService.searchRecipe(search as string);
      })
    ).subscribe({
      next: (data) => {
        this.filteredRecipes = data;
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  ngOnDestroy(): void {
    this.routerSubscription?.unsubscribe();
  }

}
