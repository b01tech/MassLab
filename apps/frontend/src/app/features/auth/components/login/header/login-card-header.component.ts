import { Component } from '@angular/core';

@Component({
  selector: 'app-login-card-header',
  standalone: true,
  imports: [],
  template: `
    <div class="p-8 pb-0 text-center">
      <div
        class="mx-auto w-16 h-16 bg-primary/10 rounded-full flex items-center justify-center mb-4 transition-colors"
      >
        <span class="material-symbols-outlined text-primary">lock</span>
      </div>
      <h2 class="text-2xl font-bold text-text-base mb-2">Acesso ao Sistema</h2>
      <p class="text-text-muted text-sm">Bem-vindo ao portal de gestão MassLab</p>
    </div>
  `,
})
export class LoginCardHeaderComponent {}
