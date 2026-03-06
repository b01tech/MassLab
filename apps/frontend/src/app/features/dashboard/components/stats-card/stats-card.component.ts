import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

export type CardVariant = 'blue' | 'yellow' | 'purple' | 'red' | 'green' | 'gray' | 'orange';

@Component({
  selector: 'app-stats-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './stats-card.component.html',
})
export class StatsCardComponent {
  title = input.required<string>();
  count = input.required<string | number>();
  label = input.required<string>();
  icon = input.required<string>();
  variant = input<CardVariant>('blue');

  getIconClass(): string {
    const classes: Record<CardVariant, string> = {
      blue: 'bg-blue-50 text-blue-600 dark:bg-blue-900/20 dark:text-blue-400',
      yellow: 'bg-yellow-50 text-yellow-600 dark:bg-yellow-900/20 dark:text-yellow-400',
      purple: 'bg-purple-50 text-purple-600 dark:bg-purple-900/20 dark:text-purple-400',
      red: 'bg-red-50 text-red-600 dark:bg-red-900/20 dark:text-red-400',
      green: 'bg-green-50 text-green-600 dark:bg-green-900/20 dark:text-green-400',
      gray: 'bg-gray-100 text-gray-600 dark:bg-gray-800 dark:text-gray-400',
      orange: 'bg-orange-50 text-orange-600 dark:bg-orange-900/20 dark:text-orange-400',
    };
    return classes[this.variant()];
  }

  getLabelClass(): string {
    // Usually same color as variant or muted. Image shows muted mostly, except "ALERTA" which is red.
    if (this.label() === 'ALERTA') return 'text-red-500 font-bold';
    return 'text-text-muted';
  }
}
