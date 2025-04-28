import { Component, Input } from '@angular/core';
import { Recipe } from '../../../model/recipe';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-recipe-card',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './recipe-card.component.html',
  styleUrl: './recipe-card.component.css'
})
export class RecipeCardComponent {
  @Input() recipe !: Recipe;

  constructor() { };

}
