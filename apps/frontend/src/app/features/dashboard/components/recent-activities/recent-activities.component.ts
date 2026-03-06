import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-recent-activities',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './recent-activities.component.html',
})
export class RecentActivitiesComponent {
  activities = [
    {
      id: 1,
      title: 'Calibração finalizada: Balança Analítica MT-200',
      user: 'Técnico: João Silva',
      time: 'Há 15 min',
      ref: '#OS-1244',
      color: 'bg-blue-500',
    },
    {
      id: 2,
      title: 'Novo cliente cadastrado: Indústria Farmacêutica Ú',
      user: 'Administrador: Maria Oliveira',
      time: 'Há 1 hora',
      ref: '#CL-882',
      color: 'bg-green-500',
    },
    {
      id: 3,
      title: 'Padrão em manutenção: Bloco Padrão 50mm',
      user: 'Logística: Carlos Souza',
      time: 'Há 3 horas',
      ref: '#EQ-044',
      color: 'bg-orange-500',
    },
  ];
}
