import { Component, inject, effect, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { User } from '../../../auth/models/user.model';
import { UserRole } from '../../../auth/models/user-role';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-form.component.html',
})
export class UserFormComponent {
  isOpen = input(false);
  user = input<User | null>(null);
  errorMessage = input<string | null>(null);

  close = output<void>();
  save = output<any>();

  private fb = inject(FormBuilder);

  protected isEditMode = false;
  protected roles = Object.values(UserRole);

  userForm: FormGroup = this.fb.group({
    userName: ['', Validators.required],
    role: [UserRole.Operator, Validators.required],
    password: [''],
  });

  constructor() {
    effect(() => {
      const isOpenValue = this.isOpen();
      const userValue = this.user();

      if (isOpenValue) {
        if (userValue) {
          this.isEditMode = true;
          this.userForm.patchValue({
            userName: userValue.userName,
            role: userValue.role,
          });
          this.userForm.get('password')?.clearValidators();
        } else {
          this.isEditMode = false;
          this.userForm.reset({ role: UserRole.Operator });
          this.userForm
            .get('password')
            ?.setValidators([Validators.required, Validators.minLength(6)]);
        }
        this.userForm.get('password')?.updateValueAndValidity();
      }
    });
  }

  getRoleLabel(role: UserRole): string {
    switch (role) {
      case UserRole.Admin:
        return 'Administrador';
      case UserRole.Manager:
        return 'Gerente';
      case UserRole.Operator:
        return 'Técnico';
      default:
        return role;
    }
  }

  onSubmit() {
    if (this.userForm.valid) {
      this.save.emit(this.userForm.value);
    }
  }
}
