import { Component, TemplateRef } from '@angular/core';
//import
import { FormGroup, FormControl } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { Validators, FormBuilder } from '@angular/forms'
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { IngredientService } from '../../../service/ingredient.service';
import { CommonModule } from '@angular/common';
import { NgbToastModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-ingredient-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, NgbToastModule, RouterModule],
  templateUrl: './ingredient-form.component.html',
  styleUrl: './ingredient-form.component.css'
})
export class IngredientFormComponent {
  ingredientForm !: FormGroup;
  isEditMode: boolean = false;  //Choose between adding or editing mode

  isAdded: boolean = false;

  ingredientId: string | null = null;
  isEdited: boolean = false;

  ifErrorOccured: boolean = false;

  isOverlayVisible: boolean = false;

  isDelete = false;

  constructor(
    private ingredientService: IngredientService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private modalService: NgbModal
  ) {
    this.ingredientForm = this.fb.group({
      ingredientName: ['', [Validators.required, Validators.pattern('^(?!\\s*$).+')]], //name cannot be empty, white spaces at the beginning or end wouldn't trigger an error message
      imageUrl: ['', [Validators.required, Validators.pattern('https?://.+')]], //url must start with http:// or https://
    });
  }

  ngOnInit() {
    this.ingredientId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.ingredientId;
    if (this.isEditMode && this.ingredientId) {
      this.ingredientService.getIngredientById(+this.ingredientId).subscribe(
        (ingredient) => {
          if (ingredient) {
            this.ingredientForm.patchValue({
              ingredientName: ingredient.ingredientName,
              imageUrl: ingredient.imageUrl
            });
          }
        },
        (error) => {
          this.ifErrorOccured = true;
        }
      );
    }
  }

  onSubmit(): void {
    if (this.ingredientForm.valid) {
      const { ingredientName, imageUrl } = this.ingredientForm.value;
      if (this.ingredientId) {
        this.ingredientService.updateIngredient(+this.ingredientId, ingredientName, imageUrl).subscribe(
          (ingredient) => {
            this.isEdited = true;
            this.isOverlayVisible = true;
          },
          (error) => {
            this.ifErrorOccured = true;
          }
        );
      } else {
        this.ingredientService.addIngredient(ingredientName, imageUrl).subscribe(
          (ingredient) => {
            this.isAdded = true;
            this.isOverlayVisible = true;
          },
          (error) => {
            this.ifErrorOccured = true;
          }
        );
      }
    }
  }

  onDeleteClick(exampleModalContent: TemplateRef<any>) {
    this.openModal(exampleModalContent);
    this.isDelete = true;
  }

  deleteAndClose(modal: any) {
    if (this.ingredientId) {
      this.ingredientService.deleteIngredient(+this.ingredientId).subscribe({
        next: () => {
          modal.close();
          this.router.navigate(['/ingredients']);
        },
        error: (error) => console.error('Error:', error)
      }
      );
    }
  }

  closeAddedToast() {
    this.isAdded = false;
    this.isOverlayVisible = false;
    this.router.navigate(['/ingredients']);
  }

  closeEditedToast() {
    this.isEdited = false;
    this.isOverlayVisible = false;
    this.router.navigate(['/ingredients']);
  }

  get kilojoulesControl() {
    return this.ingredientForm.get('kilojoulesPerUnit');
  }

  get nameControl() {
    return this.ingredientForm.get('ingredientName');
  }

  get unitControl() {
    return this.ingredientForm.get('unitOfMeasurement');
  }

  openModal(content: TemplateRef<any>) {
    this.modalService.open(content);
  }

}
