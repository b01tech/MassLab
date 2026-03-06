import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ThemeService } from '../../../../shared/services/theme.service';
import { AuthService } from '../../../../features/auth/services/auth.service';
import { ThemeButtonComponent } from '../../../../shared/components/theme-button/theme-button.component';

@Component({
  selector: 'app-private-header',
  standalone: true,
  imports: [CommonModule, ThemeButtonComponent],
  templateUrl: './header.component.html',
})
export class PrivateHeaderComponent {
  themeService = inject(ThemeService);
  authService = inject(AuthService);
  private router = inject(Router);

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
