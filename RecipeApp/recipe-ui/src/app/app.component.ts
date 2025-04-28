import { Component } from '@angular/core';
import { RouterOutlet, RouterModule } from '@angular/router';
//import custom component
import { IngredientCardComponent } from './page/ingredient-page/ingredient-card/ingredient-card.component';
import { IngredientListComponent } from './page/ingredient-page/ingredient-list/ingredient-list.component';
import { IngredientFormComponent } from './page/ingredient-page/ingredient-form/ingredient-form.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule, IngredientCardComponent, IngredientListComponent, IngredientFormComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'recipe-ui';
}
