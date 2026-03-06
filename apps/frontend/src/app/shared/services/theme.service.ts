import { Injectable, signal, effect, inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private document = inject(DOCUMENT);

  // Signal para controlar o estado do tema
  isDarkMode = signal<boolean>(this.getInitialTheme());

  constructor() {
    // Effect para aplicar as mudanças de tema automaticamente quando o signal mudar
    effect(() => {
      if (this.isDarkMode()) {
        this.document.documentElement.classList.add('dark');
        localStorage.setItem('theme', 'dark');
      } else {
        this.document.documentElement.classList.remove('dark');
        localStorage.setItem('theme', 'light');
      }
    });
  }

  toggleTheme() {
    this.isDarkMode.update((dark) => !dark);
  }

  private getInitialTheme(): boolean {
    // Verifica preferência salva ou do sistema
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
      return savedTheme === 'dark';
    }
    return window.matchMedia('(prefers-color-scheme: dark)').matches;
  }
}
