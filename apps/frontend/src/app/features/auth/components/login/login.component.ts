import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginCardHeaderComponent } from './login-card-header.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, LoginCardHeaderComponent],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  isLoading = signal(false);
  showPassword = signal(false);
  errorMessage = signal<string | null>(null);

  loginForm = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]],
    rememberMe: [false],
  });

  togglePasswordVisibility() {
    this.showPassword.update((value) => !value);
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading.set(true);
      this.errorMessage.set(null);

      const { username, password } = this.loginForm.value;

      if (username && password) {
        this.authService.login(username, password).subscribe({
          next: () => {
            this.router.navigate(['/dashboard']);
          },
          error: (err) => {
            this.errorMessage.set('Usuário ou senha inválidos');
            this.isLoading.set(false);
          },
          complete: () => {
            this.isLoading.set(false);
          },
        });
      }
    } else {
      this.loginForm.markAllAsTouched();
    }
  }
}
