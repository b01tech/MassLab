import { CommonModule } from '@angular/common';
import { Component, inject, input, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { User } from '../../../auth/models/user.model';
import { UserService } from '../../services/user.service';
import { UserRole } from '../../../auth/models/user-role';

@Component({
  selector: 'app-user-header',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-header.component.html',
})
export class UserHeaderComponent {
  private readonly userService = inject(UserService);
  private readonly fb = inject(FormBuilder);

  readonly loggedUser = input.required<User>();

  isChangePasswordOpen = signal(false);
  isSubmitting = signal(false);
  changeSuccess = signal(false);
  errorMessage = signal<string | null>(null);

  changePasswordForm: FormGroup = this.fb.group(
    {
      newPassword: [
        '',
        [Validators.required, Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{6,}$/)],
      ],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: [this.passwordsMatchValidator] },
  );

  openChangePassword() {
    this.errorMessage.set(null);
    this.changePasswordForm.reset();
    this.isChangePasswordOpen.set(true);
  }

  closeChangePassword() {
    this.isChangePasswordOpen.set(false);
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

  onSubmitChangePassword() {
    if (this.changePasswordForm.invalid) {
      this.changePasswordForm.markAllAsTouched();
      return;
    }

    const user = this.loggedUser();
    if (!user.id) {
      this.errorMessage.set('Usuário inválido para troca de senha.');
      return;
    }
    const newPassword = this.changePasswordForm.value.newPassword as string | null | undefined;
    if (!newPassword) return;

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    this.userService.changePassword(user.id, newPassword).subscribe({
      next: () => {
        this.closeChangePassword();
        this.changeSuccess.set(true);
      },
      error: (err) => {
        if (Array.isArray(err.error)) {
          this.errorMessage.set(err.error.join(', '));
        } else if (err.error?.message) {
          this.errorMessage.set(err.error.message);
        } else if (typeof err.error === 'string') {
          this.errorMessage.set(err.error);
        } else {
          this.errorMessage.set('Ocorreu um erro ao trocar a senha.');
        }
        this.isSubmitting.set(false);
      },
      complete: () => {
        this.isSubmitting.set(false);
      },
    });
  }

  private passwordsMatchValidator(group: FormGroup) {
    const newPassword = group.get('newPassword')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    if (!newPassword || !confirmPassword) return null;
    return newPassword === confirmPassword ? null : { passwordsMismatch: true };
  }
}
