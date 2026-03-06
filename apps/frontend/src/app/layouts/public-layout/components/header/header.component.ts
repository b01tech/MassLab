import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThemeService } from '../../../../shared/services/theme.service';
import { ThemeButtonComponent } from '../../../../shared/components/theme-button/theme-button.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, ThemeButtonComponent],
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  themeService = inject(ThemeService);
}
