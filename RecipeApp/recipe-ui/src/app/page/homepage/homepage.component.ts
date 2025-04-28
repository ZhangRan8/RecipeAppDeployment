import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgbCarouselModule } from '@ng-bootstrap/ng-bootstrap';
import { RecipeService } from '../../service/recipe.service';
import { Recipe } from '../../model/recipe';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-homepage',
  standalone: true,
  imports: [RouterModule, NgbCarouselModule, CommonModule],
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent {
  images = [
    'https://sharingthefoodwelove.wordpress.com/wp-content/uploads/2013/11/361.jpg',
    'https://www.tasteofhome.com/wp-content/uploads/2018/01/Fabulous-Fajitas_EXPS_SSCBZ18_2071_B10_24_5b-3.jpg?fit=700,700',
    'https://www.southernliving.com/thmb/oJl9g-DvTZN5cfGs1PkpXHZZ7GI=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/onion-boil_beauty_121-33826c1561164ad8ac0d7b5f8bca99eb.jpg'
  ];

  chefsRecipes: Recipe[] = [];

  constructor(private recipeService: RecipeService) { }

  ngOnInit() {
    this.recipeService.getLimitedRecipes(3).subscribe(
      (recipes) => {
        this.chefsRecipes = recipes;
      }
    );
  }

}
