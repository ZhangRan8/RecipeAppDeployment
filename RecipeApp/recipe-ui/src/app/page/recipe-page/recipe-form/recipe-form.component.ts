import { Component, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { Observable, OperatorFunction, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, catchError } from 'rxjs/operators';
import { FormsModule } from '@angular/forms';
import { JsonPipe } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { FormArray } from '@angular/forms';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { NgbToastModule, NgbModule, NgbModal, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { IngredientService } from '../../../service/ingredient.service';
import { Ingredient } from '../../../model/ingredient';
import { RecipeService } from '../../../service/recipe.service';
import { Recipe } from '../../../model/recipe';
import { Router } from '@angular/router';
import { OnInit } from '@angular/core';

import { RecipeIngredient } from '../../../model/recipeIngredient';

@Component({
  selector: 'app-recipe-form',
  standalone: true,
  imports: [NgbTypeaheadModule, NgbDropdownModule, NgbToastModule, NgbModule, CommonModule, FormsModule, RouterModule, JsonPipe, ReactiveFormsModule, NgbTooltip],
  templateUrl: './recipe-form.component.html',
  styleUrl: './recipe-form.component.css'
})
export class RecipeFormComponent {
  recipeForm: FormGroup;
  isEdit = false;
  isDelete = false;
  recipeId: number | undefined = undefined;

  units = ["tsp", "tbsp", "cup", "oz", "lb", "g", "kg", "ml", "l"];

  constructor(private fb: FormBuilder,
    private ingredientService: IngredientService,
    private recipeService: RecipeService,
    private route: ActivatedRoute,
    private router: Router,
    private modalService: NgbModal) {
    this.recipeForm = this.fb.group({
      recipeName: ['', [Validators.required, Validators.pattern('^(?!\\s*$).+')]], //name cannot be empty, white spaces at the beginning or end wouldn't trigger an error message
      imageUrl: ['', [Validators.required, Validators.pattern('https?://.+')]],
      instructions: ['', Validators.required],
      ingredients: this.fb.array([])
    });

  }

  ngOnInit() {
    this.checkIfEdit();
  }

  checkIfEdit() {
    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.recipeId = +params['id'];
        this.isEdit = true;
        this.loadRecipeData(this.recipeId);
      } else {
        this.isEdit = false;
        this.recipeId = undefined;
        this.addIngredient();
      }
    });
  }

  loadRecipeData(recipeId: number) {
    this.recipeService.getRecipeById(recipeId).subscribe({
      next: (recipe) => {
        this.recipeForm.patchValue({
          recipeName: recipe.recipeName,
          imageUrl: recipe.recipeImageUrl,
          instructions: recipe.instructions
        });
        this.patchIngredients(recipe.ingredients);
      },
      error: (error) => console.error('Error loading recipe:', error)
    });
  }

  private patchIngredients(ingredients: RecipeIngredient[]) {
    ingredients.forEach(ing => {
      const isInactive = !ing.isActive;
      const ingredientGroup = this.fb.group({
        ingredient: [{
          value: {
            ingredientId: ing.ingredientId,
            ingredientName: ing.ingredientName,
            isActive: ing.isActive,
          }, disabled: isInactive
        }, Validators.required],
        ingredientQuantity: [{ value: ing.quantity, disabled: isInactive }, [Validators.required, Validators.min(0)]],
        ingredientUnit: [{ value: ing.unit, disabled: isInactive }, Validators.required]
      });
      this.ingredients.push(ingredientGroup);
    });
  }

  onDeleteClick(exampleModalContent: TemplateRef<any>) {
    this.openModal(exampleModalContent);
    this.isDelete = true;
  }


  addIngredient() {
    this.ingredients.push(this.newIngredient());
  }

  removeIngredient(index: number): void {
    this.ingredients.removeAt(index);
  }

  get ingredients(): FormArray {
    return this.recipeForm.get('ingredients') as FormArray;
  }

  newIngredient(): FormGroup {
    return this.fb.group({
      ingredient: [null as Ingredient | null, Validators.required],
      ingredientQuantity: ['', [Validators.required, Validators.min(0)]], //kilojoules must be a number greater than or equal to 0, text with white spaces wouldn't trigger an error message
      ingredientUnit: ['', Validators.required]
    });
  }


  onSubmit() {
    if (!this.recipeForm.valid) {
      console.error('Form is invalid');
      return;
    }

    const { recipeName, imageUrl, instructions, ingredients } = this.recipeForm.value;

    const newRecipe: Recipe = {
      recipeId: this.recipeId,
      recipeName,
      recipeImageUrl: imageUrl,
      instructions,
      ingredients: ingredients.map((ingredientGroup: any) => ({
        ingredientId: ingredientGroup.ingredient.ingredientId,
        ingredientName: ingredientGroup.ingredient.ingredientName,
        quantity: ingredientGroup.ingredientQuantity,
        unit: ingredientGroup.ingredientUnit
      }))
    };

    const request = this.isEdit && this.recipeId
      ? this.recipeService.updateRecipe(this.recipeId, newRecipe)
      : this.recipeService.addRecipe(newRecipe);

    request.subscribe({
      next: (response) => {
        if (!this.isEdit) {
          this.recipeForm.reset();
          this.ingredients.clear();
        }
      },
      error: (error) => console.error('Error:', error)
    });
  }


  openModal(content: TemplateRef<any>) {
    this.modalService.open(content);
  }

  submitAndClose(modal: any) {
    this.onSubmit();
    modal.close();
    this.router.navigate(['/recipes']);
  }

  deleteAndClose(modal: any) {
    if (this.recipeId) {
      this.recipeService.deleteRecipe(this.recipeId).subscribe({
        next: () => {
          modal.close();
          this.router.navigate(['/recipes']);
        },
        error: (error) => console.error('Error:', error)
      });
    } else {
      console.error('Recipe ID is undefined!');
    }
  }

  get recipeNameControl() {
    return this.recipeForm.get('recipeName');
  }

  get imageUrlControl() {
    return this.recipeForm.get('imageUrl');
  }

  get instructionsControl() {
    return this.recipeForm.get('instructions');
  }

  formatter = (ingredient: Ingredient) => ingredient?.ingredientName || '';

  search: OperatorFunction<string, readonly Ingredient[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term) => {
        if (term === '') {
          return of([]);
        } else {
          return this.ingredientService.searchIngredient(term).pipe(
            catchError(() => of([]))
          );
        }
      })
    );


}
