import { Component, Input } from '@angular/core';
import { Ingredient } from '../../../model/ingredient';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-ingredient-card',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './ingredient-card.component.html',
  styleUrl: './ingredient-card.component.css'
})
export class IngredientCardComponent {
  @Input() ingredient !: Ingredient;

  constructor() { }
}
